# Arquitectura actual de NEXUS

## 1. Archivos revisados

En el archivo comprimido compartido se identificaron estos archivos principales:

- `ConsoleApp1.slnx`: solución de Visual Studio.
- `ConsoleApp1/ConsoleApp1.csproj`: proyecto C# configurado como aplicación de consola para `net10.0`.
- `ConsoleApp1/Program.cs`: contiene las clases y el flujo principal del programa.

## 2. Clases y responsabilidades

**`Explorador`**  
Guarda `Nombre`, `Energia`, `NivelAcceso` y `RealidadActual`. Incluye los métodos `ExplorarRealidad`, `RecargarEnergia` y `MostrarEstado`. La propiedad de energía limita el valor entre 0 y 100.

**`Mision`**  
Guarda `CodigoMision`, `Descripcion` y `Estado`. Sus métodos `IniciarMision` y `CompletarMision` cambian el estado y muestran mensajes en consola.

**`Program`**  
El método `Main` inicia la demostración, crea dos exploradores (`Kaelen` y `Lyra`), crea la misión `MIS-001` y ejecuta una secuencia de exploración, recarga y finalización.

## 3. Funcionamiento actual

El programa funciona por consola. Los datos de ejemplo están escritos directamente en `Main`; la demostración sigue una secuencia preparada y utiliza mensajes de texto para mostrar los resultados. La lógica básica de energía y misión está dentro de las clases correspondientes.

## 4. Limitaciones observadas

- No hay ventanas, botones ni mapa gráfico.
- No hay conexión con SQL Server ni guardado permanente de partidas.
- No se encontró una clase o sistema de inventario en el código revisado.
- La demostración usa datos fijos y no presenta un menú para jugar libremente.
- No se observó una clase separada para administrar varias misiones o zonas.

Esta descripción corresponde al `Program.cs` incluido en `NEXUS.V1.rar`. Antes de cambiar el código, se debe comprobar que el repositorio de GitHub tenga la misma versión.
