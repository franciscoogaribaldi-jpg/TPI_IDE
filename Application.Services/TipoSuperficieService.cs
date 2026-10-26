using Application.Services.Exceptions;
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class TipoSuperficieService : ITipoSuperficieService
    {
        private readonly ITipoSuperficieRepository _repository;

        public TipoSuperficieService(ITipoSuperficieRepository repository)
        {
            _repository = repository;
        }

        public async Task<TipoSuperficieDTO> AddAsync(TipoSuperficieDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre es obligatorio.", nameof(dto.Nombre));
            if (string.IsNullOrWhiteSpace(dto.Deporte))
                throw new ArgumentException("El deporte es obligatorio.", nameof(dto.Deporte));

            var dep = dto.Deporte.Trim();
            if (!dep.Equals("Futbol", StringComparison.OrdinalIgnoreCase) &&
                !dep.Equals("Padel", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("El deporte debe ser 'Futbol' o 'Padel'.", nameof(dto.Deporte));
            }

            if (await _repository.ExisteNombreAsync(dto.Nombre, dep))
                throw new ReglaDeNegocioException("Ya existe un tipo de cancha con ese nombre para este deporte.");

            var entidad = new TipoSuperficie(0, dto.Nombre, dep, Estado.Activo);
            await _repository.AddAsync(entidad);

            return MapToDto(entidad);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (await _repository.TieneCanchasAsociadasAsync(id))
                throw new ReglaDeNegocioException("No se puede eliminar porque existen canchas activas asociadas a este tipo.");

            return await _repository.DeleteAsync(id);
        }

        public async Task<TipoSuperficieDTO?> GetAsync(int id)
        {
            var tipo = await _repository.GetAsync(id);
            return tipo == null || tipo.Estado == Estado.Inactivo ? null : MapToDto(tipo);
        }

        public async Task<IEnumerable<TipoSuperficieDTO>> GetAllAsync()
        {
            var lista = await _repository.GetAllAsync();
            return lista.Select(MapToDto);
        }

        public async Task<IEnumerable<TipoSuperficieDTO>> GetByDeporteAsync(string deporte)
        {
            var lista = await _repository.GetByDeporteAsync(deporte);
            return lista.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(TipoSuperficieDTO dto)
        {
            var existente = await _repository.GetAsync(dto.IdTipoSuperficie);
            if (existente == null || existente.Estado == Estado.Inactivo) return false;

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre es obligatorio.", nameof(dto.Nombre));
            if (string.IsNullOrWhiteSpace(dto.Deporte))
                throw new ArgumentException("El deporte es obligatorio.", nameof(dto.Deporte));

            var dep = dto.Deporte.Trim();
            if (await _repository.ExisteNombreAsync(dto.Nombre, dep, dto.IdTipoSuperficie))
                throw new ReglaDeNegocioException("Ya existe un tipo de cancha con ese nombre para este deporte.");

            var entidadModificada = new TipoSuperficie(dto.IdTipoSuperficie, dto.Nombre, dep, existente.Estado);
            return await _repository.UpdateAsync(entidadModificada);
        }

        private static TipoSuperficieDTO MapToDto(TipoSuperficie t) => new TipoSuperficieDTO
        {
            IdTipoSuperficie = t.IdTipoSuperficie,
            Nombre = t.Nombre,
            Deporte = t.Deporte,
            Estado = (int)t.Estado
        };
    }
}
