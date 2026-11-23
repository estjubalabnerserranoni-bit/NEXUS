using NexusJuegoForms.Models;

namespace NexusJuegoForms.Services;

public enum TipoZona
{
    Base,
    Vacia,
    Recurso,
    Anomalia
}

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

    public Mision MisionActual => Partida.MisionActual;

    public ZonaInfo[] MapaActual =>
        Cadete.RealidadActual == "Beta" ? MapaBeta : MapaAlfa;

    public JuegoService(Partida partida)
    {
        Partida = partida ?? throw new ArgumentNullException(nameof(partida));

        if (Partida.ZonasVisitadas == null || Partida.ZonasVisitadas.Length != 9)
        {
            Partida.ZonasVisitadas = new bool[9];
        }

        if (Partida.ZonasExploradas == null || Partida.ZonasExploradas.Length != 9)
        {
            Partida.ZonasExploradas = new bool[9];
        }
    }

    public string ObtenerEstadoZona(int posicion)
    {
        if (posicion == Partida.PosicionActual)
        {
            return "Aquí";
        }

        if (Partida.ZonasExploradas[posicion])
        {
            return "Explorada";
        }

        if (Partida.ZonasVisitadas[posicion])
        {
            return "Visitada";
        }

        return "Disponible";
    }

    public ResultadoAccion Viajar(int destino)
    {
        if (destino < 0 || destino >= MapaActual.Length)
        {
            return new ResultadoAccion(false, "El sector seleccionado no existe.");
        }

        if (destino == Partida.PosicionActual)
        {
            return new ResultadoAccion(false, "Ya te encuentras en ese sector.");
        }

        if (!SonSectoresVecinos(Partida.PosicionActual, destino))
        {
            return new ResultadoAccion(false,
                "Ese sector no está conectado directamente. Avanza por un sector vecino.");
        }

        if (!Cadete.GastarEnergia(CostoViaje))
        {
            return new ResultadoAccion(false,
                "Energía insuficiente. Cada viaje cuesta 10 puntos. Regresa a la base para recargar.");
        }

        Partida.PosicionActual = destino;
        Partida.ZonasVisitadas[destino] = true;

        ZonaInfo zona = MapaActual[destino];

        switch (zona.Tipo)
        {
            case TipoZona.Base:
                if (MisionActual.Completada)
                {
                    return new ResultadoAccion(true,
                        "Regresaste a la base. La misión MIS-001 ya está completada.");
                }

                if (MisionActual.Completar(Cadete))
                {
                    return new ResultadoAccion(true,
                        "¡Misión completada! Recuperaste el recurso, analizaste la anomalía y obtuviste acceso de nivel 2.");
                }

                return new ResultadoAccion(true,
                    "Llegaste a la base. Revisa los objetivos pendientes de la misión.");

            case TipoZona.Recurso:
                if (Partida.ZonasExploradas[destino])
                {
                    return new ResultadoAccion(true, "Este depósito ya fue revisado.");
                }

                Partida.ZonasExploradas[destino] = true;
                MisionActual.TieneRecurso = true;
                Cadete.AgregarObjeto(
                    "Celda de energía",
                    "Permite recuperar 25 puntos de energía.");
                return new ResultadoAccion(true,
                    "Recurso recuperado. Se añadió una celda de energía al inventario.");

            case TipoZona.Anomalia:
                if (Partida.ZonasExploradas[destino])
                {
                    return new ResultadoAccion(true, "Esta anomalía ya fue analizada.");
                }

                return new ResultadoAccion(true,
                    "Anomalía localizada. Ajusta la frecuencia del escáner y analiza el sector.");

            default:
                Partida.ZonasExploradas[destino] = true;
                return new ResultadoAccion(true,
                    "Sector revisado. No se detectaron hallazgos importantes.");
        }
    }

    public ResultadoAccion Escanear(int frecuencia)
    {
        int posicion = Partida.PosicionActual;

        if (MapaActual[posicion].Tipo != TipoZona.Anomalia)
        {
            return new ResultadoAccion(false,
                "Primero debes viajar a un sector que contenga una anomalía.");
        }

        if (Partida.ZonasExploradas[posicion])
        {
            return new ResultadoAccion(false, "Esta anomalía ya fue analizada.");
        }

        if (frecuencia < 40 || frecuencia > 100)
        {
            return new ResultadoAccion(false,
                "La frecuencia debe estar entre 40 y 100 para detectar la anomalía. No se consumió energía.");
        }

        bool lecturaCritica = frecuencia > 80;
        int costo = lecturaCritica ? 20 : 5;

        if (!Cadete.GastarEnergia(costo))
        {
            return new ResultadoAccion(false,
                $"Energía insuficiente. Este escaneo necesita {costo} puntos.");
        }

        if (lecturaCritica)
        {
            Cadete.Estabilidad = Math.Max(0, Cadete.Estabilidad - 20);
            Cadete.NucleoInestable = true;
        }

        Partida.ZonasExploradas[posicion] = true;
        MisionActual.TieneAnalisis = true;
        Cadete.AgregarObjeto(
            "Muestra de anomalía",
            "Registro obtenido durante el análisis de una anomalía.");

        return lecturaCritica
            ? new ResultadoAccion(true,
                "Anomalía crítica registrada. Energía -20 y estabilidad -20. Regresa a la base para purgar el núcleo.")
            : new ResultadoAccion(true,
                "Anomalía estable registrada. Energía -5. La muestra se añadió al inventario.");
    }

    public ResultadoAccion Recargar()
    {
        if (Partida.PosicionActual != 0)
        {
            return new ResultadoAccion(false,
                "La recarga solo está disponible en la base central.");
        }

        if (Cadete.Energia == 100 && Cadete.Estabilidad == 100)
        {
            return new ResultadoAccion(false,
                "La energía y la estabilidad ya están al máximo.");
        }

        Cadete.Recargar();

        return new ResultadoAccion(true,
            "Recarga realizada en la base: +30 de energía y +10 de estabilidad (sin superar 100).");
    }

    public ResultadoAccion PurgarNucleo()
    {
        if (Partida.PosicionActual != 0)
        {
            return new ResultadoAccion(false,
                "Debes regresar a la base para realizar la purga del núcleo.");
        }

        if (!Cadete.NucleoInestable)
        {
            return new ResultadoAccion(false, "El núcleo ya está estable.");
        }

        Cadete.NucleoInestable = false;
        Cadete.Estabilidad = Math.Min(100, Cadete.Estabilidad + 20);

        return new ResultadoAccion(true,
            "Purga completada. El núcleo se estabilizó y recuperaste 20 puntos de estabilidad.");
    }

    public ResultadoAccion UsarCeldaEnergia()
    {
        bool usada = Cadete.UsarCeldaEnergia(out string mensaje);
        return new ResultadoAccion(usada, mensaje);
    }

    private static bool SonSectoresVecinos(int origen, int destino)
    {
        int filaOrigen = origen / 3;
        int columnaOrigen = origen % 3;
        int filaDestino = destino / 3;
        int columnaDestino = destino % 3;

        int distancia = Math.Abs(filaOrigen - filaDestino)
                      + Math.Abs(columnaOrigen - columnaDestino);

        return distancia == 1;
    }
}
