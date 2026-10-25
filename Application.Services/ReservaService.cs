using Application.Services.Exceptions;
using Data;
using Domain.Model;
using DTOs;

namespace Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ICanchaRepository _canchaRepository;
        private readonly ITurnoRepository _turnoRepository;
        private readonly IAdministradorRepository _administradorRepository;

        public ReservaService(
            IReservaRepository reservaRepository,
            IClienteRepository clienteRepository,
            ICanchaRepository canchaRepository,
            ITurnoRepository turnoRepository,
            IAdministradorRepository administradorRepository)
        {
            _reservaRepository = reservaRepository;
            _clienteRepository = clienteRepository;
            _canchaRepository = canchaRepository;
            _turnoRepository = turnoRepository;
            _administradorRepository = administradorRepository;
        }

        public async Task<ReservaDTO> AddAsync(ReservaDTO dto, int? idUsuarioQueRegistra = null)
        {
            ValidarFechaNoPasada(dto.Fecha);

            // Se arman y validan TODAS las líneas antes de tocar la base: si alguna es
            // inválida se corta acá y no se guarda nada.
            var lineas = ConstruirLineas(dto.Detalles);

            var cancha = await ValidarYObtenerCancha(dto);

            decimal importeTotal = cancha.PrecioPorHora + lineas.Sum(l => l.Subtotal);

            var reserva = new Reserva(
                idReserva: 0,
                idCliente: dto.IdCliente,
                idCancha: dto.IdCancha,
                idTurno: dto.IdTurno,
                fecha: dto.Fecha,
                estadoReserva: (EstadoReserva)dto.EstadoReserva,
                importeTotal: importeTotal,
                sena: dto.Sena);

            foreach (var linea in lineas)
                reserva.AgregarDetalle(linea);

            // Se registra qué administrador cargó la reserva (si el usuario logueado tiene ficha).
            if (idUsuarioQueRegistra.HasValue)
            {
                var administrador = await _administradorRepository.GetByIdUsuarioAsync(idUsuarioQueRegistra.Value);
                if (administrador != null)
                    reserva.SetIdAdministrador(administrador.IdAdministrador);
            }

            // Un único SaveChangesAsync guarda la cabecera y todas sus líneas juntas.
            await _reservaRepository.AddAsync(reserva);

            return MapToDto(reserva);
        }

        public async Task<bool> UpdateAsync(ReservaDTO dto)
        {
            var existente = await _reservaRepository.GetAsync(dto.IdReserva);
            if (existente == null) return false;

            // Solo se exige fecha futura si la fecha CAMBIÓ. Así se puede marcar como
            // Finalizada o Cancelada una reserva que ya pasó.
            if (dto.Fecha.Date != existente.Fecha.Date)
                ValidarFechaNoPasada(dto.Fecha);

            var lineas = ConstruirLineas(dto.Detalles);

            var cancha = await ValidarYObtenerCancha(dto, existente);

            decimal importeTotal = cancha.PrecioPorHora + lineas.Sum(l => l.Subtotal);

            var reservaModificada = new Reserva(
                idReserva: dto.IdReserva,
                idCliente: dto.IdCliente,
                idCancha: dto.IdCancha,
                idTurno: dto.IdTurno,
                fecha: dto.Fecha,
                estadoReserva: (EstadoReserva)dto.EstadoReserva,
                importeTotal: importeTotal,
                sena: dto.Sena);

            foreach (var linea in lineas)
                reservaModificada.AgregarDetalle(linea);

            return await _reservaRepository.UpdateAsync(reservaModificada);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _reservaRepository.DeleteAsync(id);
        }

        public async Task<ReservaDTO?> GetAsync(int id)
        {
            var reserva = await _reservaRepository.GetAsync(id);
            return reserva == null ? null : MapToDto(reserva);
        }

        public async Task<IEnumerable<ReservaDTO>> GetAllAsync()
        {
            // Las líneas ya vienen cargadas con cada reserva (Include en el repositorio):
            // no hace falta una consulta extra por reserva.
            var reservas = await _reservaRepository.GetAllAsync();
            return reservas.Select(MapToDto).ToList();
        }

        // ---------- Privados ----------

        private static void ValidarFechaNoPasada(DateTime fecha)
        {
            if (fecha.Date < DateTime.Today)
                throw new ReglaDeNegocioException("No se pueden hacer reservas en fechas pasadas.");
        }

        // existente = la reserva antes de editarla (null en el alta).
        private async Task<Cancha> ValidarYObtenerCancha(ReservaDTO dto, Reserva? existente = null)
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

            // Baja lógica: lo eliminado (Inactivo) no se puede usar en una reserva nueva. Si se
            // edita una reserva vieja que ya lo tenía, se permite (es historial).
            if (cliente.Estado == Estado.Inactivo && existente?.IdCliente != dto.IdCliente)
                throw new ReglaDeNegocioException("El cliente fue eliminado: no se le pueden cargar reservas.");
            if (cancha.Estado == Estado.Inactivo && existente?.IdCancha != dto.IdCancha)
                throw new ReglaDeNegocioException("La cancha fue eliminada: no se puede reservar.");
            if (turno.Estado == Estado.Inactivo && existente?.IdTurno != dto.IdTurno)
                throw new ReglaDeNegocioException("El turno fue eliminado: no se puede reservar.");

            bool ocupado = await _reservaRepository.ExisteReservaActivaAsync(dto.IdCancha, dto.IdTurno, dto.Fecha, existente?.IdReserva);
            if (ocupado)
                throw new ReglaDeNegocioException("Ya existe una reserva activa para esa cancha, ese turno y esa fecha.");

            return cancha;
        }

        // Crea todas las líneas antes de guardar nada. Si alguna es inválida, el constructor de
        // DetalleReserva lanza ArgumentException y no se escribe nada en la base.
        private static List<DetalleReserva> ConstruirLineas(IEnumerable<DetalleReservaDTO>? lineas)
        {
            return (lineas ?? Enumerable.Empty<DetalleReservaDTO>())
                .Select(l => new DetalleReserva(0, 0, l.Concepto, l.Cantidad, l.PrecioUnitario))
                .ToList();
        }

        private static ReservaDTO MapToDto(Reserva r) => new ReservaDTO
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
            NombreAdministrador = r.Administrador != null ? $"{r.Administrador.Nombre} {r.Administrador.Apellido}" : string.Empty,
            Detalles = r.Detalles.Select(d => new DetalleReservaDTO
            {
                IdDetalleReserva = d.IdDetalleReserva,
                Concepto = d.Concepto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList()
        };
    }
}