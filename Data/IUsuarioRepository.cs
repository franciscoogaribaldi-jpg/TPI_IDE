using Domain.Model;

namespace Data
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetAsync(int id);
        Task<Usuario?> GetByNombreUsuarioAsync(string nombreUsuario);
        Task<bool> NombreUsuarioExistsAsync(string nombreUsuario, int? excludeId = null);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
        Task AddAsync(Usuario usuario);
        Task<bool> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
    }
}
