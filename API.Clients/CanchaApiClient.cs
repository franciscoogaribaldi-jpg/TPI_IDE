using DTOs;
using System.Net;
using System.Net.Http.Json;

namespace API.Clients
{
    public class CanchaApiClient : BaseApiClient
    {
        public static async Task<CanchaDTO?> GetAsync(int id)
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync($"canchas/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<CanchaDTO>();
        }

        public static async Task<IEnumerable<CanchaDTO>> GetAllAsync()
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync("canchas");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<IEnumerable<CanchaDTO>>() ?? Enumerable.Empty<CanchaDTO>();
        }

        public static async Task<CanchaDTO> AddAsync(CanchaDTO cancha)
        {
            using var client = CreateHttpClient();
            var response = await client.PostAsJsonAsync("canchas", cancha);

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return (await response.Content.ReadFromJsonAsync<CanchaDTO>())!;
        }

        public static async Task UpdateAsync(CanchaDTO cancha)
        {
            using var client = CreateHttpClient();
            var response = await client.PutAsJsonAsync("canchas", cancha);

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));
        }

        public static async Task DeleteAsync(int id)
        {
            using var client = CreateHttpClient();
            var response = await client.DeleteAsync($"canchas/{id}");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));
        }
    }
}
