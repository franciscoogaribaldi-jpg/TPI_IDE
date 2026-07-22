using DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICanchaService
    {
        Task<CanchaDTO> AddAsync(CanchaDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<CanchaDTO?> GetAsync(int id);
        Task<IEnumerable<CanchaDTO>> GetAllAsync();
        Task<bool> UpdateAsync(CanchaDTO dto);
    }
}