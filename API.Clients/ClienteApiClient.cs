using DTOs;
using System.Net;
using System.Net.Http.Json;

namespace API.Clients
{
    public class ClienteApiClient : BaseApiClient
    {
        public static async Task<ClienteDTO?> GetAsync(int id)
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync($"clientes/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<ClienteDTO>();
        }

        public static async Task<IEnumerable<ClienteDTO>> GetAllAsync()
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync("clientes");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<IEnumerable<ClienteDTO>>() ?? Enumerable.Empty<ClienteDTO>();
        }

        public static async Task<IEnumerable<ClienteDTO>> BuscarAsync(string texto)
        {
            using var client = CreateHttpClient();
            var response = await client.GetAsync($"clientes/buscar?texto={Uri.EscapeDataString(texto)}");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return await response.Content.ReadFromJsonAsync<IEnumerable<ClienteDTO>>() ?? Enumerable.Empty<ClienteDTO>();
        }

        public static async Task<ClienteDTO> AddAsync(ClienteDTO cliente)
        {
            using var client = CreateHttpClient();
            var response = await client.PostAsJsonAsync("clientes", cliente);

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));

            return (await response.Content.ReadFromJsonAsync<ClienteDTO>())!;
        }

        public static async Task UpdateAsync(ClienteDTO cliente)
        {
            using var client = CreateHttpClient();
            var response = await client.PutAsJsonAsync("clientes", cliente);

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));
        }

        public static async Task DeleteAsync(int id)
        {
            using var client = CreateHttpClient();
            var response = await client.DeleteAsync($"clientes/{id}");

            if (!response.IsSuccessStatusCode) throw new Exception(await LeerMensajeDeErrorAsync(response));
        }
    }
}
