using System;

namespace Domain.Model
{
    public class Reserva
    {
        public int IdReserva { get; private set; }

       
        public int IdCliente { get; private set; }
        public Cliente? Cliente { get; private set; }

        public int IdCancha { get; private set; }
        public Cancha? Cancha { get; private set; }

        public int IdTurno { get; private set; }
        public Turno? Turno { get; private set; }

        public DateTime Fecha { get; private set; }
        public EstadoReserva EstadoReserva { get; private set; }
        public decimal ImporteTotal { get; private set; }
        public decimal Sena { get; private set; }
        public DateTime FechaCreacion { get; private set; }

        public Reserva(int idReserva, int idCliente, int idCancha, int idTurno, DateTime fecha, EstadoReserva estadoReserva, decimal importeTotal, decimal sena)
        {
            SetIdReserva(idReserva);
            SetIdCliente(idCliente);
            SetIdCancha(idCancha);
            SetIdTurno(idTurno);
            SetFecha(fecha);
            SetEstadoReserva(estadoReserva);
            SetImportes(importeTotal, sena);
            FechaCreacion = DateTime.Now; 
        }

        public void SetIdReserva(int idReserva)
        {
            if (idReserva < 0) throw new ArgumentException("El Id no puede ser negativo.");
            IdReserva = idReserva;
        }

        public void SetIdCliente(int idCliente)
        {
            if (idCliente <= 0) throw new ArgumentException("Id de cliente inválido.");
            IdCliente = idCliente;
        }

        public void SetIdCancha(int idCancha)
        {
            if (idCancha <= 0) throw new ArgumentException("Id de cancha inválido.");
            IdCancha = idCancha;
        }

        public void SetIdTurno(int idTurno)
        {
            if (idTurno <= 0) throw new ArgumentException("Id de turno inválido.");
            IdTurno = idTurno;
        }

        public void SetFecha(DateTime fecha)
        {
            // Evita que la gente reserve en el pasado
            if (fecha.Date < DateTime.Today)
                throw new ArgumentException("No se pueden hacer reservas en fechas pasadas.");
            Fecha = fecha.Date; // Asegura que solo se guarde la fecha sin la hora
        }

        public void SetEstadoReserva(EstadoReserva estado)
        {
            EstadoReserva = estado;
        }

        public void SetImportes(decimal importeTotal, decimal sena)
        {
            if (importeTotal < 0) throw new ArgumentException("El importe no puede ser negativo.");
            if (sena < 0 || sena > importeTotal) throw new ArgumentException("La seña debe ser entre 0 y el total.");

            ImporteTotal = importeTotal;
            Sena = sena;
        }
    }
}