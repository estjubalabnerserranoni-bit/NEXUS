using System.Linq;

namespace NexusJuegoForms.Models;

public class Explorador
{
    public string Nombre { get; set; } = "";
    public string RealidadActual { get; set; } = "Alfa";
    public int Energia { get; set; } = 100;
    public int Estabilidad { get; set; } = 100;
    public int NivelAcceso { get; set; } = 1;
    public bool NucleoInestable { get; set; }

    public List<ObjetoInventario> Inventario { get; set; } = new();

    public bool GastarEnergia(int cantidad)
    {
        if (cantidad <= 0 || Energia < cantidad)
        {
            return false;
        }

        Energia -= cantidad;
        return true;
    }

    public void Recargar()
    {
        Energia = Math.Min(100, Energia + 30);
        Estabilidad = Math.Min(100, Estabilidad + 10);
    }

    public void AgregarObjeto(string nombre, string descripcion)
    {
        ObjetoInventario? objeto = Inventario
            .FirstOrDefault(item => item.Nombre == nombre);

        if (objeto == null)
        {
            Inventario.Add(new ObjetoInventario
            {
                Nombre = nombre,
                Descripcion = descripcion,
                Cantidad = 1
            });
        }
        else
        {
            objeto.Cantidad++;
        }
    }

    public bool UsarObjeto(string nombre, out string mensaje)
    {
        ObjetoInventario? objeto = Inventario.FirstOrDefault(item =>
            item.Nombre == nombre && item.Cantidad > 0);

        if (objeto == null)
        {
            mensaje = "Ese objeto no está disponible en el inventario.";
            return false;
        }

        if (objeto.Nombre != "Celda de energía")
        {
            mensaje = "Este objeto es un material de investigación y no se puede consumir.";
            return false;
        }

        if (Energia >= 100)
        {
            mensaje = "La energía ya está al máximo. Guarda la celda para después.";
            return false;
        }

        Energia = Math.Min(100, Energia + 25);
        objeto.Cantidad--;
        mensaje = "Usaste una celda y recuperaste 25 puntos de energía.";
        return true;
    }}
