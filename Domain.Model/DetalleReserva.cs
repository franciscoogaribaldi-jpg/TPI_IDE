namespace Domain.Model
{
    public class DetalleReserva
    {
        public int IdDetalleReserva { get; private set; }
        public int IdReserva { get; private set; }
        public string Concepto { get; private set; } = string.Empty;
        public int Cantidad { get; private set; }
        public decimal PrecioUnitario { get; private set; }

        // Calculado, no se persiste: ver TPIContext (entity.Ignore).
        public decimal Subtotal => Cantidad * PrecioUnitario;

        public DetalleReserva(int idDetalleReserva, int idReserva, string concepto, int cantidad, decimal precioUnitario)
        {
            SetIdDetalleReserva(idDetalleReserva);
            SetIdReserva(idReserva);
            SetConcepto(concepto);
            SetCantidadYPrecio(cantidad, precioUnitario);
        }

        public void SetIdDetalleReserva(int id)
        {
            if (id < 0) throw new ArgumentException("El Id no puede ser negativo.");
            IdDetalleReserva = id;
        }

        public void SetIdReserva(int idReserva)
        {
            if (idReserva <= 0) throw new ArgumentException("Id de reserva inválido.");
            IdReserva = idReserva;
        }

        public void SetConcepto(string concepto)
        {
            if (string.IsNullOrWhiteSpace(concepto))
                throw new ArgumentException("El concepto es obligatorio.");
            Concepto = concepto.Trim();
        }

        public void SetCantidadYPrecio(int cantidad, decimal precioUnitario)
        {
            if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
            if (precioUnitario < 0) throw new ArgumentException("El precio unitario no puede ser negativo.");
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }
    }
}