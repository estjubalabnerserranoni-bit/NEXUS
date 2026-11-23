using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NexusJuegoForms.Data;
using NexusJuegoForms.Models;
using NexusJuegoForms.Services;

namespace NexusJuegoForms.Forms;

public class FormularioPrincipal : Form
{
    private readonly Color Fondo = Color.FromArgb(16, 26, 43);
    private readonly Color Panel = Color.FromArgb(32, 50, 74);
    private readonly Color Turquesa = Color.FromArgb(36, 106, 111);
    private readonly Color Texto = Color.White;

    private readonly TextBox txtNombre = new();
    private readonly ComboBox cmbRealidad = new();
    private readonly TabControl pestanas = new();

    private readonly Label lblEstado = new();
    private readonly Label lblMision = new();
    private readonly Label lblZonaSeleccionada = new();
    private readonly Label lblIris = new();
    private readonly Label lblInventarioAyuda = new();

    private readonly NumericUpDown numFrecuencia = new();
    private readonly Button btnEscanear = new();
    private readonly Button btnRecargar = new();
    private readonly Button btnPurgar = new();
    private readonly Button btnUsarCelda = new();
    private readonly Button btnGuardar = new();
    private readonly ListBox listaInventario = new();

    private readonly Button[] botonesZonas = new Button[9];

    private JuegoService? juego;
    private int zonaSeleccionada = 1;

    public FormularioPrincipal()
    {
        Text = "NEXUS | Exploración de realidades";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(950, 680);
        Size = new Size(1120, 760);
        BackColor = Fondo;
        ForeColor = Texto;
        Font = new Font("Segoe UI", 10);

        CrearInterfaz();
        pestanas.Enabled = false;
        MostrarIris("Escribe el nombre del cadete y selecciona una realidad para iniciar una partida.");
    }

    private void CrearInterfaz()
    {
        TableLayoutPanel principal = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 4,
            BackColor = Fondo
        };

        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

