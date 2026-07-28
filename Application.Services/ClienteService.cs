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

        // Aquí inyectamos el repositorio 
        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClienteDTO> AddAsync(ClienteDTO dto)
        {
            // REGLA DE NEGOCIO: Validamos que el DNI no exista
            bool existeDni = await _repository.DniExistsAsync(dto.Dni);
            if (existeDni)
                throw new Exception("Ya existe un cliente con ese DNI.");

            // Convertimos DTO (caja de envío) a Modelo (la clase real con validaciones)
            var cliente = new Cliente(
                idCliente: 0, // El Repositorio le va a poner el número de verdad (el nextId)
                idUsuario: dto.IdUsuario,
                nombre: dto.Nombre,
                apellido: dto.Apellido,
                dni: dto.Dni,
                telefono: dto.Telefono,
                fechaNacimiento: dto.FechaNacimiento,
                estado: (Estado)dto.Estado
            );

            // Lo guardamos en la "base de datos"
            await _repository.AddAsync(cliente);

            // Actualizamos la caja con el ID real que nos dio la base de datos y la devolvemos
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

            // Convertimos la lista de Modelos a una lista de DTOs para enviarla por internet
            var dtos = clientes.Select(c => new ClienteDTO
            {
                IdCliente = c.IdCliente,
                IdUsuario = c.IdUsuario,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Dni = c.Dni,
                Telefono = c.Telefono,
                FechaNacimiento = c.FechaNacimiento,
                Estado = (int)c.Estado
            });

            return dtos;
        }

        public async Task<ClienteDTO?> GetAsync(int id)
        {
            var cliente = await _repository.GetAsync(id);
            if (cliente == null) return null;

            return new ClienteDTO
            {
                IdCliente = cliente.IdCliente,
                IdUsuario = cliente.IdUsuario,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Dni = cliente.Dni,
                Telefono = cliente.Telefono,
                FechaNacimiento = cliente.FechaNacimiento,
                Estado = (int)cliente.Estado
            };
        }

        public async Task<bool> UpdateAsync(ClienteDTO dto)
        {
            // Buscamos si de verdad existe el cliente antes de modificarlo
            var clienteExistente = await _repository.GetAsync(dto.IdCliente);
            if (clienteExistente == null) return false;

            // Validamos que el DNI nuevo no le pertenezca a OTRO cliente
            bool existeDni = await _repository.DniExistsAsync(dto.Dni, dto.IdCliente);
            if (existeDni)
                throw new Exception("El DNI ya pertenece a otro cliente.");

            // Convertimos DTO a Modelo
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

            // Se lo damos al Repositorio para que lo reemplace
            return await _repository.UpdateAsync(clienteModificado);
        }

        public async Task<IEnumerable<ClienteDTO>> GetByCriteriaAsync(ClienteCriteriaDTO criteriaDTO)
        {
            var criteria = new ClienteCriteria(criteriaDTO.Texto);

            var clientes = await _repository.GetByCriteriaAsync(criteria);

            return clientes.Select(c => new ClienteDTO
            {
                IdCliente = c.IdCliente,
                IdUsuario = c.IdUsuario,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Dni = c.Dni,
                Telefono = c.Telefono,
                FechaNacimiento = c.FechaNacimiento,
                Estado = (int)c.Estado
            });
        }
    }
}
