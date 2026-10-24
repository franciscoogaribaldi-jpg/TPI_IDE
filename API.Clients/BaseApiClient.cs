using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace API.Clients
{
    public abstract class BaseApiClient
    {
        // AJUSTAR: tiene que coincidir con la URL que usa tu WebAPI al correr con F5.
        // La encontrás en WebAPI/Properties/launchSettings.json ("applicationUrl"),
        // o mirando la ventana de la consola cuando arranca ("Now listening on: ...").
        private const string BaseUrl = "https://localhost:7111/";

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
