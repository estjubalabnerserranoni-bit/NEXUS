# Guía paso a paso: clonar, abrir y ejecutar NEXUS

Esta guía está pensada para una computadora con Windows y Visual Studio 2022.

## 1. Comprobar los requisitos

Abre Visual Studio Installer y verifica que tengas instalada la carga de trabajo **Desarrollo de escritorio de .NET**.

Usa Visual Studio 2022 versión 17.8 o posterior y el SDK de .NET 8. Puedes verificar el SDK desde PowerShell:

```powershell
dotnet --list-sdks
```

En la lista debe aparecer una versión que comience por `8.0`.

## 2. Clonar desde GitHub con Visual Studio

1. Entra a https://github.com/estjubalabnerserranoni-bit/NEXUS.
2. Pulsa el botón verde **Code**.
3. Copia la dirección HTTPS del repositorio.
4. Abre Visual Studio.
5. En la pantalla de inicio selecciona **Clonar un repositorio**.
6. Pega la dirección HTTPS.
7. Elige una carpeta de destino en tu computadora.
8. Pulsa **Clonar**.

También puedes clonar desde PowerShell:

```powershell
git clone https://github.com/estjubalabnerserranoni-bit/NEXUS.git
cd NEXUS
```

## 3. Abrir la solución

1. En la carpeta clonada localiza `NEXUS.sln`.
2. Haz doble clic o usa Visual Studio → Archivo → Abrir → Proyecto o solución.
3. Espera a que termine la restauración de paquetes NuGet.
4. En el Explorador de soluciones verás `NEXUS.WinForms` y `NEXUS.Tests`.
5. Si es necesario, haz clic derecho en `NEXUS.WinForms` y selecciona **Establecer como proyecto de inicio**.

No abras `program-cs` para ejecutar la versión nueva; ese archivo se conserva únicamente como referencia.

## 4. Ejecutar

Pulsa F5 o el botón **Iniciar**. En el menú de NEXUS:

1. Escribe el nombre del cadete.
2. Selecciona la realidad y una ranura.
3. Pulsa **Nueva partida** o **Continuar partida**.
4. Sigue las indicaciones de IRIS.

El juego funciona con guardado local y no necesita SQL Server para iniciar.

## 5. Probar las reglas

Desde la carpeta raíz abre PowerShell y ejecuta:

```powershell
dotnet restore NEXUS.sln
dotnet build NEXUS.sln --configuration Release
dotnet run --project tests/NEXUS.Tests/NEXUS.Tests.csproj --configuration Release
```

Si las pruebas finalizan, verás mensajes `[OK]` y un resumen de las pruebas correctas.

## 6. Configurar SQL Server (opcional)

1. Instala SQL Server y SQL Server Management Studio.
2. Abre SSMS y conéctate a la instancia local.
3. Abre y ejecuta `database/01-esquema.sql`.
4. En PowerShell configura la variable de entorno siguiendo los ejemplos de `database/README.md`.
5. Inicia Visual Studio desde esa misma terminal para que herede la variable.
6. En el menú de NEXUS pulsa **Configuración SQL** para probar la conexión.

El guardado automático local sigue activo. El botón Guardar partida conserva el progreso local y también lo envía a SQL Server cuando la conexión está configurada.

## 7. Revisar GitHub Actions

1. Abre el repositorio en GitHub.
2. Entra a la pestaña **Actions**.
3. Abre el flujo **Compilar y probar NEXUS**.
4. Comprueba que los pasos de restauración, compilación y pruebas estén en verde.

Si algún paso falla, abre el detalle para ver el mensaje del compilador o de las pruebas.

## Problemas frecuentes

**No reconoce `dotnet`:** instala el SDK de .NET 8 y reinicia la terminal.

**Visual Studio no reconoce la solución:** actualiza Visual Studio 2022 y comprueba la carga de trabajo de escritorio.

**No restaura Microsoft.Data.SqlClient:** verifica conexión a internet y vuelve a ejecutar `dotnet restore NEXUS.sln`.

**No conecta con SQL Server:** revisa que el servicio esté iniciado, que el nombre de instancia sea correcto y que la variable `NEXUS_SQLSERVER_CONNECTION` exista en la misma sesión desde la que abriste Visual Studio.

**La partida local no aparece en otra computadora:** los archivos JSON se guardan en el equipo donde se juega. Para compartir partidas entre equipos se debe configurar SQL Server en ambos y usar la misma base de datos accesible.
