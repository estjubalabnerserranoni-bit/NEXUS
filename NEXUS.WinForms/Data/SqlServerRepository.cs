using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using NexusJuegoForms.Models;

namespace NexusJuegoForms.Data;

/// <summary>
/// Persistencia opcional en SQL Server. La cadena de conexión se obtiene
/// de la variable de entorno NEXUS_SQLSERVER_CONNECTION, nunca del código.
/// </summary>
public sealed class SqlServerRepository
{
    private readonly string connectionString;

    public static bool EstaConfigurado =>
        !string.IsNullOrWhiteSpace(
            Environment.GetEnvironmentVariable("NEXUS_SQLSERVER_CONNECTION"));

    public SqlServerRepository(string? cadenaConexion = null)
    {
        connectionString = cadenaConexion ??
            Environment.GetEnvironmentVariable("NEXUS_SQLSERVER_CONNECTION") ?? "";

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No se configuró NEXUS_SQLSERVER_CONNECTION.");
        }
    }

    public async Task ProbarConexionAsync(CancellationToken cancellationToken = default)
    {
        await using SqlConnection conexion = new(connectionString);
        await conexion.OpenAsync(cancellationToken);
    }

    public async Task GuardarAsync(
        Partida partida,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(partida);
        partida.Normalizar();

        await using SqlConnection conexion = new(connectionString);
        await conexion.OpenAsync(cancellationToken);
        await using SqlTransaction transaccion =
            (SqlTransaction)await conexion.BeginTransactionAsync(cancellationToken);

        try
        {
            int idExplorador = await ObtenerOCrearExploradorAsync(
                conexion, transaccion, partida.Cadete.Nombre, cancellationToken);

            string json = JsonSerializer.Serialize(partida);
            int idPartida = await ObtenerOCrearPartidaAsync(
                conexion, transaccion, idExplorador, partida, json, cancellationToken);

            await using (SqlCommand borrarMisiones = new(
                "DELETE FROM dbo.ProgresoMision WHERE IdPartida = @id;", conexion, transaccion))
            {
                borrarMisiones.Parameters.Add("@id", SqlDbType.Int).Value = idPartida;
                await borrarMisiones.ExecuteNonQueryAsync(cancellationToken);
            }

            foreach (Mision mision in partida.Misiones)
            {
                int idMision = await ObtenerOCrearMisionAsync(
                    conexion, transaccion, mision, cancellationToken);

                await using SqlCommand comando = new(
                    """
                    INSERT INTO dbo.ProgresoMision (IdPartida, IdMision, Estado, DatosJson)
                    VALUES (@partida, @mision, @estado, @datos);
                    """, conexion, transaccion);

                comando.Parameters.Add("@partida", SqlDbType.Int).Value = idPartida;
                comando.Parameters.Add("@mision", SqlDbType.Int).Value = idMision;
                comando.Parameters.Add("@estado", SqlDbType.NVarChar, 20).Value = mision.Estado;
                comando.Parameters.Add("@datos", SqlDbType.NVarChar, -1).Value =
                    JsonSerializer.Serialize(mision);
                await comando.ExecuteNonQueryAsync(cancellationToken);
            }

            await using (SqlCommand borrarInventario = new(
                "DELETE FROM dbo.Inventario WHERE IdPartida = @id;", conexion, transaccion))
            {
                borrarInventario.Parameters.Add("@id", SqlDbType.Int).Value = idPartida;
                await borrarInventario.ExecuteNonQueryAsync(cancellationToken);
            }

            foreach (ObjetoInventario objeto in partida.Cadete.Inventario)
            {
                await using SqlCommand comando = new(
                    """
                    INSERT INTO dbo.Inventario (IdPartida, Nombre, Descripcion, Cantidad)
                    VALUES (@partida, @nombre, @descripcion, @cantidad);
                    """, conexion, transaccion);

                comando.Parameters.Add("@partida", SqlDbType.Int).Value = idPartida;
                comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = objeto.Nombre;
                comando.Parameters.Add("@descripcion", SqlDbType.NVarChar, 300).Value = objeto.Descripcion;
                comando.Parameters.Add("@cantidad", SqlDbType.Int).Value = objeto.Cantidad;
                await comando.ExecuteNonQueryAsync(cancellationToken);
            }

            await transaccion.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaccion.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<Partida?> CargarAsync(
        string nombreCadete,
        int ranura,
        CancellationToken cancellationToken = default)
    {
        if (ranura is < 1 or > 3)
            throw new ArgumentOutOfRangeException(nameof(ranura));

        await using SqlConnection conexion = new(connectionString);
        await conexion.OpenAsync(cancellationToken);

        const string consulta = """
            SELECT p.JsonProgreso
            FROM dbo.Partida AS p
            INNER JOIN dbo.Explorador AS e ON e.IdExplorador = p.IdExplorador
            WHERE e.Nombre = @nombre AND p.NumeroRanura = @ranura;
            """;

        await using SqlCommand comando = new(consulta, conexion);
        comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 60).Value = nombreCadete;
        comando.Parameters.Add("@ranura", SqlDbType.Int).Value = ranura;

        object? resultado = await comando.ExecuteScalarAsync(cancellationToken);
        if (resultado is not string json)
            return null;

        Partida? partida = JsonSerializer.Deserialize<Partida>(json);
        if (partida == null)
            throw new InvalidDataException("La partida SQL no contiene datos válidos.");

        partida.NumeroRanura = ranura;
        partida.Normalizar();
        return partida;
    }

    private static async Task<int> ObtenerOCrearExploradorAsync(
        SqlConnection conexion,
        SqlTransaction transaccion,
        string nombre,
        CancellationToken cancellationToken)
    {
        const string sql = """
            IF NOT EXISTS (SELECT 1 FROM dbo.Explorador WHERE Nombre = @nombre)
                INSERT INTO dbo.Explorador (Nombre) VALUES (@nombre);
            SELECT IdExplorador FROM dbo.Explorador WHERE Nombre = @nombre;
            """;

        await using SqlCommand comando = new(sql, conexion, transaccion);
        comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 60).Value = nombre;
        return Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
    }

    private static async Task<int> ObtenerOCrearPartidaAsync(
        SqlConnection conexion,
        SqlTransaction transaccion,
        int idExplorador,
        Partida partida,
        string json,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE dbo.Partida
            SET FechaGuardado = SYSUTCDATETIME(),
                Energia = @energia,
                Estabilidad = @estabilidad,
                NivelAcceso = @nivel,
                RealidadActual = @realidad,
                JsonProgreso = @json
            WHERE IdExplorador = @explorador AND NumeroRanura = @ranura;

            IF @@ROWCOUNT = 0
            BEGIN
                INSERT INTO dbo.Partida
                    (IdExplorador, NumeroRanura, Energia, Estabilidad,
                     NivelAcceso, RealidadActual, JsonProgreso)
                VALUES
                    (@explorador, @ranura, @energia, @estabilidad,
                     @nivel, @realidad, @json);
            END;

            SELECT IdPartida FROM dbo.Partida
            WHERE IdExplorador = @explorador AND NumeroRanura = @ranura;
            """;

        await using SqlCommand comando = new(sql, conexion, transaccion);
        comando.Parameters.Add("@explorador", SqlDbType.Int).Value = idExplorador;
        comando.Parameters.Add("@ranura", SqlDbType.Int).Value = partida.NumeroRanura;
        comando.Parameters.Add("@energia", SqlDbType.Int).Value = partida.Cadete.Energia;
        comando.Parameters.Add("@estabilidad", SqlDbType.Int).Value = partida.Cadete.Estabilidad;
        comando.Parameters.Add("@nivel", SqlDbType.Int).Value = partida.Cadete.NivelAcceso;
        comando.Parameters.Add("@realidad", SqlDbType.NVarChar, 20).Value = partida.Cadete.RealidadActual;
        comando.Parameters.Add("@json", SqlDbType.NVarChar, -1).Value = json;

        return Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
    }

    private static async Task<int> ObtenerOCrearMisionAsync(
        SqlConnection conexion,
        SqlTransaction transaccion,
        Mision mision,
        CancellationToken cancellationToken)
    {
        const string sql = """
            IF NOT EXISTS (SELECT 1 FROM dbo.Mision WHERE Codigo = @codigo)
                INSERT INTO dbo.Mision (Codigo, Nombre, Descripcion, RealidadObjetivo, Recompensa)
                VALUES (@codigo, @nombre, @descripcion, @realidad, @recompensa);
            SELECT IdMision FROM dbo.Mision WHERE Codigo = @codigo;
            """;

        await using SqlCommand comando = new(sql, conexion, transaccion);
        comando.Parameters.Add("@codigo", SqlDbType.NVarChar, 20).Value = mision.Codigo;
        comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = mision.Nombre;
        comando.Parameters.Add("@descripcion", SqlDbType.NVarChar, 500).Value = mision.Descripcion;
        comando.Parameters.Add("@realidad", SqlDbType.NVarChar, 20).Value = mision.RealidadObjetivo;
        comando.Parameters.Add("@recompensa", SqlDbType.NVarChar, 200).Value = mision.Recompensa;

        return Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
    }
}
