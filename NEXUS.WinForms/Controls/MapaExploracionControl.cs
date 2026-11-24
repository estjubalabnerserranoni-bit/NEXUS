using System.Drawing;
using System.Drawing.Drawing2D;
using NexusJuegoForms.Services;

namespace NexusJuegoForms.Controls;

/// <summary>
/// Escena 2D de exploración. El cadete se desplaza entre sectores conectados
/// usando WASD, flechas o clic del ratón. El control dibuja el escenario y el
/// personaje directamente, sin motores ni paquetes gráficos externos.
/// </summary>
public sealed class MapaExploracionControl : Control
{
    private readonly System.Windows.Forms.Timer temporizadorAnimacion = new()
    {
        Interval = 45
    };

    private JuegoService? juego;
    private int sectorSeleccionado;
    private int sectorDesde;
    private int sectorHasta;
    private int frameCaminata;
    private float progresoMovimiento = 1f;
    private float respiracion;

    public event Action<int>? SectorSeleccionado;
    public event Action<int>? MovimientoSolicitado;

    public MapaExploracionControl()
    {
        SetStyle(ControlStyles.UserPaint |
                 ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.ResizeRedraw |
                 ControlStyles.Selectable, true);

        BackColor = Color.FromArgb(5, 18, 34);
        TabStop = true;
        Cursor = Cursors.Hand;

        temporizadorAnimacion.Tick += (_, _) =>
        {
            frameCaminata = (frameCaminata + 1) % 4;
            respiracion += 0.16f;

            if (progresoMovimiento < 1f)
            {
                progresoMovimiento = Math.Min(1f, progresoMovimiento + 0.075f);
                if (progresoMovimiento >= 1f)
                    temporizadorAnimacion.Stop();
            }

            Invalidate();
        };
    }

    public void Actualizar(JuegoService? nuevoJuego, int seleccionado)
    {
        juego = nuevoJuego;
        sectorSeleccionado = Math.Clamp(seleccionado, 0, 8);
        Invalidate();
    }

    public void AnimarMovimiento(int origen, int destino)
    {
        sectorDesde = Math.Clamp(origen, 0, 8);
        sectorHasta = Math.Clamp(destino, 0, 8);
        progresoMovimiento = 0f;
        frameCaminata = 0;
        Focus();
        temporizadorAnimacion.Start();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

        RectangleF area = ClientRectangle;
        if (area.Width < 100 || area.Height < 100)
            return;

        DibujarPaisajeAlienigena(g, area);
        DibujarRedDeSectores(g, area);
        DibujarSectores(g, area);
        DibujarCadete(g, area);
        DibujarIndicaciones(g, area);

        if (Focused)
        {
            using Pen foco = new(Color.FromArgb(95, 70, 238, 245), 2)
            {
                DashStyle = DashStyle.Dash
            };
            g.DrawRectangle(foco, 3, 3, Width - 7, Height - 7);
        }
    }

