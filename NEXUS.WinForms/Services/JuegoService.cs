using NexusJuegoForms.Models;

namespace NexusJuegoForms.Services;

public enum TipoZona { Base, Vacia, Recurso, Anomalia }

public class ZonaInfo
{
    public string Nombre { get; }
    public TipoZona Tipo { get; }

    public ZonaInfo(string nombre, TipoZona tipo)
    {
        Nombre = nombre;
        Tipo = tipo;
    }
}

public class ResultadoAccion
{
    public bool Exito { get; }
    public string Mensaje { get; }

    public ResultadoAccion(bool exito, string mensaje)
    {
        Exito = exito;
        Mensaje = mensaje;
    }
}

public class JuegoService
{
    private const int CostoViaje = 10;

    private static readonly ZonaInfo[] MapaAlfa =
    {
        new("Base central", TipoZona.Base),
        new("Pasaje norte", TipoZona.Vacia),
        new("Sala vacía", TipoZona.Vacia),
        new("Pasaje oeste", TipoZona.Vacia),
        new("Depósito de energía", TipoZona.Recurso),
        new("Pasaje este", TipoZona.Vacia),
        new("Cámara en ruinas", TipoZona.Vacia),
        new("Anomalía central", TipoZona.Anomalia),
        new("Pasaje sur", TipoZona.Vacia)
    };

    private static readonly ZonaInfo[] MapaBeta =
    {
        new("Base central", TipoZona.Base),
        new("Anomalía norte", TipoZona.Anomalia),
        new("Pasaje superior", TipoZona.Vacia),
        new("Galería oeste", TipoZona.Vacia),
        new("Cámara vacía", TipoZona.Vacia),
        new("Depósito de energía", TipoZona.Recurso),
        new("Anomalía oeste", TipoZona.Anomalia),
        new("Pasaje inferior", TipoZona.Vacia),
        new("Anomalía este", TipoZona.Anomalia)
    };

    public Partida Partida { get; }
    public Explorador Cadete => Partida.Cadete;
    public bool CampanaCompletada => Partida.CampanaCompletada;
    public Mision? MisionActual => CampanaCompletada ? null : Partida.MisionActual;

    public ZonaInfo[] MapaActual => Cadete.RealidadActual == "Beta" ? MapaBeta : MapaAlfa;

    public JuegoService(Partida partida)
    {
        Partida = partida ?? throw new ArgumentNullException(nameof(partida));
        Partida.Normalizar();
    }

    public string ObtenerEstadoZona(int posicion)
    {
        if (posicion < 0 || posicion >= 9) return "No disponible";
        if (posicion == Partida.PosicionActual) return "Aquí";
        if (Partida.ZonasExploradas[posicion]) return "Explorada";
        if (Partida.ZonasVisitadas[posicion]) return "Visitada";
        return "Disponible";
    }

    public bool PuedeAvanzarMision =>
        !CampanaCompletada && MisionActual?.Completada == true;

