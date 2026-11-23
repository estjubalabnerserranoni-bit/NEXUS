using System.Text.Json;
using NexusJuegoForms.Models;

namespace NexusJuegoForms.Data;

public static class GuardadoLocal
{
    private static readonly string Carpeta = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "NEXUS");

    private static readonly string Archivo = Path.Combine(Carpeta, "partida.json");

    private static readonly JsonSerializerOptions Opciones = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public static bool ExistePartida()
    {
        return File.Exists(Archivo);
    }

    public static void Guardar(Partida partida)
    {
        Directory.CreateDirectory(Carpeta);

        string temporal = Archivo + ".tmp";
        string contenido = JsonSerializer.Serialize(partida, Opciones);

        File.WriteAllText(temporal, contenido);
        File.Move(temporal, Archivo, true);
    }

    public static Partida? Cargar()
    {
        if (!File.Exists(Archivo))
        {
            return null;
        }

        string contenido = File.ReadAllText(Archivo);
        Partida? partida = JsonSerializer.Deserialize<Partida>(contenido, Opciones);

        if (partida == null ||
            partida.Cadete == null ||
            partida.MisionActual == null ||
            partida.PosicionActual < 0 ||
            partida.PosicionActual > 8 ||
            partida.Cadete.Inventario == null)
        {
            throw new InvalidDataException("El archivo de la partida está incompleto o dañado.");
        }

        if (partida.Cadete.Energia is < 0 or > 100 ||
            partida.Cadete.Estabilidad is < 0 or > 100 ||
            (partida.Cadete.RealidadActual != "Alfa" &&
             partida.Cadete.RealidadActual != "Beta"))
        {
            throw new InvalidDataException("Los datos guardados no son válidos.");
        }

        return partida;
    }
}
