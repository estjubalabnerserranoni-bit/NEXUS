using System.Text.Json.Serialization;

namespace NexusJuegoForms.Models;

public class Partida
{
    public int NumeroRanura { get; set; } = 1;
    public int VersionDatos { get; set; } = 2;
    public Explorador Cadete { get; set; } = new();
    public List<Mision> Misiones { get; set; } = new();
    public int IndiceMisionActual { get; set; }
    public ProgresoMapa ProgresoAlfa { get; set; } = new();
    public ProgresoMapa ProgresoBeta { get; set; } = new();
    public Mision? MisionGuardadaAnterior { get; set; }

    [JsonIgnore]
    public ProgresoMapa ProgresoActual => Cadete.RealidadActual == "Beta" ? ProgresoBeta : ProgresoAlfa;

    [JsonIgnore]
    public Mision MisionActual => Misiones[Math.Clamp(IndiceMisionActual, 0, Misiones.Count - 1)];

    [JsonIgnore]
    public bool CampanaCompletada => IndiceMisionActual >= Misiones.Count;

    [JsonIgnore]
    public int PosicionActual { get => ProgresoActual.Posicion; set => ProgresoActual.Posicion = value; }

    [JsonIgnore]
    public bool[] ZonasVisitadas => ProgresoActual.ZonasVisitadas;

    [JsonIgnore]
    public bool[] ZonasExploradas => ProgresoActual.ZonasExploradas;

    public void Normalizar()
    {
        Cadete ??= new Explorador();
        ProgresoAlfa ??= new ProgresoMapa();
        ProgresoBeta ??= new ProgresoMapa();
        ProgresoAlfa.Normalizar();
        ProgresoBeta.Normalizar();

        if (Cadete.RealidadActual != "Alfa" && Cadete.RealidadActual != "Beta")
            Cadete.RealidadActual = "Alfa";

        if (Misiones == null || Misiones.Count == 0)
        {
            Misiones = Services.CampanaService.CrearCampana();
            if (MisionGuardadaAnterior != null)
            {
                Misiones[0].TieneRecurso = MisionGuardadaAnterior.TieneRecurso;
                Misiones[0].AnalisisCompletados = MisionGuardadaAnterior.TieneAnalisis ? 1 : 0;
                Misiones[0].Completada = MisionGuardadaAnterior.Completada;
                if (Misiones[0].Completada) IndiceMisionActual = 1;
            }
        }

        IndiceMisionActual = Math.Clamp(IndiceMisionActual, 0, Misiones.Count);
        VersionDatos = 2;
    }

    public bool AvanzarMision()
    {
        if (CampanaCompletada || !MisionActual.Completada) return false;
        IndiceMisionActual++;
        return true;
    }
}
