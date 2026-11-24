# NEXUS — Exploration / Reality System

Videojuego 2D de exploración dimensional desarrollado en C# con Windows Forms. El jugador controla a un cadete que recorre realidades alternativas, investiga anomalías, recupera recursos y completa misiones con ayuda de IRIS.

## Abrir desde GitHub

**Requisitos:** Windows, Visual Studio 2022 versión 17.8 o posterior, carga de trabajo **Desarrollo de escritorio de .NET** y SDK de .NET 8.

1. Clona este repositorio desde GitHub con **Code → Clone**, o ejecuta:
   ```powershell
   git clone https://github.com/estjubalabnerserranoni-bit/NEXUS.git
   cd NEXUS
   ```
2. Abre `NEXUS.sln` desde la raíz del repositorio.
3. Espera la restauración de paquetes NuGet.
4. Establece `NEXUS.WinForms` como proyecto de inicio.
5. Pulsa F5 para abrir el menú principal.

También puedes compilar y probar desde PowerShell:

```powershell
dotnet restore NEXUS.sln
dotnet build NEXUS.sln --configuration Release
dotnet run --project tests/NEXUS.Tests/NEXUS.Tests.csproj --configuration Release
```

NEXUS usa Windows Forms y se ejecuta en Windows. SQL Server no es obligatorio para iniciar una partida local.

## Experiencia de juego implementada

- Menú principal con portal dimensional, estética azul/cian y botones luminosos.
- Registro del cadete, selección de realidad Alfa/Beta y tres ranuras.
- Escena 2D alienígena dibujada por código, con planeta, torres, agua, plataformas y rutas holográficas.
- Cadete visible con animación básica de caminata entre sectores.
- Movimiento con flechas o WASD; también se puede hacer clic en un sector vecino.
- Dos realidades con progreso separado y mapas de nueve sectores.
- Energía, estabilidad, escáner normal/crítico, recursos y núcleo dimensional.
- Campaña inicial de cinco misiones con objetivos y recompensas.
- Inventario, mensajes de IRIS y guardado automático local.

## Controles

| Teclas / acción | Resultado |
|---|---|
| Flechas | Caminar a un sector vecino. |
| W, A, S, D | Caminar a un sector vecino. |
| Clic sobre un sector | Seleccionarlo y caminar si está conectado directamente. |
| Pestaña Exploración | Mapa y acciones de exploración. |
| Pestaña Misiones | Consultar objetivos y avanzar. |
| Pestaña Inventario | Revisar y usar celdas de energía. |

Cada viaje cuesta 10 puntos de energía. El escáner normal cuesta 5 y el crítico 20. El cambio entre realidades se realiza desde la base.

## Organización del repositorio

| Ruta | Responsabilidad |
|---|---|
| `NEXUS.sln` | Solución principal de Visual Studio. |
| `NEXUS.WinForms/Forms` | Menú y pantallas del juego. |
| `NEXUS.WinForms/Controls` | Menú dibujado, botones neon y escena 2D. |
| `NEXUS.WinForms/Models` | Cadete, partida, mapas, misiones e inventario. |
| `NEXUS.WinForms/Services` | Reglas del juego y campaña. |
| `NEXUS.WinForms/Data` | Guardado JSON y acceso SQL Server. |
| `database` | Script de SQL Server y guía de conexión. |
| `tests` | Pruebas automatizadas. |
| `docs` | Diseño, arquitectura y guías. |

## Persistencia

El guardado local usa tres ranuras JSON en la carpeta de datos de usuario de Windows. La integración con SQL Server está preparada en `SqlServerRepository`; para activarla, ejecuta `database/01-esquema.sql` y configura la variable de entorno `NEXUS_SQLSERVER_CONNECTION`. Los pasos están en [database/README.md](database/README.md). No se publican credenciales en GitHub.

## Pruebas automáticas

GitHub Actions restaura paquetes, compila los proyectos y ejecuta pruebas de las reglas del juego en Windows después de cada actualización de `main` o pull request. Consulta la pestaña **Actions** del repositorio.

## Documentación

- [Diseño del videojuego](docs/diseno-del-juego.md)
- [Estado de implementación](docs/estado-implementacion.md)
- [Arquitectura](docs/arquitectura-propuesta.md)
- [Modelo de datos SQL Server](docs/modelo-de-datos-propuestos.md)
- [Guía para clonar y ejecutar](docs/guia-clonado-y-ejecucion.md)

## Mejoras futuras

Se propone ampliar las ilustraciones originales por sector y realidad, incorporar música y efectos de sonido, añadir animaciones de portal, anomalías y objetos, y desarrollar nuevas campañas después de la historia inicial de cinco misiones. La versión actual ya tiene una escena alienígena dibujada por código y una animación básica de caminata; los efectos avanzados y las campañas adicionales siguen pendientes.
