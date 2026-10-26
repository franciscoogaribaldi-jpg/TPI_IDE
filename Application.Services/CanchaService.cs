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
    public class CanchaService : ICanchaService
    {
        private readonly ICanchaRepository _repository;

        public CanchaService(ICanchaRepository repository)
        {
            _repository = repository;
        }

        public async Task<CanchaDTO> AddAsync(CanchaDTO dto)
        {
            // Toda alta nueva queda Activa; solo la baja la pasa a Inactiva.
            Cancha cancha = CrearCancha(dto, idCancha: 0, Estado.Activo);

            await _repository.AddAsync(cancha);
            return MapToDto(cancha);
        }

        // Baja lógica (ver CanchaRepository.DeleteAsync): no se borra, se marca Inactiva.
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CanchaDTO>> GetAllAsync()
        {
            var canchas = await _repository.GetAllAsync();
            return canchas.Select(MapToDto);
        }

        public async Task<CanchaDTO?> GetAsync(int id)
        {
            // Una cancha eliminada (Inactiva) ya no existe para el sistema.
            var c = await _repository.GetAsync(id);
            return c == null || c.Estado == Estado.Inactivo ? null : MapToDto(c);
        }

        public async Task<bool> UpdateAsync(CanchaDTO dto)
        {
            var existing = await _repository.GetAsync(dto.IdCancha);
            if (existing == null || existing.Estado == Estado.Inactivo) return false;

            // El Estado no se edita: se conserva el que tiene.
            Cancha canchaModificada = CrearCancha(dto, dto.IdCancha, existing.Estado);

            return await _repository.UpdateAsync(canchaModificada);
        }

        // Valida el DTO y arma la clase hija que corresponde (CanchaFutbol o CanchaPadel).
        // La usan el alta y la modificación, así las reglas están en un solo lugar.
        // El Estado no viene del DTO: lo decide el servicio (ver AddAsync y UpdateAsync).
        private static Cancha CrearCancha(CanchaDTO dto, int idCancha, Estado estado)
        {
            // VALIDACIÓN: Enum.TryParse centraliza los valores válidos en un solo lugar
            // (Domain.Model.TipoCancha) y rechaza cualquier otra cosa de forma prolija.
            if (!Enum.TryParse<TipoCancha>(dto.TipoCancha, ignoreCase: true, out var tipo)) // nos fijamos si es valido y si lo es lo pasamos a Enum
                throw new ArgumentException("Tipo de cancha inválido. Use 'Futbol' o 'Padel'.", nameof(dto.TipoCancha));

            // Los enums viajan como int en el DTO: se valida con Enum.IsDefined antes de castear.
            if (tipo == TipoCancha.Futbol)
            {
                if (dto.TipoFutbol == null || !Enum.IsDefined(typeof(TipoFutbol), dto.TipoFutbol.Value))
                    throw new ArgumentException("Elegí si la cancha es de fútbol 5, 7 u 11.");
                if (dto.TipoSuperficie == null || !Enum.IsDefined(typeof(TipoSuperficie), dto.TipoSuperficie.Value))
                    throw new ArgumentException("El tipo de superficie no es válido.");

                return new CanchaFutbol(idCancha, dto.Nombre, estado, dto.PrecioPorHora, dto.EsTechada,
                    (TipoFutbol)dto.TipoFutbol.Value, (TipoSuperficie)dto.TipoSuperficie.Value);
            }
            else
            {
                if (dto.TipoPared == null || !Enum.IsDefined(typeof(TipoPared), dto.TipoPared.Value))
                    throw new ArgumentException("El tipo de pared no es válido.");

                return new CanchaPadel(idCancha, dto.Nombre, estado, dto.PrecioPorHora, dto.EsTechada,
                    (TipoPared)dto.TipoPared.Value);
            }
        }

        private static CanchaDTO MapToDto(Cancha c)
        {
            var dto = new CanchaDTO
            {
                IdCancha = c.IdCancha,
                Nombre = c.Nombre,
                Estado = (int)c.Estado,
                PrecioPorHora = c.PrecioPorHora,
                EsTechada = c.EsTechada,
                // Polimorfismo: cada hija responde a su manera, el servicio no pregunta el tipo.
                CantidadJugadores = c.CantidadJugadores,
                Caracteristicas = c.Caracteristicas
            };

            if (c is CanchaFutbol futbol)
            {
                dto.TipoCancha = nameof(TipoCancha.Futbol);
                dto.TipoFutbol = (int)futbol.TipoFutbol;
                dto.TipoSuperficie = (int)futbol.TipoSuperficie;
            }
            else if (c is CanchaPadel padel)
            {
                dto.TipoCancha = nameof(TipoCancha.Padel);
                dto.TipoPared = (int)padel.TipoPared;
            }

            return dto;
        }
    }
}
