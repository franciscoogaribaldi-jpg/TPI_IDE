using System;

namespace Domain.Model
{
    public class CanchaPadel : Cancha
    {
        public int CantidadRaquetas { get; private set; }
        public decimal PrecioTotalRaquetas { get; private set; }

        public CanchaPadel(int idCancha, string nombre, Estado estado, decimal precioPorHora, int cantidadRaquetas, decimal precioTotalRaquetas)
            : base(idCancha, nombre, estado, precioPorHora)
        {
            SetRaquetas(cantidadRaquetas, precioTotalRaquetas);
        }

        public void SetRaquetas(int cantidad, decimal precioTotal)
        {
            if (cantidad < 0)
                throw new ArgumentException("La cantidad de raquetas no puede ser negativa.");
            if (precioTotal < 0)
                throw new ArgumentException("El precio de raquetas no puede ser negativo.");

            CantidadRaquetas = cantidad;
            PrecioTotalRaquetas = precioTotal;
        }
    }
}