using System.Net;

namespace API.Clients
{
    /// <summary>
    /// Intermediario entre el HttpClient y la red: deja pasar el pedido, mira la respuesta y,
    /// si es 401 y el pedido llevaba token, lanza SesionExpiradaException. Así no hay que
    /// repetir el chequeo en cada método de cada ApiClient.
    /// </summary>
    public class SesionExpiradaHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            // Solo si había token: el login no lleva, y ahí un 401 significa "usuario o
            // contraseña incorrectos" (lo resuelve AuthApiClient), no "sesión vencida".
            bool llevabaToken = request.Headers.Authorization != null;

            if (response.StatusCode == HttpStatusCode.Unauthorized && llevabaToken)
            {
                response.Dispose();
                throw new SesionExpiradaException();
            }

            return response;
        }
    }
}
