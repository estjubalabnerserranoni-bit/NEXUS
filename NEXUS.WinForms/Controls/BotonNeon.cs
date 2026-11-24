using System.Drawing.Drawing2D;

namespace NexusJuegoForms.Controls;

public sealed class BotonNeon : Button
{
    public Color ColorNeon { get; set; } = Color.FromArgb(26, 220, 232);
    public bool Destacado { get; set; }

    public BotonNeon()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        BackColor = Color.Transparent;
        ForeColor = Color.FromArgb(194, 250, 255);
        Font = new Font("Segoe UI", 12, FontStyle.Bold);
        Cursor = Cursors.Hand;
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        RectangleF bounds = new(2, 2, Width - 5, Height - 5);
        PointF[] shape =
        {
            new(bounds.Left + 18, bounds.Top),
            new(bounds.Right, bounds.Top),
            new(bounds.Right, bounds.Bottom),
            new(bounds.Left + 18, bounds.Bottom),
            new(bounds.Left, bounds.Top + bounds.Height / 2)
        };

        Color baseColor = Destacado
            ? Color.FromArgb(90, 145, 116, 5)
            : Color.FromArgb(125, 2, 43, 66);

        if (ClientRectangle.Contains(PointToClient(MousePosition)))
            baseColor = Color.FromArgb(150, 4, 83, 101);

        using SolidBrush fill = new(baseColor);
        using Pen glow = new(Color.FromArgb(45, ColorNeon), 8);
        using Pen border = new(ColorNeon, 2);
        g.FillPolygon(fill, shape);
        g.DrawPolygon(glow, shape);
        g.DrawPolygon(border, shape);

        TextRenderer.DrawText(g, Text, Font,
            Rectangle.Round(bounds), Destacado ? Color.FromArgb(255, 249, 225, 54) : ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

        if (Focused)
        {
            using Pen foco = new(Color.FromArgb(150, 220, 255, 255), 1)
            { DashStyle = DashStyle.Dash };
            g.DrawPolygon(foco, shape);
        }
    }

    protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); Invalidate(); }
    protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); Invalidate(); }
}
