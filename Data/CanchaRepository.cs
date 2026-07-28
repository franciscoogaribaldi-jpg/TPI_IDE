using Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class CanchaRepository : ICanchaRepository
    {
        private static readonly List<Cancha> canchas = new List<Cancha>();
        private static int nextId = 1;

        public Task AddAsync(Cancha cancha)
        {
            // Simular auto-increment de ID igual que el profe
            cancha.SetIdCancha(nextId);
            nextId++;

            canchas.Add(cancha);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var cancha = canchas.FirstOrDefault(c => c.IdCancha == id);
            if (cancha != null)
            {
                canchas.Remove(cancha);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Cancha?> GetAsync(int id)
        {
            return Task.FromResult(canchas.FirstOrDefault(c => c.IdCancha == id));
        }

        public Task<IEnumerable<Cancha>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Cancha>>(canchas.ToList());
        }

        public Task<bool> UpdateAsync(Cancha cancha)
        {
            var existing = canchas.FirstOrDefault(c => c.IdCancha == cancha.IdCancha);
            if (existing != null)
            {
                existing.SetNombre(cancha.Nombre);
                existing.SetEstado(cancha.Estado);
                existing.SetPrecioPorHora(cancha.PrecioPorHora);

                if (existing is CanchaPadel padelExistente && cancha is CanchaPadel padelNueva)
                {
                    padelExistente.SetRaquetas(padelNueva.CantidadRaquetas, padelNueva.PrecioTotalRaquetas);
                }

                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}