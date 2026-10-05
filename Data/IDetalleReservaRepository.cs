using Domain.Model;

namespace Data
{
    public interface IDetalleReservaRepository
    {
        Task<IEnumerable<DetalleReserva>> GetByReservaAsync(int idReserva);
        Task AddAsync(DetalleReserva detalle);
        Task DeleteByReservaAsync(int idReserva);
    }
}