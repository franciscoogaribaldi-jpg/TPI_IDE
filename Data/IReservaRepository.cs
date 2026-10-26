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

        // Para el rol Cliente: sus propias reservas.
        Task<IEnumerable<Reserva>> GetByClienteAsync(int idCliente);

        // Reservas no canceladas de un día (para calcular los turnos libres).
        Task<IEnumerable<Reserva>> GetActivasPorFechaAsync(DateTime fecha);

        // DeleteAsync es una baja lógica: pasa la reserva a Cancelada, no la borra.
    }
}