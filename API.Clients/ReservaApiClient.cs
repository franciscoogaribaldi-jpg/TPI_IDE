using DTOs;
using System.Net;
using System.Net.Http.Json;

namespace API.Clients
{
    public class ReservaApiClient : BaseApiClient
    {
        public static async Task<IEnumerable<ReservaDTO>> GetAllAsync()
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync("reservas");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<IEnumerable<ReservaDTO>>() ?? Enumerable.Empty<ReservaDTO>();
        }

        public static async Task<ReservaDTO> AddAsync(ReservaDTO reserva)
        {
            using var client = CreateHttpClient();
            var response = await client.PostAsJsonAsync("reservas", reserva);

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return (await response.Content.ReadFromJsonAsync<ReservaDTO>())!;
        }

        public static async Task UpdateAsync(ReservaDTO reserva)
        {
            using var client = CreateHttpClient();
            var response = await client.PutAsJsonAsync("reservas", reserva);

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));
        }

        public static async Task DeleteAsync(int id)
        {
            using var client = CreateHttpClient();
            var response = await client.DeleteAsync($"reservas/{id}");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));
        }
    }
}