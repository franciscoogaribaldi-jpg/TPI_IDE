using API.Clients;
using DTOs;

namespace WindowsForms
{
    /// <summary>
    /// Rol Cliente: elige una fecha, ve los turnos libres de todas las canchas y reserva uno.
    /// La reserva queda Pendiente a su nombre (la API toma el cliente del token).
    /// </summary>
    public partial class ReservarTurno : Form
    {
        public ReservarTurno()
        {
            InitializeComponent();
            ConfigurarGrilla();

            // No se puede reservar en el pasado: el calendario arranca en hoy.
            dtpFecha.MinDate = DateTime.Today;
            dtpFecha.Value = DateTime.Today.AddDays(1);

            Load += async (_, _) => await BuscarAsync();
        }

        private void ConfigurarGrilla()
        {
            dgvLibres.AutoGenerateColumns = false;
            dgvLibres.Columns.Clear();

            dgvLibres.Columns.Add(new DataGridViewTextBoxColumn { Name = "NombreCancha", HeaderText = "Cancha", DataPropertyName = "NombreCancha", Width = 120 });
            dgvLibres.Columns.Add(new DataGridViewTextBoxColumn { Name = "Caracteristicas", HeaderText = "Características", DataPropertyName = "Caracteristicas", Width = 200 });
            dgvLibres.Columns.Add(new DataGridViewTextBoxColumn { Name = "Horario", HeaderText = "Turno", DataPropertyName = "Horario", Width = 110 });
            dgvLibres.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PrecioPorHora", HeaderText = "Precio", DataPropertyName = "PrecioPorHora", Width = 110,
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await BuscarAsync();
        }

        private async Task BuscarAsync()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var libres = (await ReservaApiClient.GetTurnosLibresAsync(dtpFecha.Value.Date)).ToList();
                dgvLibres.DataSource = libres;
                lblResultado.Text = libres.Count == 0
                    ? "No hay turnos libres ese día."
                    : $"{libres.Count} turnos libres. Elegí uno y tocá \"Reservar\".";
            }
            catch (Exception ex)
            {
                ErroresUI.Mostrar(this, ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void btnReservar_Click(object sender, EventArgs e)
        {
            var elegido = dgvLibres.CurrentRow?.DataBoundItem as TurnoLibreDTO;
            if (elegido == null)
            {
                MessageBox.Show("Elegí un turno de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Reservar {elegido.NombreCancha} el {dtpFecha.Value:dd/MM/yyyy} de {elegido.Horario} ({elegido.PrecioPorHora:C2})?",
                "Confirmar reserva", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            btnReservar.Enabled = false;
            try
            {
                await ReservaApiClient.ReservarAsync(elegido.IdCancha, elegido.IdTurno, dtpFecha.Value.Date);

                MessageBox.Show("¡Listo! Tu reserva quedó registrada como Pendiente.", "Reserva registrada",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                // Por ejemplo, si otro cliente reservó ese turno mientras tanto.
                ErroresUI.Mostrar(this, ex, "No se pudo reservar");
                await BuscarAsync();
            }
            finally
            {
                btnReservar.Enabled = true;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
