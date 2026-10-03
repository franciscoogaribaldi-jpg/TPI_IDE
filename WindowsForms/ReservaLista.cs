using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class ReservaLista : Form
    {
        public ReservaLista()
        {
            InitializeComponent();
            Load += async (_, _) => await CargarTodosAsync();
        }

        private async Task CargarTodosAsync()
        {
            await EjecutarConEsperaAsync(async () =>
            {
                var reservas = (await ReservaApiClient.GetAllAsync()).ToList();
                dgvReservas.DataSource = reservas;
            });
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var form = new ReservaDetalle();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                _ = CargarTodosAsync();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            var seleccionada = ObtenerSeleccionada();
            if (seleccionada == null)
            {
                MessageBox.Show("Seleccioná una reserva de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new ReservaDetalle(seleccionada);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                _ = CargarTodosAsync();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            var seleccionada = ObtenerSeleccionada();
            if (seleccionada == null)
            {
                MessageBox.Show("Seleccioná una reserva de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Eliminar la reserva de {seleccionada.NombreCliente} del {seleccionada.Fecha:dd/MM/yyyy}?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes) return;

            await EjecutarConEsperaAsync(async () =>
            {
                await ReservaApiClient.DeleteAsync(seleccionada.IdReserva);
                await CargarTodosAsync();
            });
        }

        private void dgvReservas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnEditar_Click(sender, e);
        }

        private ReservaDTO? ObtenerSeleccionada()
        {
            return dgvReservas.CurrentRow?.DataBoundItem as ReservaDTO;
        }

        private void dgvReservas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvReservas.Columns[e.ColumnIndex].Name == "colEstado" && e.Value is int estado)
            {
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }
}