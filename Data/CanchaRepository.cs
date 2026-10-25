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

        // Baja lógica: la cancha queda en la base como Inactiva, así las reservas que la usan
        // conservan su historial.
        public async Task<bool> DeleteAsync(int id)
        {
            var cancha = await _context.Canchas.FindAsync(id);
            if (cancha == null || cancha.Estado == Estado.Inactivo) return false;

            cancha.SetEstado(Estado.Inactivo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Cancha?> GetAsync(int id)
        {
            return await _context.Canchas.FindAsync(id);
        }

        // Solo las activas: las eliminadas (baja lógica) siguen en la base pero no se muestran.
        public async Task<IEnumerable<Cancha>> GetAllAsync()
        {
            return await _context.Canchas.Where(c => c.Estado == Estado.Activo).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Cancha cancha)
        {
            var existing = await _context.Canchas.FindAsync(cancha.IdCancha);
            if (existing == null) return false;

            // El Estado no se cambia al editar: solo lo cambia la baja (DeleteAsync).
            existing.SetNombre(cancha.Nombre);
            existing.SetPrecioPorHora(cancha.PrecioPorHora);
            existing.SetEsTechada(cancha.EsTechada);

            // Nota heredada de la Entrega 1: si cambia el TIPO de cancha (Futbol<->Padel)
            // esto no lo contempla (ni lo contemplaba la versión en memoria). Cambiar el
            // tipo de una entidad ya persistida en una jerarquía TPH requiere borrar y
            // recrear la fila; lo dejamos afuera del alcance de la Entrega 2 a propósito.
            if (existing is CanchaFutbol futbolExistente && cancha is CanchaFutbol futbolNueva)
            {
                futbolExistente.SetTipoFutbol(futbolNueva.TipoFutbol);
                futbolExistente.SetTipoSuperficie(futbolNueva.TipoSuperficie);
            }
            else if (existing is CanchaPadel padelExistente && cancha is CanchaPadel padelNueva)
            {
                padelExistente.SetTipoPared(padelNueva.TipoPared);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
