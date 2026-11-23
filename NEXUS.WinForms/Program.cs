using System;
using System.Windows.Forms;
using NexusJuegoForms.Forms;

namespace NexusJuegoForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormularioPrincipal());
    }
}
