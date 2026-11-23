# Estado de implementación de NEXUS

Este documento distingue lo que ya existe en el repositorio de lo que todavía forma parte de la propuesta.

## 1. Código de partida

El archivo `program-cs` se conserva como referencia del prototipo anterior de Windows Forms. Contiene una ventana única, un mapa de nueve sectores, las realidades Alfa y Beta, energía, estabilidad, escaneo de anomalías, una misión inicial, mensajes de IRIS y guardado local en JSON.

La documentación de H3 describe además una aplicación de consola anterior. Ese código original no está incluido como proyecto completo dentro de la raíz actual del repositorio; por eso no se ha eliminado ni sustituido el archivo de referencia.

## 2. Nueva organización

La carpeta `NEXUS.WinForms` contiene el proyecto organizado que se debe abrir y ejecutar:

| Carpeta o archivo | Responsabilidad |
|---|---|
| `Models` | Explorador, misión, partida e inventario. |
| `Services` | Reglas de movimiento, exploración, escaneo, energía y estabilidad. |
| `Forms` | Ventana y controles de Windows Forms. |
| `Data` | Guardado y recuperación local de partidas. |
| `Program.cs` | Inicio de la aplicación. |

La solución `NEXUS.sln` permite abrir el proyecto desde la raíz del repositorio en Visual Studio.

## 3. Funciones disponibles en esta etapa

- Crear una partida con nombre de cadete y realidad Alfa o Beta.
- Recorrer un mapa de nueve sectores, avanzando solamente a sectores vecinos.
- Consumir 10 puntos de energía por viaje.
- Recuperar una celda de energía y una muestra de anomalía.
- Usar una celda del inventario para recuperar energía.
- Analizar anomalías con una frecuencia normal o crítica.
- Actualizar la estabilidad y el estado del núcleo.
- Recargar y purgar el núcleo al regresar a la base.
- Mostrar el estado y los objetivos de MIS-001.
- Recibir mensajes de ayuda y alertas mediante IRIS.
- Guardar automáticamente y continuar una partida desde un archivo JSON local.

## 4. Persistencia y trabajo pendiente

El guardado JSON es temporal y local al equipo donde se ejecuta el juego. Todavía no equivale a una base de datos compartida.

SQL Server sigue siendo la siguiente etapa de persistencia. Se deberá implementar a través de clases de acceso a datos separadas de los formularios y usar consultas parametrizadas. No hay conexión ni credenciales de SQL Server dentro de esta versión.

También quedan para etapas posteriores el menú principal independiente, varias misiones, un inventario más amplio y las funciones adicionales descritas en el diseño. La interfaz actual es una primera versión funcional para probar el ciclo central, no la versión final completa.

## 5. Cómo ejecutar

1. Abre `NEXUS.sln` con Visual Studio 2022 actualizado.
2. Comprueba que esté instalada la carga de trabajo **Desarrollo de escritorio de .NET** y el SDK de .NET 10.
3. Ejecuta el proyecto `NEXUS.WinForms` con F5.
4. Escribe el nombre del cadete y elige Alfa o Beta.
5. Usa el mapa para viajar, explorar y completar la misión inicial.

El proyecto está dirigido a Windows porque utiliza Windows Forms.
