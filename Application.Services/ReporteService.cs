using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    /// <summary>
    /// Arma los datos de los dos reportes a partir de las reservas. Los cálculos se hacen acá
    /// (no en las pantallas), así el escritorio y la web muestran exactamente lo mismo.
    /// </summary>
    public class ReporteService : IReporteService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly ICanchaRepository _canchaRepository;
        private readonly ITurnoRepository _turnoRepository;

        public ReporteService(IReservaRepository reservaRepository, ICanchaRepository canchaRepository, ITurnoRepository turnoRepository)
        {
            _reservaRepository = reservaRepository;
            _canchaRepository = canchaRepository;
            _turnoRepository = turnoRepository;
        }

        // Ocupación por cancha: reservas no canceladas sobre turnos disponibles en el período.
        public async Task<IEnumerable<ReporteOcupacionDTO>> GetOcupacionAsync(DateTime desde, DateTime hasta)
        {
            ValidarPeriodo(desde, hasta);

            var reservas = (await _reservaRepository.GetAllAsync())
                .Where(r => r.Fecha.Date >= desde.Date && r.Fecha.Date <= hasta.Date
                            && r.EstadoReserva != EstadoReserva.Cancelada)
                .ToList();

            int dias = (hasta.Date - desde.Date).Days + 1;
            int turnosPorDia = (await _turnoRepository.GetAllAsync()).Count();
            int turnosDisponibles = dias * turnosPorDia;

            // Todas las canchas activas (aunque no tengan reservas, para que aparezcan en 0) más
            // las eliminadas que sí tuvieron reservas en el período (son historial).
            var canchas = (await _canchaRepository.GetAllAsync()).ToList();
            foreach (var cancha in reservas.Select(r => r.Cancha).OfType<Cancha>())
            {
                if (!canchas.Any(c => c.IdCancha == cancha.IdCancha))
                    canchas.Add(cancha);
            }

            return canchas
                .Select(c =>
                {
                    int cantidad = reservas.Count(r => r.IdCancha == c.IdCancha);
                    return new ReporteOcupacionDTO
                    {
                        IdCancha = c.IdCancha,
                        NombreCancha = c.Estado == Estado.Activo ? c.Nombre : $"{c.Nombre} (eliminada)",
                        TipoCancha = TipoDe(c),
                        CantidadReservas = cantidad,
                        TurnosDisponibles = turnosDisponibles,
                        PorcentajeOcupacion = turnosDisponibles == 0 ? 0 : Math.Round(cantidad * 100m / turnosDisponibles, 1)
                    };
                })
                .OrderByDescending(o => o.CantidadReservas)
                .ToList();
        }

        // Recaudación por mes y tipo de cancha. Solo cuentan las reservas Finalizadas: son las
        // que ya se jugaron y se cobraron (las pendientes o confirmadas todavía pueden cancelarse).
        public async Task<IEnumerable<ReporteRecaudacionDTO>> GetRecaudacionAsync(DateTime desde, DateTime hasta)
        {
            ValidarPeriodo(desde, hasta);

            var reservas = (await _reservaRepository.GetAllAsync())
                .Where(r => r.Fecha.Date >= desde.Date && r.Fecha.Date <= hasta.Date
                            && r.EstadoReserva == EstadoReserva.Finalizada
                            && r.Cancha != null)
                .ToList();

            return reservas
                .GroupBy(r => new { r.Fecha.Year, r.Fecha.Month, Tipo = TipoDe(r.Cancha!) })
                .Select(g =>
                {
                    decimal adicionales = g.Sum(r => r.Detalles.Sum(d => d.Subtotal));
                    decimal total = g.Sum(r => r.ImporteTotal);
                    return new ReporteRecaudacionDTO
                    {
                        Anio = g.Key.Year,
                        Mes = g.Key.Month,
                        TipoCancha = g.Key.Tipo,
                        CantidadReservas = g.Count(),
                        ImporteAdicionales = adicionales,
                        ImporteCanchas = total - adicionales,
                        ImporteTotal = total
                    };
                })
                .OrderBy(r => r.Anio).ThenBy(r => r.Mes).ThenBy(r => r.TipoCancha)
                .ToList();
        }

        private static void ValidarPeriodo(DateTime desde, DateTime hasta)
        {
            if (desde.Date > hasta.Date)
                throw new ArgumentException("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.");
            if ((hasta.Date - desde.Date).Days > 366)
                throw new ArgumentException("El período del reporte no puede superar un año.");
        }

        // Polimorfismo de la herencia: el tipo sale de la clase hija, no de un campo de texto.
        private static string TipoDe(Cancha cancha) => cancha is CanchaPadel ? "Pádel" : "Fútbol";
    }
}
