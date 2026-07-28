using Domain.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public class ClienteRepository : IClienteRepository
    {
        
        private static readonly List<Cliente> clientes = new List<Cliente>();

        
        private static int nextId = 1;

        public Task AddAsync(Cliente cliente)
        {
            // Simular auto-increment de ID
            cliente.SetIdCliente(nextId);
            nextId++;

             
            // cuando creemos UsuarioRepository lo descomentamoe.
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
            
            return Task.FromResult<IEnumerable<Cliente>>(clientes.ToList());
        }

        public Task<bool> UpdateAsync(Cliente cliente)
        {
            var existing = clientes.FirstOrDefault(c => c.IdCliente == cliente.IdCliente);
            if (existing != null)
            {
                existing.SetIdUsuario(cliente.IdUsuario);
                existing.SetNombre(cliente.Nombre);
                existing.SetApellido(cliente.Apellido);
                existing.SetDni(cliente.Dni);
                existing.SetTelefono(cliente.Telefono);
                existing.SetFechaNacimiento(cliente.FechaNacimiento);
                existing.SetEstado(cliente.Estado);

                // lo descomentamos al crear UsuarioRepository
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

        public Task<IEnumerable<Cliente>> GetByCriteriaAsync(ClienteCriteria criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.Texto))
            {
                return Task.FromResult<IEnumerable<Cliente>>(clientes.ToList());
            }

            string busqueda = criteria.Texto.ToLower();

            var filtrados = clientes.Where(c => 
                (c.Nombre != null && c.Nombre.ToLower().Contains(busqueda)) ||
                (c.Apellido != null && c.Apellido.ToLower().Contains(busqueda)) ||
                (c.Dni != null && c.Dni.ToLower().Contains(busqueda))
            ).ToList();

            return Task.FromResult<IEnumerable<Cliente>>(filtrados);
        }
    }
}