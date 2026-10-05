using Domain.Model;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ITurnoService
    {
        Task<IEnumerable<TurnoDTO>> GetAllAsync();
        Task<TurnoDTO?> GetAsync(int id);
        Task<TurnoDTO> AddAsync(TurnoDTO dto);
        Task<bool> UpdateAsync(TurnoDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
