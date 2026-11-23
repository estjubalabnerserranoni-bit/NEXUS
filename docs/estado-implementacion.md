# Estado de implementación de NEXUS

Este documento describe lo que está en el repositorio y separa las funciones implementadas de las que todavía requieren una ampliación.

## 1. Proyecto para abrir

La solución principal es `NEXUS.sln`, ubicada en la raíz. Contiene:

- `NEXUS.WinForms`: aplicación gráfica Windows Forms para .NET 8.
- `NEXUS.Tests`: programa de pruebas de las reglas principales.

El archivo `program-cs` se conserva como referencia del prototipo inicial y no es el proyecto que debe abrirse para ejecutar el videojuego.

## 2. Funciones implementadas

- Menú principal de acceso.
- Registro del nombre del cadete y selección de realidad inicial.
- Tres ranuras de guardado local, con confirmación antes de reemplazarlas.
- Continuar una partida guardada.
- Realidades Alfa y Beta con progreso independiente.
- Mapa de nueve sectores y movimiento únicamente entre sectores vecinos.
- Viajes con costo de energía y validación previa.
- Recuperación de celdas y muestras de anomalía.
- Análisis normal y crítico con diferentes costos y efectos sobre la estabilidad.
- Recarga y purga del núcleo en la base.
- Campaña de cinco misiones con objetivos por realidad, sectores y requisitos.
- Inventario básico con uso de celdas energéticas.
- Mensajes de IRIS.
- Guardado automático local en JSON.
- Repositorio SQL Server, script de tablas y prueba de conexión opcional.
- Pruebas automáticas para movimiento, energía, MIS-001, análisis crítico, campaña y migración de guardados antiguos.

## 3. Persistencia

El modo local guarda las partidas en la carpeta de datos de usuario de Windows, no dentro del repositorio. Esto evita subir partidas personales a GitHub.

La integración SQL está preparada mediante `SqlServerRepository`. Requiere ejecutar el script `database/01-esquema.sql` y configurar la variable de entorno `NEXUS_SQLSERVER_CONNECTION`. Sin esos pasos, el videojuego continúa funcionando con JSON local.

## 4. Funcionalidades que pueden ampliarse después

- Diseño visual con ilustraciones y animaciones propias.
- Sonidos ambientales y efectos de interfaz.
- Más tipos de objetos y efectos de inventario.
- Más mapas y nuevas campañas.
- Selección y administración visual de varias partidas.
- Mayor detalle visual de sectores bloqueados y descubiertos.
- Estadísticas, logros y niveles adicionales.

Estas funciones no son necesarias para compilar ni iniciar el proyecto actual.

## 5. Ejecución

Consulta [Guía para clonar y ejecutar](guia-clonado-y-ejecucion.md). El requisito mínimo del proyecto es Visual Studio 2022 17.8 o posterior con el SDK de .NET 8 y la carga de trabajo de escritorio de .NET.
