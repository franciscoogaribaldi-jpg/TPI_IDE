using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface ITurnoRepository
    {
        Task<IEnumerable<Turno>> GetAllAsync();
        Task<Turno?> GetAsync(int id);
        Task AddAsync(Turno turno);
        Task<bool> UpdateAsync(Turno turno);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExisteSolapadoAsync(TimeSpan horaInicio, TimeSpan horaFin, int? excludeId = null);
    }
}

