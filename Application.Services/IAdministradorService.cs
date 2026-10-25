using DTOs;

namespace Application.Services
{
    public interface IAdministradorService
    {
        Task<AdministradorDTO> AddAsync(AdministradorDTO dto);
        // Baja lógica. idUsuarioLogueado sale del token: un administrador no puede eliminarse a sí mismo.
        Task<bool> DeleteAsync(int id, int? idUsuarioLogueado = null);
        Task<AdministradorDTO?> GetAsync(int id);
        Task<IEnumerable<AdministradorDTO>> GetAllAsync();
        Task<bool> UpdateAsync(AdministradorDTO dto);
    }
}
