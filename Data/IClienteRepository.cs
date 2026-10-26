using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    public interface IClienteRepository
    {
        // El cajero nos dice: "Te devuelvo una Tarea que, cuando termine, adentro tendrá una Lista de Clientes"
        Task<IEnumerable<Cliente>> GetAllAsync(); 

        // "Te devuelvo una Tarea que, cuando termine, tendrá UN Cliente (o nada, por eso el ?)"
        Task<Cliente?> GetAsync(int id);

        // Agregamos esta porque en nuestro sistema validamos por DNI, no por Email
        Task<bool> DniExistsAsync(string dni, int? excludeId = null);

        Task AddAsync(Cliente cliente);

        Task<bool> UpdateAsync(Cliente cliente);

        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Cliente>> GetByCriteriaAsync(ClienteCriteria criteria);

        // ¿Ese usuario ya tiene ficha de cliente? (para no borrar un usuario en uso)
        Task<bool> ExisteConUsuarioAsync(int idUsuario);

        // El cliente que corresponde al usuario logueado (el token trae el IdUsuario).
        Task<Cliente?> GetByIdUsuarioAsync(int idUsuario);

        // Busca por DNI aunque esté eliminado (baja lógica), con su usuario: el alta lo usa para
        // recuperar a un cliente eliminado por error en lugar de crear otro.
        Task<Cliente?> GetByDniAsync(string dni);

        // Guarda un cliente recuperado (ya modificado, el que devolvió GetByDniAsync) y su usuario.
        Task RecuperarAsync(Cliente cliente);
    }
}