using DTOs;

namespace Application.Services
{
    public interface ITipoSuperficieService
    {
        Task<TipoSuperficieDTO> AddAsync(TipoSuperficieDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<TipoSuperficieDTO?> GetAsync(int id);
        Task<IEnumerable<TipoSuperficieDTO>> GetAllAsync();
        Task<IEnumerable<TipoSuperficieDTO>> GetByDeporteAsync(string deporte);
        Task<bool> UpdateAsync(TipoSuperficieDTO dto);
    }
}
