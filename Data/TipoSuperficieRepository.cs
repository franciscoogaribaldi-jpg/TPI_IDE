using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TipoSuperficieRepository : ITipoSuperficieRepository
    {
        private readonly TPIContext _context;

        public TipoSuperficieRepository(TPIContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TipoSuperficie tipo)
        {
            _context.TiposSuperficie.Add(tipo);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tipo = await _context.TiposSuperficie.FindAsync(id);
            if (tipo == null || tipo.Estado == Estado.Inactivo) return false;

            tipo.SetEstado(Estado.Inactivo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TipoSuperficie?> GetAsync(int id)
        {
            return await _context.TiposSuperficie.FindAsync(id);
        }

        public async Task<IEnumerable<TipoSuperficie>> GetAllAsync()
        {
            return await _context.TiposSuperficie
                .Where(t => t.Estado == Estado.Activo)
                .OrderBy(t => t.Deporte)
                .ThenBy(t => t.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<TipoSuperficie>> GetByDeporteAsync(string deporte)
        {
            return await _context.TiposSuperficie
                .Where(t => t.Estado == Estado.Activo && t.Deporte.ToLower() == deporte.Trim().ToLower())
                .OrderBy(t => t.Nombre)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(TipoSuperficie tipo)
        {
            var existing = await _context.TiposSuperficie.FindAsync(tipo.IdTipoSuperficie);
            if (existing == null || existing.Estado == Estado.Inactivo) return false;

            existing.SetNombre(tipo.Nombre);
            existing.SetDeporte(tipo.Deporte);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteNombreAsync(string nombre, string deporte, int? excludeId = null)
        {
            var query = _context.TiposSuperficie.Where(t =>
                t.Estado == Estado.Activo &&
                t.Nombre.ToLower() == nombre.Trim().ToLower() &&
                t.Deporte.ToLower() == deporte.Trim().ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(t => t.IdTipoSuperficie != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> TieneCanchasAsociadasAsync(int idTipoSuperficie)
        {
            return await _context.Canchas.AnyAsync(c => c.IdTipoSuperficie == idTipoSuperficie && c.Estado == Estado.Activo);
        }
    }
}
