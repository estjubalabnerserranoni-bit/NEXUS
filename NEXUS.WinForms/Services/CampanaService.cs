using NexusJuegoForms.Models;

namespace NexusJuegoForms.Services;

public static class CampanaService
{
    public static List<Mision> CrearCampana() => new()
    {
        new()
        {
            Codigo = "MIS-001", Nombre = "Primer contacto",
            Descripcion = "En Alfa, recupera una celda, analiza la primera anomalía y regresa a la base.",
            RealidadObjetivo = "Alfa", RequiereRecurso = true, AnalisisRequeridos = 1,
            Recompensa = "Acceso de nivel 2"
        },
        new()
        {
            Codigo = "MIS-002", Nombre = "Señal desconocida",
            Descripcion = "Viaja a Beta, localiza la anomalía del sector norte, analiza la señal y regresa a la base.",
            RealidadObjetivo = "Beta", AnalisisRequeridos = 1,
            Recompensa = "Acceso de nivel 3"
        },
        new()
        {
            Codigo = "MIS-003", Nombre = "Núcleo perdido",
            Descripcion = "En Beta, encuentra el depósito de energía y recupera el recurso.",
            RealidadObjetivo = "Beta", RequiereRecurso = true, AnalisisRequeridos = 0,
            Recompensa = "Celda de energía adicional"
        },
        new()
        {
            Codigo = "MIS-004", Nombre = "Ruptura dimensional",
            Descripcion = "Analiza dos anomalías de Beta. Al menos una lectura debe ser crítica. Regresa a la base y purga el núcleo.",
            RealidadObjetivo = "Beta", AnalisisRequeridos = 2,
            AnalisisCriticosRequeridos = 1, RequierePurga = true,
            Recompensa = "Acceso de nivel 4"
        },
        new()
        {
            Codigo = "MIS-005", Nombre = "El origen de NEXUS",
            Descripcion = "Regresa a Alfa, visita la anomalía central y vuelve a la base para sincronizar el registro.",
            RealidadObjetivo = "Alfa", AnalisisRequeridos = 0,
            RequiereVisitaObjetivo = true, SectorObjetivo = 7,
            Recompensa = "Campaña inicial completada"
        }
    };
}
