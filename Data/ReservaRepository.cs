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

            // Baja lógica de una reserva = cancelarla. Queda registrada (sirve para los reportes)
            // y, como la doble reserva no cuenta las canceladas, libera la cancha y el turno.
            reserva.SetEstadoReserva(EstadoReserva.Cancelada);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Reserva?> GetAsync(int id)
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Cancha)
                .Include(r => r.Turno)
                .Include(r => r.Administrador)
                .Include(r => r.Detalles)
                .FirstOrDefaultAsync(r => r.IdReserva == id);
        }

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Cancha)
                .Include(r => r.Turno)
                .Include(r => r.Administrador)
                .Include(r => r.Detalles)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Reserva reserva)
        {
            var existing = await _context.Reservas
                .Include(r => r.Detalles)
                .FirstOrDefaultAsync(r => r.IdReserva == reserva.IdReserva);
            if (existing == null) return false;

            existing.SetIdCliente(reserva.IdCliente);
            existing.SetIdCancha(reserva.IdCancha);
            existing.SetIdTurno(reserva.IdTurno);
            existing.SetFecha(reserva.Fecha);
            existing.SetEstadoReserva(reserva.EstadoReserva);
            existing.SetImportes(reserva.ImporteTotal, reserva.Sena);

            _context.DetallesReserva.RemoveRange(existing.Detalles.ToList());
            foreach (var linea in reserva.Detalles)
                existing.AgregarDetalle(linea);

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