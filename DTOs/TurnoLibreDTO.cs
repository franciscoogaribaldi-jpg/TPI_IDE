namespace DTOs
{
    /// <summary>
    /// Una cancha con un turno libre en una fecha: lo que el Cliente ve para elegir qué reservar.
    /// </summary>
    public class TurnoLibreDTO
    {
        public int IdCancha { get; set; }
        public string NombreCancha { get; set; } = string.Empty;
        public string TipoCancha { get; set; } = string.Empty;
        public string Caracteristicas { get; set; } = string.Empty;
        public decimal PrecioPorHora { get; set; }

        public int IdTurno { get; set; }
        public string Horario { get; set; } = string.Empty; // "18:00 - 19:00"
    }
}
