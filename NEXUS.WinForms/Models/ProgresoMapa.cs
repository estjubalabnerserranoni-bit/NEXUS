namespace NexusJuegoForms.Models;

public class ProgresoMapa
{
    public int Posicion { get; set; }
    public bool[] ZonasVisitadas { get; set; } = new bool[9];
    public bool[] ZonasExploradas { get; set; } = new bool[9];

    public void Normalizar()
    {
        if (Posicion < 0 || Posicion > 8) Posicion = 0;
        if (ZonasVisitadas == null || ZonasVisitadas.Length != 9) ZonasVisitadas = new bool[9];
        if (ZonasExploradas == null || ZonasExploradas.Length != 9) ZonasExploradas = new bool[9];
        ZonasVisitadas[Posicion] = true;
    }
}
