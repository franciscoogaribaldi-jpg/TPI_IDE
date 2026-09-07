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
        private readonly IClienteRepository _repository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ClienteService(IClienteRepository repository, IUsuarioRepository usuarioRepository)
        {
            _repository = repository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<ClienteDTO> AddAsync(ClienteDTO dto)
        {
          
            if (!Enum.IsDefined(typeof(Estado), dto.Estado))
                throw new ArgumentException("El estado del cliente no es válido.", nameof(dto.Estado));

            var usuario = await _usuarioRepository.GetAsync(dto.IdUsuario);
            if (usuario == null)
                throw new ReglaDeNegocioException("El usuario indicado no existe.");

            bool existeDni = await _repository.DniExistsAsync(dto.Dni);
            if (existeDni)
                throw new ReglaDeNegocioException("Ya existe un cliente con ese DNI.");

            var cliente = new Cliente(
                idCliente: 0,
                idUsuario: dto.IdUsuario,
                nombre: dto.Nombre,
                apellido: dto.Apellido,
                dni: dto.Dni,
                telefono: dto.Telefono,
                fechaNacimiento: dto.FechaNacimiento,
                estado: (Estado)dto.Estado
            );

            // CORRECCIÓN DE CÁTEDRA: SetUsuario no se invocaba nunca; la navigation
            // property se quedaba siempre en null aunque IdUsuario tuviera un valor válido.
            cliente.SetUsuario(usuario);

            await _repository.AddAsync(cliente);

            dto.IdCliente = cliente.IdCliente;
            return dto;
        }

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
                estado: (Estado)dto.Estado
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
            Estado = (int)c.Estado
        };
    }
}
