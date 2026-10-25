using API.Clients;

namespace WindowsForms
{
    /// <summary>
    /// Un solo lugar para mostrar los errores en las pantallas del escritorio.
    /// Si el error es que venció el token, además cierra la sesión y vuelve al login.
    /// </summary>
    internal static class ErroresUI
    {
        public static void Mostrar(IWin32Window ventana, Exception ex, string titulo = "Error", string? contexto = null)
        {
            if (ex is SesionExpiradaException)
            {
                // Puede llegar más de un error a la vez (por ejemplo dos pantallas cargando):
                // si la sesión ya se cerró, el aviso ya se mostró.
                if (!SesionActual.EstaLogueado) return;

                MessageBox.Show(ventana, ex.Message, "Sesión vencida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SesionActual.CerrarSesion();

                // Si el error vino de un formulario modal (un Detalle), se cierra primero.
                if (ventana is Form formulario && formulario.Modal)
                    formulario.Close();

                // Se cierra Home "después" (BeginInvoke) para que el modal termine de cerrarse.
                // Al cerrarse Home, el while de Program.cs ve que no hay sesión y muestra el login.
                var home = Application.OpenForms.OfType<Home>().FirstOrDefault();
                home?.BeginInvoke(new Action(home.Close));
                return;
            }

            string mensaje = contexto == null ? ex.Message : $"{contexto}\n\n{ex.Message}";
            MessageBox.Show(ventana, mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
