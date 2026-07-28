using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public interface IClienteRepository
    {
        
        Task<IEnumerable<Cliente>> GetAllAsync();

        Task<Cliente?> GetAsync(int id);

        Task<bool> DniExistsAsync(string dni, int? excludeId = null);

        Task AddAsync(Cliente cliente);

        Task<bool> UpdateAsync(Cliente cliente);

        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Cliente>> GetByCriteriaAsync(ClienteCriteria criteria);
    }
}