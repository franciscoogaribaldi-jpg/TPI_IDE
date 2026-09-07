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

                if (!SesionActual.EstaLogueado)
                    continue;

                break;
            }
        }
    }
}
