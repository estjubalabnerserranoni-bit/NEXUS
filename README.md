# NEXUS — Exploration / Reality System

Videojuego de exploración dimensional desarrollado en C# con Windows Forms. El jugador controla a un cadete que investiga anomalías, recupera recursos, administra energía y estabilidad y cumple una campaña inicial de cinco misiones con ayuda de IRIS.

## Abrir y ejecutar desde GitHub

**Requisito:** Windows, Visual Studio 2022 versión 17.8 o posterior, carga de trabajo **Desarrollo de escritorio de .NET** y SDK de .NET 8.

1. Abre este repositorio en GitHub.
2. Pulsa **Code → Clone** y copia la dirección HTTPS, o usa:
   ```powershell
   git clone https://github.com/estjubalabnerserranoni-bit/NEXUS.git
   cd NEXUS
   ```
3. Abre el archivo `NEXUS.sln` que está en la raíz del repositorio.
4. Espera a que Visual Studio restaure los paquetes NuGet.
5. En el selector de inicio, elige `NEXUS.WinForms`.
6. Pulsa **F5** o el botón **Iniciar**.

También se puede compilar desde una terminal con .NET 8:

```powershell
dotnet restore NEXUS.sln
dotnet build NEXUS.sln --configuration Release
dotnet run --project tests/NEXUS.Tests/NEXUS.Tests.csproj --configuration Release
```

NEXUS utiliza Windows Forms, por lo que la aplicación gráfica se ejecuta en Windows. No requiere SQL Server para iniciar una partida local.

## Funciones incluidas

- Menú principal con identificación del cadete, selección de realidad y tres ranuras de partida.
- Nueva partida, continuación y confirmación antes de reemplazar un guardado.
- Realidades Alfa y Beta con progreso independiente.
- Mapa de nueve sectores y desplazamiento entre sectores vecinos.
- Energía, estabilidad y núcleo dimensional.
- Escáner de anomalías con lecturas normales y críticas.
- Inventario con celdas de energía y muestras de anomalía.
- Cinco misiones con objetivos, requisitos y recompensas de progresión.
- Cambio de realidad desde la base central.
- Alertas y mensajes de IRIS.
- Guardado local automático en JSON.
- Persistencia opcional en SQL Server.

## Tecnologías

- C# y Windows Forms.
- .NET 8 para Windows.
- SQL Server y ADO.NET mediante `Microsoft.Data.SqlClient` 7.1.2.
- GitHub Actions para compilación y pruebas automáticas.

## Organización

| Ruta | Contenido |
|---|---|
| `NEXUS.WinForms/Forms` | Menú principal y pantallas del juego. |
| `NEXUS.WinForms/Models` | Cadete, partida, misiones, inventario y mapas. |
| `NEXUS.WinForms/Services` | Reglas de movimiento, energía, anomalías y campaña. |
| `NEXUS.WinForms/Data` | Guardado local y repositorio SQL Server. |
| `database` | Script de creación del esquema SQL Server y guía de conexión. |
| `tests` | Pruebas automáticas de las reglas principales. |
| `docs` | Diseño, arquitectura, análisis y guía de uso. |

## SQL Server (opcional)

1. Instala SQL Server y SQL Server Management Studio.
2. Ejecuta `database/01-esquema.sql`.
3. Configura la variable de entorno `NEXUS_SQLSERVER_CONNECTION`. Los ejemplos están en `database/README.md`.
4. Abre Visual Studio desde la terminal donde configuraste esa variable.
5. Usa **Configuración SQL** en el menú para probar la conexión.

El guardado local funciona incluso si SQL Server no está configurado. El botón **Guardar partida** guarda localmente y, cuando existe una conexión configurada, también guarda en SQL Server. La opción **Continuar partida** busca primero el guardado local y, si no existe, puede recuperar la partida SQL usando el nombre del cadete.

No guardes contraseñas ni cadenas privadas en el repositorio.

## Verificación automática

El flujo de GitHub Actions `.github/workflows/compilar-nexus.yml` restaura paquetes, compila la solución y ejecuta las pruebas de reglas del juego en Windows cada vez que se actualiza `main` o se crea un pull request.

Puedes consultar el resultado en la pestaña **Actions** del repositorio.

## Documentación

- [Diseño del videojuego](docs/diseno-del-juego.md)
- [Arquitectura actual](docs/arquitectura-actual.md)
- [Arquitectura propuesta e implementación](docs/arquitectura-propuesta.md)
- [Modelo de datos SQL Server](docs/modelo-de-datos-propuestos.md)
- [Estado de implementación](docs/estado-implementacion.md)
- [Guía para clonar y ejecutar](docs/guia-clonado-y-ejecucion.md)
