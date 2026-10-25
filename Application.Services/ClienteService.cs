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
            // El alta crea el cliente Y su usuario de acceso (rol Cliente) en una sola operación.
            if (string.IsNullOrWhiteSpace(dto.NombreUsuario))
                throw new ArgumentException("El nombre de usuario es obligatorio.", nameof(dto.NombreUsuario));
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("El email es obligatorio.", nameof(dto.Email));
            if (string.IsNullOrWhiteSpace(dto.Contrasena))
                throw new ArgumentException("La contraseña es obligatoria.", nameof(dto.Contrasena));

            // Si ese DNI es de un cliente eliminado (baja lógica), no se crea otro: se lo recupera.
            var existente = await _repository.GetByDniAsync(dto.Dni);
            if (existente != null)
            {
                if (existente.Estado == Estado.Activo)
                    throw new ReglaDeNegocioException("Ya existe un cliente con ese DNI.");

                return await RecuperarAsync(existente, dto);
            }

            // Reglas de negocio: se validan TODAS antes de guardar nada.
            // (los usuarios eliminados siguen guardados con baja lógica, así que también cuentan)
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
                estado: Estado.Activo // todo alta nueva queda Activa; solo la baja la pasa a Inactiva
            );
            cliente.SetUsuario(usuario);

            // Un único SaveChangesAsync inserta el usuario y el cliente: si algo falla,
            // no queda guardado ninguno de los dos.
            await _repository.AddAsync(cliente);

            return MapToDto(cliente);
        }

        // Recupera un cliente eliminado por error: vuelve a Activo con los datos nuevos del
        // formulario, y su usuario también (con el usuario, email y contraseña nuevos). Como es
        // el mismo registro, conserva su historial de reservas.
        private async Task<ClienteDTO> RecuperarAsync(Cliente cliente, ClienteDTO dto)
        {
            // El usuario y el email pueden ser los mismos que tenía; solo se rechazan si son de OTRO usuario.
            if (await _usuarioRepository.NombreUsuarioExistsAsync(dto.NombreUsuario!, cliente.IdUsuario))
                throw new ReglaDeNegocioException("Ya existe un usuario con ese nombre de usuario.");
            if (await _usuarioRepository.EmailExistsAsync(dto.Email!, cliente.IdUsuario))
                throw new ReglaDeNegocioException("Ya existe un usuario con ese email.");

            // Se usan los setters del dominio: las mismas validaciones que en un alta normal.
            cliente.SetNombre(dto.Nombre);
            cliente.SetApellido(dto.Apellido);
            cliente.SetTelefono(dto.Telefono);
            cliente.SetFechaNacimiento(dto.FechaNacimiento);
            cliente.SetEstado(Estado.Activo);

            if (cliente.Usuario != null)
            {
                cliente.Usuario.SetNombreUsuario(dto.NombreUsuario!.Trim());
                cliente.Usuario.SetEmail(dto.Email!.Trim());
                cliente.Usuario.SetContrasena(PasswordHasher.Hash(dto.Contrasena!));
                cliente.Usuario.SetEstado(Estado.Activo);
            }

            // Cliente y usuario se guardan juntos, en un único SaveChangesAsync.
            await _repository.RecuperarAsync(cliente);

            var resultado = MapToDto(cliente);
            resultado.Recuperado = true;
            return resultado;
        }

        // Baja lógica (ver ClienteRepository.DeleteAsync): no se borra, se marca Inactivo.
        public async Task<bool> DeleteAsync(int id)
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
            // Un cliente eliminado (Inactivo) ya no existe para el sistema.
            var cliente = await _repository.GetAsync(id);
            return cliente == null || cliente.Estado == Estado.Inactivo ? null : MapToDto(cliente);
        }

        public async Task<bool> UpdateAsync(ClienteDTO dto)
        {
            var clienteExistente = await _repository.GetAsync(dto.IdCliente);
            if (clienteExistente == null || clienteExistente.Estado == Estado.Inactivo) return false;

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
                estado: clienteExistente.Estado // el Estado no se edita: solo lo cambia la baja
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
