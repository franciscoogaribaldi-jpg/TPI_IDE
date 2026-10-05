namespace DTOs
{
    public class ReservaDTO
    {
        public int IdReserva { get; set; }
        public int IdCliente { get; set; }
        public int IdCancha { get; set; }
        public int IdTurno { get; set; }
        public DateTime Fecha { get; set; }
        public int EstadoReserva { get; set; }
        public decimal ImporteTotal { get; set; }
        public decimal Sena { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<DetalleReservaDTO> Detalles { get; set; } = new();

        // Solo lectura, para mostrar en la grilla sin tener que ir a buscar cada entidad por separado desde el WinForms
        public string NombreCliente { get; set; } = string.Empty;
        public string NombreCancha { get; set; } = string.Empty;
        public string HorarioTurno { get; set; } = string.Empty;
    }
}