using Application.Services.Exceptions;
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IAdministradorRepository _administradorRepository;

        public UsuarioService(IUsuarioRepository repository, IClienteRepository clienteRepository, IAdministradorRepository administradorRepository)
        {
            _repository = repository;
            _clienteRepository = clienteRepository;
            _administradorRepository = administradorRepository;
        }

        public async Task<UsuarioDTO> AddAsync(UsuarioDTO dto)
        {
            if (!Enum.IsDefined(typeof(RolUsuario), dto.Rol))
                throw new ArgumentException("El rol indicado no es válido.", nameof(dto.Rol));

            // Si ese nombre de usuario es de un usuario eliminado (baja lógica), no se crea otro:
            // se lo recupera.
            var existente = await _repository.GetByNombreUsuarioAsync(dto.NombreUsuario.Trim());
            if (existente != null)
            {
                if (existente.Estado == Estado.Activo)
                    throw new ReglaDeNegocioException("Ya existe un usuario con ese nombre de usuario.");

                // El usuario de un cliente o administrador se recupera desde su pantalla, junto con la persona.
                if (await TieneFichaAsync(existente.IdUsuario))
                    throw new ReglaDeNegocioException("Ese usuario es de un cliente o administrador eliminado: cargalo de nuevo desde la pantalla de Clientes o Administradores y se recupera solo.");

                return await RecuperarAsync(existente, dto);
            }

            // REGLAS DE NEGOCIO: usuario y email únicos (pedido explícito de la propuesta)
            // (los eliminados siguen guardados con baja lógica, así que también cuentan)
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

        // Recupera un usuario eliminado por error: vuelve a Activo con el email, el rol y la
        // contraseña nuevos del formulario.
        private async Task<UsuarioDTO> RecuperarAsync(Usuario usuario, UsuarioDTO dto)
        {
            // El email puede ser el mismo que tenía; solo se rechaza si es de OTRO usuario.
            if (await _repository.EmailExistsAsync(dto.Email, usuario.IdUsuario))
                throw new ReglaDeNegocioException("Ya existe un usuario con ese email.");

            usuario.SetEmail(dto.Email.Trim());
            usuario.SetRol((RolUsuario)dto.Rol);
            usuario.SetContrasena(PasswordHasher.Hash(dto.Contrasena));
            usuario.SetEstado(Estado.Activo);

            await _repository.RecuperarAsync(usuario);

            var resultado = MapToDto(usuario);
            resultado.Recuperado = true;
            return resultado;
        }

        // Baja lógica (ver UsuarioRepository.DeleteAsync): no se borra, se marca Inactivo y ya
        // no puede iniciar sesión. Si está asociado a un cliente o administrador, da de baja
        // también a la persona para que no quede una ficha activa sin acceso.
        public async Task<bool> DeleteAsync(int id, int? idUsuarioLogueado = null)
        {
            if (idUsuarioLogueado.HasValue && id == idUsuarioLogueado.Value)
                throw new ReglaDeNegocioException("No podés eliminarte a vos mismo: te quedarías sin acceso al sistema.");

            var usuario = await _repository.GetAsync(id);
            if (usuario == null || usuario.Estado == Estado.Inactivo)
                return false;

            // Si el usuario pertenece a un administrador activo, damos de baja la ficha también
            var admin = await _administradorRepository.GetByIdUsuarioAsync(id);
            if (admin != null && admin.Estado == Estado.Activo)
            {
                await _administradorRepository.DeleteAsync(admin.IdAdministrador);
                return true;
            }

            // Si el usuario pertenece a un cliente activo, damos de baja la ficha también
            var cliente = await _clienteRepository.GetByIdUsuarioAsync(id);
            if (cliente != null && cliente.Estado == Estado.Activo)
            {
                await _clienteRepository.DeleteAsync(cliente.IdCliente);
                return true;
            }

            return await _repository.DeleteAsync(id);
        }

        private async Task<bool> TieneFichaAsync(int idUsuario)
        {
            return await _clienteRepository.ExisteConUsuarioAsync(idUsuario)
                || await _administradorRepository.ExisteConUsuarioAsync(idUsuario);
        }

        public async Task<UsuarioDTO?> GetAsync(int id)
        {
            // Un usuario eliminado (Inactivo) ya no existe para el sistema.
            var usuario = await _repository.GetAsync(id);
            return usuario == null || usuario.Estado == Estado.Inactivo ? null : MapToDto(usuario);
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

            var existente = await _repository.GetAsync(dto.IdUsuario);
            if (existente == null || existente.Estado == Estado.Inactivo) return false;

            if (await _repository.NombreUsuarioExistsAsync(dto.NombreUsuario, dto.IdUsuario))
                throw new ReglaDeNegocioException("Ese nombre de usuario ya está en uso por otro usuario.");

            if (await _repository.EmailExistsAsync(dto.Email, dto.IdUsuario))
                throw new ReglaDeNegocioException("Ese email ya está en uso por otro usuario.");

            // Un cliente no puede pasar a ser administrador (ni al revés) cambiando solo el rol:
            // quedaría con la ficha equivocada.
            if (dto.Rol != (int)existente.Rol && await TieneFichaAsync(dto.IdUsuario))
                throw new ReglaDeNegocioException("No se puede cambiar el rol de un usuario que tiene ficha de cliente o administrador.");

            var usuarioModificado = new Usuario(
                idUsuario: dto.IdUsuario,
                nombreUsuario: dto.NombreUsuario,
                contrasena: existente.Contrasena, // este método no cambia la contraseña
                email: dto.Email,
                rol: (RolUsuario)dto.Rol,
                estado: existente.Estado); // el Estado no se edita: solo lo cambia la baja

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
