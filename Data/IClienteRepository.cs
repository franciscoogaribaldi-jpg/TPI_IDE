using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public interface IClienteRepository
    {
        // El cajero nos dice: "Te devuelvo una Tarea que, cuando termine, adentro tendrá una Lista de Clientes"
        Task<IEnumerable<Cliente>> GetAllAsync(); 

        // "Te devuelvo una Tarea que, cuando termine, tendrá UN Cliente (o nada, por eso el ?)"
        Task<Cliente?> GetAsync(int id);

        // Agregamos esta porque en nuestro sistema validamos por DNI, no por Email
        Task<bool> DniExistsAsync(string dni, int? excludeId = null);

        Task AddAsync(Cliente cliente);

        Task<bool> UpdateAsync(Cliente cliente);

        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Cliente>> GetByCriteriaAsync(ClienteCriteria criteria);
    }
}