using Application.Services.Exceptions;
using Data;
using Domain.Model;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;    // usa la interface porque esta 'Bajo Acoplamiento', que significa que no le importa como trabaje con la bd
        private readonly IUsuarioRepository _usuarioRepository;

        public ClienteService(IClienteRepository repository, IUsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ClienteDTO> AddAsync(ClienteDTO dto)
        {
            if (!Enum.IsDefined(typeof(Estado), dto.Estado)) // dto.Estado esta definido en la lista Estado de enum
                throw new ArgumentException("El estado del cliente no es válido.", nameof(dto.Estado));

            // El alta crea el cliente Y su usuario de acceso (rol Cliente) en una sola operación.
            if (string.IsNullOrWhiteSpace(dto.NombreUsuario))
                throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(dto.NombreUsuario));
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("El email es obligatorio.", nameof(dto.Email));
            if (string.IsNullOrWhiteSpace(dto.Contrasena))
                throw new ArgumentException("La contraseña es obligatoria.", nameof(dto.Contrasena));

            // Reglas de negocio: se validan TODAS antes de guardar nada.
            if (await _repository.DniExistsAsync(dto.Dni))
                throw new ReglaDeNegocioException("Ya existe un cliente con ese DNI.");
            if (await _usuarioRepository.NombreUsuarioExistsAsync(dto.NombreUsuario))
                throw new ReglaDeNegocioException("Ya existe un usuario con ese nombre de usuario.");
            if (await _usuarioRepository.EmailExistsAsync(dto.Email))
                throw new ReglaDeNegocioException("Ya existe un usuario con ese email.");

            // La contraseña se guarda hasheada, igual que en UsuarioService.
            var usuario = new Usuario(
                idUsuario: 0,
                nombreUsuario: dto.NombreUsuario.Trim(),
                contrasena: PasswordHasher.Hash(dto.Contrasena),
                email: dto.Email.Trim(),
                rol: RolUsuario.Cliente,
                estado: Estado.Activo);

            var cliente = new Cliente(
                idCliente: 0,
                idUsuario: 0, // todavía no existe: se completa al guardar
                nombre: dto.Nombre,
                apellido: dto.Apellido,
                dni: dto.Dni,
                telefono: dto.Telefono,
                fechaNacimiento: dto.FechaNacimiento,
                estado: (Estado)dto.Estado
            );
            cliente.SetUsuario(usuario);

            // Un único SaveChangesAsync inserta el usuario y el cliente: si algo falla,
            // no queda guardado ninguno de los dos.
            await _repository.AddAsync(cliente);

            return MapToDto(cliente);
        }

        public async Task<bool> DeleteAsync(int id) // esta parte de la capa de servicios esta solo para cumplir con la arquitectura porque hasta ahora solo sirve para llamar a repository
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ClienteDTO>> GetAllAsync()
        {
            var clientes = await _repository.GetAllAsync();
            return clientes.Select(MapToDto);
        }

        public async Task<ClienteDTO?> GetAsync(int id)
        {
            var cliente = await _repository.GetAsync(id);
            return cliente == null ? null : MapToDto(cliente);
        }

        public async Task<bool> UpdateAsync(ClienteDTO dto)
        {
            var clienteExistente = await _repository.GetAsync(dto.IdCliente);
            if (clienteExistente == null) return false;

            if (!Enum.IsDefined(typeof(Estado), dto.Estado))
                throw new ArgumentException("El estado del cliente no es válido.", nameof(dto.Estado));

            var usuario = await _usuarioRepository.GetAsync(dto.IdUsuario);
            if (usuario == null)
                throw new ReglaDeNegocioException("El usuario indicado no existe.");

            bool existeDni = await _repository.DniExistsAsync(dto.Dni, dto.IdCliente);
            if (existeDni)
                throw new ReglaDeNegocioException("El DNI ya pertenece a otro cliente.");

            var clienteModificado = new Cliente(
                idCliente: dto.IdCliente,
                idUsuario: dto.IdUsuario,
                nombre: dto.Nombre,
                apellido: dto.Apellido,
                dni: dto.Dni,
                telefono: dto.Telefono,
                fechaNacimiento: dto.FechaNacimiento,
                estado: (Estado)dto.Estado // casteo del estado
            );
            clienteModificado.SetUsuario(usuario);

            return await _repository.UpdateAsync(clienteModificado);
        }

        public async Task<IEnumerable<ClienteDTO>> GetByCriteriaAsync(ClienteCriteriaDTO criteriaDTO)
        {
            var criteria = new ClienteCriteria(criteriaDTO.Texto);
            var clientes = await _repository.GetByCriteriaAsync(criteria);
            return clientes.Select(MapToDto);
        }

        private static ClienteDTO MapToDto(Cliente c) => new ClienteDTO
        {
            IdCliente = c.IdCliente,
            IdUsuario = c.IdUsuario,
            Nombre = c.Nombre,
            Apellido = c.Apellido,
            Dni = c.Dni,
            Telefono = c.Telefono,
            FechaNacimiento = c.FechaNacimiento,
            Estado = (int)c.Estado,
            NombreUsuario = c.Usuario?.NombreUsuario,
            Email = c.Usuario?.Email
        };
    }
}
