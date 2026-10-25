using Application.Services.Exceptions;
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradorRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;

        public AdministradorService(IAdministradorRepository repository, IUsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<AdministradorDTO> AddAsync(AdministradorDTO dto)
        {
            // El alta crea el administrador Y su usuario de acceso (rol Administrador) en una sola operación.
            if (string.IsNullOrWhiteSpace(dto.NombreUsuario))
                throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(dto.NombreUsuario));
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("El email es obligatorio.", nameof(dto.Email));
            if (string.IsNullOrWhiteSpace(dto.Contrasena))
                throw new ArgumentException("La contraseña es obligatoria.", nameof(dto.Contrasena));

            // Si ese nombre de usuario es de un administrador eliminado (baja lógica), no se crea
            // otro: se lo recupera.
            var usuarioExistente = await _usuarioRepository.GetByNombreUsuarioAsync(dto.NombreUsuario.Trim());
            if (usuarioExistente != null)
            {
                if (usuarioExistente.Estado == Estado.Activo)
                    throw new ReglaDeNegocioException("Ya existe un usuario con ese nombre de usuario.");

                var eliminado = await _repository.GetByIdUsuarioAsync(usuarioExistente.IdUsuario);
                if (eliminado == null)
                    throw new ReglaDeNegocioException("Ese nombre de usuario pertenece a un usuario eliminado que no es administrador.");

                return await RecuperarAsync(eliminado, dto);
            }

            // Reglas de negocio: se validan TODAS antes de guardar nada.
            // (los usuarios eliminados siguen guardados con baja lógica, así que también cuentan)
            if (await _usuarioRepository.EmailExistsAsync(dto.Email))
                throw new ReglaDeNegocioException("Ya existe un usuario con ese email.");

            var usuario = new Usuario(
                idUsuario: 0,
                nombreUsuario: dto.NombreUsuario.Trim(),
                contrasena: PasswordHasher.Hash(dto.Contrasena),
                email: dto.Email.Trim(),
                rol: RolUsuario.Administrador,
                estado: Estado.Activo);

            var administrador = new Administrador(
                idAdministrador: 0,
                idUsuario: 0, // todavía no existe: se completa al guardar
                nombre: dto.Nombre,
                apellido: dto.Apellido,
                telefono: dto.Telefono,
                estado: Estado.Activo); // todo alta nueva queda Activa; solo la baja la pasa a Inactiva
            administrador.SetUsuario(usuario);

            // Un único SaveChangesAsync inserta el usuario y el administrador juntos.
            await _repository.AddAsync(administrador);

            return MapToDto(administrador);
        }

        // Recupera un administrador eliminado por error: vuelve a Activo con los datos nuevos del
        // formulario, y su usuario también (con el email y la contraseña nuevos). Como es el mismo
        // registro, las reservas que había registrado siguen a su nombre.
        private async Task<AdministradorDTO> RecuperarAsync(Administrador administrador, AdministradorDTO dto)
        {
            // El email puede ser el mismo que tenía; solo se rechaza si es de OTRO usuario.
            if (await _usuarioRepository.EmailExistsAsync(dto.Email!, administrador.IdUsuario))
                throw new ReglaDeNegocioException("Ya existe un usuario con ese email.");

            administrador.SetNombre(dto.Nombre);
            administrador.SetApellido(dto.Apellido);
            administrador.SetTelefono(dto.Telefono);
            administrador.SetEstado(Estado.Activo);

            if (administrador.Usuario != null)
            {
                administrador.Usuario.SetEmail(dto.Email!.Trim());
                administrador.Usuario.SetContrasena(PasswordHasher.Hash(dto.Contrasena!));
                administrador.Usuario.SetEstado(Estado.Activo);
            }

            // Administrador y usuario se guardan juntos, en un único SaveChangesAsync.
            await _repository.RecuperarAsync(administrador);

            var resultado = MapToDto(administrador);
            resultado.Recuperado = true;
            return resultado;
        }

        // Baja lógica (ver AdministradorRepository.DeleteAsync): no se borra, se marca Inactivo
        // junto con su usuario. idUsuarioLogueado sale del token (lo pasa el endpoint).
        public async Task<bool> DeleteAsync(int id, int? idUsuarioLogueado = null)
        {
            var administrador = await _repository.GetAsync(id);
            if (administrador != null && administrador.IdUsuario == idUsuarioLogueado)
                throw new ReglaDeNegocioException("No podés eliminarte a vos mismo: te quedarías sin acceso al sistema.");

            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<AdministradorDTO>> GetAllAsync()
        {
            var administradores = await _repository.GetAllAsync();
            return administradores.Select(MapToDto);
        }

        public async Task<AdministradorDTO?> GetAsync(int id)
        {
            // Un administrador eliminado (Inactivo) ya no existe para el sistema.
            var administrador = await _repository.GetAsync(id);
            return administrador == null || administrador.Estado == Estado.Inactivo ? null : MapToDto(administrador);
        }

        public async Task<bool> UpdateAsync(AdministradorDTO dto)
        {
            var existente = await _repository.GetAsync(dto.IdAdministrador);
            if (existente == null || existente.Estado == Estado.Inactivo) return false;

            // El usuario de acceso no cambia en la edición (se gestiona desde Usuarios):
            // se usa el que ya tenía el administrador, no el que mande el DTO.
            var administradorModificado = new Administrador(
                idAdministrador: dto.IdAdministrador,
                idUsuario: existente.IdUsuario,
                nombre: dto.Nombre,
                apellido: dto.Apellido,
                telefono: dto.Telefono,
                estado: existente.Estado); // el Estado no se edita: solo lo cambia la baja

            return await _repository.UpdateAsync(administradorModificado);
        }

        private static AdministradorDTO MapToDto(Administrador a) => new AdministradorDTO
        {
            IdAdministrador = a.IdAdministrador,
            IdUsuario = a.IdUsuario,
            Nombre = a.Nombre,
            Apellido = a.Apellido,
            Telefono = a.Telefono,
            Estado = (int)a.Estado,
            NombreUsuario = a.Usuario?.NombreUsuario,
            Email = a.Usuario?.Email
        };
    }
}
