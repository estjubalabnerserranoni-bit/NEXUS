using System.Text.Json;
using NexusJuegoForms.Models;
using NexusJuegoForms.Services;

namespace NexusTests;

internal static class Program
{
    private static int pruebasEjecutadas;

    private static void Main()
    {
        Ejecutar("No permite saltar sectores", ProbarMovimientoNoVecino);
        Ejecutar("No consume energía si no alcanza", ProbarEnergiaInsuficiente);
        Ejecutar("Completa MIS-001 con sus objetivos", ProbarMisionInicial);
        Ejecutar("El análisis crítico reduce estabilidad", ProbarAnalisisCritico);
        Ejecutar("La campaña tiene cinco misiones", ProbarCampana);
        Ejecutar("No consume recursos antes de su misión", ProbarRecursoReservado);
        Ejecutar("La purga completa la misión de ruptura", ProbarPurgaMision);
        Ejecutar("Migra un guardado del prototipo anterior", ProbarMigracion);

        Console.WriteLine();
        Console.WriteLine($"Resultado: {pruebasEjecutadas} pruebas correctas.");
    }

    private static void Ejecutar(string nombre, Action prueba)
    {
        prueba();
        pruebasEjecutadas++;
        Console.WriteLine($"[OK] {nombre}");
    }

    private static void Verificar(bool condicion, string mensaje)
    {
        if (!condicion)
            throw new InvalidOperationException("PRUEBA FALLIDA: " + mensaje);
    }

    private static JuegoService CrearJuego(string realidad = "Alfa", int energia = 100)
    {
        Partida partida = new()
        {
            Cadete = new Explorador
            {
                Nombre = "Cadete de prueba",
                RealidadActual = realidad,
                Energia = energia,
                Estabilidad = 100
            },
            Misiones = CampanaService.CrearCampana(),
            IndiceMisionActual = realidad == "Beta" ? 1 : 0
        };

        return new JuegoService(partida);
    }

    private static void ProbarMovimientoNoVecino()
    {
        JuegoService juego = CrearJuego();
        ResultadoAccion resultado = juego.Viajar(4);

        Verificar(!resultado.Exito, "No debería viajar directamente de 0 a 4.");
        Verificar(juego.Cadete.Energia == 100, "El viaje inválido no debe gastar energía.");
    }

    private static void ProbarEnergiaInsuficiente()
    {
        JuegoService juego = CrearJuego(energia: 5);
        ResultadoAccion resultado = juego.Viajar(1);

        Verificar(!resultado.Exito, "El viaje debería rechazarse con 5 puntos.");
        Verificar(juego.Cadete.Energia == 5, "La energía no debe cambiar cuando se rechaza la acción.");
    }

    private static void ProbarMisionInicial()
    {
        JuegoService juego = CrearJuego();

        Verificar(juego.Viajar(1).Exito, "No pudo entrar al sector 1.");
        Verificar(juego.Viajar(4).Exito, "No pudo llegar al depósito de recursos.");
        Verificar(juego.Partida.MisionActual.TieneRecurso, "No registró el recurso requerido.");

        Verificar(juego.Viajar(5).Exito, "No pudo continuar al sector 5.");
        Verificar(juego.Viajar(8).Exito, "No pudo continuar al sector 8.");
        Verificar(juego.Viajar(7).Exito, "No pudo llegar a la anomalía.");
        Verificar(juego.Escanear(60).Exito, "El análisis normal debería funcionar.");

        Verificar(juego.Viajar(6).Exito, "No pudo regresar por el sector 6.");
        Verificar(juego.Viajar(3).Exito, "No pudo regresar por el sector 3.");
        Verificar(juego.Viajar(0).Exito, "No pudo regresar a la base.");

        Verificar(juego.Partida.MisionActual.Completada, "MIS-001 debería quedar completada.");
        Verificar(juego.Cadete.NivelAcceso == 2, "La recompensa debe aumentar el nivel de acceso.");
        Verificar(juego.AvanzarMision().Exito, "No pudo avanzar a MIS-002.");
        Verificar(juego.Partida.IndiceMisionActual == 1, "El índice de misión no avanzó.");
    }

