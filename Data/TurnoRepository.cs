using Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Data
{
    public class TurnoRepository : ITurnoRepository
    {
        private readonly TPIContext _context;

        public TurnoRepository(TPIContext context) 
        { 
            _context = context;
        }

        public async Task AddAsync(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync(); 
        }

        public async Task<Turno?> GetAsync(int id)
        {
            return await _context.Turnos.FindAsync(id);
        }

        public async Task<IEnumerable<Turno>> GetAllAsync()
        {
            return await _context.Turnos.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Turno turno)
        {
            var existing = await _context.Turnos.FindAsync(turno.IdTurno);
            if (existing == null) return false;

            existing.SetEstado(turno.Estado);
            existing.SetHorarios(turno.HoraInicio, turno.HoraFin);


            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var turno = await _context.Turnos.FindAsync(id); 
            if (turno == null) return false;

            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> ExisteSolapadoAsync(TimeSpan horaInicio, TimeSpan horaFin, int? excludeId = null)
        {
            var query = _context.Turnos.Where(t => horaInicio < t.HoraFin && t.HoraInicio < horaFin);

            if (excludeId.HasValue)
                query = query.Where(t => t.IdTurno != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