    private void DibujarPaisajeAlienigena(Graphics g, RectangleF area)
    {
        using (LinearGradientBrush cielo = new(
            area,
            Color.FromArgb(5, 13, 39),
            Color.FromArgb(13, 100, 126),
            LinearGradientMode.Vertical))
        {
            g.FillRectangle(cielo, area);
        }

        // Estrellas pequeñas distribuidas de forma estable.
        for (int i = 0; i < 95; i++)
        {
            float x = (float)((i * 137.53) % Math.Max(1, area.Width));
            float y = (float)((i * 71.91) % Math.Max(1, area.Height * 0.62f));
            int radio = i % 9 == 0 ? 2 : 1;
            int alpha = 55 + (i * 17 % 130);
            using SolidBrush estrella = new(Color.FromArgb(alpha, 112, 241, 248));
            g.FillEllipse(estrella, x, y, radio, radio);
        }

        // Planeta grande en la parte superior derecha.
        float planetaR = Math.Min(area.Width, area.Height) * 0.205f;
        float planetaX = area.Width * 0.73f;
        float planetaY = area.Height * 0.19f;

        using (GraphicsPath orbPath = new())
        {
            orbPath.AddEllipse(planetaX - planetaR, planetaY - planetaR,
                planetaR * 2, planetaR * 2);
            using PathGradientBrush orb = new(orbPath)
            {
                CenterPoint = new PointF(planetaX - planetaR * 0.28f, planetaY - planetaR * 0.25f),
                CenterColor = Color.FromArgb(190, 135, 255, 250),
                SurroundColors = new[] { Color.FromArgb(18, 65, 126, 171) }
            };
            g.FillPath(orb, orbPath);
        }

        using Pen bordePlaneta = new(Color.FromArgb(175, 90, 248, 245), 2.5f);
        g.DrawEllipse(bordePlaneta, planetaX - planetaR, planetaY - planetaR,
            planetaR * 2, planetaR * 2);

        using Pen tramaPlaneta = new(Color.FromArgb(42, 205, 255, 250), 1f);
        for (int i = 1; i < 7; i++)
        {
            float y = planetaY - planetaR + i * planetaR / 3.5f;
            g.DrawArc(tramaPlaneta, planetaX - planetaR * 0.92f, y - 12,
                planetaR * 1.84f, planetaR * 0.36f, 0, 180);
        }

        // Torres alienígenas a lo lejos.
        DibujarTorre(g, area.Width * 0.12f, area.Height * 0.57f, area.Width * 0.075f, area.Height * 0.48f);
        DibujarTorre(g, area.Width * 0.31f, area.Height * 0.60f, area.Width * 0.055f, area.Height * 0.35f);
        DibujarTorre(g, area.Width * 0.55f, area.Height * 0.58f, area.Width * 0.08f, area.Height * 0.51f);
        DibujarTorre(g, area.Width * 0.91f, area.Height * 0.56f, area.Width * 0.09f, area.Height * 0.47f);

        // Agua luminosa y horizonte.
        float aguaY = area.Height * 0.59f;
        using (LinearGradientBrush agua = new(
            new RectangleF(0, aguaY, area.Width, area.Height - aguaY),
            Color.FromArgb(24, 185, 202, 210),
            Color.FromArgb(5, 12, 38),
            LinearGradientMode.Vertical))
        {
            g.FillRectangle(agua, 0, aguaY, area.Width, area.Height - aguaY);
        }

        using Pen onda = new(Color.FromArgb(72, 95, 240, 238), 1f);
        for (int i = 0; i < 17; i++)
        {
            float y = aguaY + 12 + i * area.Height * 0.021f;
            float wave = 6 + i * 0.5f;
            g.DrawBezier(onda,
                new PointF(0, y),
                new PointF(area.Width * 0.28f, y - wave),
                new PointF(area.Width * 0.70f, y + wave),
                new PointF(area.Width, y - wave * 0.2f));
        }

        // Plataformas de cristal en primer plano.
        DibujarIsla(g, area.Width * 0.02f, area.Height * 0.79f, area.Width * 0.30f, area.Height * 0.15f);
        DibujarIsla(g, area.Width * 0.72f, area.Height * 0.78f, area.Width * 0.28f, area.Height * 0.17f);
        DibujarEsfera(g, area.Width * 0.15f, area.Height * 0.84f, Math.Min(area.Width, area.Height) * 0.045f);
        DibujarEsfera(g, area.Width * 0.83f, area.Height * 0.82f, Math.Min(area.Width, area.Height) * 0.052f);
        DibujarEsfera(g, area.Width * 0.92f, area.Height * 0.91f, Math.Min(area.Width, area.Height) * 0.027f);

        // Velo oscuro muy suave para que se lean los nodos y el personaje.
        using SolidBrush velo = new(Color.FromArgb(42, 2, 10, 25));
        g.FillRectangle(velo, area);
    }

    private static void DibujarTorre(Graphics g, float x, float baseY, float width, float height)
    {
        PointF[] puntos =
        {
            new(x - width * .50f, baseY),
            new(x - width * .28f, baseY - height * .72f),
            new(x - width * .10f, baseY - height),
            new(x + width * .08f, baseY - height * .65f),
            new(x + width * .35f, baseY - height * .38f),
            new(x + width * .48f, baseY)
        };

        using SolidBrush sombra = new(Color.FromArgb(205, 9, 24, 54));
        using Pen luz = new(Color.FromArgb(105, 66, 221, 231), 2);
        g.FillPolygon(sombra, puntos);
        g.DrawLines(luz, puntos);
        using Pen franja = new(Color.FromArgb(70, 155, 98, 237), 3);
        g.DrawLine(franja, x - width * .16f, baseY - height * .52f,
            x + width * .22f, baseY - height * .52f);
    }

