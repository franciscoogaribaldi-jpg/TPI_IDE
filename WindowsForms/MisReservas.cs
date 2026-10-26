using API.Clients;
using DTOs;

namespace WindowsForms
{
    /// <summary>
    /// Pantalla del rol Cliente: sus propias reservas. Desde acá puede reservar un turno nuevo y
    /// cancelar una reserva futura. La API sabe quién es el cliente por el token.
    /// </summary>
    public partial class MisReservas : Form
    {
        public MisReservas()
        {
            InitializeComponent();
            ConfigurarGrilla();
            Load += async (_, _) => await CargarAsync();
        }

        private void ConfigurarGrilla()
        {
            dgvReservas.AutoGenerateColumns = false;
            dgvReservas.Columns.Clear();

            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Fecha", HeaderText = "Fecha", DataPropertyName = "Fecha", Width = 100,
                DefaultCellStyle = { Format = "dd/MM/yyyy" }
            });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn { Name = "NombreCancha", HeaderText = "Cancha", DataPropertyName = "NombreCancha", Width = 150 });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn { Name = "HorarioTurno", HeaderText = "Turno", DataPropertyName = "HorarioTurno", Width = 120 });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn { Name = "EstadoReserva", HeaderText = "Estado", DataPropertyName = "EstadoReserva", Width = 110 });
            dgvReservas.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ImporteTotal", HeaderText = "Importe", DataPropertyName = "ImporteTotal", Width = 120,
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private async Task CargarAsync()
        {
            Cursor = Cursors.WaitCursor;
            Enabled = false;
            try
            {
                var reservas = (await ReservaApiClient.GetMisReservasAsync()).ToList();
                dgvReservas.DataSource = reservas;
            }
            catch (Exception ex)
            {
                ErroresUI.Mostrar(this, ex);
            }
            finally
            {
                Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private async void btnReservar_Click(object sender, EventArgs e)
        {
            using var form = new ReservarTurno();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                await CargarAsync();
            }
        }

        private async void btnCancelar_Click(object sender, EventArgs e)
        {
            var seleccionada = dgvReservas.CurrentRow?.DataBoundItem as ReservaDTO;
            if (seleccionada == null)
            {
                MessageBox.Show("Seleccioná una reserva de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Cancelar tu reserva del {seleccionada.Fecha:dd/MM/yyyy} en {seleccionada.NombreCancha} ({seleccionada.HorarioTurno})?",
                "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmacion != DialogResult.Yes) return;

            try
            {
                await ReservaApiClient.CancelarMiReservaAsync(seleccionada.IdReserva);
                await CargarAsync();
            }
            catch (Exception ex)
            {
                // Por ejemplo: "No se puede cancelar una reserva que ya pasó."
                ErroresUI.Mostrar(this, ex, "No se pudo cancelar");
            }
        }

        private void dgvReservas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvReservas.Columns[e.ColumnIndex].Name == "EstadoReserva" && e.Value is int estado)
            {
                // 1 Pendiente, 2 Confirmada, 3 Cancelada, 4 Finalizada (Domain.Model.EstadoReserva).
                e.Value = estado switch
                {
                    1 => "Pendiente",
                    2 => "Confirmada",
                    3 => "Cancelada",
                    4 => "Finalizada",
                    _ => estado.ToString()
                };
                e.FormattingApplied = true;
            }
        }
    }
}