    public ResultadoAccion Viajar(int destino)
    {
        if (CampanaCompletada)
            return new(false, "La campaña terminó. Puedes iniciar una nueva partida.");

        if (destino < 0 || destino >= MapaActual.Length)
            return new(false, "El sector seleccionado no existe.");

        if (destino == Partida.PosicionActual)
            return new(false, "Ya te encuentras en ese sector.");

        if (!SonSectoresVecinos(Partida.PosicionActual, destino))
            return new(false, "Ese sector no está conectado directamente. Avanza por un sector vecino.");

        if (!Cadete.GastarEnergia(CostoViaje))
            return new(false, "Energía insuficiente. Cada viaje cuesta 10 puntos. Regresa a la base para recargar.");

        Partida.PosicionActual = destino;
        Partida.ZonasVisitadas[destino] = true;

        Mision mision = Partida.MisionActual;
        ZonaInfo zona = MapaActual[destino];

        if (mision.RequiereVisitaObjetivo && Cadete.RealidadActual == mision.RealidadObjetivo &&
            destino == mision.SectorObjetivo)
        {
            mision.SectorObjetivoVisitado = true;
        }

        switch (zona.Tipo)
        {
            case TipoZona.Base:
                if (mision.Completada)
                    return new(true, "Regresaste a la base. Pulsa «Siguiente misión» para continuar.");
                if (mision.Completar(Cadete))
                    return new(true, $"¡{mision.Codigo} completada! Recompensa: {mision.Recompensa}. Pulsa «Siguiente misión».");
                return new(true, "Llegaste a la base. Todavía tienes objetivos pendientes. " + ResumenObjetivos(mision));

            case TipoZona.Recurso:
                if (Partida.ZonasExploradas[destino])
                    return new(true, "Este depósito ya fue revisado.");

                // Un recurso reservado para otra misión no se consume antes de tiempo.
                if (!mision.RequiereRecurso ||
                    Cadete.RealidadActual != mision.RealidadObjetivo ||
                    (mision.SectorRecursoObjetivo >= 0 && destino != mision.SectorRecursoObjetivo))
                    return new(true, "Depósito localizado. IRIS lo reservará para la misión que tenga ese objetivo.");

                Partida.ZonasExploradas[destino] = true;
                mision.TieneRecurso = true;
                Cadete.AgregarObjeto("Celda de energía", "Permite recuperar 25 puntos de energía.");
                return new(true, "Recurso recuperado. Se añadió una celda de energía al inventario.");

            case TipoZona.Anomalia:
                if (Partida.ZonasExploradas[destino])
                    return new(true, "Esta anomalía ya fue analizada.");

                return new(true, "Anomalía localizada. Ajusta la frecuencia del escáner y analiza el sector.");

            default:
                Partida.ZonasExploradas[destino] = true;
                return new(true, "Sector revisado. No se detectaron hallazgos importantes.");
        }
    }

    public ResultadoAccion Escanear(int frecuencia)
    {
        if (CampanaCompletada) return new(false, "La campaña ya terminó.");
        int posicion = Partida.PosicionActual;
        Mision mision = Partida.MisionActual;

        if (MapaActual[posicion].Tipo != TipoZona.Anomalia)
            return new(false, "Primero debes viajar a un sector que contenga una anomalía.");

        if (Partida.ZonasExploradas[posicion])
            return new(false, "Esta anomalía ya fue analizada.");

        if (Cadete.RealidadActual != mision.RealidadObjetivo)
            return new(false, $"La misión actual requiere explorar la realidad {mision.RealidadObjetivo}.");

        if (mision.SectoresAnalisisObjetivo.Count > 0 &&
            !mision.SectoresAnalisisObjetivo.Contains(posicion))
            return new(false, "Esta anomalía no forma parte del objetivo de la misión.");

        if (frecuencia < 40 || frecuencia > 100)
            return new(false, "La frecuencia debe estar entre 40 y 100. No se consumió energía.");

        bool critica = frecuencia > 80;
        int costo = critica ? 20 : 5;

        if (!Cadete.GastarEnergia(costo))
            return new(false, $"Energía insuficiente. Este escaneo necesita {costo} puntos.");

        if (critica)
        {
            Cadete.Estabilidad = Math.Max(0, Cadete.Estabilidad - 20);
            Cadete.NucleoInestable = true;

            if (mision.RequierePurga)
                mision.PurgaRealizada = false;
        }

        Partida.ZonasExploradas[posicion] = true;
        mision.RegistrarAnalisis(posicion, critica);
        Cadete.AgregarObjeto("Muestra de anomalía", "Registro obtenido durante el análisis de una anomalía.");

        return critica
            ? new(true, "Anomalía crítica registrada. Energía -20 y estabilidad -20. Regresa a la base para purgar el núcleo.")
            : new(true, "Anomalía estable registrada. Energía -5. La muestra se añadió al inventario.");
    }