    private static void DibujarIsla(Graphics g, float x, float y, float width, float height)
    {
        PointF[] roca =
        {
            new(x, y + height * .42f),
            new(x + width * .12f, y + height * .12f),
            new(x + width * .38f, y),
            new(x + width * .63f, y + height * .10f),
            new(x + width, y + height * .34f),
            new(x + width * .88f, y + height),
            new(x + width * .18f, y + height * .86f)
        };

        using SolidBrush relleno = new(Color.FromArgb(220, 13, 28, 63));
        using Pen borde = new(Color.FromArgb(130, 61, 213, 226), 2);
        g.FillPolygon(relleno, roca);
        g.DrawLines(borde, roca);
        using Pen magenta = new(Color.FromArgb(95, 192, 84, 237), 3);
        g.DrawLine(magenta, x + width * .15f, y + height * .22f,
            x + width * .78f, y + height * .28f);
    }

    private static void DibujarEsfera(Graphics g, float x, float y, float radio)
    {
        using SolidBrush brillo = new(Color.FromArgb(30, 90, 250, 245));
        g.FillEllipse(brillo, x - radio * 1.7f, y - radio * 1.7f, radio * 3.4f, radio * 3.4f);
        using Pen exterior = new(Color.FromArgb(180, 101, 250, 240), 2);
        using Pen interior = new(Color.FromArgb(130, 186, 120, 250), 1);
        g.DrawEllipse(exterior, x - radio, y - radio, radio * 2, radio * 2);
        g.DrawEllipse(interior, x - radio * .62f, y - radio * .62f, radio * 1.24f, radio * 1.24f);
    }

    private void DibujarRedDeSectores(Graphics g, RectangleF area)
    {
        using Pen rutaBase = new(Color.FromArgb(95, 93, 237, 235), 3f)
        {
            DashStyle = DashStyle.Dash
        };
        using Pen rutaBrillo = new(Color.FromArgb(28, 106, 246, 240), 10f);

        for (int i = 0; i < 9; i++)
        {
            int fila = i / 3;
            int columna = i % 3;

            if (columna < 2)
                DibujarRuta(g, area, i, i + 1, rutaBase, rutaBrillo);
            if (fila < 2)
                DibujarRuta(g, area, i, i + 3, rutaBase, rutaBrillo);
        }
    }

    private void DibujarRuta(Graphics g, RectangleF area, int desde, int hasta, Pen basePen, Pen glowPen)
    {
        PointF a = CentroSector(area, desde);
        PointF b = CentroSector(area, hasta);
        g.DrawLine(glowPen, a, b);
        g.DrawLine(basePen, a, b);
    }

