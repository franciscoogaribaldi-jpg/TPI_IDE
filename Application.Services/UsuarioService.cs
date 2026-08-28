using Application.Services.Exceptions;
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<UsuarioDTO> AddAsync(UsuarioDTO dto)
        {
            if (!Enum.IsDefined(typeof(RolUsuario), dto.Rol))
                throw new ArgumentException("El rol indicado no es válido.", nameof(dto.Rol));

            // REGLAS DE NEGOCIO: usuario y email únicos (pedido explícito de la propuesta)
            if (await _repository.NombreUsuarioExistsAsync(dto.NombreUsuario))
                throw new ReglaDeNegocioException("Ya existe un usuario con ese nombre de usuario.");

            if (await _repository.EmailExistsAsync(dto.Email))
                throw new ReglaDeNegocioException("Ya existe un usuario con ese email.");

            var usuario = new Usuario(
                idUsuario: 0,
                nombreUsuario: dto.NombreUsuario,
                contrasena: PasswordHasher.Hash(dto.Contrasena),
                email: dto.Email,
                rol: (RolUsuario)dto.Rol,
                estado: Estado.Activo);

            await _repository.AddAsync(usuario);

            dto.IdUsuario = usuario.IdUsuario;
            dto.Estado = (int)usuario.Estado; // todo alta nueva queda Activo, independientemente de lo que trajera el DTO
            dto.Contrasena = string.Empty; // nunca devolvemos ni la contraseña ni el hash
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<UsuarioDTO?> GetAsync(int id)
        {
            var usuario = await _repository.GetAsync(id);
            return usuario == null ? null : MapToDto(usuario);
        }

        public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
        {
            var usuarios = await _repository.GetAllAsync();
            return usuarios.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(UsuarioDTO dto)
        {
            if (!Enum.IsDefined(typeof(RolUsuario), dto.Rol))
                throw new ArgumentException("El rol indicado no es válido.", nameof(dto.Rol));

            if (!Enum.IsDefined(typeof(Estado), dto.Estado))
                throw new ArgumentException("El estado indicado no es válido.", nameof(dto.Estado));

            var existente = await _repository.GetAsync(dto.IdUsuario);
            if (existente == null) return false;

            if (await _repository.NombreUsuarioExistsAsync(dto.NombreUsuario, dto.IdUsuario))
                throw new ReglaDeNegocioException("Ese nombre de usuario ya está en uso por otro usuario.");

            if (await _repository.EmailExistsAsync(dto.Email, dto.IdUsuario))
                throw new ReglaDeNegocioException("Ese email ya está en uso por otro usuario.");

            var usuarioModificado = new Usuario(
                idUsuario: dto.IdUsuario,
                nombreUsuario: dto.NombreUsuario,
                contrasena: existente.Contrasena, // este método no cambia la contraseña
                email: dto.Email,
                rol: (RolUsuario)dto.Rol,
                estado: (Estado)dto.Estado);

            return await _repository.UpdateAsync(usuarioModificado);
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
        {
            var usuario = await _repository.GetByNombreUsuarioAsync(request.NombreUsuario);

            if (usuario == null || usuario.Estado != Estado.Activo)
                return null; // no diferenciamos "no existe" de "está inactivo": no dar pistas

            if (!PasswordHasher.Verify(request.Contrasena, usuario.Contrasena))
                return null;

            return new LoginResponseDTO
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Rol = (int)usuario.Rol
            };
        }

        private static UsuarioDTO MapToDto(Usuario usuario) => new UsuarioDTO
        {
            IdUsuario = usuario.IdUsuario,
            NombreUsuario = usuario.NombreUsuario,
            Email = usuario.Email,
            Rol = (int)usuario.Rol,
            Estado = (int)usuario.Estado,
            Contrasena = string.Empty
        };
    }
}
