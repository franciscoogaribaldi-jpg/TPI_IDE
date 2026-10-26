using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class TipoSuperficieLista : Form
    {
        public TipoSuperficieLista()
        {
            InitializeComponent();
            Load += async (_, _) => await CargarTodosAsync();
        }

        private async Task CargarTodosAsync()
        {
            await EjecutarConEsperaAsync(async () =>
            {
                var tipos = (await TipoSuperficieApiClient.GetAllAsync()).ToList();
                dgvTipos.DataSource = tipos;
            });
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var form = new TipoSuperficieDetalle();
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
                MessageBox.Show("Seleccioná un tipo de cancha primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new TipoSuperficieDetalle(seleccionado);
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
                MessageBox.Show("Seleccioná un tipo de cancha primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Eliminar el tipo de cancha '{seleccionado.Nombre}' ({seleccionado.Deporte})?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes) return;

            await EjecutarConEsperaAsync(async () =>
            {
                await TipoSuperficieApiClient.DeleteAsync(seleccionado.IdTipoSuperficie);
                await CargarTodosAsync();
            });
        }

        private void dgvTipos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnEditar_Click(sender, e);
        }

        private TipoSuperficieDTO? ObtenerSeleccionado()
        {
            return dgvTipos.CurrentRow?.DataBoundItem as TipoSuperficieDTO;
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
