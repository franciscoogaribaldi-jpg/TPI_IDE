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

        // Solo los activos: los eliminados (baja lógica) siguen en la base pero no se muestran.
        public async Task<IEnumerable<Turno>> GetAllAsync()
        {
            return await _context.Turnos.Where(t => t.Estado == Estado.Activo).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Turno turno)
        {
            var existing = await _context.Turnos.FindAsync(turno.IdTurno);
            if (existing == null) return false;

            // El Estado no se cambia al editar: solo lo cambia la baja (DeleteAsync).
            existing.SetHorarios(turno.HoraInicio, turno.HoraFin);

            await _context.SaveChangesAsync();
            return true;
        }

        // Baja lógica: el turno queda en la base como Inactivo, así las reservas que lo usan
        // conservan su historial.
        public async Task<bool> DeleteAsync(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null || turno.Estado == Estado.Inactivo) return false;

            turno.SetEstado(Estado.Inactivo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteSolapadoAsync(TimeSpan horaInicio, TimeSpan horaFin, int? excludeId = null)
        {
            // Un turno eliminado no ocupa más su horario.
            var query = _context.Turnos.Where(t => t.Estado == Estado.Activo && horaInicio < t.HoraFin && t.HoraInicio < horaFin);

            if (excludeId.HasValue)
                query = query.Where(t => t.IdTurno != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}
