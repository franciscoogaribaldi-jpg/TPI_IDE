using DTOs;
using System.Net;
using System.Net.Http.Json;

namespace API.Clients
{
    public class AuthApiClient : BaseApiClient
    {
        public static async Task<LoginResponseDTO?> LoginAsync(string nombreUsuario, string contrasena)
        {
            using var client = CreateHttpClient();
            var request = new LoginRequestDTO { NombreUsuario = nombreUsuario, Contrasena = contrasena };

            HttpResponseMessage response = await client.PostAsJsonAsync("auth/login", request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return null; // credenciales incorrectas: no es un error de conexión, es un "no"

            if (!response.IsSuccessStatusCode)
                throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
        }
    }
}
