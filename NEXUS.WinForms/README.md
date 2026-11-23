# NEXUS — Windows Forms

Este proyecto es la aplicación gráfica del videojuego NEXUS.

## Requisitos

- Windows 10/11.
- Visual Studio 2022 versión 17.8 o posterior.
- Carga de trabajo **Desarrollo de escritorio de .NET**.
- SDK de .NET 8.

## Ejecutar

Se recomienda abrir `NEXUS.sln` desde la raíz del repositorio. En Visual Studio, establece `NEXUS.WinForms` como proyecto de inicio y pulsa F5.

## Funciones

- Menú principal con tres ranuras de partida.
- Realidades Alfa y Beta.
- Mapa de nueve sectores y viajes entre sectores vecinos.
- Energía, estabilidad y núcleo.
- Escaneo de anomalías normal y crítico.
- Inventario con objetos consumibles y materiales.
- Campaña de cinco misiones.
- Alertas de IRIS.
- Guardado local automático en JSON.
- Repositorio opcional de SQL Server.

## Estructura

- `Models`: explorador, partida, mapas, misiones e inventario.
- `Services`: reglas del juego y campaña.
- `Forms`: menú y pantallas Windows Forms.
- `Data`: guardado JSON y SQL Server.

SQL Server no es necesario para iniciar el juego. Para activarlo, ejecuta `database/01-esquema.sql` y configura `NEXUS_SQLSERVER_CONNECTION` como explica `database/README.md`.
