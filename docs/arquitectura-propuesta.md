# Arquitectura propuesta

La evolución se hará sin rehacer NEXUS desde cero. Se conservarán las reglas de `Explorador` y `Mision`, separándolas de la interfaz y del guardado de datos.

## 1. Partes del programa

1. **Interfaz (Windows Forms):** mostrará el menú, exploración, misiones y datos del explorador. Una ventana principal puede cambiar entre paneles.
2. **Lógica del juego:** contendrá las clases `Explorador` y `Mision` y las reglas de energía, exploración y estados. Los botones llamarán a esta lógica, sin decidir las reglas por su cuenta.
3. **Acceso a datos:** tendrá clases sencillas para guardar y recuperar el progreso desde SQL Server. La interfaz no ejecutará consultas directamente.

## 2. Funcionamiento de la arquitectura

El programa estará organizado en cuatro partes que trabajarán de manera conectada. Primero, el usuario interactuará con la interfaz de Windows Forms, donde podrá acceder al menú, mapa, misiones e inventario. Después, las acciones realizadas pasarán 
a la lógica del juego, donde las clases `Explorador` y `Mision` se encargarán de aplicar las reglas correspondientes. A continuación, el acceso a datos gestionará la información que se desea guardar o recuperar. Finalmente, los datos se almacenarán en SQL Server para que el progreso pueda mantenerse.

## 3. Cómo se relaciona con H3

`Explorador` seguirá controlando nombre, energía, nivel de acceso y realidad actual. `Mision` seguirá controlando código, descripción y estado. `Program.Main`, que hoy coordina toda la demostración, se reducirá y la coordinación pasará a la ventana principal y a las clases de lógica.

Se agregarán formularios o paneles para mostrar los datos, y una capa de acceso a datos para guardar el progreso. Estas partes son nuevas y se implementarán después; no están presentes en la versión de consola revisada.

## 4. Orden recomendado

1. Crear el proyecto Windows Forms y mostrar una ventana principal.
2. Reutilizar `Explorador` y `Mision` y probar las acciones desde botones.
3. Agregar las pantallas de misión y exploración.
4. Crear la base de datos y conectar el guardado/cargado.
5. Probar que el progreso siga disponible después de cerrar y abrir el juego.
