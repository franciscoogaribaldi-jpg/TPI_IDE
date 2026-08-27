using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CanchaRepository : ICanchaRepository
    {
        private readonly TPIContext _context;

        public CanchaRepository(TPIContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Cancha cancha)
        {
            _context.Canchas.Add(cancha);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cancha = await _context.Canchas.FindAsync(id);
            if (cancha == null) return false;

            _context.Canchas.Remove(cancha);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Cancha?> GetAsync(int id)
        {
            return await _context.Canchas.FindAsync(id);
        }

        public async Task<IEnumerable<Cancha>> GetAllAsync()
        {
            return await _context.Canchas.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Cancha cancha)
        {
            var existing = await _context.Canchas.FindAsync(cancha.IdCancha);
            if (existing == null) return false;

            existing.SetNombre(cancha.Nombre);
            existing.SetEstado(cancha.Estado);
            existing.SetPrecioPorHora(cancha.PrecioPorHora);

            // Nota heredada de la Entrega 1: si cambia el TIPO de cancha (Futbol<->Padel)
            // esto no lo contempla (ni lo contemplaba la versión en memoria). Cambiar el
            // tipo de una entidad ya persistida en una jerarquía TPH requiere borrar y
            // recrear la fila; lo dejamos afuera del alcance de la Entrega 2 a propósito.
            if (existing is CanchaPadel padelExistente && cancha is CanchaPadel padelNueva)
            {
                padelExistente.SetRaquetas(padelNueva.CantidadRaquetas, padelNueva.PrecioTotalRaquetas);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
