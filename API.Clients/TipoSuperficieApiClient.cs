using DTOs;
using System.Net;
using System.Net.Http.Json;

namespace API.Clients
{
    public class TipoSuperficieApiClient : BaseApiClient
    {
        public static async Task<TipoSuperficieDTO?> GetAsync(int id, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync($"tipos-superficie/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<TipoSuperficieDTO>();
        }

        public static async Task<IEnumerable<TipoSuperficieDTO>> GetAllAsync(string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync("tipos-superficie");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<IEnumerable<TipoSuperficieDTO>>() ?? Enumerable.Empty<TipoSuperficieDTO>();
        }

        public static async Task<IEnumerable<TipoSuperficieDTO>> GetByDeporteAsync(string deporte, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync($"tipos-superficie/deporte/{deporte}");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<IEnumerable<TipoSuperficieDTO>>() ?? Enumerable.Empty<TipoSuperficieDTO>();
        }

        public static async Task<TipoSuperficieDTO> AddAsync(TipoSuperficieDTO dto, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PostAsJsonAsync("tipos-superficie", dto);

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return (await response.Content.ReadFromJsonAsync<TipoSuperficieDTO>())!;
        }

        public static async Task UpdateAsync(TipoSuperficieDTO dto, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PutAsJsonAsync("tipos-superficie", dto);

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);
        }

        public static async Task DeleteAsync(int id, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.DeleteAsync($"tipos-superficie/{id}");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);
        }
    }
}
