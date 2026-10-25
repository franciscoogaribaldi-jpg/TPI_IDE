using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Clients
{
    public abstract class BaseApiClient
    {
        public const string BaseUrlPorDefecto = "https://localhost:7111/";

        private static string _baseUrl = BaseUrlPorDefecto;

        // URL de la WebAPI. Cada aplicación la carga al arrancar desde la clave "ApiUrl" de su
        // appsettings.json (WinForms en Program.cs, Blazor en Program.cs). Si no viene, queda la
        // de por defecto. Siempre termina en "/" para que las rutas relativas ("canchas") se
        // peguen bien a la dirección base.
        public static string BaseUrl
        {
            get => _baseUrl;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _baseUrl = BaseUrlPorDefecto;
                    return;
                }

                var url = value.Trim();
                _baseUrl = url.EndsWith("/") ? url : url + "/";
            }
        }

        // Token de la sesión del ESCRITORIO: ahí cada proceso tiene un solo usuario, así que un
        // estático alcanza. La web NO lo usa: en Blazor Server cada usuario conectado pasa su
        // propio token como parámetro en cada llamada (ver SesionUsuario).
        public static string? TokenActual { get; set; }

        protected static HttpClient CreateHttpClient(string? token = null)
        {
            // El handler revisa cada respuesta: si es 401 con token, la sesión venció (ver SesionExpiradaHandler).
            var handler = new SesionExpiradaHandler { InnerHandler = new HttpClientHandler() };
            var client = new HttpClient(handler) { BaseAddress = new Uri(BaseUrl) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Si la llamada trae su propio token (web) se usa ese; si no, el del escritorio.
            var tokenAUsar = token ?? TokenActual;
            if (!string.IsNullOrEmpty(tokenAUsar))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenAUsar);
            }

            return client;
        }

        /// <summary>
        /// Lee el { "error": "..." } que devuelven nuestros endpoints en los BadRequest,
        /// para mostrar el mensaje de la regla de negocio en vez de un código HTTP pelado.
        /// </summary>
        protected static async Task<string> LeerMensajeDeErrorAsync(HttpResponseMessage response)
        {
            try
            {
                var cuerpo = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (cuerpo != null && cuerpo.TryGetValue("error", out var mensaje) && !string.IsNullOrWhiteSpace(mensaje))
                    return mensaje;
            }
            catch
            {
                // El cuerpo no tenía el formato { "error": "..." } esperado; seguimos al mensaje genérico.
            }

            return $"El servidor respondió con un error ({(int)response.StatusCode} {response.StatusCode}).";
        }
    }
}
