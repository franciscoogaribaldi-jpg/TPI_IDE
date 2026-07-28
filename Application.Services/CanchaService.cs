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
            Cancha cancha;

            // Lógica para decidir qué tipo de cancha crear según lo que manden en el JSON
            if (dto.TipoCancha?.ToLower() == "futbol")
            {
                cancha = new CanchaFutbol(0, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora);
            }
            else if (dto.TipoCancha?.ToLower() == "padel")
            {
                cancha = new CanchaPadel(0, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora,
                    dto.CantidadRaquetas ?? 0, dto.PrecioTotalRaquetas ?? 0);
            }
            else
            {
                throw new Exception("Tipo de cancha inválido. Use 'Futbol' o 'Padel'.");
            }

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

            // Convertir Modelo a DTO
            var dtos = canchas.Select(c =>
            {
                var dto = new CanchaDTO
                {
                    IdCancha = c.IdCancha,
                    Nombre = c.Nombre,
                    Estado = (int)c.Estado,
                    PrecioPorHora = c.PrecioPorHora
                };

                // Identificamos de qué tipo es para rellenar la info extra en el DTO
                if (c is CanchaFutbol)
                {
                    dto.TipoCancha = "Futbol";
                }
                else if (c is CanchaPadel padel)
                {
                    dto.TipoCancha = "Padel";
                    dto.CantidadRaquetas = padel.CantidadRaquetas;
                    dto.PrecioTotalRaquetas = padel.PrecioTotalRaquetas;
                }

                return dto;
            });

            return dtos;
        }

        public async Task<CanchaDTO?> GetAsync(int id)
        {
            var c = await _repository.GetAsync(id);
            if (c == null) return null;

            var dto = new CanchaDTO
            {
                IdCancha = c.IdCancha,
                Nombre = c.Nombre,
                Estado = (int)c.Estado,
                PrecioPorHora = c.PrecioPorHora
            };

            if (c is CanchaFutbol)
            {
                dto.TipoCancha = "Futbol";
            }
            else if (c is CanchaPadel padel)
            {
                dto.TipoCancha = "Padel";
                dto.CantidadRaquetas = padel.CantidadRaquetas;
                dto.PrecioTotalRaquetas = padel.PrecioTotalRaquetas;
            }

            return dto;
        }

        public async Task<bool> UpdateAsync(CanchaDTO dto)
        {
            var existing = await _repository.GetAsync(dto.IdCancha);
            if (existing == null) return false;

            Cancha canchaModificada;

            if (dto.TipoCancha?.ToLower() == "futbol")
            {
                canchaModificada = new CanchaFutbol(dto.IdCancha, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora);
            }
            else if (dto.TipoCancha?.ToLower() == "padel")
            {
                canchaModificada = new CanchaPadel(dto.IdCancha, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora,
                    dto.CantidadRaquetas ?? 0, dto.PrecioTotalRaquetas ?? 0);
            }
            else
            {
                throw new Exception("Tipo de cancha inválido.");
            }

            return await _repository.UpdateAsync(canchaModificada);
        }
    }
}