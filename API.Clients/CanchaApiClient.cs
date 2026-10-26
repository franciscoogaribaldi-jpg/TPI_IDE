using DTOs;
using System.Net;
using System.Net.Http.Json;

namespace API.Clients
{
    public class CanchaApiClient : BaseApiClient
    {
        public static async Task<CanchaDTO?> GetAsync(int id, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync($"canchas/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<CanchaDTO>();
        }

        public static async Task<IEnumerable<CanchaDTO>> GetAllAsync(string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync("canchas");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<IEnumerable<CanchaDTO>>() ?? Enumerable.Empty<CanchaDTO>();
        }

        public static async Task<CanchaDTO> AddAsync(CanchaDTO cancha, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PostAsJsonAsync("canchas", cancha);

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return (await response.Content.ReadFromJsonAsync<CanchaDTO>())!;
        }

        public static async Task UpdateAsync(CanchaDTO cancha, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PutAsJsonAsync("canchas", cancha);

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);
        }

        public static async Task DeleteAsync(int id, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.DeleteAsync($"canchas/{id}");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);
        }
    }
}
