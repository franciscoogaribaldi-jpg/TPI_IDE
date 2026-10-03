using Domain.Model;

namespace Data
{
    public interface IReservaRepository
    {
        Task<IEnumerable<Reserva>> GetAllAsync();
        Task<Reserva?> GetAsync(int id);
        Task AddAsync(Reserva reserva);
        Task<bool> UpdateAsync(Reserva reserva);
        Task<bool> DeleteAsync(int id);

        // Evita doble reserva: misma cancha, mismo turno, misma fecha, ya activa.
        Task<bool> ExisteReservaActivaAsync(int idCancha, int idTurno, DateTime fecha, int? excludeId = null);
    }
}