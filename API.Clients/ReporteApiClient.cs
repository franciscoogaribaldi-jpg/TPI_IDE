using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class ReporteApiClient : BaseApiClient
    {
        public static async Task<IEnumerable<ReporteOcupacionDTO>> GetOcupacionAsync(DateTime desde, DateTime hasta, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync($"reportes/ocupacion?desde={desde:yyyy-MM-dd}&hasta={hasta:yyyy-MM-dd}");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<IEnumerable<ReporteOcupacionDTO>>() ?? Enumerable.Empty<ReporteOcupacionDTO>();
        }

        public static async Task<IEnumerable<ReporteRecaudacionDTO>> GetRecaudacionAsync(DateTime desde, DateTime hasta, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync($"reportes/recaudacion?desde={desde:yyyy-MM-dd}&hasta={hasta:yyyy-MM-dd}");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<IEnumerable<ReporteRecaudacionDTO>>() ?? Enumerable.Empty<ReporteRecaudacionDTO>();
        }
    }
}
