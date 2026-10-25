using API.Clients;
using System.Text.Json;

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
        // compilar, ver WindowsForms.csproj). Si el archivo no está o no trae "ApiUrl", queda
        // la URL por defecto de BaseApiClient.
        private static void CargarConfiguracion()
        {
            string ruta = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(ruta)) return;

            try
            {
                using var documento = JsonDocument.Parse(File.ReadAllText(ruta));
                if (documento.RootElement.TryGetProperty("ApiUrl", out var apiUrl) && apiUrl.ValueKind == JsonValueKind.String)
                {
                    BaseApiClient.BaseUrl = apiUrl.GetString()!;
                }
            }
            catch (JsonException)
            {
                MessageBox.Show("El archivo appsettings.json tiene un formato inválido. Se usará la URL de la API por defecto.",
                    "Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
