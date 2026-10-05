using Application.Services.Exceptions;
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class TurnoService : ITurnoService
    {
        private readonly ITurnoRepository _repository;

        public TurnoService(ITurnoRepository repository)
        {
            _repository = repository;
        }

        public async Task<TurnoDTO> AddAsync(TurnoDTO dto)
        {
            var (horaInicio, horaFin) = ParsearHorarios(dto);

            bool solapado = await _repository.ExisteSolapadoAsync(horaInicio, horaFin);
            if (solapado)
                throw new ReglaDeNegocioException("Ya existe un turno que se superpone con ese horario.");

            var turno = new Turno(
                idTurno: 0,
                horaInicio: horaInicio,
                horaFin: horaFin,
                estado: Estado.Activo);

            await _repository.AddAsync(turno);

            dto.IdTurno = turno.IdTurno;
            dto.Estado = (int)turno.Estado;
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<TurnoDTO?> GetAsync(int id)
        {
            var turno = await _repository.GetAsync(id);
            return turno == null ? null : MapToDto(turno);
        }

        public async Task<IEnumerable<TurnoDTO>> GetAllAsync()
        {
            var turnos = await _repository.GetAllAsync();
            return turnos.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(TurnoDTO dto)
        {
            var existente = await _repository.GetAsync(dto.IdTurno);
            if (existente == null) return false;

            var (horaInicio, horaFin) = ParsearHorarios(dto);

            bool solapado = await _repository.ExisteSolapadoAsync(horaInicio, horaFin, dto.IdTurno);
            if (solapado)
                throw new ReglaDeNegocioException("Ya existe otro turno que se superpone con ese horario.");

            if (!Enum.IsDefined(typeof(Estado), dto.Estado))
                throw new ArgumentException("El estado del turno no es válido.", nameof(dto.Estado));

            var turnoModificado = new Turno(
                idTurno: dto.IdTurno,
                horaInicio: horaInicio,
                horaFin: horaFin,
                estado: (Estado)dto.Estado);

            return await _repository.UpdateAsync(turnoModificado);
        }

        // Centraliza el parseo de texto a TimeSpan y la validación de formato + orden,
        // ya que AddAsync y UpdateAsync necesitan exactamente lo mismo.
        private static (TimeSpan horaInicio, TimeSpan horaFin) ParsearHorarios(TurnoDTO dto)
        {
            if (!TimeSpan.TryParse(dto.HoraInicio, out var horaInicio) ||
                !TimeSpan.TryParse(dto.HoraFin, out var horaFin))
            {
                throw new ArgumentException("El horario debe tener formato HH:mm:ss, por ejemplo 08:00:00.");
            }

            if (horaInicio >= horaFin)
                throw new ArgumentException("La hora de inicio debe ser menor a la hora de fin.");

            return (horaInicio, horaFin);
        }

        private static TurnoDTO MapToDto(Turno t) => new TurnoDTO
        {
            IdTurno = t.IdTurno,
            HoraInicio = t.HoraInicio.ToString(),
            HoraFin = t.HoraFin.ToString(),
            Estado = (int)t.Estado
        };
    }
}