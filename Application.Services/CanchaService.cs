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
            // VALIDACIÓN: antes esto era un cast directo (Estado)dto.Estado; un Estado=99
            // entraba sin error. Corrección pedida por la cátedra sobre la Entrega 1.
            if (!Enum.IsDefined(typeof(Estado), dto.Estado))
                throw new ArgumentException("El estado de la cancha no es válido.", nameof(dto.Estado));

            // VALIDACIÓN: antes era dto.TipoCancha?.ToLower() == "futbol"/"padel" repetido
            // en dos métodos. Enum.TryParse centraliza los valores válidos en un solo lugar
            // (Domain.Model.TipoCancha) y rechaza cualquier otra cosa de forma prolija.
            if (!Enum.TryParse<TipoCancha>(dto.TipoCancha, ignoreCase: true, out var tipo))
                throw new ArgumentException("Tipo de cancha inválido. Use 'Futbol' o 'Padel'.", nameof(dto.TipoCancha));

            Cancha cancha = tipo switch
            {
                TipoCancha.Futbol => new CanchaFutbol(0, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora),
                TipoCancha.Padel => new CanchaPadel(0, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora,
                    dto.CantidadRaquetas ?? 0, dto.PrecioTotalRaquetas ?? 0),
                _ => throw new ArgumentException("Tipo de cancha inválido. Use 'Futbol' o 'Padel'.", nameof(dto.TipoCancha))
            };

            await _repository.AddAsync(cancha);
            dto.IdCancha = cancha.IdCancha;
            return dto;
        }

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
            var c = await _repository.GetAsync(id);
            return c == null ? null : MapToDto(c);
        }

        public async Task<bool> UpdateAsync(CanchaDTO dto)
        {
            var existing = await _repository.GetAsync(dto.IdCancha);
            if (existing == null) return false;

            if (!Enum.IsDefined(typeof(Estado), dto.Estado))
                throw new ArgumentException("El estado de la cancha no es válido.", nameof(dto.Estado));

            if (!Enum.TryParse<TipoCancha>(dto.TipoCancha, ignoreCase: true, out var tipo))
                throw new ArgumentException("Tipo de cancha inválido. Use 'Futbol' o 'Padel'.", nameof(dto.TipoCancha));

            Cancha canchaModificada = tipo switch
            {
                TipoCancha.Futbol => new CanchaFutbol(dto.IdCancha, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora),
                TipoCancha.Padel => new CanchaPadel(dto.IdCancha, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora,
                    dto.CantidadRaquetas ?? 0, dto.PrecioTotalRaquetas ?? 0),
                _ => throw new ArgumentException("Tipo de cancha inválido. Use 'Futbol' o 'Padel'.", nameof(dto.TipoCancha))
            };

            return await _repository.UpdateAsync(canchaModificada);
        }

        private static CanchaDTO MapToDto(Cancha c)
        {
            var dto = new CanchaDTO
            {
                IdCancha = c.IdCancha,
                Nombre = c.Nombre,
                Estado = (int)c.Estado,
                PrecioPorHora = c.PrecioPorHora
            };

            if (c is CanchaFutbol)
            {
                dto.TipoCancha = nameof(TipoCancha.Futbol);
            }
            else if (c is CanchaPadel padel)
            {
                dto.TipoCancha = nameof(TipoCancha.Padel);
                dto.CantidadRaquetas = padel.CantidadRaquetas;
                dto.PrecioTotalRaquetas = padel.PrecioTotalRaquetas;
            }

            return dto;
        }
    }
}
