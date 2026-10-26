namespace DTOs
{
    /// <summary>
    /// Una fila del reporte de recaudación: lo cobrado en un mes por un tipo de cancha.
    /// </summary>
    public class ReporteRecaudacionDTO
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public string TipoCancha { get; set; } = string.Empty;

        // Reservas Finalizadas (ya jugadas y cobradas) de ese mes y tipo de cancha.
        public int CantidadReservas { get; set; }

        // Parte del importe que corresponde al alquiler de la cancha.
        public decimal ImporteCanchas { get; set; }

        // Parte que corresponde a las líneas de detalle (raquetas, pelotas, bebidas...).
        public decimal ImporteAdicionales { get; set; }

        public decimal ImporteTotal { get; set; }
    }
}
