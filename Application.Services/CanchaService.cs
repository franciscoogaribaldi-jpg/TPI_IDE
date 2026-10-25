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
            Cancha cancha = CrearCancha(dto, idCancha: 0);

            await _repository.AddAsync(cancha);
            return MapToDto(cancha);
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

            Cancha canchaModificada = CrearCancha(dto, dto.IdCancha);

            return await _repository.UpdateAsync(canchaModificada);
        }

        // Valida el DTO y arma la clase hija que corresponde (CanchaFutbol o CanchaPadel).
        // La usan el alta y la modificación, así las reglas están en un solo lugar.
        private static Cancha CrearCancha(CanchaDTO dto, int idCancha)
        {
            // VALIDACIÓN: antes esto era un cast directo (Estado)dto.Estado; un Estado=99
            // entraba sin error. Corrección pedida por la cátedra sobre la Entrega 1.
            if (!Enum.IsDefined(typeof(Estado), dto.Estado))
                throw new ArgumentException("El estado de la cancha no es válido.", nameof(dto.Estado));

            // VALIDACIÓN: Enum.TryParse centraliza los valores válidos en un solo lugar
            // (Domain.Model.TipoCancha) y rechaza cualquier otra cosa de forma prolija.
            if (!Enum.TryParse<TipoCancha>(dto.TipoCancha, ignoreCase: true, out var tipo)) // nos fijamos si es valido y si lo es lo pasamos a Enum
                throw new ArgumentException("Tipo de cancha inválido. Use 'Futbol' o 'Padel'.", nameof(dto.TipoCancha));

            return tipo switch
            {
                TipoCancha.Futbol => new CanchaFutbol(idCancha, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora, dto.EsTechada,
                    ValidarEnum<TipoFutbol>(dto.TipoFutbol, "Elegí si la cancha es de fútbol 5, 7 u 11."),
                    ValidarEnum<TipoSuperficie>(dto.TipoSuperficie, "El tipo de superficie no es válido.")),
                TipoCancha.Padel => new CanchaPadel(idCancha, dto.Nombre, (Estado)dto.Estado, dto.PrecioPorHora, dto.EsTechada,
                    ValidarEnum<TipoPared>(dto.TipoPared, "El tipo de pared no es válido.")),
                _ => throw new ArgumentException("Tipo de cancha inválido. Use 'Futbol' o 'Padel'.", nameof(dto.TipoCancha))
            };
        }

        // Los enums viajan como int en el DTO: se valida con Enum.IsDefined antes de castear.
        private static TEnum ValidarEnum<TEnum>(int? valor, string mensaje) where TEnum : struct, Enum
        {
            if (valor == null || !Enum.IsDefined(typeof(TEnum), valor.Value))
                throw new ArgumentException(mensaje);
            return (TEnum)Enum.ToObject(typeof(TEnum), valor.Value);
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