    private static void ProbarAnalisisCritico()
    {
        JuegoService juego = CrearJuego("Beta", 100);
        Verificar(juego.Viajar(1).Exito, "No pudo entrar al sector de anomalía de Beta.");

        ResultadoAccion resultado = juego.Escanear(90);

        Verificar(resultado.Exito, "El análisis crítico debería funcionar.");
        Verificar(juego.Cadete.Energia == 70, "El viaje cuesta 10 y el análisis crítico cuesta 20.");
        Verificar(juego.Cadete.Estabilidad == 80, "El análisis crítico debe reducir estabilidad en 20.");
        Verificar(juego.Cadete.NucleoInestable, "El núcleo debe marcarse como inestable.");
        Verificar(juego.Partida.MisionActual.AnalisisCriticosCompletados == 1,
            "La misión debe registrar un análisis crítico.");
    }

    private static void ProbarCampana()
    {
        JuegoService juego = CrearJuego();
        Verificar(juego.Partida.Misiones.Count == 5, "La campaña debe tener cinco misiones.");
        Verificar(juego.Partida.Misiones[0].Codigo == "MIS-001", "La primera misión debe ser MIS-001.");
        Verificar(juego.Partida.Misiones[4].Codigo == "MIS-005", "La última misión debe ser MIS-005.");
    }

    private static void ProbarRecursoReservado()
    {
        JuegoService juego = CrearJuego("Beta", 100);

        Verificar(juego.Viajar(1).Exito, "No pudo salir de la base.");
        Verificar(juego.Viajar(4).Exito, "No pudo llegar al sector intermedio.");
        Verificar(juego.Viajar(5).Exito, "No pudo llegar al depósito.");

        Verificar(!juego.Partida.ZonasExploradas[5],
            "El depósito de MIS-003 no debe agotarse durante MIS-002.");
        Verificar(!juego.Partida.MisionActual.TieneRecurso,
            "MIS-002 no debe recibir un recurso que no forma parte de su objetivo.");

        // Simula el inicio de MIS-003 para comprobar que el depósito sigue disponible.
        juego.Partida.PosicionActual = 0;
        juego.Partida.IndiceMisionActual = 2;
        Verificar(juego.Viajar(1).Exito, "No pudo iniciar la ruta de MIS-003.");
        Verificar(juego.Viajar(4).Exito, "No pudo cruzar el sector intermedio.");
        Verificar(juego.Viajar(5).Exito, "No pudo recuperar el depósito reservado.");

        Verificar(juego.Partida.MisionActual.TieneRecurso,
            "MIS-003 debe poder recuperar el recurso reservado.");
        Verificar(juego.Partida.ZonasExploradas[5],
            "El depósito debe marcarse explorado después de la recolección correcta.");
    }

    private static void ProbarPurgaMision()
    {
        JuegoService juego = CrearJuego("Beta", 100);
        juego.Partida.IndiceMisionActual = 3;
        Mision mision = juego.Partida.MisionActual;
        mision.AnalisisCompletados = 2;
        mision.AnalisisCriticosCompletados = 1;
        mision.SectoresAnalizados.Add(6);
        mision.SectoresAnalizados.Add(8);
        juego.Cadete.NucleoInestable = true;

        ResultadoAccion resultado = juego.PurgarNucleo();

        Verificar(resultado.Exito, "La purga debe realizarse en la base.");
        Verificar(mision.PurgaRealizada, "La misión debe registrar la purga.");
        Verificar(mision.Completada, "La misión debe completarse al cumplir los demás objetivos y purgar.");
        Verificar(juego.Cadete.NivelAcceso == 2, "La recompensa debe aumentar el nivel de acceso.");
    }

    private static void ProbarMigracion()
    {
        const string jsonAnterior = """
        {
          "Cadete": {
            "Nombre": "Cadete antiguo",
            "RealidadActual": "Alfa",
            "Energia": 55,
            "Estabilidad": 80,
            "NivelAcceso": 1,
            "NucleoInestable": false
          },
          "Mision": {
            "Codigo": "MIS-001",
            "TieneRecurso": true,
            "TieneAnalisis": true,
            "Completada": true
          },
          "Posicion": 4,
          "Investigados": [false, false, false, false, true, false, false, true, false]
        }
        """;

        Partida? partida = JsonSerializer.Deserialize<Partida>(jsonAnterior);
        Verificar(partida != null, "No pudo leer la partida anterior.");

        partida!.Normalizar();

        Verificar(partida.Misiones.Count == 5, "No creó la nueva campaña.");
        Verificar(partida.IndiceMisionActual == 1, "La misión completada debe migrarse.");
        Verificar(partida.ProgresoAlfa.Posicion == 4, "No conservó la posición antigua.");
        Verificar(partida.ProgresoAlfa.ZonasExploradas[7],
            "No conservó los sectores investigados.");
    }
}
