namespace DTOs
{
    /// <summary>
    /// Una fila del reporte de ocupación: cuántos turnos se reservaron de una cancha en el
    /// período, contra cuántos tenía disponibles.
    /// </summary>
    public class ReporteOcupacionDTO
    {
        public int IdCancha { get; set; }
        public string NombreCancha { get; set; } = string.Empty;
        public string TipoCancha { get; set; } = string.Empty;

        // Reservas no canceladas de esa cancha en el período.
        public int CantidadReservas { get; set; }

        // Turnos que se podían reservar en el período (turnos activos x días).
        public int TurnosDisponibles { get; set; }

        // CantidadReservas / TurnosDisponibles, de 0 a 100.
        public decimal PorcentajeOcupacion { get; set; }
    }
}
