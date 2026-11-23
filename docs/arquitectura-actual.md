# Arquitectura y evolución actual de NEXUS

## 1. Código de partida

La primera versión documentada en H3 era una aplicación de consola en C#. Incluía las clases `Explorador` y `Mision` y una demostración coordinada desde `Program.Main`. El repositorio conserva el archivo `program-cs` como referencia del prototipo gráfico inicial que se preparó después.

## 2. Prototipo gráfico anterior

El archivo `program-cs` contiene una ventana única con mapa de nueve sectores, realidades Alfa y Beta, energía, estabilidad, análisis de anomalías, una misión inicial, mensajes de IRIS y guardado JSON en un único archivo.

Sus límites principales eran:

- Todas las clases y controles estaban en un mismo archivo.
- Solo existía una ranura local.
- La misión no formaba una campaña.
- El mapa no separaba por completo la progresión de cada realidad.
- La base de datos SQL Server no estaba conectada.

## 3. Solución organizada actual

La solución `NEXUS.sln` contiene el proyecto `NEXUS.WinForms` y el proyecto de pruebas `NEXUS.Tests`. El proyecto se dirige a `net8.0-windows`.

El código se separó en carpetas `Models`, `Services`, `Forms` y `Data`. La nueva interfaz incorpora menú principal, tres ranuras, campaña inicial de cinco misiones, inventario y guardado local.

## 4. Datos y persistencia

El guardado local usa JSON en la carpeta de datos de usuario de Windows. El sistema puede leer los campos principales del guardado del prototipo anterior y convertirlos al nuevo formato de campaña.

SQL Server se prepara por separado con el script de `database/01-esquema.sql` y `SqlServerRepository`. Para conectarlo hace falta configurar la variable de entorno indicada en `database/README.md`.

## 5. Validación

La compilación y las pruebas se ejecutan mediante el flujo de GitHub Actions. Esto permite detectar errores de compilación y de reglas básicas cada vez que se modifica el repositorio. Las pruebas no sustituyen la revisión manual de la interfaz en Windows.