    private void DibujarSectores(Graphics g, RectangleF area)
    {
        if (juego == null)
            return;

        for (int i = 0; i < 9; i++)
        {
            PointF centro = CentroSector(area, i);
            bool actual = i == juego.Partida.PosicionActual;
            bool seleccionado = i == sectorSeleccionado;
            bool explorado = juego.Partida.ZonasExploradas[i];
            bool visitado = juego.Partida.ZonasVisitadas[i];
            ZonaInfo zona = juego.MapaActual[i];

            Color color = actual ? Color.FromArgb(255, 255, 95) :
                explorado ? Color.FromArgb(100, 248, 176) :
                visitado ? Color.FromArgb(130, 227, 247) :
                Color.FromArgb(91, 207, 229);

            float radio = actual ? 24f : seleccionado ? 22f : 17f;

            using SolidBrush halo = new(Color.FromArgb(actual ? 74 : 35, color));
            g.FillEllipse(halo, centro.X - radio * 1.9f, centro.Y - radio * 1.9f,
                radio * 3.8f, radio * 3.8f);

            using SolidBrush placa = new(Color.FromArgb(205, 3, 18, 38));
            using Pen contorno = new(color, seleccionado ? 3f : 1.7f);
            g.FillEllipse(placa, centro.X - radio, centro.Y - radio, radio * 2, radio * 2);
            g.DrawEllipse(contorno, centro.X - radio, centro.Y - radio, radio * 2, radio * 2);

            DibujarIconoZona(g, zona.Tipo, centro, radio * .68f, color);

            string etiqueta = zona.Nombre;
            using Font fuente = new("Segoe UI", Math.Max(7f, Math.Min(9f, area.Width / 90f)), FontStyle.Bold);
            using SolidBrush texto = new(Color.FromArgb(235, 236, 250, 255));
            SizeF medida = g.MeasureString(etiqueta, fuente);
            float etiquetaY = centro.Y + radio + 3;
            g.DrawString(etiqueta, fuente, texto, centro.X - medida.Width / 2, etiquetaY);

            if (actual)
            {
                using Font actualFont = new("Segoe UI", 8f, FontStyle.Bold);
                using SolidBrush tag = new(Color.FromArgb(240, 255, 220, 86));
                g.DrawString("CADETE", actualFont, tag, centro.X - 25, centro.Y - radio - 20);
            }
        }
    }

    private static void DibujarIconoZona(Graphics g, TipoZona tipo, PointF centro, float radio, Color color)
    {
        using Pen pluma = new(color, 2.2f);
        switch (tipo)
        {
            case TipoZona.Base:
                PointF[] casa =
                {
                    new(centro.X - radio, centro.Y),
                    new(centro.X, centro.Y - radio * .85f),
                    new(centro.X + radio, centro.Y),
                    new(centro.X + radio * .72f, centro.Y),
                    new(centro.X + radio * .72f, centro.Y + radio * .75f),
                    new(centro.X - radio * .72f, centro.Y + radio * .75f),
                    new(centro.X - radio * .72f, centro.Y)
                };
                g.DrawLines(pluma, casa);
                break;
            case TipoZona.Recurso:
                g.DrawRectangle(pluma, centro.X - radio * .65f, centro.Y - radio * .55f,
                    radio * 1.3f, radio * 1.1f);
                g.DrawLine(pluma, centro.X - radio * .65f, centro.Y - radio * .15f,
                    centro.X + radio * .65f, centro.Y - radio * .15f);
                g.DrawLine(pluma, centro.X, centro.Y - radio * .55f,
                    centro.X, centro.Y + radio * .55f);
                break;
            case TipoZona.Anomalia:
                PointF[] diamante =
                {
                    new(centro.X, centro.Y - radio),
                    new(centro.X + radio * .8f, centro.Y),
                    new(centro.X, centro.Y + radio),
                    new(centro.X - radio * .8f, centro.Y)
                };
                g.DrawPolygon(pluma, diamante);
                g.DrawEllipse(pluma, centro.X - radio * .28f, centro.Y - radio * .28f,
                    radio * .56f, radio * .56f);
                break;
            default:
                g.DrawEllipse(pluma, centro.X - radio * .55f, centro.Y - radio * .55f,
                    radio * 1.1f, radio * 1.1f);
                g.DrawLine(pluma, centro.X - radio * .8f, centro.Y,
                    centro.X + radio * .8f, centro.Y);
                break;
        }
    }

    private void DibujarCadete(Graphics g, RectangleF area)
    {
        if (juego == null)
            return;

        PointF destino = CentroSector(area, juego.Partida.PosicionActual);
        PointF posicion = destino;

        if (progresoMovimiento < 1f)
        {
            PointF origen = CentroSector(area, sectorDesde);
            PointF final = CentroSector(area, sectorHasta);
            float t = progresoMovimiento * progresoMovimiento * (3 - 2 * progresoMovimiento);
            posicion = new PointF(
                origen.X + (final.X - origen.X) * t,
                origen.Y + (final.Y - origen.Y) * t);
        }

        float escala = Math.Clamp(area.Width / 950f, .68f, 1.15f);
        float bob = progresoMovimiento < 1f
            ? (float)Math.Sin(frameCaminata * Math.PI / 2) * 4f * escala
            : (float)Math.Sin(respiracion) * 1.2f * escala;

        RectangleF personaje = new(
            posicion.X - 27f * escala,
            posicion.Y - 76f * escala + bob,
            54f * escala,
            76f * escala);

        // Sombra en el suelo.
        using SolidBrush sombra = new(Color.FromArgb(145, 0, 8, 21));
        g.FillEllipse(sombra, posicion.X - 24 * escala, posicion.Y - 7 * escala,
            48 * escala, 13 * escala);

        DibujarFiguraCadete(g, personaje, escala, frameCaminata, progresoMovimiento < 1f);
    }

