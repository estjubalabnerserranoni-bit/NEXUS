using System.Drawing.Drawing2D;

namespace NexusJuegoForms.Controls;

/// <summary>Portada dibujada por código: portal dimensional, nebulosa y suelo holográfico.</summary>
public sealed class FondoMenuControl : Control
{
    private float fase;

    public FondoMenuControl()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.FromArgb(3, 12, 25);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        RectangleF r = ClientRectangle;

        using (LinearGradientBrush fondo = new(r, Color.FromArgb(2, 8, 20),
            Color.FromArgb(4, 34, 56), LinearGradientMode.Vertical))
            g.FillRectangle(fondo, r);

        // Nebulosa
        using (GraphicsPath nube = new())
        {
            nube.AddEllipse(-Width * .1f, Height * .04f, Width * .56f, Height * .58f);
            using PathGradientBrush luz = new(nube)
            {
                CenterColor = Color.FromArgb(46, 12, 219, 221),
                SurroundColors = new[] { Color.FromArgb(0, 4, 15, 34) }
            };
            g.FillPath(luz, nube);
        }

        // Estrellas estáticas
        for (int i = 0; i < 130; i++)
        {
            float x = (i * 173.71f) % Math.Max(1, Width);
            float y = (i * 89.17f) % Math.Max(1, Height * .68f);
            int size = i % 17 == 0 ? 3 : 1;
            int alpha = 55 + (i * 21 % 135);
            using SolidBrush star = new(Color.FromArgb(alpha, 79, 222, 245));
            g.FillEllipse(star, x, y, size, size);
        }

        DibujarPortal(g);
        DibujarSuelo(g);
        DibujarDecoracion(g);
    }

    private void DibujarPortal(Graphics g)
    {
        float x = Width * .30f;
        float y = Height * .34f;
        float radio = Math.Min(Width * .18f, Height * .31f);

        for (int i = 0; i < 9; i++)
        {
            float rr = radio * (1.35f - i * .07f);
            int alpha = 12 + i * 3;
            using Pen halo = new(Color.FromArgb(alpha, 35, 235, 235), 2 + i * .35f);
            g.DrawEllipse(halo, x - rr, y - rr, rr * 2, rr * 2);
        }

        using (GraphicsPath portal = new())
        {
            portal.AddEllipse(x - radio, y - radio, radio * 2, radio * 2);
            using PathGradientBrush brillo = new(portal)
            {
                CenterPoint = new PointF(x - radio * .20f, y - radio * .22f),
                CenterColor = Color.FromArgb(130, 27, 241, 212),
                SurroundColors = new[] { Color.FromArgb(20, 4, 19, 36) }
            };
            g.FillPath(brillo, portal);
        }

        using Pen exterior = new(Color.FromArgb(175, 38, 229, 236), 3);
        using Pen interior = new(Color.FromArgb(150, 69, 250, 220), 5);
        g.DrawEllipse(exterior, x - radio * .78f, y - radio * .78f, radio * 1.56f, radio * 1.56f);
        g.DrawEllipse(interior, x - radio * .55f, y - radio * .55f, radio * 1.1f, radio * 1.1f);

        for (int i = 0; i < 20; i++)
        {
            float a = (float)(i * Math.PI / 10);
            float inner = radio * .78f;
            float outer = radio * (1.06f + .06f * (float)Math.Sin(fase + i));
            using Pen ray = new(Color.FromArgb(95, 48, 208, 235), 1.3f);
            g.DrawLine(ray,
                x + MathF.Cos(a) * inner, y + MathF.Sin(a) * inner,
                x + MathF.Cos(a) * outer, y + MathF.Sin(a) * outer);
        }

        using SolidBrush voidPortal = new(Color.FromArgb(215, 2, 12, 29));
        g.FillEllipse(voidPortal, x - radio * .47f, y - radio * .47f, radio * .94f, radio * .94f);
        using Pen innerGlow = new(Color.FromArgb(160, 85, 245, 230), 4);
        g.DrawEllipse(innerGlow, x - radio * .47f, y - radio * .47f, radio * .94f, radio * .94f);

        // Luz en el suelo bajo el portal
        using Pen horizonte = new(Color.FromArgb(90, 86, 241, 245), 2);
        g.DrawArc(horizonte, x - radio * .9f, y + radio * 1.1f, radio * 1.8f, 30, 190, 160);
    }

    private void DibujarSuelo(Graphics g)
    {
        float horizonte = Height * .72f;
        using Pen linea = new(Color.FromArgb(28, 45, 204, 224), 1);
        PointF vanishing = new(Width * .45f, horizonte);

        for (int i = 0; i <= 42; i++)
        {
            float x = Width * i / 42f;
            g.DrawLine(linea, vanishing, new PointF(x, Height));
        }

        for (int j = 1; j <= 18; j++)
        {
            float t = j / 18f;
            float y = horizonte + (Height - horizonte) * t * t;
            int alpha = 15 + j * 2;
            using Pen row = new(Color.FromArgb(alpha, 37, 187, 219), 1);
            g.DrawLine(row, 0, y, Width, y);
        }

        for (int y = 0; y < 17; y++)
        {
            for (int x = 0; x < 90; x++)
            {
                float px = x * Width / 89f;
                float py = horizonte + y * (Height - horizonte) / 16f;
                float wave = MathF.Sin(px * .012f + y * .45f) * 5;
                using SolidBrush dot = new(Color.FromArgb(18 + y * 2, 51, 211, 227));
                g.FillEllipse(dot, px, py + wave, 2, 2);
            }
        }
    }

    private void DibujarDecoracion(Graphics g)
    {
        string[] labels = { "NEXUS", "EXPLORATION  /  REALITY SYSTEM", "PROTOCOLO DE EXPLORACIÓN DIMENSIONAL" };
        using Font title = new("Segoe UI", Math.Max(30, Height * .055f), FontStyle.Bold);
        using Font subtitle = new("Segoe UI", Math.Max(10, Height * .017f), FontStyle.Bold);
        using Font tiny = new("Segoe UI", Math.Max(8, Height * .012f), FontStyle.Regular);
        using SolidBrush cyan = new(Color.FromArgb(240, 171, 250, 255));
        using SolidBrush white = new(Color.FromArgb(225, 223, 248, 255));
        using SolidBrush muted = new(Color.FromArgb(165, 129, 187, 208));

        float left = Width * .065f;
        float y = Height * .13f;
        g.DrawString(labels[0], title, cyan, left, y);
        g.DrawString(labels[1], subtitle, white, left + 3, y + Height * .075f);
        g.DrawString("AÑO 2297   •   PORTAL DIMENSIONAL EN LÍNEA", tiny, muted, left + 4, y + Height * .118f);

        using Pen underline = new(Color.FromArgb(125, 48, 225, 232), 2);
        g.DrawLine(underline, left, y + Height * .105f, left + Width * .30f, y + Height * .105f);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        fase += .35f;
        Invalidate();
    }
}
