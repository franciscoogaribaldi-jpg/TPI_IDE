namespace DTOs
{
    public class CanchaDTO
    {
        public int IdCancha { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public decimal PrecioPorHora { get; set; }

        // Este campo nos servirá en Swagger para escribir "Futbol" o "Padel" y que el sistema sepa cuál crear
        public string TipoCancha { get; set; }

        // Propiedades exclusivas de Padel (les ponemos el ? para que puedan quedar vacías si la cancha es de Fútbol)
        public int? CantidadRaquetas { get; set; }
        public decimal? PrecioTotalRaquetas { get; set; }
    }
}