    private static void DibujarFiguraCadete(Graphics g, RectangleF r, float s, int frame, bool caminando)
    {
        float x = r.X;
        float y = r.Y;
        float swing = caminando ? new[] { -5f, 0f, 5f, 0f }[frame] * s : 0f;

        using Pen contorno = new(Color.FromArgb(245, 70, 231, 238), 2.1f * s);
        using SolidBrush traje = new(Color.FromArgb(245, 214, 236, 241));
        using SolidBrush sombra = new(Color.FromArgb(255, 18, 68, 91));
        using SolidBrush visor = new(Color.FromArgb(255, 5, 31, 56));
        using SolidBrush brillo = new(Color.FromArgb(255, 74, 249, 229));

        // Mochila y torso.
        g.FillRoundedRectangle(sombra, x + 36 * s, y + 33 * s, 16 * s, 30 * s, 5 * s);
        g.DrawRoundedRectangle(contorno, x + 36 * s, y + 33 * s, 16 * s, 30 * s, 5 * s);
        g.FillRoundedRectangle(traje, x + 12 * s, y + 31 * s, 31 * s, 35 * s, 9 * s);
        g.DrawRoundedRectangle(contorno, x + 12 * s, y + 31 * s, 31 * s, 35 * s, 9 * s);

        // Brazos alternados al caminar.
        float brazo = caminando ? swing : 0f;
        using Pen brazoTraje = new(Color.FromArgb(240, 211, 234, 239), 8 * s);
        using Pen guante = new(Color.FromArgb(255, 29, 103, 128), 7 * s);
        g.DrawLine(brazoTraje, x + 16 * s, y + 39 * s, x + 7 * s + brazo, y + 57 * s);
        g.DrawLine(guante, x + 7 * s + brazo, y + 56 * s, x + 5 * s + brazo, y + 61 * s);
        g.DrawLine(brazoTraje, x + 40 * s, y + 39 * s, x + 48 * s - brazo, y + 55 * s);
        g.DrawLine(guante, x + 48 * s - brazo, y + 55 * s, x + 50 * s - brazo, y + 60 * s);

        // Panel energético.
        g.FillRoundedRectangle(visor, x + 21 * s, y + 40 * s, 14 * s, 15 * s, 3 * s);
        g.DrawRoundedRectangle(contorno, x + 21 * s, y + 40 * s, 14 * s, 15 * s, 3 * s);
        g.FillEllipse(brillo, x + 25 * s, y + 44 * s, 6 * s, 6 * s);

        // Piernas con paso alternado.
        using Pen pierna = new(Color.FromArgb(240, 218, 239, 242), 8 * s);
        using Pen bota = new(Color.FromArgb(255, 21, 67, 91), 7 * s);
        g.DrawLine(pierna, x + 22 * s, y + 62 * s, x + 18 * s + swing, y + 73 * s);
        g.DrawLine(pierna, x + 35 * s, y + 62 * s, x + 38 * s - swing, y + 73 * s);
        g.DrawLine(bota, x + 18 * s + swing, y + 73 * s, x + 12 * s + swing, y + 73 * s);
        g.DrawLine(bota, x + 38 * s - swing, y + 73 * s, x + 45 * s - swing, y + 73 * s);

        // Casco y visor.
        g.FillEllipse(traje, x + 12 * s, y + 2 * s, 31 * s, 32 * s);
        g.DrawEllipse(contorno, x + 12 * s, y + 2 * s, 31 * s, 32 * s);
        g.FillEllipse(visor, x + 17 * s, y + 8 * s, 22 * s, 21 * s);
        g.DrawArc(contorno, x + 18 * s, y + 8 * s, 20 * s, 19 * s, 195, 125);
        g.FillEllipse(brillo, x + 31 * s, y + 11 * s, 4 * s, 4 * s);
    }

