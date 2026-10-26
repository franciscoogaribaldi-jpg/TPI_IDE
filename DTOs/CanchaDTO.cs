namespace DTOs
{
    public class CanchaDTO
    {
        public int IdCancha { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Estado { get; set; }
        public decimal PrecioPorHora { get; set; }
        public string TipoCancha { get; set; } = string.Empty;
        public bool EsTechada { get; set; }

        public int IdTipoSuperficie { get; set; }
        public string? NombreSuperficie { get; set; }

        // Solo fútbol (Domain.Model.TipoFutbol viaja como int).
        public int? TipoFutbol { get; set; }

        // Compatibilidad con vistas previas
        public int? TipoSuperficie
        {
            get => IdTipoSuperficie;
            set => IdTipoSuperficie = value ?? 0;
        }

        public int? TipoPared { get; set; }

        // Solo lectura: los calcula el dominio (polimorfismo). Al guardar se ignoran.
        public int CantidadJugadores { get; set; }
        public string? Caracteristicas { get; set; }
    }
}