        Panel encabezado = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Fondo
        };

        Label titulo = new()
        {
            Text = "NEXUS",
            Font = new Font("Segoe UI", 25, FontStyle.Bold),
            ForeColor = Color.FromArgb(100, 220, 220),
            AutoSize = true,
            Location = new Point(5, 2)
        };

        Label subtitulo = new()
        {
            Text = "EXPLORATION / REALITY SYSTEM     •     AÑO 2297",
            ForeColor = Texto,
            AutoSize = true,
            Location = new Point(160, 20)
        };

        encabezado.Controls.Add(titulo);
        encabezado.Controls.Add(subtitulo);
        principal.Controls.Add(encabezado, 0, 0);

        FlowLayoutPanel perfil = new()
        {
            Dock = DockStyle.Fill,
            WrapContents = false,
            FlowDirection = FlowDirection.LeftToRight,
            BackColor = Panel,
            Padding = new Padding(10, 12, 5, 5)
        };

        perfil.Controls.Add(CrearEtiqueta("Cadete:"));
        txtNombre.Width = 190;
        txtNombre.MaxLength = 35;
        txtNombre.Margin = new Padding(5, 2, 18, 2);
        perfil.Controls.Add(txtNombre);

        perfil.Controls.Add(CrearEtiqueta("Realidad:"));
        cmbRealidad.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRealidad.Items.AddRange(new object[] { "Alfa", "Beta" });
        cmbRealidad.SelectedIndex = 0;
        cmbRealidad.Width = 100;
        cmbRealidad.Margin = new Padding(5, 2, 18, 2);
        perfil.Controls.Add(cmbRealidad);

        perfil.Controls.Add(CrearBoton("Nueva partida", NuevaPartida, 135));
        perfil.Controls.Add(CrearBoton("Continuar guardada", ContinuarPartida, 165));

        principal.Controls.Add(perfil, 0, 1);

        pestanas.Dock = DockStyle.Fill;
        pestanas.Appearance = TabAppearance.Normal;
        pestanas.Controls.Add(CrearPaginaExploracion());
        pestanas.Controls.Add(CrearPaginaMision());
        pestanas.Controls.Add(CrearPaginaInventario());
        principal.Controls.Add(pestanas, 0, 2);

        lblIris.Dock = DockStyle.Fill;
        lblIris.BackColor = Panel;
        lblIris.ForeColor = Color.FromArgb(180, 235, 235);
        lblIris.TextAlign = ContentAlignment.MiddleLeft;
        lblIris.Padding = new Padding(12, 5, 8, 5);
        principal.Controls.Add(lblIris, 0, 3);

        Controls.Add(principal);
    }

    private TabPage CrearPaginaExploracion()
    {
        TabPage pagina = new("Exploración")
        {
            BackColor = Fondo,
            Padding = new Padding(10)
        };

        SplitContainer division = new()
        {
            Dock = DockStyle.Fill,
            SplitterDistance = 570,
            BackColor = Fondo,
            BorderStyle = BorderStyle.None
        };

        TableLayoutPanel mapa = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Fondo,
            Padding = new Padding(5)
        };

        mapa.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        mapa.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mapa.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));

        Label tituloMapa = CrearEtiqueta("MAPA DE SECTORES");
        tituloMapa.Font = new Font("Segoe UI", 15, FontStyle.Bold);
        mapa.Controls.Add(tituloMapa, 0, 0);

        TableLayoutPanel cuadrilla = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 3,
            BackColor = Fondo
        };

        for (int columna = 0; columna < 3; columna++)
        {
            cuadrilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        }

        for (int fila = 0; fila < 3; fila++)
        {
            cuadrilla.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        }

        for (int i = 0; i < botonesZonas.Length; i++)
        {
            int indice = i;
            Button boton = CrearBoton("", () => SeleccionarZona(indice), 120);
            boton.Dock = DockStyle.Fill;
            boton.Margin = new Padding(5);
            boton.Height = 95;
            botonesZonas[i] = boton;
            cuadrilla.Controls.Add(boton, i % 3, i / 3);
        }

        mapa.Controls.Add(cuadrilla, 0, 1);

        FlowLayoutPanel seleccion = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Padding = new Padding(2, 12, 2, 2)
        };

        lblZonaSeleccionada.AutoSize = false;
        lblZonaSeleccionada.Width = 320;
        lblZonaSeleccionada.Height = 45;
        lblZonaSeleccionada.TextAlign = ContentAlignment.MiddleLeft;
        seleccion.Controls.Add(lblZonaSeleccionada);
        seleccion.Controls.Add(CrearBoton("Viajar a zona", ViajarSeleccionado, 150));
        mapa.Controls.Add(seleccion, 0, 2);

        division.Panel1.Controls.Add(mapa);

        FlowLayoutPanel controles = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(15, 5, 5, 5),
            BackColor = Fondo
        };

        Label tituloEstado = CrearEtiqueta("ESTADO DEL EXPLORADOR");
        tituloEstado.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        tituloEstado.Width = 390;
        controles.Controls.Add(tituloEstado);

        lblEstado.AutoSize = false;
        lblEstado.Width = 390;
        lblEstado.Height = 105;
        lblEstado.BackColor = Panel;
        lblEstado.Padding = new Padding(10);
        controles.Controls.Add(lblEstado);

        Label tituloMision = CrearEtiqueta("MISIÓN ACTIVA");
        tituloMision.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        controles.Controls.Add(tituloMision);

        lblMision.AutoSize = false;
        lblMision.Width = 390;
        lblMision.Height = 115;
        lblMision.BackColor = Panel;
        lblMision.Padding = new Padding(10);
        controles.Controls.Add(lblMision);

        Label tituloFrecuencia = CrearEtiqueta("Frecuencia del escáner (1–100)");
        controles.Controls.Add(tituloFrecuencia);

        numFrecuencia.Minimum = 1;
        numFrecuencia.Maximum = 100;
        numFrecuencia.Value = 60;
        numFrecuencia.Width = 90;
        controles.Controls.Add(numFrecuencia);

        btnEscanear = CrearBoton("Escanear anomalía", Escanear, 190);
        controles.Controls.Add(btnEscanear);

        btnRecargar = CrearBoton("Recargar en la base", Recargar, 190);
        controles.Controls.Add(btnRecargar);

        btnPurgar = CrearBoton("Purgar núcleo", Purgar, 190);
        controles.Controls.Add(btnPurgar);

        btnGuardar = CrearBoton("Guardar partida", GuardarPartida, 190);
        controles.Controls.Add(btnGuardar);

        division.Panel2.Controls.Add(controles);
        pagina.Controls.Add(division);

        return pagina;
    }

    private TabPage CrearPaginaMision()
    {
        TabPage pagina = new("Misiones")
        {
            BackColor = Fondo,
            Padding = new Padding(25)
        };

        Label titulo = CrearEtiqueta("REGISTRO DE MISIONES");
        titulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
        titulo.Dock = DockStyle.Top;
        titulo.Height = 55;

        TextBox detalle = new()
        {
            Name = "txtDetalleMision",
            Multiline = true,
            ReadOnly = true,
            Dock = DockStyle.Fill,
            BackColor = Panel,
            ForeColor = Texto,
            BorderStyle = BorderStyle.None,
            Font = new Font("Segoe UI", 12),
            ScrollBars = ScrollBars.Vertical
        };

        pagina.Controls.Add(detalle);
        pagina.Controls.Add(titulo);

        return pagina;
    }

    private TabPage CrearPaginaInventario()
    {
        TabPage pagina = new("Inventario")
        {
            BackColor = Fondo,
            Padding = new Padding(25)
        };

        TableLayoutPanel contenido = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Fondo
        };

        contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
        contenido.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 85));

        Label titulo = CrearEtiqueta("INVENTARIO DEL CADETE");
        titulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
        contenido.Controls.Add(titulo, 0, 0);

        listaInventario.Dock = DockStyle.Fill;
        listaInventario.BackColor = Panel;
        listaInventario.ForeColor = Texto;
        listaInventario.Font = new Font("Segoe UI", 11);
        contenido.Controls.Add(listaInventario, 0, 1);

        FlowLayoutPanel acciones = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(2, 12, 2, 2)
        };

        btnUsarCelda = CrearBoton("Usar celda de energía", UsarCelda, 200);
        acciones.Controls.Add(btnUsarCelda);

        lblInventarioAyuda.AutoSize = false;
        lblInventarioAyuda.Width = 430;
        lblInventarioAyuda.Height = 50;
        lblInventarioAyuda.TextAlign = ContentAlignment.MiddleLeft;
        acciones.Controls.Add(lblInventarioAyuda);

        contenido.Controls.Add(acciones, 0, 2);
        pagina.Controls.Add(contenido);

        return pagina;
    }

    private Label CrearEtiqueta(string texto)
    {
        return new Label
        {
            Text = texto,
            ForeColor = Texto,
            AutoSize = true,
            Margin = new Padding(5, 8, 5, 5)
        };
    }

    private Button CrearBoton(string texto, Action accion, int ancho)
    {
        Button boton = new()
        {
            Text = texto,
            Width = ancho,
            Height = 38,
            BackColor = Panel,
            ForeColor = Texto,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Margin = new Padding(5)
        };

        boton.FlatAppearance.BorderColor = Turquesa;
        boton.Click += (_, _) => accion();

        return boton;
    }

    private void NuevaPartida()
    {
        string nombre = txtNombre.Text.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            MostrarIris("Debes escribir el nombre del cadete antes de iniciar.");
            txtNombre.Focus();
            return;
        }

        if (juego != null)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Quieres iniciar otra partida? El progreso actual se reemplazará al guardar.",
                "NEXUS",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }
        }

        Partida nueva = new()
        {
            Cadete = new Explorador
            {
                Nombre = nombre,
                RealidadActual = cmbRealidad.SelectedItem?.ToString() ?? "Alfa"
            }
        };

        juego = new JuegoService(nueva);
        zonaSeleccionada = 1;
        pestanas.Enabled = true;
        ActualizarInterfaz();
        MostrarIris("MIS-001 iniciada. Recupera una celda, analiza una anomalía y regresa a la base.");
        GuardarAutomaticamente();
    }

    private void ContinuarPartida()
    {
        try
        {
            Partida? guardada = GuardadoLocal.Cargar();

            if (guardada == null)
            {
                MostrarIris("Todavía no existe una partida guardada en este equipo.");
                return;
            }

            juego = new JuegoService(guardada);
            txtNombre.Text = juego.Cadete.Nombre;
            cmbRealidad.SelectedItem = juego.Cadete.RealidadActual;
            zonaSeleccionada = juego.Partida.PosicionActual;
            pestanas.Enabled = true;

            ActualizarInterfaz();
            MostrarIris("Partida recuperada. Puedes continuar desde el último sector guardado.");
        }
        catch (Exception)
        {
            MostrarIris("No se pudo cargar la partida. El archivo guardado puede estar dañado.");
        }
    }

    private void SeleccionarZona(int indice)
    {
        if (juego == null)
        {
            return;
        }

        zonaSeleccionada = indice;
        ActualizarInterfaz();
    }

    private void ViajarSeleccionado()
    {
        EjecutarAccion(() => juego!.Viajar(zonaSeleccionada));
    }

    private void Escanear()
    {
        int frecuencia = (int)numFrecuencia.Value;
        EjecutarAccion(() => juego!.Escanear(frecuencia));
    }

    private void Recargar()
    {
        EjecutarAccion(() => juego!.Recargar());
    }

    private void Purgar()
    {
        EjecutarAccion(() => juego!.PurgarNucleo());
    }

    private void UsarCelda()
    {
        EjecutarAccion(() => juego!.UsarCeldaEnergia());
    }

    private void EjecutarAccion(Func<ResultadoAccion> accion)
    {
        if (juego == null)
        {
            return;
        }

        ResultadoAccion resultado = accion();
        MostrarIris(resultado.Mensaje);
        ActualizarInterfaz();

        if (resultado.Exito)
        {
            GuardarAutomaticamente();
        }
    }

    private void GuardarPartida()
    {
        if (juego == null)
        {
            MostrarIris("Primero debes iniciar o continuar una partida.");
            return;
        }

        try
        {
            GuardadoLocal.Guardar(juego.Partida);
            MostrarIris("Partida guardada correctamente en este equipo.");
        }
        catch (Exception)
        {
            MostrarIris("No se pudo guardar la partida. Comprueba los permisos de la carpeta del usuario.");
        }
    }

    private void GuardarAutomaticamente()
    {
        if (juego == null)
        {
            return;
        }

        try
        {
            GuardadoLocal.Guardar(juego.Partida);
        }
        catch (Exception)
        {
            MostrarIris("La acción se realizó, pero no se pudo guardar automáticamente. Pulsa Guardar partida.");
        }
    }

    private void ActualizarInterfaz()
    {
        if (juego == null)
        {
            return;
        }

        Explorador cadete = juego.Cadete;
        Mision mision = juego.MisionActual;

        lblEstado.Text =
            $"Cadete: {cadete.Nombre}\n" +
            $"Realidad: {cadete.RealidadActual}   |   Nivel de acceso: {cadete.NivelAcceso}\n" +
            $"Energía: {cadete.Energia}/100   |   Estabilidad: {cadete.Estabilidad}/100\n" +
            $"Núcleo: {(cadete.NucleoInestable ? "Inestable" : "Normal")}";

        lblMision.Text =
            $"{mision.Codigo} - {mision.Nombre}\n" +
            $"Recurso: {(mision.TieneRecurso ? "Recuperado" : "Pendiente")}\n" +
            $"Anomalía: {(mision.TieneAnalisis ? "Analizada" : "Pendiente")}\n" +
            $"Estado: {mision.Estado}\n" +
            "Objetivo final: regresar a la base central.";

        for (int i = 0; i < botonesZonas.Length; i++)
        {
            ZonaInfo zona = juego.MapaActual[i];
            string estado = juego.ObtenerEstadoZona(i);

            botonesZonas[i].Text = $"{zona.Nombre}\n{estado}";

            if (i == juego.Partida.PosicionActual)
            {
                botonesZonas[i].BackColor = Turquesa;
            }
            else if (i == zonaSeleccionada)
            {
                botonesZonas[i].BackColor = Color.FromArgb(65, 85, 115);
            }
            else
            {
                botonesZonas[i].BackColor = Panel;
            }
        }

        ZonaInfo seleccion = juego.MapaActual[zonaSeleccionada];
        lblZonaSeleccionada.Text =
            $"Seleccionada: {seleccion.Nombre}\nEstado: {juego.ObtenerEstadoZona(zonaSeleccionada)}";

        btnEscanear.Enabled =
            juego.MapaActual[juego.Partida.PosicionActual].Tipo == TipoZona.Anomalia &&
            !juego.Partida.ZonasExploradas[juego.Partida.PosicionActual];

        btnRecargar.Enabled = juego.Partida.PosicionActual == 0;
        btnPurgar.Enabled = juego.Partida.PosicionActual == 0 && cadete.NucleoInestable;

        TextBox? detalleMision = pestanas.TabPages["Misiones"]?
            .Controls.Find("txtDetalleMision", true).FirstOrDefault() as TextBox;

        if (detalleMision != null)
        {
            detalleMision.Text =
                $"{mision.Codigo} - {mision.Nombre}\r\n\r\n" +
                $"{mision.Descripcion}\r\n\r\n" +
                $"1. Recuperar una celda de energía: {(mision.TieneRecurso ? "COMPLETADO" : "PENDIENTE")}\r\n" +
                $"2. Analizar una anomalía: {(mision.TieneAnalisis ? "COMPLETADO" : "PENDIENTE")}\r\n" +
                "3. Regresar a la base central: " +
                $"{(mision.Completada ? "COMPLETADO" : "PENDIENTE")}\r\n\r\n" +
                $"Estado: {mision.Estado}\r\n" +
                $"Nivel de acceso actual: {cadete.NivelAcceso}";
        }

        listaInventario.Items.Clear();

        foreach (ObjetoInventario objeto in cadete.Inventario.Where(item => item.Cantidad > 0))
        {
            listaInventario.Items.Add(objeto);
        }

        bool tieneCelda = cadete.Inventario.Any(item =>
            item.Nombre == "Celda de energía" && item.Cantidad > 0);

        btnUsarCelda.Enabled = tieneCelda && cadete.Energia < 100;
        lblInventarioAyuda.Text = listaInventario.Items.Count == 0
            ? "Todavía no encontraste objetos."
            : "Selecciona una celda de energía para recuperar 25 puntos.";
    }

    private void MostrarIris(string mensaje)
    {
        lblIris.Text = "IRIS  |  " + mensaje;
    }
}
