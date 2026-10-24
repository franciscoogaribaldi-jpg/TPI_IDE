using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class TurnoDetalle : Form
    {
        private readonly TurnoDTO? _turnoOriginal;
        private bool EsAlta => _turnoOriginal == null;

        public TurnoDetalle()
        {
            InitializeComponent();
            _turnoOriginal = null;
        }

        public TurnoDetalle(TurnoDTO turno)
        {
            InitializeComponent();
            _turnoOriginal = turno;
        }

        private void TurnoDetalle_Load(object sender, EventArgs e)
        {
            Text = EsAlta ? "Nuevo turno" : "Editar turno";
            cmbEstado.DataSource = new[] { "Activo", "Inactivo" };

            if (EsAlta)
            {
                dtpHoraInicio.Value = DateTime.Today.AddHours(8);
                dtpHoraFin.Value = DateTime.Today.AddHours(9);
                cmbEstado.SelectedIndex = 0;
                cmbEstado.Enabled = false; // el servidor siempre crea el turno como Activo
            }
            else if (_turnoOriginal != null)
            {
                dtpHoraInicio.Value = DateTime.Today + TimeSpan.Parse(_turnoOriginal.HoraInicio);
                dtpHoraFin.Value = DateTime.Today + TimeSpan.Parse(_turnoOriginal.HoraFin);
                cmbEstado.SelectedIndex = _turnoOriginal.Estado == 1 ? 0 : 1;
            }
        }

        // DateTime -> "HH:mm:ss" (el formato que espera la API)
        private static string FormatearHora(DateTime valor)
        {
            return new TimeSpan(valor.Hour, valor.Minute, 0).ToString(@"hh\:mm\:ss");
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (dtpHoraInicio.Value.TimeOfDay >= dtpHoraFin.Value.TimeOfDay)
            {
                MessageBox.Show("La hora de inicio debe ser menor a la hora de fin.", "Dato inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dto = new TurnoDTO
            {
                IdTurno = _turnoOriginal?.IdTurno ?? 0,
                HoraInicio = FormatearHora(dtpHoraInicio.Value),
                HoraFin = FormatearHora(dtpHoraFin.Value),
                Estado = cmbEstado.SelectedIndex == 0 ? 1 : 0
            };

            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                if (EsAlta)
                {
                    await TurnoApiClient.AddAsync(dto);
                }
                else
                {
                    await TurnoApiClient.UpdateAsync(dto);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                // Acá llegan los mensajes del servidor, por ejemplo el de turno superpuesto.
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
