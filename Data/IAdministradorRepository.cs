using Domain.Model;

namespace Data
{
    public interface IAdministradorRepository
    {
        Task<IEnumerable<Administrador>> GetAllAsync();
        Task<Administrador?> GetAsync(int id);

        // Para saber qué administrador está logueado (el token trae el IdUsuario).
        Task<Administrador?> GetByIdUsuarioAsync(int idUsuario);

        // ¿Ese usuario ya tiene ficha de administrador? (para no borrar un usuario en uso)
        Task<bool> ExisteConUsuarioAsync(int idUsuario);

        Task AddAsync(Administrador administrador);

        // Guarda un administrador recuperado (ya modificado, el que devolvió GetByIdUsuarioAsync) y su usuario.
        Task RecuperarAsync(Administrador administrador);
        Task<bool> UpdateAsync(Administrador administrador);
        Task<bool> DeleteAsync(int id);
    }
}
