using DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IClienteService
    {
        Task<ClienteDTO> AddAsync(ClienteDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<ClienteDTO?> GetAsync(int id);
        Task<IEnumerable<ClienteDTO>> GetAllAsync();
        Task<bool> UpdateAsync(ClienteDTO dto);
        Task<IEnumerable<ClienteDTO>> GetByCriteriaAsync(ClienteCriteriaDTO criteriaDTO);
    }
}