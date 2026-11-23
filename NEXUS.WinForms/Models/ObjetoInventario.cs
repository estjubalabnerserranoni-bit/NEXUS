namespace NexusJuegoForms.Models;

public class ObjetoInventario
{
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public int Cantidad { get; set; }

    public override string ToString()
    {
        return $"{Nombre} x{Cantidad} - {Descripcion}";
    }
}
