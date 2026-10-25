using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class TurnoLista : Form
    {
        public TurnoLista()
        {
            InitializeComponent();
            Load += async (_, _) => await CargarTodosAsync();
        }

        private async Task CargarTodosAsync()
        {
            await EjecutarConEsperaAsync(async () =>
            {
                var turnos = (await TurnoApiClient.GetAllAsync()).OrderBy(t => t.HoraInicio).ToList();
                dgvTurnos.DataSource = turnos;
            });
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var form = new TurnoDetalle();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                _ = CargarTodosAsync();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show("Seleccioná un turno de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new TurnoDetalle(seleccionado);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                _ = CargarTodosAsync();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                MessageBox.Show("Seleccioná un turno de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Eliminar el turno {RecortarHora(seleccionado.HoraInicio)} - {RecortarHora(seleccionado.HoraFin)}?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes) return;

            await EjecutarConEsperaAsync(async () =>
            {
                await TurnoApiClient.DeleteAsync(seleccionado.IdTurno);
                await CargarTodosAsync();
            });
        }

        private void dgvTurnos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnEditar_Click(sender, e);
        }

        private void dgvTurnos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var columna = dgvTurnos.Columns[e.ColumnIndex].Name;

            if (columna == "colEstado" && e.Value is int estado)
            {
                e.Value = estado == 1 ? "Activo" : "Inactivo";
                e.FormattingApplied = true;
            }
            else if ((columna == "colHoraInicio" || columna == "colHoraFin") && e.Value is string hora)
            {
                e.Value = RecortarHora(hora);
                e.FormattingApplied = true;
            }
        }

        // "08:00:00" -> "08:00"
        private static string RecortarHora(string hora)
        {
            return hora.Length >= 5 ? hora.Substring(0, 5) : hora;
        }

        private TurnoDTO? ObtenerSeleccionado()
        {
            return dgvTurnos.CurrentRow?.DataBoundItem as TurnoDTO;
        }

        private async Task EjecutarConEsperaAsync(Func<Task> accion)
        {
            Cursor = Cursors.WaitCursor;
            Enabled = false;
            try
            {
                await accion();
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
    }
}
