using System.Text.Json;
using NexusJuegoForms.Models;

namespace NexusJuegoForms.Data;

public static class GuardadoLocal
{
    public const int CantidadRanuras = 3;

    private static readonly string Carpeta = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NEXUS");

    private static readonly JsonSerializerOptions Opciones = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private static string ObtenerRuta(int ranura)
    {
        ValidarRanura(ranura);
        return Path.Combine(Carpeta, $"partida-{ranura}.json");
    }

    private static void ValidarRanura(int ranura)
    {
        if (ranura < 1 || ranura > CantidadRanuras)
            throw new ArgumentOutOfRangeException(nameof(ranura), "La ranura debe estar entre 1 y 3.");
    }

    public static bool ExistePartida(int ranura) => File.Exists(ObtenerRuta(ranura));

    public static void Guardar(Partida partida)
    {
        ArgumentNullException.ThrowIfNull(partida);
        string ruta = ObtenerRuta(partida.NumeroRanura);
        Directory.CreateDirectory(Carpeta);

        string temporal = ruta + ".tmp";
        string contenido = JsonSerializer.Serialize(partida, Opciones);

        File.WriteAllText(temporal, contenido);
        File.Move(temporal, ruta, true);
    }

    public static Partida? Cargar(int ranura)
    {
        string ruta = ObtenerRuta(ranura);
        if (!File.Exists(ruta))
            return null;

        string contenido = File.ReadAllText(ruta);
        Partida? partida = JsonSerializer.Deserialize<Partida>(contenido, Opciones);

        if (partida == null || partida.Cadete == null)
            throw new InvalidDataException("El archivo de la partida está incompleto.");

        partida.NumeroRanura = ranura;
        partida.Normalizar();

        if (partida.Cadete.Energia is < 0 or > 100 ||
            partida.Cadete.Estabilidad is < 0 or > 100 ||
            string.IsNullOrWhiteSpace(partida.Cadete.Nombre) ||
            partida.ProgresoAlfa.Posicion is < 0 or > 8 ||
            partida.ProgresoBeta.Posicion is < 0 or > 8)
        {
            throw new InvalidDataException("Los datos guardados no son válidos.");
        }

        return partida;
    }

    public static string ObtenerDescripcionRanura(int ranura)
    {
        Partida? partida = Cargar(ranura);

        if (partida == null)
            return $"Ranura {ranura} — Vacía";

        string estado = partida.CampanaCompletada
            ? "Campaña completada"
            : partida.MisionActual.Codigo + " — " + partida.MisionActual.Nombre;

        return $"Ranura {ranura} — {partida.Cadete.Nombre} — {estado}";
    }
}
