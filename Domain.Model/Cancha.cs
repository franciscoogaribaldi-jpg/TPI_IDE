using System;

namespace Domain.Model
{
    public abstract class Cancha
    {
        public int IdCancha { get; private set; }
        public string Nombre { get; private set; }
        public Estado Estado { get; private set; }
        public decimal PrecioPorHora { get; private set; }

        // Va en la clase padre porque tanto una cancha de fútbol como una de pádel pueden ser techadas.
        public bool EsTechada { get; private set; }

        public int IdTipoSuperficie { get; private set; }
        public TipoSuperficie? TipoSuperficie { get; private set; }

        // POLIMORFISMO: cada tipo de cancha calcula esto a su manera (ver CanchaFutbol y CanchaPadel).
        // No se guardan en la base (ver TPIContext): se calculan a partir de los datos propios de cada hija.
        public abstract int CantidadJugadores { get; }
        public abstract string Caracteristicas { get; }

        // El constructor de la clase padre
        protected Cancha(int idCancha, string nombre, Estado estado, decimal precioPorHora, bool esTechada, int idTipoSuperficie)
        {
            SetIdCancha(idCancha);
            SetNombre(nombre);
            SetEstado(estado);
            SetPrecioPorHora(precioPorHora);
            SetEsTechada(esTechada);
            SetIdTipoSuperficie(idTipoSuperficie);
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

        public void SetEsTechada(bool esTechada)
        {
            EsTechada = esTechada;
        }

        public void SetIdTipoSuperficie(int idTipoSuperficie)
        {
            if (idTipoSuperficie < 0)
                throw new ArgumentException("El Id del tipo de superficie no puede ser negativo.", nameof(idTipoSuperficie));
            IdTipoSuperficie = idTipoSuperficie;
        }

        public void SetTipoSuperficie(TipoSuperficie tipoSuperficie)
        {
            ArgumentNullException.ThrowIfNull(tipoSuperficie);
            TipoSuperficie = tipoSuperficie;
            IdTipoSuperficie = tipoSuperficie.IdTipoSuperficie;
        }
    }
}
