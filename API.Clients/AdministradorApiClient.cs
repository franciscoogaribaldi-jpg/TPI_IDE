using DTOs;
using System.Net;
using System.Net.Http.Json;

namespace API.Clients
{
    public class AdministradorApiClient : BaseApiClient
    {
        public static async Task<AdministradorDTO?> GetAsync(int id, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync($"administradores/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<AdministradorDTO>();
        }

        public static async Task<IEnumerable<AdministradorDTO>> GetAllAsync(string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync("administradores");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<IEnumerable<AdministradorDTO>>() ?? Enumerable.Empty<AdministradorDTO>();
        }

        public static async Task<AdministradorDTO> AddAsync(AdministradorDTO administrador, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PostAsJsonAsync("administradores", administrador);

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return (await response.Content.ReadFromJsonAsync<AdministradorDTO>())!;
        }

        public static async Task UpdateAsync(AdministradorDTO administrador, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PutAsJsonAsync("administradores", administrador);

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));
        }

        public static async Task DeleteAsync(int id, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.DeleteAsync($"administradores/{id}");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));
        }
    }
}
