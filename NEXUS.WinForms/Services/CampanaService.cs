using NexusJuegoForms.Models;

namespace NexusJuegoForms.Services;

public static class CampanaService
{
    public static List<Mision> CrearCampana() => new()
    {
        new()
        {
            Codigo = "MIS-001", Nombre = "Primer contacto",
            Descripcion = "En Alfa, recupera una celda de energía en el depósito, analiza la anomalía central y regresa a la base.",
            RealidadObjetivo = "Alfa", RequiereRecurso = true, SectorRecursoObjetivo = 4,
            AnalisisRequeridos = 1, SectoresAnalisisObjetivo = new() { 7 },
            Recompensa = "Acceso de nivel 2"
        },
        new()
        {
            Codigo = "MIS-002", Nombre = "Señal desconocida",
            Descripcion = "Viaja a Beta, analiza la señal del sector norte y vuelve a la base.",
            RealidadObjetivo = "Beta", AnalisisRequeridos = 1,
            SectoresAnalisisObjetivo = new() { 1 }, Recompensa = "Acceso de nivel 3"
        },
        new()
        {
            Codigo = "MIS-003", Nombre = "Núcleo perdido",
            Descripcion = "En Beta, recupera la celda del depósito del sector inferior central y regresa a la base.",
            RealidadObjetivo = "Beta", RequiereRecurso = true, SectorRecursoObjetivo = 5,
            AnalisisRequeridos = 0, Recompensa = "Una celda de energía adicional"
        },
        new()
        {
            Codigo = "MIS-004", Nombre = "Ruptura dimensional",
            Descripcion = "Analiza las anomalías de los sectores 6 y 8 de Beta. Al menos una lectura debe ser crítica. Regresa a la base y purga el núcleo.",
            RealidadObjetivo = "Beta", AnalisisRequeridos = 2,
            AnalisisCriticosRequeridos = 1, SectoresAnalisisObjetivo = new() { 6, 8 },
            RequierePurga = true, Recompensa = "Acceso de nivel 4 y núcleo estabilizado"
        },
        new()
        {
            Codigo = "MIS-005", Nombre = "El origen de NEXUS",
            Descripcion = "Regresa a Alfa, visita la anomalía central y vuelve a la base para sincronizar el registro final.",
            RealidadObjetivo = "Alfa", AnalisisRequeridos = 0,
            RequiereVisitaObjetivo = true, SectorObjetivo = 7,
            Recompensa = "Campaña inicial completada"
        }
    };
}
