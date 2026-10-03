using Application.Services.Exceptions;
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IDetalleReservaRepository _detalleRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ICanchaRepository _canchaRepository;
        private readonly ITurnoRepository _turnoRepository;

        public ReservaService(
            IReservaRepository reservaRepository,
            IDetalleReservaRepository detalleRepository,
            IClienteRepository clienteRepository,
            ICanchaRepository canchaRepository,
            ITurnoRepository turnoRepository)
        {
            _reservaRepository = reservaRepository;
            _detalleRepository = detalleRepository;
            _clienteRepository = clienteRepository;
            _canchaRepository = canchaRepository;
            _turnoRepository = turnoRepository;
        }

        public async Task<ReservaDTO> AddAsync(ReservaDTO dto)
        {
            var cancha = await ValidarYObtenerCancha(dto);

            decimal importeTotal = await CalcularImporteTotal(cancha, dto.Detalles);

            var reserva = new Reserva(
                idReserva: 0,
                idCliente: dto.IdCliente,
                idCancha: dto.IdCancha,
                idTurno: dto.IdTurno,
                fecha: dto.Fecha,
                estadoReserva: (EstadoReserva)dto.EstadoReserva,
                importeTotal: importeTotal,
                sena: dto.Sena);

            await _reservaRepository.AddAsync(reserva);
            await GuardarDetalles(reserva.IdReserva, dto.Detalles);

            var detallesGuardados = await _detalleRepository.GetByReservaAsync(reserva.IdReserva);
            return MapToDto(reserva, detallesGuardados);
        }

        public async Task<bool> UpdateAsync(ReservaDTO dto)
        {
            var existente = await _reservaRepository.GetAsync(dto.IdReserva);
            if (existente == null) return false;

            var cancha = await ValidarYObtenerCancha(dto, dto.IdReserva);

            decimal importeTotal = await CalcularImporteTotal(cancha, dto.Detalles);

            var reservaModificada = new Reserva(
                idReserva: dto.IdReserva,
                idCliente: dto.IdCliente,
                idCancha: dto.IdCancha,
                idTurno: dto.IdTurno,
                fecha: dto.Fecha,
                estadoReserva: (EstadoReserva)dto.EstadoReserva,
                importeTotal: importeTotal,
                sena: dto.Sena);

            bool actualizado = await _reservaRepository.UpdateAsync(reservaModificada);
            if (!actualizado) return false;

            await _detalleRepository.DeleteByReservaAsync(dto.IdReserva);
            await GuardarDetalles(dto.IdReserva, dto.Detalles);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _reservaRepository.DeleteAsync(id);
        }

        public async Task<ReservaDTO?> GetAsync(int id)
        {
            var reserva = await _reservaRepository.GetAsync(id);
            if (reserva == null) return null;

            var detalles = await _detalleRepository.GetByReservaAsync(id);
            return MapToDto(reserva, detalles);
        }

        public async Task<IEnumerable<ReservaDTO>> GetAllAsync()
        {
            var reservas = await _reservaRepository.GetAllAsync();
            var resultado = new List<ReservaDTO>();

            foreach (var reserva in reservas)
            {
                var detalles = await _detalleRepository.GetByReservaAsync(reserva.IdReserva);
                resultado.Add(MapToDto(reserva, detalles));
            }

            return resultado;
        }

        // ---------- Privados: la orquestación propia del Maestro/Detalle ----------

        private async Task<Cancha> ValidarYObtenerCancha(ReservaDTO dto, int? idReservaExcluida = null)
        {
            if (!Enum.IsDefined(typeof(EstadoReserva), dto.EstadoReserva))
                throw new ArgumentException("El estado de la reserva no es válido.", nameof(dto.EstadoReserva));

            var cliente = await _clienteRepository.GetAsync(dto.IdCliente);
            if (cliente == null)
                throw new ReglaDeNegocioException("El cliente indicado no existe.");

            var cancha = await _canchaRepository.GetAsync(dto.IdCancha);
            if (cancha == null)
                throw new ReglaDeNegocioException("La cancha indicada no existe.");

            var turno = await _turnoRepository.GetAsync(dto.IdTurno);
            if (turno == null)
                throw new ReglaDeNegocioException("El turno indicado no existe.");

            bool ocupado = await _reservaRepository.ExisteReservaActivaAsync(dto.IdCancha, dto.IdTurno, dto.Fecha, idReservaExcluida);
            if (ocupado)
                throw new ReglaDeNegocioException("Ya existe una reserva activa para esa cancha, ese turno y esa fecha.");

            return cancha;
        }

        private static Task<decimal> CalcularImporteTotal(Cancha cancha, List<DetalleReservaDTO> detalles)
        {
            decimal totalDetalles = detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            return Task.FromResult(cancha.PrecioPorHora + totalDetalles);
        }

        private async Task GuardarDetalles(int idReserva, List<DetalleReservaDTO> detalles)
        {
            foreach (var lineaDto in detalles)
            {
                var linea = new DetalleReserva(0, idReserva, lineaDto.Concepto, lineaDto.Cantidad, lineaDto.PrecioUnitario);
                await _detalleRepository.AddAsync(linea);
            }
        }

        private static ReservaDTO MapToDto(Reserva r, IEnumerable<DetalleReserva>? detalles = null) => new ReservaDTO
        {
            IdReserva = r.IdReserva,
            IdCliente = r.IdCliente,
            IdCancha = r.IdCancha,
            IdTurno = r.IdTurno,
            Fecha = r.Fecha,
            EstadoReserva = (int)r.EstadoReserva,
            ImporteTotal = r.ImporteTotal,
            Sena = r.Sena,
            FechaCreacion = r.FechaCreacion,
            NombreCliente = r.Cliente != null ? $"{r.Cliente.Nombre} {r.Cliente.Apellido}" : string.Empty,
            NombreCancha = r.Cancha?.Nombre ?? string.Empty,
            HorarioTurno = r.Turno != null ? $"{r.Turno.HoraInicio:hh\\:mm} - {r.Turno.HoraFin:hh\\:mm}" : string.Empty,
            Detalles = detalles?.Select(d => new DetalleReservaDTO
            {
                IdDetalleReserva = d.IdDetalleReserva,
                Concepto = d.Concepto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList() ?? new List<DetalleReservaDTO>()
        };
    }
}