    public ResultadoAccion Recargar()
    {
        if (Partida.PosicionActual != 0)
            return new(false, "La recarga solo está disponible en la base central.");
        if (Cadete.Energia == 100 && Cadete.Estabilidad == 100)
            return new(false, "La energía y la estabilidad ya están al máximo.");

        Cadete.Recargar();
        return new(true, "Recarga realizada en la base: +30 de energía y +10 de estabilidad (máximo 100).");
    }

    public ResultadoAccion PurgarNucleo()
    {
        if (Partida.PosicionActual != 0)
            return new(false, "Debes regresar a la base para realizar la purga.");
        if (!Cadete.NucleoInestable)
            return new(false, "El núcleo ya está estable.");

        Cadete.NucleoInestable = false;
        Cadete.Estabilidad = Math.Min(100, Cadete.Estabilidad + 20);

        if (MisionActual != null && MisionActual.RequierePurga)
        {
            MisionActual.PurgaRealizada = true;

            if (MisionActual.Completar(Cadete))
                return new(true, $"Purga completada. ¡{MisionActual.Codigo} finalizada! Pulsa «Siguiente misión» para continuar.");
        }

        return new(true, "Purga completada. El núcleo se estabilizó y recuperaste 20 puntos de estabilidad.");
    }

    public ResultadoAccion CambiarRealidad()
    {
        if (Partida.PosicionActual != 0)
            return new(false, "Debes estar en la base central para viajar entre realidades.");

        if (CampanaCompletada)
            return new(false, "La campaña está completa.");

        string destino = Partida.MisionActual.RealidadObjetivo;

        if (Cadete.RealidadActual == destino)
            return new(false, $"Ya estás en la realidad {destino}.");

        Cadete.RealidadActual = destino;
        Partida.ProgresoActual.ZonasVisitadas[0] = true;

        return new(true, $"Portal dimensional activado. Ahora estás en la realidad {destino}.");
    }

    public ResultadoAccion UsarObjeto(string nombre)
    {
        bool usado = Cadete.UsarObjeto(nombre, out string mensaje);
        return new(usado, mensaje);
    }

    public ResultadoAccion AvanzarMision()
    {
        if (!PuedeAvanzarMision)
            return new(false, "Primero debes completar los objetivos y regresar a la base.");

        string codigoCompletado = Partida.MisionActual.Codigo;
        Partida.AvanzarMision();

        if (CampanaCompletada)
            return new(true, $"¡Campaña completada! Has terminado {codigoCompletado}. NEXUS agradece tu servicio.");

        return new(true, $"Se activó {Partida.MisionActual.Codigo}: {Partida.MisionActual.Nombre}. Objetivo: realidad {Partida.MisionActual.RealidadObjetivo}.");
    }

    public static string ResumenObjetivos(Mision mision)
    {
        var pendientes = new List<string>();

        if (mision.RequiereRecurso && !mision.TieneRecurso) pendientes.Add("recuperar el recurso");
        if (mision.AnalisisCompletados < mision.AnalisisRequeridos) pendientes.Add("analizar las anomalías requeridas");
        if (mision.AnalisisCriticosCompletados < mision.AnalisisCriticosRequeridos) pendientes.Add("realizar un análisis crítico");
        if (mision.RequierePurga && !mision.PurgaRealizada) pendientes.Add("purgar el núcleo");
        if (mision.RequiereVisitaObjetivo && !mision.SectorObjetivoVisitado) pendientes.Add("visitar el sector objetivo");

        return pendientes.Count == 0 ? "Regresa a la base para completar la misión." :
            "Pendiente: " + string.Join(", ", pendientes) + ".";
    }

    private static bool SonSectoresVecinos(int origen, int destino)
    {
        int filaOrigen = origen / 3, columnaOrigen = origen % 3;
        int filaDestino = destino / 3, columnaDestino = destino % 3;
        return Math.Abs(filaOrigen - filaDestino) + Math.Abs(columnaOrigen - columnaDestino) == 1;
    }
}
