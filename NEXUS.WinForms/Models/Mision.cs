namespace NexusJuegoForms.Models;

public class Mision
{
    public string Codigo { get; set; } = "MIS-001";
    public string Nombre { get; set; } = "La primera anomalía";
    public string Descripcion { get; set; } =
        "Recupera una celda de energía, analiza una anomalía y regresa a la base.";

    public bool TieneRecurso { get; set; }
    public bool TieneAnalisis { get; set; }
    public bool Completada { get; set; }

    public string Estado => Completada ? "Completada" : "En progreso";

    public bool Completar(Explorador explorador)
    {
        if (Completada || !TieneRecurso || !TieneAnalisis)
        {
            return false;
        }

        Completada = true;
        explorador.NivelAcceso++;
        return true;
    }
}
