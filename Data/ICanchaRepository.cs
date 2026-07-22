using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public interface ICanchaRepository
    {
        Task AddAsync(Cancha cancha);
        Task<bool> DeleteAsync(int id);
        Task<Cancha?> GetAsync(int id);
        Task<IEnumerable<Cancha>> GetAllAsync();
        Task<bool> UpdateAsync(Cancha cancha);
    }
}