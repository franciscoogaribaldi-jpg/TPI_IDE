using API.Clients;
using Microsoft.Extensions.Configuration;

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

            CargarConfiguracion();

            while (true)
            {
                using var loginForm = new LoginForm();
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return; // se canceló el login: cerramos la app
                }

                Application.Run(new Home());

                // Si Home se cerró sin sesión (cerrar sesión o token vencido, ver ErroresUI),
                // volvemos a pedir el login. Si no, el usuario salió de la aplicación.
                if (!SesionActual.EstaLogueado)
                    continue;

                break;
            }
        }

        // Lee la URL de la API del appsettings.json que está junto al .exe (se copia ahí al
        // compilar, ver WindowsForms.csproj), igual que el TPIContext de la cátedra lee su
        // appsettings.json. Si no trae "ApiUrl", queda la URL por defecto de BaseApiClient.
        private static void CargarConfiguracion()
        {
            var configuracion = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            BaseApiClient.BaseUrl = configuracion["ApiUrl"] ?? BaseApiClient.BaseUrlPorDefecto;
        }
    }
}
