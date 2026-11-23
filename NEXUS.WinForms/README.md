# NEXUS — Aplicación Windows Forms

Esta carpeta contiene la primera versión organizada de la interfaz gráfica de NEXUS. Conserva las mecánicas de exploración, energía, estabilidad, anomalías, misión e IRIS que ya estaban en el prototipo del repositorio.

## Requisitos

- Windows.
- Visual Studio 2022 actualizado con el SDK de .NET 10 y la carga de trabajo **Desarrollo de escritorio de .NET**.
- No necesita paquetes externos para ejecutar esta versión.

## Cómo abrir el proyecto

1. Descarga o clona el repositorio.
2. Abre la carpeta `NEXUS.WinForms`.
3. Abre el archivo `NEXUS.WinForms.csproj` con Visual Studio.
4. Espera a que Visual Studio cargue el proyecto.
5. Ejecuta con el botón **Iniciar** o la tecla F5.

## Qué incluye esta primera versión

- Registro del nombre del cadete y selección de realidad Alfa o Beta.
- Mapa de nueve sectores con movimiento entre sectores vecinos.
- Costo de 10 puntos de energía por viaje.
- Recolección de una celda de energía.
- Análisis de anomalías con frecuencia normal o crítica.
- Control de energía, estabilidad y estado del núcleo.
- Misión MIS-001 con objetivos y recompensa de nivel de acceso.
- Inventario básico con uso de celdas de energía.
- Mensajes y alertas de IRIS.
- Guardado y continuación local de la partida en formato JSON.

## Guardado y SQL Server

Por ahora, el guardado se realiza en un archivo JSON dentro de la carpeta local de datos del usuario de Windows. Es una solución provisional para probar el ciclo de juego y continuar partidas.

**SQL Server todavía no está conectado.** La conexión con SQL Server se debe implementar en la siguiente etapa mediante una capa de acceso a datos, como indican los documentos de arquitectura del repositorio. No se guarda ninguna contraseña ni cadena de conexión en el código público.

## Organización del código

- `Models`: explorador, misión, partida e inventario.
- `Services`: reglas de movimiento, energía, exploración y anomalías.
- `Forms`: pantallas y controles de Windows Forms.
- `Data`: guardado y carga local provisional.
- `Program.cs`: punto de inicio de la aplicación.

El código anterior del repositorio, guardado como `program-cs`, se conserva como referencia. El proyecto organizado que se debe abrir y ejecutar está dentro de esta carpeta.
