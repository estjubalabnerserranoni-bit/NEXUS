# Arquitectura de NEXUS

NEXUS se organiza en capas para mantener separadas la interfaz, las reglas del juego y el almacenamiento.

## 1. Interfaz — Windows Forms

La carpeta `NEXUS.WinForms/Forms` contiene `FormularioPrincipal`, que construye el menú principal, la exploración, las misiones y el inventario. Los botones llaman a los servicios del juego y muestran sus resultados.

## 2. Modelos

La carpeta `Models` contiene las clases que representan los datos:

- `Explorador`: nombre, realidad, energía, estabilidad, nivel y objetos.
- `Mision`: objetivo, requisitos, progreso, recompensa y estado.
- `Partida`: ranura, campaña y progreso del cadete.
- `ProgresoMapa`: posición, sectores visitados y explorados por realidad.
- `ObjetoInventario`: nombre, descripción y cantidad.

## 3. Lógica del juego

La carpeta `Services` contiene las reglas:

- `JuegoService`: movimiento entre sectores, costos de energía, análisis, recarga, purga y cambio de realidad.
- `CampanaService`: configuración de las cinco misiones iniciales.

Los formularios no deciden cuánto cuesta una acción ni cuándo se completa una misión. Solicitan la operación al servicio y muestran el resultado.

## 4. Acceso a datos

La carpeta `Data` separa dos formas de persistencia:

- `GuardadoLocal`: guarda y recupera tres ranuras JSON en el equipo del usuario.
- `SqlServerRepository`: guarda y recupera partidas en SQL Server, utiliza consultas parametrizadas y una transacción para mantener consistentes los datos relacionados.

La cadena de conexión SQL se obtiene de la variable de entorno `NEXUS_SQLSERVER_CONNECTION`; no está escrita en el código público.

## 5. Relación con el proyecto anterior

El archivo `program-cs` conserva el prototipo de Windows Forms como referencia. Las clases se reorganizaron para que sea más sencillo mantener el juego. La documentación de H3 sigue describiendo la versión de consola original; esta solución representa la etapa gráfica posterior.

## 6. Pruebas

`tests/NEXUS.Tests` verifica reglas importantes sin necesitar abrir la interfaz. GitHub Actions ejecuta estas pruebas y compila la solución en Windows cuando hay cambios en `main` o pull requests.

## 7. Orden de evolución

1. Mantener la solución compilable y probar las reglas.
2. Mejorar la presentación de pantallas y mapas.
3. Ampliar misiones y objetos.
4. Configurar y probar SQL Server en el equipo de desarrollo.
5. Incorporar efectos visuales y de sonido sin cambiar las reglas ya probadas.
