namespace NexusJuegoForms.Models;

public class Partida
{
    public Explorador Cadete { get; set; } = new();
    public Mision MisionActual { get; set; } = new();

    // El mapa tiene nueve sectores. Se guarda qué sectores se visitaron
    // y cuáles ya fueron explorados o investigados.
    public int PosicionActual { get; set; }
    public bool[] ZonasVisitadas { get; set; } = new bool[9];
    public bool[] ZonasExploradas { get; set; } = new bool[9];
}
