using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class TurnoApiClient : BaseApiClient
    {
        public static async Task<IEnumerable<TurnoDTO>> GetAllAsync(string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.GetAsync("turnos");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return await response.Content.ReadFromJsonAsync<IEnumerable<TurnoDTO>>() ?? Enumerable.Empty<TurnoDTO>();
        }

        public static async Task<TurnoDTO> AddAsync(TurnoDTO turno, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PostAsJsonAsync("turnos", turno);

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);

            return (await response.Content.ReadFromJsonAsync<TurnoDTO>())!;
        }

        public static async Task UpdateAsync(TurnoDTO turno, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.PutAsJsonAsync("turnos", turno);

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);
        }

        public static async Task DeleteAsync(int id, string? token = null)
        {
            using var client = CreateHttpClient(token);
            var response = await client.DeleteAsync($"turnos/{id}");

            if (!response.IsSuccessStatusCode) throw await CrearExcepcionAsync(response);
        }
    }
}
