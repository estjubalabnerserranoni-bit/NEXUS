using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NexusJuegoForms.Data;
using NexusJuegoForms.Models;
using NexusJuegoForms.Services;

namespace NexusJuegoForms.Forms;

public class FormularioPrincipal : Form
{
    private static readonly Color Fondo = Color.FromArgb(12, 20, 36);
    private static readonly Color FondoPanel = Color.FromArgb(24, 39, 62);
    private static readonly Color PanelClaro = Color.FromArgb(34, 55, 82);
    private static readonly Color Cian = Color.FromArgb(77, 211, 219);
    private static readonly Color Texto = Color.FromArgb(236, 244, 252);
    private static readonly Color TextoSuave = Color.FromArgb(166, 187, 207);
    private static readonly Color Verde = Color.FromArgb(72, 190, 145);

    private readonly Panel panelMenu = new();
    private readonly Panel panelJuego = new();
    private readonly Label lblIris = new();

    private readonly TextBox txtNombre = new();
    private readonly ComboBox cmbRealidad = new();
    private readonly ComboBox cmbRanura = new();
    private readonly Label lblInfoRanura = new();

    private readonly TabControl pestanas = new();
    private readonly Label lblCadete = new();
    private readonly Label lblRecursos = new();
    private readonly Label lblZonaSeleccionada = new();
    private readonly Label lblEstadoExplorador = new();
    private readonly Label lblResumenMision = new();
    private readonly Label lblInventarioAyuda = new();

    private readonly TextBox txtDetalleMision = new();
    private readonly NumericUpDown numFrecuencia = new();
    private readonly ListBox listaInventario = new();

    private readonly Button[] botonesZonas = new Button[9];
    private Button btnEscanear = new();
    private Button btnRecargar = new();
    private Button btnPurgar = new();
    private Button btnCambiarRealidad = new();
    private Button btnSiguienteMision = new();
    private Button btnUsarCelda = new();

    private JuegoService? juego;
    private int zonaSeleccionada = 1;

    private int RanuraElegida =>
        int.TryParse(cmbRanura.SelectedItem?.ToString(), out int ranura) ? ranura : 1;

    public FormularioPrincipal()
    {
        Text = "NEXUS | Exploration / Reality System";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1000, 700);
        Size = new Size(1180, 790);
        BackColor = Fondo;
        ForeColor = Texto;
        Font = new Font("Segoe UI", 10);
        AutoScaleMode = AutoScaleMode.Dpi;

        CrearEstructuraPrincipal();
        CrearMenuPrincipal();
        CrearInterfazJuego();

