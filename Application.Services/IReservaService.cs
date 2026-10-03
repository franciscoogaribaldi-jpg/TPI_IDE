using DTOs;

namespace Application.Services
{
    public interface IReservaService
    {
        Task<ReservaDTO> AddAsync(ReservaDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<ReservaDTO?> GetAsync(int id);
        Task<IEnumerable<ReservaDTO>> GetAllAsync();
        Task<bool> UpdateAsync(ReservaDTO dto);
    }
}