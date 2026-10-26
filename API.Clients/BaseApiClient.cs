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
            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
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
        /// Convierte una respuesta de error de la API en la excepción que corresponde. Todos los
        /// ApiClient la usan igual: if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);
        /// - 401 en un pedido que llevaba token: el token venció -> SesionExpiradaException
        ///   (el escritorio vuelve al login, ver ErroresUI). El login no lleva token, así que una
        ///   contraseña incorrecta nunca se confunde con una sesión vencida.
        /// - Cualquier otro error: Exception con el mensaje de la regla de negocio.
        /// </summary>
        protected static async Task<Exception> CrearExcepcionAsync(HttpResponseMessage response)
        {
            bool llevabaToken = response.RequestMessage?.Headers.Authorization != null;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && llevabaToken)
                return new SesionExpiradaException();

            return new Exception(await LeerMensajeDeErrorAsync(response));
        }

        /// <summary>
        /// Lee el { "error": "..." } que devuelven nuestros endpoints en los BadRequest,
        /// para mostrar el mensaje de la regla de negocio en vez de un código HTTP pelado.
        /// </summary>
        private static async Task<string> LeerMensajeDeErrorAsync(HttpResponseMessage response)
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
