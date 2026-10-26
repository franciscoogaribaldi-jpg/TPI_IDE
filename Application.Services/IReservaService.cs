using DTOs;

namespace Application.Services
{
    public interface IReservaService
    {
        // idUsuarioQueRegistra sale del token (lo pasa el endpoint), nunca del cuerpo del pedido.
        Task<ReservaDTO> AddAsync(ReservaDTO dto, int? idUsuarioQueRegistra = null);
        Task<bool> DeleteAsync(int id);
        Task<ReservaDTO?> GetAsync(int id);
        Task<IEnumerable<ReservaDTO>> GetAllAsync();
        Task<bool> UpdateAsync(ReservaDTO dto);

        // Rol Cliente: idUsuario sale del token, así cada cliente solo maneja lo suyo.
        Task<IEnumerable<ReservaDTO>> GetMisReservasAsync(int idUsuario);
        Task<ReservaDTO> ReservarComoClienteAsync(ReservaDTO dto, int idUsuario);
        Task<bool> CancelarComoClienteAsync(int idReserva, int idUsuario);
        Task<IEnumerable<TurnoLibreDTO>> GetTurnosLibresAsync(DateTime fecha);
    }
}