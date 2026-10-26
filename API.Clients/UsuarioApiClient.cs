using DTOs;
using System.Net;
using System.Net.Http.Json;

namespace API.Clients
{
    public class UsuarioApiClient : BaseApiClient
    {
        public static async Task<IEnumerable<UsuarioDTO>> GetAllAsync(string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync("usuarios");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<IEnumerable<UsuarioDTO>>() ?? Enumerable.Empty<UsuarioDTO>();
        }

        public static async Task<UsuarioDTO?> GetAsync(int id, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync($"usuarios/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<UsuarioDTO>();
        }

        public static async Task<UsuarioDTO> AddAsync(UsuarioDTO usuario, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PostAsJsonAsync("usuarios", usuario);

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return (await response.Content.ReadFromJsonAsync<UsuarioDTO>())!;
        }

        public static async Task UpdateAsync(UsuarioDTO usuario, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PutAsJsonAsync("usuarios", usuario);

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);
        }

        public static async Task DeleteAsync(int id, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.DeleteAsync($"usuarios/{id}");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);
        }
    }
}
