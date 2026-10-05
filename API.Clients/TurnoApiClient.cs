using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class TurnoApiClient : BaseApiClient
    {
        public static async Task<IEnumerable<TurnoDTO>> GetAllAsync()
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync("turnos");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<IEnumerable<TurnoDTO>>() ?? Enumerable.Empty<TurnoDTO>();
        }
    }
}