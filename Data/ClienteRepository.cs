using Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class ClienteRepository : IClienteRepository
    {
        // 1. Usa nombres en minúscula como el profe
        private static readonly List<Cliente> clientes = new List<Cliente>();

        // 2. Simula el Auto-Increment del Id que el profe tenía
        private static int nextId = 1;

        public Task AddAsync(Cliente cliente)
        {
            // Simular auto-increment de ID igual que el profe
            cliente.SetIdCliente(nextId);
            nextId++;

            // Aquí el profe actualizaba el País. 
            // Nosotros haríamos lo mismo con Usuario. 
            // Lo dejo comentado para que lo veas; cuando creemos UsuarioRepository lo podremos descomentar.
            /*
            var usuarioRepo = new UsuarioRepository();
            var usuario = usuarioRepo.GetAllSync().FirstOrDefault(u => u.IdUsuario == cliente.IdUsuario);
            if (usuario != null)
                cliente.SetUsuario(usuario);
            */

            clientes.Add(cliente);
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(int id)
        {
            var cliente = clientes.FirstOrDefault(c => c.IdCliente == id);
            if (cliente != null)
            {
                clientes.Remove(cliente);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Cliente?> GetAsync(int id)
        {
            return Task.FromResult(clientes.FirstOrDefault(c => c.IdCliente == id));
        }

        public Task<IEnumerable<Cliente>> GetAllAsync()
        {
            // El profe usa ToList() para asegurar que devuelve una nueva colección
            return Task.FromResult<IEnumerable<Cliente>>(clientes.ToList());
        }

        public Task<bool> UpdateAsync(Cliente cliente)
        {
            var existing = clientes.FirstOrDefault(c => c.IdCliente == cliente.IdCliente);
            if (existing != null)
            {
                existing.SetIdUsuario(cliente.IdUsuario);
                existing.SetNombreCompleto(cliente.NombreCompleto);
                existing.SetDni(cliente.Dni);
                existing.SetTelefono(cliente.Telefono);
                existing.SetFechaNacimiento(cliente.FechaNacimiento);
                existing.SetEstado(cliente.Estado);

                // Igual que en el AddAsync, el profe actualizaba la Navigation Property aquí
                /*
                var usuarioRepo = new UsuarioRepository();
                var usuario = usuarioRepo.GetAllSync().FirstOrDefault(u => u.IdUsuario == cliente.IdUsuario);
                if (usuario != null)
                    existing.SetUsuario(usuario);
                */

                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> DniExistsAsync(string dni, int? excludeId = null)
        {
            // Copiada exactamente la estructura del EmailExistsAsync del profe
            var query = clientes.Where(c => c.Dni.ToLower() == dni.ToLower());
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.IdCliente != excludeId.Value);
            }
            return Task.FromResult(query.Any());
        }
    }
}