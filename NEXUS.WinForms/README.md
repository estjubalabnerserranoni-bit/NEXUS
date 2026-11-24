# NEXUS — videojuego Windows Forms

Aplicación gráfica de exploración dimensional en C# para Windows.

## Requisitos

- Windows 10/11.
- Visual Studio 2022 versión 17.8 o posterior.
- Carga de trabajo **Desarrollo de escritorio de .NET**.
- SDK de .NET 8.

## Ejecutar

Abre `NEXUS.sln` desde la raíz del repositorio. Establece `NEXUS.WinForms` como proyecto de inicio y pulsa F5.

## Funciones actuales

- Menú con portal dimensional y botones futuristas.
- Registro del cadete, selección de realidad y tres ranuras.
- Escena 2D alienígena dibujada en C#.
- Personaje visible con animación de caminata.
- Movimiento con flechas, W, A, S, D y clic en sectores vecinos.
- Realidades Alfa y Beta, mapa de nueve sectores y rutas.
- Energía, estabilidad, anomalías, recursos y núcleo.
- Campaña inicial de cinco misiones.
- Inventario, mensajes IRIS y guardado local.
- Conexión opcional preparada para SQL Server.

## Carpetas

- `Controls`: escena 2D, menú, botones y dibujo.
- `Models`: cadete, partida, mapa, misión e inventario.
- `Services`: reglas del juego y campaña.
- `Forms`: menú y pantallas.
- `Data`: guardado JSON y SQL Server.

## Mejoras futuras

Se proponen ilustraciones originales detalladas por sector, banda sonora y efectos de sonido, animaciones avanzadas y nuevas campañas. La escena ya tiene arte vectorial y el cadete tiene animación básica de caminata; los efectos y campañas ampliadas quedan para posteriores versiones.

SQL Server no es obligatorio para jugar localmente. Para configurarlo, sigue `database/README.md`.
