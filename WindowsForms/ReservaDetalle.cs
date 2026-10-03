using API.Clients;
using DTOs;
using System.ComponentModel;

namespace WindowsForms
{
    public partial class ReservaDetalle : Form
    {
        private readonly ReservaDTO? _reservaOriginal;
        private bool EsAlta => _reservaOriginal == null;

        private List<CanchaDTO> _canchas = new();
        private BindingList<DetalleReservaDTO> _lineasDetalle = new();

        public ReservaDetalle()
        {
            InitializeComponent();
            _reservaOriginal = null;
        }

        public ReservaDetalle(ReservaDTO reserva)
        {
            InitializeComponent();
            _reservaOriginal = reserva;
        }

        private async void ReservaDetalle_Load(object sender, EventArgs e)
        {
            Text = EsAlta ? "Nueva reserva" : "Editar reserva";

            cmbEstado.DataSource = new[] { "Pendiente", "Confirmada", "Cancelada", "Finalizada" };

            Cursor = Cursors.WaitCursor;
            try
            {
                var clientes = (await ClienteApiClient.GetAllAsync()).ToList();
                cmbCliente.DataSource = clientes
                    .Select(c => new { c.IdCliente, Display = $"{c.Apellido}, {c.Nombre} (DNI {c.Dni})" })
                    .ToList();
                cmbCliente.DisplayMember = "Display";
                cmbCliente.ValueMember = "IdCliente";

                _canchas = (await CanchaApiClient.GetAllAsync()).ToList();
                cmbCancha.DataSource = _canchas
                    .Select(c => new { c.IdCancha, Display = $"{c.Nombre} ({c.TipoCancha}) - {c.PrecioPorHora:C0}/h" })
                    .ToList();
                cmbCancha.DisplayMember = "Display";
                cmbCancha.ValueMember = "IdCancha";

                var turnos = (await TurnoApiClient.GetAllAsync()).ToList();
                cmbTurno.DataSource = turnos
                    .Select(t => new { t.IdTurno, Display = $"{t.HoraInicio} - {t.HoraFin}" })
                    .ToList();
                cmbTurno.DisplayMember = "Display";
                cmbTurno.ValueMember = "IdTurno";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los datos necesarios.\n\n{ex.Message}",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            if (EsAlta)
            {
                dtpFecha.Value = DateTime.Today.AddDays(1);
                cmbEstado.SelectedIndex = 0;
                numSena.Value = 0;
            }
            else if (_reservaOriginal != null)
            {
                cmbCliente.SelectedValue = _reservaOriginal.IdCliente;
                cmbCancha.SelectedValue = _reservaOriginal.IdCancha;
                cmbTurno.SelectedValue = _reservaOriginal.IdTurno;
                dtpFecha.Value = _reservaOriginal.Fecha;
                cmbEstado.SelectedIndex = _reservaOriginal.EstadoReserva - 1;
                numSena.Value = _reservaOriginal.Sena;

                foreach (var linea in _reservaOriginal.Detalles)
                {
                    _lineasDetalle.Add(new DetalleReservaDTO
                    {
                        IdDetalleReserva = linea.IdDetalleReserva,
                        Concepto = linea.Concepto,
                        Cantidad = linea.Cantidad,
                        PrecioUnitario = linea.PrecioUnitario,
                        Subtotal = linea.Subtotal
                    });
                }
            }

            dgvDetalle.DataSource = _lineasDetalle;
            ActualizarTotalEstimado();
        }

        private void cmbCancha_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarTotalEstimado();
        }

        private void dgvDetalle_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _lineasDetalle.Count) return;

            var linea = _lineasDetalle[e.RowIndex];
            linea.Subtotal = linea.Cantidad * linea.PrecioUnitario;
            dgvDetalle.InvalidateRow(e.RowIndex);
            ActualizarTotalEstimado();
        }

        private void dgvDetalle_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            ActualizarTotalEstimado();
        }

        private void dgvDetalle_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Ese valor no es válido para esta columna. Cantidad y Precio unitario tienen que ser números.",
                "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.ThrowException = false;
            e.Cancel = true;
        }
        private void ActualizarTotalEstimado()
        {
            decimal precioCancha = 0;
            if (cmbCancha.SelectedValue is int idCancha)
            {
                var cancha = _canchas.FirstOrDefault(c => c.IdCancha == idCancha);
                precioCancha = cancha?.PrecioPorHora ?? 0;
            }

            decimal totalDetalle = _lineasDetalle.Sum(d => d.Cantidad * d.PrecioUnitario);
            lblTotalEstimado.Text = $"Total estimado: {(precioCancha + totalDetalle):C2}";
        }

        private bool ValidarCampos()
        {
            if (cmbCliente.SelectedValue == null)
            {
                MessageBox.Show("Elegí un cliente.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbCancha.SelectedValue == null)
            {
                MessageBox.Show("Elegí una cancha.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbTurno.SelectedValue == null)
            {
                MessageBox.Show("Elegí un turno.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            foreach (var linea in _lineasDetalle)
            {
                if (string.IsNullOrWhiteSpace(linea.Concepto))
                {
                    MessageBox.Show("Hay una línea de detalle sin concepto.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (linea.Cantidad <= 0)
                {
                    MessageBox.Show($"La cantidad de \"{linea.Concepto}\" tiene que ser mayor a cero.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (linea.PrecioUnitario < 0)
                {
                    MessageBox.Show($"El precio unitario de \"{linea.Concepto}\" no puede ser negativo.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var dto = new ReservaDTO
            {
                IdReserva = _reservaOriginal?.IdReserva ?? 0,
                IdCliente = (int)cmbCliente.SelectedValue!,
                IdCancha = (int)cmbCancha.SelectedValue!,
                IdTurno = (int)cmbTurno.SelectedValue!,
                Fecha = dtpFecha.Value.Date,
                EstadoReserva = cmbEstado.SelectedIndex + 1,
                Sena = numSena.Value,
                Detalles = _lineasDetalle.ToList()
            };

            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                if (EsAlta)
                {
                    await ReservaApiClient.AddAsync(dto);
                }
                else
                {
                    await ReservaApiClient.UpdateAsync(dto);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGuardar.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}