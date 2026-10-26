using DTOs;

namespace Application.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO> AddAsync(UsuarioDTO dto);
        Task<bool> DeleteAsync(int id, int? idUsuarioLogueado = null);
        Task<UsuarioDTO?> GetAsync(int id);
        Task<IEnumerable<UsuarioDTO>> GetAllAsync();
        Task<bool> UpdateAsync(UsuarioDTO dto);
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request);
    }
}
