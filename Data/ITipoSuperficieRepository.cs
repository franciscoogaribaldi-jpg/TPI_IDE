using Domain.Model;

namespace Data
{
    public interface ITipoSuperficieRepository
    {
        Task AddAsync(TipoSuperficie tipo);
        Task<bool> DeleteAsync(int id);
        Task<TipoSuperficie?> GetAsync(int id);
        Task<IEnumerable<TipoSuperficie>> GetAllAsync();
        Task<IEnumerable<TipoSuperficie>> GetByDeporteAsync(string deporte);
        Task<bool> UpdateAsync(TipoSuperficie tipo);
        Task<bool> ExisteNombreAsync(string nombre, string deporte, int? excludeId = null);
        Task<bool> TieneCanchasAsociadasAsync(int idTipoSuperficie);
    }
}
