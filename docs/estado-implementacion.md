# Estado de implementación de NEXUS

Este documento separa las funciones que ya están en el código de las ampliaciones que se proponen para versiones posteriores.

## 1. Proyecto que debe abrirse

La solución principal es `NEXUS.sln`, en la raíz. Contiene:

- `NEXUS.WinForms`: videojuego Windows Forms para .NET 8.
- `NEXUS.Tests`: pruebas automatizadas de las reglas principales.

El archivo `program-cs` se conserva como referencia del prototipo anterior. Para ejecutar el videojuego actual se debe abrir `NEXUS.sln`.

## 2. Funciones implementadas en esta versión

- Menú de inicio NEXUS con portal dimensional dibujado, colores azul/cian y botones de estilo futurista.
- Registro del cadete, selección de realidad Alfa/Beta y tres ranuras locales.
- Nueva partida, continuación y confirmación antes de reemplazar una ranura.
- Escena de exploración 2D dibujada en C# con paisaje alienígena, planeta, estructuras, agua luminosa, plataformas y nodos de mapa.
- Personaje cadete visible, con animación de caminata entre sectores.
- Movimiento mediante flechas o teclas W, A, S, D.
- Selección y desplazamiento mediante clic sobre sectores conectados.
- Mapa 3×3 con sectores de base, pasajes, recursos y anomalías.
- Realidades Alfa y Beta con progreso y ambientación cromática diferenciados.
- Energía, estabilidad, núcleo dimensional y costos de viaje.
- Escáner normal y crítico con efectos diferentes.
- Recuperación de recursos, inventario y uso de celdas energéticas.
- Recarga y purga del núcleo en la base.
- Campaña inicial de cinco misiones con objetivos, sectores requeridos y recompensas.
- Mensajes de apoyo de IRIS.
- Guardado automático local en JSON con tres ranuras y compatibilidad básica con el guardado anterior.
- Acceso a SQL Server preparado mediante `SqlServerRepository`, script de base de datos y cadena de conexión configurada por variable de entorno.
- Pruebas automáticas de las reglas principales y compilación mediante GitHub Actions.

## 3. Controles del videojuego

| Control | Acción |
|---|---|
| Flechas | Caminar a un sector vecino. |
| W, A, S, D | Caminar a un sector vecino. |
| Clic en un sector | Seleccionarlo y caminar si es vecino del sector actual. |
| Pestaña Exploración | Mapa, escáner, energía y estabilidad. |
| Pestaña Misiones | Consultar objetivos y avanzar al completar. |
| Pestaña Inventario | Consultar objetos y consumir celdas. |

Cada viaje cuesta 10 puntos de energía. El análisis normal cuesta 5 y el crítico cuesta 20. Los viajes entre realidades se realizan desde la base y siguen el objetivo de la misión activa.

## 4. Persistencia

El guardado JSON se almacena en la carpeta local de datos de usuario de Windows y no se sube al repositorio. SQL Server está preparado como una opción adicional: es necesario ejecutar `database/01-esquema.sql` y configurar `NEXUS_SQLSERVER_CONNECTION` en cada equipo que vaya a conectarse al servidor.

La aplicación funciona localmente aunque SQL Server no esté configurado.

## 5. Mejoras futuras propuestas

Las siguientes mejoras siguen siendo trabajo futuro y no deben presentarse como terminadas:

1. **Ilustraciones propias por sector:** reemplazar progresivamente los elementos vectoriales generales por arte original detallado para cada mapa, bioma, anomalía y realidad.
2. **Sonido:** incorporar música ambiental, pasos, interfaz, escáner, portales, alertas de IRIS y efectos de misión.
3. **Animaciones avanzadas:** ampliar la caminata actual con ciclos completos, transiciones de portal, partículas, efectos de anomalías y animaciones de interacción.
4. **Nuevas campañas:** crear nuevas historias después de la campaña inicial de cinco misiones, con mapas, personajes, objetivos y recompensas.
5. **Más jugabilidad:** incorporar criaturas, peligros ambientales, objetos con nuevas funciones, logros y estadísticas.
6. **Personalización:** permitir elegir apariencia y equipamiento del cadete.

## 6. Ejecución

Consulta [Guía para clonar y ejecutar](guia-clonado-y-ejecucion.md). Se requiere Windows, Visual Studio 2022 17.8 o posterior, SDK de .NET 8 y la carga de trabajo Desarrollo de escritorio de .NET.
