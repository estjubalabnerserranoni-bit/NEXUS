namespace NexusJuegoForms.Models;

public class Mision
{
    public string Codigo { get; set; } = "MIS-001";
    public string Nombre { get; set; } = "Primer contacto";
    public string Descripcion { get; set; } = "";
    public string RealidadObjetivo { get; set; } = "Alfa";
    public string Recompensa { get; set; } = "";

    public bool RequiereRecurso { get; set; }
    public bool TieneRecurso { get; set; }
    public int SectorRecursoObjetivo { get; set; } = -1;

    public int AnalisisRequeridos { get; set; }
    public int AnalisisCompletados { get; set; }
    public int AnalisisCriticosRequeridos { get; set; }
    public int AnalisisCriticosCompletados { get; set; }
    public List<int> SectoresAnalisisObjetivo { get; set; } = new();
    public List<int> SectoresAnalizados { get; set; } = new();

    public bool RequierePurga { get; set; }
    public bool PurgaRealizada { get; set; }

    public bool RequiereVisitaObjetivo { get; set; }
    public int SectorObjetivo { get; set; } = -1;
    public bool SectorObjetivoVisitado { get; set; }

    public bool Completada { get; set; }

    // Permite leer el campo usado por el primer prototipo JSON.
    public bool TieneAnalisis
    {
        get => AnalisisCompletados > 0;
        set
        {
            if (value && AnalisisCompletados == 0)
                AnalisisCompletados = 1;
        }
    }

    public string Estado => Completada ? "Completada" : "En progreso";

    public bool ObjetivosCumplidos =>
        (!RequiereRecurso || TieneRecurso) &&
        AnalisisCompletados >= AnalisisRequeridos &&
        AnalisisCriticosCompletados >= AnalisisCriticosRequeridos &&
        SectoresAnalisisObjetivo.All(SectoresAnalizados.Contains) &&
        (!RequierePurga || PurgaRealizada) &&
        (!RequiereVisitaObjetivo || SectorObjetivoVisitado);

    public void RegistrarAnalisis(int sector, bool critico)
    {
        if (!SectoresAnalizados.Contains(sector))
            SectoresAnalizados.Add(sector);

        AnalisisCompletados++;

        if (critico)
            AnalisisCriticosCompletados++;
    }

    public bool Completar(Explorador explorador)
    {
        if (Completada || !ObjetivosCumplidos)
            return false;

        Completada = true;
        explorador.NivelAcceso++;
        return true;
    }
}
