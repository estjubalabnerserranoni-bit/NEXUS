# NEXUS — Diseño del videojuego

**Proyecto:** NEXUS — Exploration / Reality System  
**Género:** Exploración y ciencia ficción 2D  
**Plataforma:** Windows  
**Tecnología:** C#, Windows Forms y SQL Server (persistencia opcional)

## 1. Visión del juego

Año 2297. Las realidades conocidas han comenzado a cambiar y se detectan anomalías dimensionales en distintos sectores. El jugador es un cadete explorador que debe recorrer estas realidades, recuperar recursos y analizar anomalías para proteger la estabilidad del sistema NEXUS.

IRIS es el sistema de apoyo que acompaña al cadete. Informa sobre riesgos, energía, objetivos de misión y resultados de las acciones.

La experiencia se apoya en cuatro pilares:

- **Exploración:** recorrer sectores conectados y descubrir recursos o anomalías.
- **Decisiones:** administrar la energía y decidir cuándo analizar, regresar o cambiar de realidad.
- **Progreso:** completar misiones, obtener recompensas y desbloquear la siguiente parte de la campaña.
- **Investigación:** registrar muestras y utilizar el inventario para mantener al cadete preparado.

## 2. Ciclo principal de juego

1. **Inicio:** el jugador escribe el nombre del cadete, elige una realidad inicial y una ranura.
2. **Planificación:** consulta la misión activa y el estado del cadete.
3. **Exploración:** camina a sectores vecinos mediante las flechas, WASD o clic del ratón.
4. **Interacción:** recupera recursos, examina anomalías y administra energía y estabilidad.
5. **Regreso:** vuelve a la base para recargar, purgar el núcleo o completar el objetivo.
6. **Progresión:** recibe la recompensa, guarda la partida y activa la siguiente misión.

## 3. Mecánicas y reglas

### Explorador

El cadete tiene nombre, realidad actual, energía, estabilidad, nivel de acceso y un inventario. Se representa dentro de la escena 2D y cuenta con una animación básica de caminata cuando se desplaza entre sectores.

### Mapas y realidades

La campaña inicial contiene dos realidades:

- **Alfa:** base central, depósito de energía, pasajes y anomalía central.
- **Beta:** base central, anomalías y un depósito de energía.

Cada realidad tiene nueve sectores en una cuadrícula 3×3. El cadete solo puede moverse a un sector vecino. El avance de las zonas visitadas y exploradas se conserva por separado para Alfa y Beta.

### Energía

Cada viaje cuesta 10 puntos. Un escaneo normal cuesta 5 y uno crítico cuesta 20. Si el cadete no tiene energía suficiente, la acción se rechaza sin descontar energía. La base permite recargar y el inventario incluye celdas que recuperan energía.

### Estabilidad y núcleo

Un escaneo crítico reduce 20 puntos de estabilidad y vuelve inestable el núcleo. El cadete debe regresar a la base y ejecutar la purga para recuperar estabilidad y completar la misión que lo requiera.

### Misiones

La campaña inicial incluye cinco misiones con requisitos y recompensas:

| Código | Misión | Objetivos principales |
|---|---|---|
| MIS-001 | Primer contacto | Recuperar una celda en Alfa y analizar la anomalía central. |
| MIS-002 | Señal desconocida | Investigar la señal del sector norte de Beta. |
| MIS-003 | Núcleo perdido | Recuperar el recurso del depósito de Beta. |
| MIS-004 | Ruptura dimensional | Analizar dos anomalías de Beta, realizar una lectura crítica y purgar el núcleo. |
| MIS-005 | El origen de NEXUS | Regresar a Alfa, visitar la anomalía central y volver a la base. |

### Inventario

Los objetos de la versión actual son la celda de energía, que puede consumirse para recuperar 25 puntos, y la muestra de anomalía, que se conserva como material de investigación.

## 4. Interfaz y navegación

### Menú principal

La portada utiliza una paleta oscura azul/cian, un portal dimensional dibujado y botones poligonales luminosos. Desde allí se puede iniciar o continuar una partida, elegir realidad, configurar la conexión SQL, consultar información del juego y salir.

### Pantalla de exploración

La escena presenta un paisaje alienígena con planeta, estructuras, agua luminosa, plataformas y rutas holográficas. Los sectores se muestran como nodos conectados. El cadete aparece sobre el mapa y camina entre nodos al desplazarse.

### Misiones e inventario

Las pestañas permiten revisar los objetivos de campaña y los objetos. Los controles de energía, análisis, purga y cambio de realidad se activan según la situación del cadete.

## 5. Persistencia y arquitectura

La aplicación separa modelos, reglas, interfaz y acceso a datos:

- `Models`: cadete, partida, mapas, misiones e inventario.
- `Services`: movimiento, análisis, recursos, energía y campaña.
- `Forms`: menú y pantallas.
- `Controls`: dibujo de la portada, botones y escena 2D.
- `Data`: guardado JSON y repositorio SQL Server.

El guardado local dispone de tres ranuras. SQL Server está preparado mediante el script `database/01-esquema.sql` y la clase `SqlServerRepository`. La conexión necesita configuración en cada equipo mediante `NEXUS_SQLSERVER_CONNECTION`; por eso no se incluyen contraseñas en el repositorio.

## 6. Mejoras futuras

Las siguientes son propuestas para versiones posteriores, no funciones terminadas de la versión actual:

1. Crear ilustraciones originales detalladas para cada sector, bioma, anomalía y realidad.
2. Añadir música ambiental y efectos de sonido para pasos, escáner, portales, interfaz y alertas de IRIS.
3. Ampliar las animaciones actuales con ciclos completos de caminata, interacciones, partículas, anomalías y transiciones dimensionales.
4. Desarrollar nuevas campañas después de MIS-005 con historias, mapas, objetivos y recompensas adicionales.
5. Incorporar más objetos, peligros ambientales, criaturas, estadísticas y logros.

La versión actual ya incluye una escena 2D dibujada por código y una animación básica de caminata. Las mejoras futuras ampliarían esa base sin sustituir las reglas centrales ya implementadas.

## 7. Cómo ejecutar

Abrir `NEXUS.sln` desde Visual Studio 2022 versión 17.8 o posterior, con .NET 8 y la carga de trabajo Desarrollo de escritorio de .NET. La guía paso a paso se encuentra en [guia-clonado-y-ejecucion.md](guia-clonado-y-ejecucion.md).
