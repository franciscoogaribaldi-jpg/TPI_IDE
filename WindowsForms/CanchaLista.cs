using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class CanchaLista : Form
    {
        public CanchaLista()
        {
            InitializeComponent();
            Load += async (_, _) => await CargarTodosAsync();
        }

        private async Task CargarTodosAsync()
        {
            await EjecutarConEsperaAsync(async () =>
            {
                var canchas = (await CanchaApiClient.GetAllAsync()).ToList();
                dgvCanchas.DataSource = canchas;
                AjustarColumnas();
            });
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var form = new CanchaDetalle();
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
                MessageBox.Show("Seleccioná una cancha de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new CanchaDetalle(seleccionada);
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
                MessageBox.Show("Seleccioná una cancha de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Eliminar la cancha \"{seleccionada.Nombre}\"?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes) return;

            await EjecutarConEsperaAsync(async () =>
            {
                await CanchaApiClient.DeleteAsync(seleccionada.IdCancha);
                await CargarTodosAsync();
            });
        }

        private void dgvCanchas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnEditar_Click(sender, e);
        }

        private CanchaDTO? ObtenerSeleccionada()
        {
            return dgvCanchas.CurrentRow?.DataBoundItem as CanchaDTO;
        }

        private void AjustarColumnas()
        {
            if (dgvCanchas.Columns["IdCancha"] != null)
                dgvCanchas.Columns["IdCancha"].HeaderText = "ID";

            if (dgvCanchas.Columns["PrecioPorHora"] != null)
                dgvCanchas.Columns["PrecioPorHora"].DefaultCellStyle.Format = "C2";

            if (dgvCanchas.Columns["PrecioTotalRaquetas"] != null)
                dgvCanchas.Columns["PrecioTotalRaquetas"].DefaultCellStyle.Format = "C2";
        }

        private void dgvCanchas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCanchas.Columns[e.ColumnIndex].Name == "Estado" && e.Value is int estado)
            {
                e.Value = estado == 1 ? "Activo" : "Inactivo";
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
