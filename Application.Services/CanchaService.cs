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
        private readonly ITipoSuperficieRepository _superficieRepository;

        public CanchaService(ICanchaRepository repository, ITipoSuperficieRepository superficieRepository)
        {
            _repository = repository;
            _superficieRepository = superficieRepository;
        }

        public async Task<CanchaDTO> AddAsync(CanchaDTO dto)
        {
            // Toda alta nueva queda Activa; solo la baja la pasa a Inactiva.
            Cancha cancha = await CrearCanchaAsync(dto, idCancha: 0, Estado.Activo);

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
            Cancha canchaModificada = await CrearCanchaAsync(dto, dto.IdCancha, existing.Estado);

            return await _repository.UpdateAsync(canchaModificada);
        }

        // Valida el DTO y arma la clase hija que corresponde (CanchaFutbol o CanchaPadel).
        private async Task<Cancha> CrearCanchaAsync(CanchaDTO dto, int idCancha, Estado estado)
        {
            if (!Enum.TryParse<TipoCancha>(dto.TipoCancha, ignoreCase: true, out var tipo))
                throw new ArgumentException("Tipo de cancha inválido. Use 'Futbol' o 'Padel'.", nameof(dto.TipoCancha));

            int idSuperficie = dto.IdTipoSuperficie > 0 ? dto.IdTipoSuperficie : (dto.TipoSuperficie ?? 0);
            if (idSuperficie <= 0)
                throw new ReglaDeNegocioException("Debe seleccionar un tipo de cancha/superficie válido.");

            var superficie = await _superficieRepository.GetAsync(idSuperficie);
            if (superficie == null || superficie.Estado == Estado.Inactivo)
                throw new ReglaDeNegocioException("El tipo de cancha/superficie seleccionado no existe o está inactivo.");

            if (tipo == TipoCancha.Futbol)
            {
                if (dto.TipoFutbol == null || !Enum.IsDefined(typeof(TipoFutbol), dto.TipoFutbol.Value))
                    throw new ArgumentException("Elegí si la cancha es de fútbol 5, 7 u 11.");

                return new CanchaFutbol(idCancha, dto.Nombre, estado, dto.PrecioPorHora, dto.EsTechada,
                    idSuperficie, (TipoFutbol)dto.TipoFutbol.Value);
            }
            else
            {
                return new CanchaPadel(idCancha, dto.Nombre, estado, dto.PrecioPorHora, dto.EsTechada, idSuperficie);
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
                IdTipoSuperficie = c.IdTipoSuperficie,
                NombreSuperficie = c.TipoSuperficie?.Nombre,
                CantidadJugadores = c.CantidadJugadores,
                Caracteristicas = c.Caracteristicas
            };

            if (c is CanchaFutbol futbol)
            {
                dto.TipoCancha = nameof(TipoCancha.Futbol);
                dto.TipoFutbol = (int)futbol.TipoFutbol;
            }
            else if (c is CanchaPadel)
            {
                dto.TipoCancha = nameof(TipoCancha.Padel);
            }

            return dto;
        }
    }
}
