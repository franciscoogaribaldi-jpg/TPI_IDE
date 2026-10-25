namespace DTOs
{
    public class CanchaDTO
    {
        public int IdCancha { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
        public decimal PrecioPorHora { get; set; }
        public string TipoCancha { get; set; }
        public bool EsTechada { get; set; }

        // Solo fútbol (Domain.Model.TipoFutbol y TipoSuperficie, viajan como int).
        public int? TipoFutbol { get; set; }
        public int? TipoSuperficie { get; set; }

        // Solo pádel (Domain.Model.TipoPared).
        public int? TipoPared { get; set; }

        // Solo lectura: los calcula el dominio (polimorfismo). Al guardar se ignoran.
        public int CantidadJugadores { get; set; }
        public string? Caracteristicas { get; set; }
    }
}
