using System;

namespace Domain.Model
{
    public class TipoSuperficie
    {
        public int IdTipoSuperficie { get; private set; }
        public string Nombre { get; private set; }
        public string Deporte { get; private set; }
        public Estado Estado { get; private set; }

        public TipoSuperficie(int idTipoSuperficie, string nombre, string deporte, Estado estado)
        {
            SetIdTipoSuperficie(idTipoSuperficie);
            SetNombre(nombre);
            SetDeporte(deporte);
            SetEstado(estado);
        }

        public void SetIdTipoSuperficie(int idTipoSuperficie)
        {
            if (idTipoSuperficie < 0)
                throw new ArgumentException("El Id no puede ser negativo.", nameof(idTipoSuperficie));
            IdTipoSuperficie = idTipoSuperficie;
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
            Nombre = nombre.Trim();
        }

        public void SetDeporte(string deporte)
        {
            if (string.IsNullOrWhiteSpace(deporte))
                throw new ArgumentException("El deporte no puede estar vacío.", nameof(deporte));

            var dep = deporte.Trim();
            if (!dep.Equals("Futbol", StringComparison.OrdinalIgnoreCase) &&
                !dep.Equals("Padel", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("El deporte debe ser 'Futbol' o 'Padel'.", nameof(deporte));
            }

            Deporte = dep;
        }

        public void SetEstado(Estado estado)
        {
            Estado = estado;
        }
    }
}
