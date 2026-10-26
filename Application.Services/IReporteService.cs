using DTOs;

namespace Application.Services
{
    public interface IReporteService
    {
        Task<IEnumerable<ReporteOcupacionDTO>> GetOcupacionAsync(DateTime desde, DateTime hasta);
        Task<IEnumerable<ReporteRecaudacionDTO>> GetRecaudacionAsync(DateTime desde, DateTime hasta);
    }
}
