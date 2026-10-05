namespace DTOs
{
    public class DetalleReservaDTO
    {
        public int IdDetalleReserva { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}