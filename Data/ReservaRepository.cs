using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly TPIContext _context;

        public ReservaRepository(TPIContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return false;

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Reserva?> GetAsync(int id)
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Cancha)
                .Include(r => r.Turno)
                .FirstOrDefaultAsync(r => r.IdReserva == id);
        }

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Cancha)
                .Include(r => r.Turno)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Reserva reserva)
        {
            var existing = await _context.Reservas.FindAsync(reserva.IdReserva);
            if (existing == null) return false;

            existing.SetIdCliente(reserva.IdCliente);
            existing.SetIdCancha(reserva.IdCancha);
            existing.SetIdTurno(reserva.IdTurno);

            // Solo tocamos la fecha si de verdad cambió. Si no, SetFecha explota en
            // cuanto una reserva ya pasó de fecha y solo querés, por ejemplo, marcarla
            // como Finalizada al día siguiente (su propia validación de dominio rechaza
            // cualquier fecha pasada, incluso si es la MISMA que ya tenía guardada).
            if (existing.Fecha != reserva.Fecha)
                existing.SetFecha(reserva.Fecha);

            existing.SetEstadoReserva(reserva.EstadoReserva);
            existing.SetImportes(reserva.ImporteTotal, reserva.Sena);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteReservaActivaAsync(int idCancha, int idTurno, DateTime fecha, int? excludeId = null)
        {
            var query = _context.Reservas.Where(r =>
                r.IdCancha == idCancha &&
                r.IdTurno == idTurno &&
                r.Fecha.Date == fecha.Date &&
                r.EstadoReserva != EstadoReserva.Cancelada);

            if (excludeId.HasValue)
                query = query.Where(r => r.IdReserva != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}