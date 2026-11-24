using System.Drawing.Drawing2D;

namespace NexusJuegoForms.Controls;

internal static class GraphicsExtensions
{
    public static void FillRoundedRectangle(this Graphics graphics, Brush brush,
        float x, float y, float width, float height, float radius)
    {
        using GraphicsPath path = RoundedRectangle(x, y, width, height, radius);
        graphics.FillPath(brush, path);
    }

    public static void DrawRoundedRectangle(this Graphics graphics, Pen pen,
        float x, float y, float width, float height, float radius)
    {
        using GraphicsPath path = RoundedRectangle(x, y, width, height, radius);
        graphics.DrawPath(pen, path);
    }

    private static GraphicsPath RoundedRectangle(float x, float y,
        float width, float height, float radius)
    {
        GraphicsPath path = new();
        float d = Math.Max(1f, radius * 2f);
        path.AddArc(x, y, d, d, 180, 90);
        path.AddArc(x + width - d, y, d, d, 270, 90);
        path.AddArc(x + width - d, y + height - d, d, d, 0, 90);
        path.AddArc(x, y + height - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
