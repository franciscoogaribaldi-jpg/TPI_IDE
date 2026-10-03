using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DetalleReservaRepository : IDetalleReservaRepository
    {
        private readonly TPIContext _context;

        public DetalleReservaRepository(TPIContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DetalleReserva>> GetByReservaAsync(int idReserva)
        {
            return await _context.DetallesReserva
                .Where(d => d.IdReserva == idReserva)
                .ToListAsync();
        }

        public async Task AddAsync(DetalleReserva detalle)
        {
            _context.DetallesReserva.Add(detalle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByReservaAsync(int idReserva)
        {
            var lineas = await _context.DetallesReserva
                .Where(d => d.IdReserva == idReserva)
                .ToListAsync();

            _context.DetallesReserva.RemoveRange(lineas);
            await _context.SaveChangesAsync();
        }
    }
}