        panelJuego.Visible = false;
        panelMenu.Visible = true;
        panelMenu.BringToFront();
        CargarInformacionRanura();
    }

    private void CrearEstructuraPrincipal()
    {
        Panel zonaContenido = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Fondo
        };

        panelMenu.Dock = DockStyle.Fill;
        panelJuego.Dock = DockStyle.Fill;

        zonaContenido.Controls.Add(panelJuego);
        zonaContenido.Controls.Add(panelMenu);

        lblIris.Dock = DockStyle.Bottom;
        lblIris.Height = 48;
        lblIris.BackColor = FondoPanel;
        lblIris.ForeColor = Cian;
        lblIris.TextAlign = ContentAlignment.MiddleLeft;
        lblIris.Padding = new Padding(18, 4, 12, 4);
        lblIris.Text = "IRIS  |  Sistema de apoyo listo.";

        Controls.Add(zonaContenido);
        Controls.Add(lblIris);
        lblIris.BringToFront();
    }

    private void CrearMenuPrincipal()
    {
        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(36),
            BackColor = Fondo
        };

        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43));

        Panel presentacion = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Fondo,
            Padding = new Padding(12, 25, 30, 20)
        };

        Label marca = new()
        {
            Text = "NEXUS",
            Font = new Font("Segoe UI", 42, FontStyle.Bold),
            ForeColor = Cian,
            AutoSize = true,
            Location = new Point(15, 35)
        };

        Label subtitulo = new()
        {
            Text = "EXPLORATION  /  REALITY SYSTEM",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Texto,
            AutoSize = true,
            Location = new Point(20, 105)
        };

        Label linea = new()
        {
            Text = "AÑO 2297     •     PROTOCOLO DE EXPLORACIÓN DIMENSIONAL",
            Font = new Font("Segoe UI", 9),
            ForeColor = TextoSuave,
            AutoSize = true,
            Location = new Point(20, 145)
        };

        Label historia = new()
        {
            Text = "Las realidades están cambiando.\nLas señales no identificadas aumentan.\nTu misión es investigar las anomalías,\nproteger la estabilidad y descubrir\nel origen del sistema NEXUS.",
            Font = new Font("Segoe UI", 15),
            ForeColor = Texto,
            AutoSize = true,
            Location = new Point(20, 225)
        };

        Label estadoSistema = new()
        {
            Text = "● IRIS EN LÍNEA     ● SISTEMA DE NAVEGACIÓN DISPONIBLE",
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            ForeColor = Verde,
            AutoSize = true,
            Location = new Point(20, 420)
        };

        presentacion.Controls.AddRange(new Control[] { marca, subtitulo, linea, historia, estadoSistema });

        Panel tarjeta = new()
        {
            Dock = DockStyle.Fill,
            BackColor = FondoPanel,
            Padding = new Padding(26)
        };

        Label acceso = new()
        {
            Text = "CENTRO DE ACCESO",
            Font = new Font("Segoe UI", 17, FontStyle.Bold),
            ForeColor = Texto,
            AutoSize = true,
            Location = new Point(26, 25)
        };

        Label ayuda = new()
        {
            Text = "Identifica al cadete para iniciar la exploración.",
            Font = new Font("Segoe UI", 9),
            ForeColor = TextoSuave,
            AutoSize = true,
            Location = new Point(28, 65)
        };

        Label lblNombre = CrearEtiqueta("Nombre del cadete", 10);
        lblNombre.Location = new Point(28, 115);
        txtNombre.SetBounds(28, 145, 330, 34);
        txtNombre.MaxLength = 35;
        txtNombre.Font = new Font("Segoe UI", 11);
        txtNombre.BackColor = FondoPanelClaro;
        txtNombre.ForeColor = Texto;
        txtNombre.BorderStyle = BorderStyle.FixedSingle;

        Label lblRealidad = CrearEtiqueta("Realidad inicial", 10);
        lblRealidad.Location = new Point(28, 195);
        cmbRealidad.SetBounds(28, 225, 155, 34);
        cmbRealidad.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRealidad.Items.AddRange(new object[] { "Alfa", "Beta" });
        cmbRealidad.SelectedIndex = 0;
        cmbRealidad.BackColor = FondoPanelClaro;
        cmbRealidad.ForeColor = Texto;

        Label lblSlot = CrearEtiqueta("Ranura de partida", 10);
        lblSlot.Location = new Point(205, 195);
        cmbRanura.SetBounds(205, 225, 153, 34);
        cmbRanura.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRanura.Items.AddRange(new object[] { "1", "2", "3" });
        cmbRanura.SelectedIndex = 0;
        cmbRanura.BackColor = FondoPanelClaro;
        cmbRanura.ForeColor = Texto;
        cmbRanura.SelectedIndexChanged += (_, _) => CargarInformacionRanura();

        lblInfoRanura.SetBounds(28, 268, 330, 42);
        lblInfoRanura.ForeColor = TextoSuave;
        lblInfoRanura.Font = new Font("Segoe UI", 9);

        Button btnNueva = CrearBoton("NUEVA PARTIDA", NuevaPartida, 330, 45, destacado: true);
        btnNueva.SetBounds(28, 330, 330, 45);

        Button btnContinuar = CrearBoton("CONTINUAR PARTIDA", ContinuarPartida, 330, 42);
        btnContinuar.SetBounds(28, 387, 330, 42);

        Button btnAjustes = CrearBoton("CONFIGURACIÓN / AYUDA", MostrarAyuda, 158, 38);
        btnAjustes.SetBounds(28, 445, 158, 38);

        Button btnSalir = CrearBoton("SALIR", Salir, 158, 38);
        btnSalir.SetBounds(200, 445, 158, 38);

        tarjeta.Controls.AddRange(new Control[]
        {
            acceso, ayuda, lblNombre, txtNombre, lblRealidad, cmbRealidad,
            lblSlot, cmbRanura, lblInfoRanura, btnNueva, btnContinuar,
            btnAjustes, btnSalir
        });

        layout.Controls.Add(presentacion, 0, 0);
        layout.Controls.Add(tarjeta, 1, 0);
        panelMenu.Controls.Add(layout);
    }

    private void CrearInterfazJuego()
    {
        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(12),
            BackColor = Fondo
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        FlowLayoutPanel encabezado = new()
        {
            Dock = DockStyle.Fill,
            BackColor = FondoPanel,
            Padding = new Padding(12, 9, 8, 5),
            WrapContents = false
        };

        lblCadete.Width = 230;
        lblCadete.Height = 50;
        lblCadete.ForeColor = Cian;
        lblCadete.TextAlign = ContentAlignment.MiddleLeft;
        encabezado.Controls.Add(lblCadete);

        lblRecursos.Width = 345;
        lblRecursos.Height = 50;
        lblRecursos.ForeColor = Texto;
        lblRecursos.TextAlign = ContentAlignment.MiddleLeft;
        encabezado.Controls.Add(lblRecursos);

        Button btnMenu = CrearBoton("MENÚ PRINCIPAL", VolverAlMenu, 150, 38);
        btnMenu.Margin = new Padding(10, 6, 5, 5);
        encabezado.Controls.Add(btnMenu);

        pestanas.Dock = DockStyle.Fill;
        pestanas.Controls.Add(CrearPaginaExploracion());
        pestanas.Controls.Add(CrearPaginaMisiones());
        pestanas.Controls.Add(CrearPaginaInventario());

        layout.Controls.Add(encabezado, 0, 0);
        layout.Controls.Add(pestanas, 0, 1);
        panelJuego.Controls.Add(layout);
    }

    private TabPage CrearPaginaExploracion()
    {
        TabPage pagina = new("Exploración")
        {
            BackColor = Fondo,
            Padding = new Padding(8)
        };

        SplitContainer split = new()
        {
            Dock = DockStyle.Fill,
            Size = new Size(1000, 600),
            Orientation = Orientation.Vertical,
            SplitterDistance = 560,
            BackColor = Fondo
        };

        TableLayoutPanel panelMapa = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(5),
            BackColor = Fondo
        };
        panelMapa.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
        panelMapa.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        panelMapa.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));

        Label tituloMapa = CrearEtiqueta("MAPA DE SECTORES", 15);
        tituloMapa.Font = new Font("Segoe UI", 15, FontStyle.Bold);
        panelMapa.Controls.Add(tituloMapa, 0, 0);

        TableLayoutPanel grid = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 3,
            BackColor = Fondo
        };

        for (int i = 0; i < 3; i++)
        {
            grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            grid.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        }

        for (int i = 0; i < 9; i++)
        {
            int indice = i;
            Button boton = CrearBoton("", () => SeleccionarZona(indice), 120, 70);
            boton.Dock = DockStyle.Fill;
            boton.Margin = new Padding(5);
            botonesZonas[i] = boton;
            grid.Controls.Add(boton, i % 3, i / 3);
        }

        panelMapa.Controls.Add(grid, 0, 1);

        FlowLayoutPanel filaSeleccion = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            Padding = new Padding(3, 10, 2, 2)
        };

        lblZonaSeleccionada.Width = 300;
        lblZonaSeleccionada.Height = 48;
        lblZonaSeleccionada.ForeColor = Texto;
        lblZonaSeleccionada.TextAlign = ContentAlignment.MiddleLeft;
        filaSeleccion.Controls.Add(lblZonaSeleccionada);
        filaSeleccion.Controls.Add(CrearBoton("VIAJAR A ZONA", ViajarSeleccionado, 150, 40, true));
        panelMapa.Controls.Add(filaSeleccion, 0, 2);

        split.Panel1.Controls.Add(panelMapa);

        FlowLayoutPanel acciones = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(15, 8, 5, 5),
            BackColor = Fondo
        };

        Label tituloEstado = CrearEtiqueta("ESTADO DEL EXPLORADOR", 13);
        tituloEstado.Font = new Font("Segoe UI", 13, FontStyle.Bold);
        acciones.Controls.Add(tituloEstado);

        lblEstadoExplorador.AutoSize = false;
        lblEstadoExplorador.Width = 390;
        lblEstadoExplorador.Height = 95;
        lblEstadoExplorador.BackColor = FondoPanel;
        lblEstadoExplorador.Padding = new Padding(10);
        acciones.Controls.Add(lblEstadoExplorador);

        Label tituloResumen = CrearEtiqueta("MISIÓN ACTIVA", 12);
        tituloResumen.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        acciones.Controls.Add(tituloResumen);

        lblResumenMision.AutoSize = false;
        lblResumenMision.Width = 390;
        lblResumenMision.Height = 115;
        lblResumenMision.BackColor = FondoPanel;
        lblResumenMision.Padding = new Padding(10);
        acciones.Controls.Add(lblResumenMision);

        acciones.Controls.Add(CrearEtiqueta("Frecuencia del escáner", 10));
        numFrecuencia.Minimum = 1;
        numFrecuencia.Maximum = 100;
        numFrecuencia.Value = 60;
        numFrecuencia.Width = 100;
        acciones.Controls.Add(numFrecuencia);

        btnEscanear = CrearBoton("ESCANEAR ANOMALÍA", Escanear, 215, 40, true);
        btnRecargar = CrearBoton("RECARGAR EN LA BASE", Recargar, 215, 40);
        btnPurgar = CrearBoton("PURGAR NÚCLEO", Purgar, 215, 40);
        btnCambiarRealidad = CrearBoton("CAMBIAR REALIDAD", CambiarRealidad, 215, 40);
        Button btnGuardar = CrearBoton("GUARDAR PARTIDA", GuardarPartida, 215, 40);

        acciones.Controls.Add(btnEscanear);
        acciones.Controls.Add(btnRecargar);
        acciones.Controls.Add(btnPurgar);
        acciones.Controls.Add(btnCambiarRealidad);
        acciones.Controls.Add(btnGuardar);

        split.Panel2.Controls.Add(acciones);
        pagina.Controls.Add(split);
        return pagina;
    }

    private TabPage CrearPaginaMisiones()
    {
        TabPage pagina = new("Misiones")
        {
            BackColor = Fondo,
            Padding = new Padding(22)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Fondo
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));

        Label titulo = CrearEtiqueta("REGISTRO DE CAMPAÑA", 18);
        titulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
        layout.Controls.Add(titulo, 0, 0);

        txtDetalleMision.Dock = DockStyle.Fill;
        txtDetalleMision.Multiline = true;
        txtDetalleMision.ReadOnly = true;
        txtDetalleMision.BackColor = FondoPanel;
        txtDetalleMision.ForeColor = Texto;
        txtDetalleMision.BorderStyle = BorderStyle.None;
        txtDetalleMision.Font = new Font("Segoe UI", 12);
        txtDetalleMision.ScrollBars = ScrollBars.Vertical;
        txtDetalleMision.Padding = new Padding(12);
        layout.Controls.Add(txtDetalleMision, 0, 1);

        FlowLayoutPanel botones = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(2, 12, 2, 2)
        };
        btnSiguienteMision = CrearBoton("SIGUIENTE MISIÓN", SiguienteMision, 220, 42, true);
        botones.Controls.Add(btnSiguienteMision);
        botones.Controls.Add(CrearBoton("VOLVER A EXPLORACIÓN", () => pestanas.SelectedIndex = 0, 220, 42));
        layout.Controls.Add(botones, 0, 2);

        pagina.Controls.Add(layout);
        return pagina;
    }

    private TabPage CrearPaginaInventario()
    {
        TabPage pagina = new("Inventario")
        {
            BackColor = Fondo,
            Padding = new Padding(22)
        };

        TableLayoutPanel layout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            BackColor = Fondo
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 78));

        Label titulo = CrearEtiqueta("INVENTARIO DEL CADETE", 18);
        titulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
        layout.Controls.Add(titulo, 0, 0);

        listaInventario.Dock = DockStyle.Fill;
        listaInventario.BackColor = FondoPanel;
        listaInventario.ForeColor = Texto;
        listaInventario.Font = new Font("Segoe UI", 11);
        layout.Controls.Add(listaInventario, 0, 1);

        FlowLayoutPanel fila = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(2, 10, 2, 2)
        };
        btnUsarCelda = CrearBoton("USAR CELDA DE ENERGÍA", UsarCelda, 220, 42, true);
        fila.Controls.Add(btnUsarCelda);

        lblInventarioAyuda.Width = 500;
        lblInventarioAyuda.Height = 48;
        lblInventarioAyuda.ForeColor = TextoSuave;
        lblInventarioAyuda.TextAlign = ContentAlignment.MiddleLeft;
        fila.Controls.Add(lblInventarioAyuda);

        layout.Controls.Add(fila, 0, 2);
        pagina.Controls.Add(layout);
        return pagina;
    }

    private Label CrearEtiqueta(string texto, int tamano)
    {
        return new Label
        {
            Text = texto,
            AutoSize = true,
            ForeColor = Texto,
            Font = new Font("Segoe UI", tamano),
            Margin = new Padding(5, 7, 5, 5)
        };
    }

    private Button CrearBoton(string texto, Action accion, int ancho, int alto, bool destacado = false)
    {
        Button boton = new()
        {
            Text = texto,
            Width = ancho,
            Height = alto,
            BackColor = destacado ? Color.FromArgb(27, 106, 119) : PanelClaro,
            ForeColor = Texto,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            Margin = new Padding(4)
        };
        boton.FlatAppearance.BorderColor = destacado ? Cian : Color.FromArgb(65, 92, 120);
        boton.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 85, 112);
        boton.Click += (_, _) => accion();
        return boton;
    }

    private void NuevaPartida()
    {
        string nombre = txtNombre.Text.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            MostrarIris("Debes escribir el nombre del cadete.");
            txtNombre.Focus();
            return;
        }

        int ranura = RanuraElegida;

        if (GuardadoLocal.ExistePartida(ranura))
        {
            DialogResult respuesta = MessageBox.Show(
                $"La ranura {ranura} ya contiene una partida. ¿Quieres reemplazarla?",
                "Confirmar nueva partida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (respuesta != DialogResult.Yes)
                return;
        }

        Partida partida = new()
        {
            NumeroRanura = ranura,
            Cadete = new Explorador
            {
                Nombre = nombre,
                RealidadActual = cmbRealidad.SelectedItem?.ToString() ?? "Alfa"
            },
            Misiones = CampanaService.CrearCampana(),
            ProgresoAlfa = new ProgresoMapa(),
            ProgresoBeta = new ProgresoMapa()
        };

        juego = new JuegoService(partida);
        zonaSeleccionada = 1;
        AbrirJuego();
        GuardarAutomaticamente();
        MostrarIris($"Bienvenido, {nombre}. MIS-001 está activa. IRIS te guiará durante la exploración.");
    }

    private void ContinuarPartida()
    {
        try
        {
            Partida? partida = GuardadoLocal.Cargar(RanuraElegida);
            if (partida == null)
            {
                MostrarIris("Esta ranura está vacía. Crea una nueva partida o selecciona otra ranura.");
                return;
            }

            juego = new JuegoService(partida);
            zonaSeleccionada = juego.Partida.PosicionActual;
            AbrirJuego();
            MostrarIris($"Partida de {juego.Cadete.Nombre} recuperada correctamente.");
        }
        catch (Exception)
        {
            MostrarIris("No se pudo leer esta partida. El archivo puede estar dañado.");
        }
    }

    private void AbrirJuego()
    {
        panelMenu.Visible = false;
        panelJuego.Visible = true;
        panelJuego.BringToFront();
        pestanas.SelectedIndex = 0;
        ActualizarInterfaz();
    }

    private void VolverAlMenu()
    {
        if (juego != null)
            GuardarAutomaticamente();

        panelJuego.Visible = false;
        panelMenu.Visible = true;
        panelMenu.BringToFront();
        CargarInformacionRanura();
        MostrarIris("Estás en el centro de acceso. Tu progreso se conserva en la ranura seleccionada.");
    }

    private void CargarInformacionRanura()
    {
        if (cmbRanura.SelectedItem == null)
            return;

        try
        {
            lblInfoRanura.Text = GuardadoLocal.ObtenerDescripcionRanura(RanuraElegida);
        }
        catch
        {
            lblInfoRanura.Text = "No se pudo leer la información de la ranura.";
        }
    }

    private void MostrarAyuda()
    {
        MessageBox.Show(
            "NEXUS — Ayuda rápida\n\n" +
            "1. Escribe el nombre del cadete y elige una ranura.\n" +
            "2. Inicia una partida o continúa una guardada.\n" +
            "3. Selecciona un sector vecino y pulsa VIAJAR A ZONA.\n" +
            "4. Recupera recursos y analiza las anomalías indicadas por la misión.\n" +
            "5. Vuelve a la base para recargar, purgar el núcleo y completar objetivos.\n" +
            "6. Usa SIGUIENTE MISIÓN para avanzar en la campaña.\n\n" +
            "Cada viaje cuesta 10 puntos de energía. Los análisis normales cuestan 5 y los críticos 20.",
            "Ayuda de NEXUS",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void Salir()
    {
        if (MessageBox.Show("¿Deseas cerrar NEXUS?", "Salir",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            if (juego != null)
                GuardarAutomaticamente();
            Close();
        }
    }

    private void SeleccionarZona(int indice)
    {
        if (juego == null)
            return;

        zonaSeleccionada = indice;
        ActualizarInterfaz();
    }

    private void ViajarSeleccionado()
    {
        EjecutarAccion(() => juego!.Viajar(zonaSeleccionada));
    }

    private void Escanear()
    {
        EjecutarAccion(() => juego!.Escanear((int)numFrecuencia.Value));
    }

    private void Recargar()
    {
        EjecutarAccion(() => juego!.Recargar());
    }

    private void Purgar()
    {
        EjecutarAccion(() => juego!.PurgarNucleo());
    }

    private void CambiarRealidad()
    {
        EjecutarAccion(() => juego!.CambiarRealidad());
    }

    private void SiguienteMision()
    {
        if (juego == null)
            return;

        ResultadoAccion resultado = juego.AvanzarMision();
        MostrarIris(resultado.Mensaje);
        ActualizarInterfaz();

        if (resultado.Exito)
            GuardarAutomaticamente();
    }

    private void UsarCelda()
    {
        EjecutarAccion(() => juego!.UsarCeldaEnergia());
    }

    private void EjecutarAccion(Func<ResultadoAccion> accion)
    {
        if (juego == null)
            return;

        ResultadoAccion resultado = accion();
        MostrarIris(resultado.Mensaje);
        ActualizarInterfaz();

        if (resultado.Exito)
            GuardarAutomaticamente();
    }

    private void GuardarPartida()
    {
        if (juego == null)
        {
            MostrarIris("No existe una partida activa para guardar.");
            return;
        }

        try
        {
            GuardadoLocal.Guardar(juego.Partida);
            MostrarIris($"Partida guardada correctamente en la ranura {juego.Partida.NumeroRanura}.");
        }
        catch
        {
            MostrarIris("No se pudo guardar. Comprueba los permisos de la carpeta de datos de usuario.");
        }
    }

    private void GuardarAutomaticamente()
    {
        if (juego == null)
            return;

        try
        {
            GuardadoLocal.Guardar(juego.Partida);
        }
        catch
        {
            MostrarIris("No se pudo guardar automáticamente. Usa el botón Guardar partida.");
        }
    }

    private void ActualizarInterfaz()
    {
        if (juego == null)
            return;

        Explorador cadete = juego.Cadete;
        Mision? mision = juego.MisionActual;

        lblCadete.Text = $"NEXUS  /  {cadete.Nombre}\nREALIDAD {cadete.RealidadActual.ToUpperInvariant()}";
        lblRecursos.Text =
            $"ENERGÍA  {cadete.Energia}/100     ESTABILIDAD  {cadete.Estabilidad}/100\n" +
            $"NIVEL {cadete.NivelAcceso}     NÚCLEO {(cadete.NucleoInestable ? "INESTABLE" : "NORMAL")}";

        if (mision == null)
        {
            lblEstadoExplorador.Text = "Campaña completada. Puedes revisar el inventario o iniciar otra partida.";
            lblResumenMision.Text = "Todas las misiones de la campaña inicial están completadas.";
            txtDetalleMision.Text = "¡Felicidades, cadete! Has completado la campaña inicial de NEXUS.";
            btnEscanear.Enabled = false;
            btnRecargar.Enabled = juego.Partida.PosicionActual == 0;
            btnPurgar.Enabled = false;
            btnCambiarRealidad.Enabled = false;
            btnSiguienteMision.Enabled = false;
        }
        else
        {
            lblEstadoExplorador.Text =
                $"Cadete: {cadete.Nombre}\n" +
                $"Realidad: {cadete.RealidadActual} | Nivel de acceso: {cadete.NivelAcceso}\n" +
                $"Energía: {cadete.Energia}/100 | Estabilidad: {cadete.Estabilidad}/100\n" +
                $"Núcleo: {(cadete.NucleoInestable ? "Inestable" : "Normal")}";

            lblResumenMision.Text =
                $"{mision.Codigo} — {mision.Nombre}\n" +
                $"Realidad requerida: {mision.RealidadObjetivo}\n" +
                $"{JuegoService.ResumenObjetivos(mision)}\n" +
                $"Estado: {mision.Estado}";

            btnSiguienteMision.Enabled = mision.Completada;
            btnEscanear.Enabled =
                juego.MapaActual[juego.Partida.PosicionActual].Tipo == TipoZona.Anomalia &&
                !juego.Partida.ZonasExploradas[juego.Partida.PosicionActual];
            btnRecargar.Enabled = juego.Partida.PosicionActual == 0 &&
                (cadete.Energia < 100 || cadete.Estabilidad < 100);
            btnPurgar.Enabled = juego.Partida.PosicionActual == 0 && cadete.NucleoInestable;
            btnCambiarRealidad.Enabled = juego.Partida.PosicionActual == 0 &&
                cadete.RealidadActual != mision.RealidadObjetivo;

            var lineas = juego.Partida.Misiones.Select((item, indice) =>
                $"{item.Codigo} — {item.Nombre}\r\n" +
                $"Estado: {(indice < juego.Partida.IndiceMisionActual ? "Completada" : item.Estado)}\r\n" +
                $"Realidad: {item.RealidadObjetivo}\r\n" +
                $"Objetivo: {item.Descripcion}\r\n" +
                $"Recompensa: {item.Recompensa}\r\n");

            txtDetalleMision.Text =
                string.Join("\r\n------------------------------\r\n", lineas) +
                $"\r\n\r\nMISIÓN ACTUAL: {mision.Codigo}\r\n" +
                JuegoService.ResumenObjetivos(mision);
        }

        for (int i = 0; i < botonesZonas.Length; i++)
        {
            ZonaInfo zona = juego.MapaActual[i];
            string estado = juego.ObtenerEstadoZona(i);

            botonesZonas[i].Text = $"{zona.Nombre}\n{estado}";
            botonesZonas[i].BackColor =
                i == juego.Partida.PosicionActual ? Color.FromArgb(27, 106, 119) :
                i == zonaSeleccionada ? Color.FromArgb(65, 85, 115) : PanelClaro;
        }

        ZonaInfo seleccion = juego.MapaActual[zonaSeleccionada];
        lblZonaSeleccionada.Text =
            $"Seleccionada: {seleccion.Nombre}\nEstado: {juego.ObtenerEstadoZona(zonaSeleccionada)}";

        listaInventario.Items.Clear();
        foreach (ObjetoInventario objeto in cadete.Inventario.Where(item => item.Cantidad > 0))
            listaInventario.Items.Add(objeto);

        bool tieneCelda = cadete.Inventario.Any(item =>
            item.Nombre == "Celda de energía" && item.Cantidad > 0);
        btnUsarCelda.Enabled = tieneCelda && cadete.Energia < 100;
        lblInventarioAyuda.Text = listaInventario.Items.Count == 0
            ? "Todavía no encontraste objetos."
            : "Una celda recupera 25 puntos de energía. Selecciona una para consultar su cantidad.";
    }

    private void MostrarIris(string mensaje)
    {
        lblIris.Text = "IRIS  |  " + mensaje;
    }
}
