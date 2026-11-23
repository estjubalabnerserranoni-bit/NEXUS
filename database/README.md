# Base de datos de NEXUS

## Crear el esquema

1. Instala SQL Server y SQL Server Management Studio (SSMS).
2. Abre SSMS y conéctate a tu instancia.
3. Abre `01-esquema.sql`.
4. Ejecuta el script completo. Se creará la base de datos `NEXUS_DB` y sus tablas.

## Configurar la conexión sin publicar contraseñas

La aplicación lee la cadena de conexión de la variable de entorno:

`NEXUS_SQLSERVER_CONNECTION`

Ejemplo para autenticación de Windows en PowerShell:

```powershell
$env:NEXUS_SQLSERVER_CONNECTION = "Server=localhost;Database=NEXUS_DB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
```

Ejemplo para una instancia SQL Express:

```powershell
$env:NEXUS_SQLSERVER_CONNECTION = "Server=localhost\SQLEXPRESS;Database=NEXUS_DB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;"
```

La opción `TrustServerCertificate=True` se muestra solo como ejemplo para una instancia local de desarrollo. En un servidor real se debe configurar un certificado válido y no desactivar su verificación.

Después, inicia Visual Studio desde esa misma ventana de PowerShell para que herede la variable. No subas cadenas con usuario o contraseña a GitHub.

## Tablas

- `Explorador`: perfiles.
- `Partida`: ranura, recursos y progreso serializado.
- `Mision`: catálogo de misiones.
- `ProgresoMision`: estado de cada misión dentro de una partida.
- `Inventario`: objetos y cantidades.

La aplicación incluye `SqlServerRepository` con consultas parametrizadas y transacciones. El guardado local JSON continúa disponible cuando no hay un servidor configurado.
