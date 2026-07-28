namespace DTOs
{
    public class CanchaDTO
    {
        public int IdCancha { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public decimal PrecioPorHora { get; set; }

        public string TipoCancha { get; set; }

        public int? CantidadRaquetas { get; set; }
        public decimal? PrecioTotalRaquetas { get; set; }
    }
}