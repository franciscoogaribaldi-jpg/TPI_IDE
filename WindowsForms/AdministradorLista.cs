using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class AdministradorLista : Form
    {
        public AdministradorLista()
        {
            InitializeComponent();
            ConfigurarGrilla();
            Load += async (_, _) => await CargarTodosAsync();
        }

        private void ConfigurarGrilla()
        {
            dgvAdministradores.AutoGenerateColumns = false;
            dgvAdministradores.Columns.Clear();

            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdAdministrador", HeaderText = "ID", DataPropertyName = "IdAdministrador", Width = 50 });
            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Nombre", DataPropertyName = "Nombre" });
            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn { Name = "Apellido", HeaderText = "Apellido", DataPropertyName = "Apellido" });
            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn { Name = "Telefono", HeaderText = "Teléfono", DataPropertyName = "Telefono" });
            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn { Name = "NombreUsuario", HeaderText = "Usuario", DataPropertyName = "NombreUsuario" });
            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email", DataPropertyName = "Email", Width = 180 });
        }

        private async Task CargarTodosAsync()
        {
            await EjecutarConEsperaAsync(async () =>
            {
                var administradores = (await AdministradorApiClient.GetAllAsync()).ToList();
                dgvAdministradores.DataSource = administradores;
            });
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var form = new AdministradorDetalle();
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
                MessageBox.Show("Seleccioná un administrador de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new AdministradorDetalle(seleccionado);
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
                MessageBox.Show("Seleccioná un administrador de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Eliminar a {seleccionado.Nombre} {seleccionado.Apellido}? También se elimina su usuario \"{seleccionado.NombreUsuario}\" y ya no va a poder iniciar sesión.",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes) return;

            await EjecutarConEsperaAsync(async () =>
            {
                await AdministradorApiClient.DeleteAsync(seleccionado.IdAdministrador);
                await CargarTodosAsync();
            });
        }

        private void dgvAdministradores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnEditar_Click(sender, e);
        }

        private AdministradorDTO? ObtenerSeleccionado()
        {
            return dgvAdministradores.CurrentRow?.DataBoundItem as AdministradorDTO;
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
