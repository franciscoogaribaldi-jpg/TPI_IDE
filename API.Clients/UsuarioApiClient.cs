using DTOs;
using System.Net;
using System.Net.Http.Json;

namespace API.Clients
{
    public class UsuarioApiClient : BaseApiClient
    {
        public static async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync("usuarios");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<IEnumerable<UsuarioDTO>>() ?? Enumerable.Empty<UsuarioDTO>();
        }

        public static async Task<UsuarioDTO?> GetAsync(int id)
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync($"usuarios/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<UsuarioDTO>();
        }
    }
}
