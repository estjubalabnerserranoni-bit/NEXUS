# Análisis técnico

## 1. Tecnologías evaluadas

**Windows Forms:** es la opción propuesta porque la actividad pide una aplicación de escritorio y permite crear ventanas con controles como botones, etiquetas, listas y paneles. Para NEXUS es suficiente; no se necesita una interfaz 3D.

**WPF:** también permite crear aplicaciones gráficas para Windows y ofrece más opciones de presentación, pero agrega conceptos que no son necesarios para la primera versión del juego. Por eso no se elige para esta etapa.

**SQL Server:** se propone para guardar exploradores, partidas, misiones e inventario. Permite organizar la información en tablas relacionadas.

**Acceso desde C#:** se recomienda usar ADO.NET con `Microsoft.Data.SqlClient` para abrir conexiones y ejecutar consultas. Es una opción directa para un proyecto pequeño. Entity Framework Core podría evaluarse después, pero agrega otra herramienta que no es necesaria para comenzar.

## 2. Decisión recomendada

La propuesta será C# con Windows Forms para la interfaz y SQL Server para la persistencia. La lógica se mantendrá en clases como `Explorador` y `Mision`, y el acceso a datos estará separado en clases propias. Así será más fácil cambiar una pantalla sin modificar las reglas del juego.

## 3. Ventajas y límites

| Decisión | Ventaja | Límite |
|---|---|---|
| Windows Forms | Controles visuales conocidos y diseño sencillo en Visual Studio. | Está enfocado en aplicaciones de Windows. |
| SQL Server | Guarda datos relacionados y permite consultarlos después. | Requiere instalar/configurar el motor o disponer de una instancia. |
| ADO.NET / SqlClient | Acceso directo desde C# y control de las consultas. | Hay que escribir y mantener las consultas. |
| Separar capas | Evita mezclar botones, reglas y consultas. | Requiere organizar los archivos desde el inicio. |

## 4. Verificación de información consultada con IA

Las sugerencias sobre tecnologías se contrastaron con documentación oficial de Microsoft. La documentación confirma que Windows Forms es un marco para crear 
interfaces gráficas de escritorio para Windows. Microsoft también identifica `Microsoft.Data.SqlClient` como proveedor de .NET para conectarse y trabajar con SQL Server. La documentación de SQL Server explica el uso de claves primarias y foráneas para identificar registros y relacionar tablas.

La IA se utilizó como apoyo para ordenar y resumir alternativas. Las decisiones finales se compararon con las fuentes oficiales indicadas abajo. No se tomó una respuesta de IA como única prueba.

## 5. Fuentes técnicas

- Microsoft Learn, [Documentación de Windows Forms](https://learn.microsoft.com/dotnet/desktop/winforms/).
- Microsoft Learn, [Microsoft.Data.SqlClient para SQL Server](https://learn.microsoft.com/sql/connect/ado-net/).
- Microsoft Learn, [Claves principales y foráneas en SQL Server](https://learn.microsoft.com/sql/relational-databases/tables/primary-and-foreign-key-constraints).
- GitHub Docs, [Ignorar archivos con .gitignore](https://docs.github.com/en/get-started/git-basics/ignoring-files).

Consulta realizada el 9 de noviembre de 2026.
