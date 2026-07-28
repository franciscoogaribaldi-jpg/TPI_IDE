using System;

namespace Domain.Model
{
    public abstract class Cancha
    {
        public int IdCancha { get; private set; }
        public string Nombre { get; private set; }
        public Estado Estado { get; private set; }
        public decimal PrecioPorHora { get; private set; }

        
        protected Cancha(int idCancha, string nombre, Estado estado, decimal precioPorHora)
        {
            SetIdCancha(idCancha);
            SetNombre(nombre);
            SetEstado(estado);
            SetPrecioPorHora(precioPorHora);
        }

        public void SetIdCancha(int idCancha)
        {
            if (idCancha < 0)
                throw new ArgumentException("El Id no puede ser negativo.", nameof(idCancha));
            IdCancha = idCancha;
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
            Nombre = nombre;
        }

        public void SetEstado(Estado estado)
        {
            Estado = estado;
        }

        public void SetPrecioPorHora(decimal precioPorHora)
        {
            if (precioPorHora <= 0)
                throw new ArgumentException("El precio por hora debe ser mayor a cero.", nameof(precioPorHora));
            PrecioPorHora = precioPorHora;
        }
    }
}