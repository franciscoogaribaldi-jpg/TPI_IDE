using System;

namespace Domain.Model
{
    public class Turno
    {
        public int IdTurno { get; private set; }
        public TimeSpan HoraInicio { get; private set; }
        public TimeSpan HoraFin { get; private set; }
        public Estado Estado { get; private set; }

        public Turno(int idTurno, TimeSpan horaInicio, TimeSpan horaFin, Estado estado)
        {
            SetIdTurno(idTurno);
            SetHorarios(horaInicio, horaFin);
            SetEstado(estado);
        }

        public void SetIdTurno(int idTurno)
        {
            if (idTurno < 0)
                throw new ArgumentException("El Id no puede ser negativo.", nameof(idTurno));
            IdTurno = idTurno;
        }

        public void SetHorarios(TimeSpan horaInicio, TimeSpan horaFin)
        {
            
            if (horaInicio >= horaFin)
                throw new ArgumentException("La hora de inicio debe ser menor a la hora de fin.");

            HoraInicio = horaInicio;
            HoraFin = horaFin;
        }

        public void SetEstado(Estado estado)
        {
            Estado = estado;
        }
    }
}