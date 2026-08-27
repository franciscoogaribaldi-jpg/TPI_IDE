namespace WindowsForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                using var loginForm = new LoginForm();
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return; // se canceló el login: cerramos la app
                }

                Application.Run(new Home());

                // Si "Home" se cerró porque el usuario hizo Logout, SesionActual.Usuario
                // ya quedó en null (lo limpia Home antes de cerrarse) y volvemos a mostrar
                // el login. Si se cerró por "Salir" (o la X de la ventana), la sesión sigue
                // activa y cortamos el loop: la app termina de una.
                if (!SesionActual.EstaLogueado)
                    continue;

                break;
            }
        }
    }
}