    private static void DibujarIndicaciones(Graphics g, RectangleF area)
    {
        RectangleF caja = new(12, area.Height - 34, Math.Min(area.Width - 24, 480), 25);
        using SolidBrush fondo = new(Color.FromArgb(190, 2, 15, 33));
        using Pen borde = new(Color.FromArgb(110, 64, 218, 230), 1);
        g.FillRoundedRectangle(fondo, caja.X, caja.Y, caja.Width, caja.Height, 7);
        g.DrawRoundedRectangle(borde, caja.X, caja.Y, caja.Width, caja.Height, 7);

        using Font fuente = new("Segoe UI", 8.5f, FontStyle.Bold);
        using SolidBrush texto = new(Color.FromArgb(235, 210, 245, 250));
        g.DrawString("W A S D  /  FLECHAS: CAMINAR     •     CLIC: SELECCIONAR SECTOR",
            fuente, texto, caja.X + 10, caja.Y + 5);
    }

    private static PointF CentroSector(RectangleF area, int sector)
    {
        int fila = sector / 3;
        int columna = sector % 3;
        float margenX = Math.Max(70f, area.Width * .16f);
        float margenY = Math.Max(100f, area.Height * .23f);
        float pasoX = (area.Width - 2 * margenX) / 2f;
        float pasoY = (area.Height - 2 * margenY) / 2f;

        return new PointF(margenX + columna * pasoX, margenY + fila * pasoY);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        if (juego == null)
            return;

        float mejorDistancia = float.MaxValue;
        int encontrado = -1;

        for (int i = 0; i < 9; i++)
        {
            PointF centro = CentroSector(ClientRectangle, i);
            float dx = centro.X - e.X;
            float dy = centro.Y - e.Y;
            float distancia = dx * dx + dy * dy;

            if (distancia < mejorDistancia)
            {
                mejorDistancia = distancia;
                encontrado = i;
            }
        }

        if (encontrado < 0 || mejorDistancia > 62 * 62)
            return;

        Focus();
        sectorSeleccionado = encontrado;
        SectorSeleccionado?.Invoke(encontrado);
        Invalidate();

        if (encontrado != juego.Partida.PosicionActual &&
            SonSectoresVecinos(juego.Partida.PosicionActual, encontrado))
        {
            MovimientoSolicitado?.Invoke(encontrado);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (juego == null)
            return;

        int actual = juego.Partida.PosicionActual;
        int fila = actual / 3;
        int columna = actual % 3;
        int destino = actual;

        switch (e.KeyCode)
        {
            case Keys.Up:
            case Keys.W:
                if (fila > 0) destino -= 3;
                break;
            case Keys.Down:
            case Keys.S:
                if (fila < 2) destino += 3;
                break;
            case Keys.Left:
            case Keys.A:
                if (columna > 0) destino -= 1;
                break;
            case Keys.Right:
            case Keys.D:
                if (columna < 2) destino += 1;
                break;
            case Keys.Enter:
            case Keys.Space:
                if (sectorSeleccionado != actual)
                    MovimientoSolicitado?.Invoke(sectorSeleccionado);
                e.Handled = true;
                return;
            default:
                return;
        }

        if (destino != actual)
        {
            sectorSeleccionado = destino;
            SectorSeleccionado?.Invoke(destino);
            MovimientoSolicitado?.Invoke(destino);
            e.Handled = true;
        }
    }

    private static bool SonSectoresVecinos(int origen, int destino)
    {
        int filaOrigen = origen / 3;
        int columnaOrigen = origen % 3;
        int filaDestino = destino / 3;
        int columnaDestino = destino % 3;

        return Math.Abs(filaOrigen - filaDestino) + Math.Abs(columnaOrigen - columnaDestino) == 1;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            temporizadorAnimacion.Dispose();

        base.Dispose(disposing);
    }
}
