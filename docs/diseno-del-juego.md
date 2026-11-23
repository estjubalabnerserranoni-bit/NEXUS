# NEXUS — Diseño del videojuego

**Estudiante:** Jubal Abner Serrano Nina  
**Proyecto:** NEXUS  
**Versión de partida:** aplicación de consola en C#  
**Documento:** Propuesta de experiencia de usuario para la evolución gráfica
---
1. Visión General del Juego
Ambientación: Año 2297. El jugador es un cadete explorador que investiga anomalías en dimensiones desconocidas.

Pilares de la experiencia:

Descubrimiento: Explorar zonas y revelar anomalías ocultas.
Decisiones: Gestionar cuándo avanzar, gastar energía o retroceder para proteger el progreso.
Progreso: Completar misiones, registrar realidades y desbloquear nuevos objetivos.
Sistema de Apoyo: IRIS actúa como la inteligencia artificial que emite alertas y mensajes de estado críticos.

 2. Ciclo Principal de Juego

El flujo de una partida sigue un ciclo estructurado:
Inicio: Crear un nuevo perfil de cadete o cargar una partida guardada.
Planificación: Consultar la misión activa, el estado actual y el mapa disponible.
Exploración: Seleccionar una zona y verificar si hay energía suficiente para la acción.
Interacción: Explorar la zona, actualizar recursos (energía/estabilidad) y reunir hallazgos.
Cierre: Al cumplir el objetivo, se otorga la recompensa, se guarda el progreso y se avanza a la siguiente misión o al menú.

3. Mecánicas Clave

Cadete: Muestra nombre, energía actual, estabilidad de la realidad y misión activa de forma persistente en pantalla.
Mapas y Zonas: Las realidades se clasifican en Disponible, En exploración, Explorada o Bloqueada.
Recursos (Energía y Estabilidad): Las acciones consumen energía (con validación previa estricta). La estabilidad mide la seguridad de la realidad y avisa de riesgos críticos mediante IRIS. La recarga debe ser una acción explícita.
Misiones e Inventario: Gestión de misiones con estados (Pendiente, En progreso, Completada) y un inventario básico para consultar hallazgos u objetos consumibles.

4. Pantallas y Navegación Propuestas

El diseño visual se estructurará en Windows Forms mediante wireframes claros y consistentes:
Menú Principal: Nueva partida, continuar, perfiles, configuración y salida.
Pantalla de Exploración / Mapa: Panel central con indicadores de recursos, mapa en 2D y controles de acción.
Pantalla de Misiones: Listado de objetivos, descripciones detalladas y estados.
Exploradores e Inventario: Visualización del cadete, objetos encontrados y opciones de uso.

5. Alcance y Restricciones

Prioridad de la 1ª Etapa Gráfica:
Mantener la base lógica del código original de C# (H3).
Crear la interfaz en Windows Forms con el flujo central operativo (perfil, mapa, validación de energía, misiones básicas e inventario).
Separar la lógica del juego de los eventos visuales de los botones.
Fuera del Alcance Inicial (Futuro): Animaciones complejas, efectos de sonido avanzados, generación procedural avanzada, IA compleja de enemigos o multijugador.
Seguridad y Persistencia: El uso de SQL Server y el guardado de partidas se integrarán de forma robusta en una fase posterior, evitando credenciales hardcodeadas y manejando errores de conexión de manera segura.

 
## 6. Estado de implementación de la propuesta

La primera versión gráfica organizada se encuentra en la solución `NEXUS.sln` y el proyecto `NEXUS.WinForms`. Esta versión incluye un menú de acceso, tres ranuras de guardado, mapa de nueve sectores, progreso separado para Alfa y Beta, pantallas de exploración, misiones e inventario y mensajes del sistema IRIS.

La campaña inicial incluye cinco misiones: Primer contacto, Señal desconocida, Núcleo perdido, Ruptura dimensional y El origen de NEXUS. Las misiones tienen objetivos concretos, como recuperar recursos, analizar sectores específicos, realizar una lectura crítica, purgar el núcleo y regresar a la base.

El guardado local en JSON está implementado. También se añadió el acceso a SQL Server mediante `SqlServerRepository` y el script `database/01-esquema.sql`. La conexión real necesita que cada computadora ejecute el script y configure su propia variable de entorno; no se incluye una contraseña en el repositorio.

## 7. Funciones para futuras versiones

La versión actual es una primera campaña jugable y una base de Windows Forms. Todavía se pueden mejorar los gráficos del mapa, agregar ilustraciones de las realidades, incorporar efectos de sonido, aumentar el catálogo de objetos y crear nuevas campañas. Estas mejoras no deben confundirse con funciones ya terminadas.

Para abrir el proyecto se debe utilizar `NEXUS.sln` desde Visual Studio 2022 17.8 o posterior, con .NET 8 y la carga de trabajo de desarrollo de escritorio. La guía completa está en `docs/guia-clonado-y-ejecucion.md`.
