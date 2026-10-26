using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class UsuarioLista : Form
    {
        public UsuarioLista()
        {
            InitializeComponent();
            ConfigurarGrilla();
            Load += async (_, _) => await CargarTodosAsync();
        }

        private void ConfigurarGrilla()
        {
            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdUsuario", HeaderText = "ID", DataPropertyName = "IdUsuario", Width = 50 });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "NombreUsuario", HeaderText = "Usuario", DataPropertyName = "NombreUsuario", Width = 150 });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email", DataPropertyName = "Email", Width = 220 });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { Name = "Rol", HeaderText = "Rol", DataPropertyName = "Rol", Width = 120 });
        }

        private async Task CargarTodosAsync()
        {
            await EjecutarConEsperaAsync(async () =>
            {
                var usuarios = (await UsuarioApiClient.GetAllAsync()).ToList();
                dgvUsuarios.DataSource = usuarios;
            });
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var form = new UsuarioDetalle();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                _ = CargarTodosAsync();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccionadoOAvisar();
            if (seleccionado == null) return;

            using var form = new UsuarioDetalle(seleccionado);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                _ = CargarTodosAsync();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            var seleccionado = ObtenerSeleccionadoOAvisar();
            if (seleccionado == null) return;

            if (SesionActual.Usuario != null && seleccionado.IdUsuario == SesionActual.Usuario.IdUsuario)
            {
                MessageBox.Show("No podés eliminarte a vos mismo: te quedarías sin acceso al sistema.",
                    "Operación no permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Eliminar el usuario \"{seleccionado.NombreUsuario}\"? Ya no va a poder iniciar sesión.",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes) return;

            await EjecutarConEsperaAsync(async () =>
            {
                await UsuarioApiClient.DeleteAsync(seleccionado.IdUsuario);
                await CargarTodosAsync();
            });
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnEditar_Click(sender, e);
        }

        private UsuarioDTO? ObtenerSeleccionadoOAvisar()
        {
            var seleccionado = dgvUsuarios.CurrentRow?.DataBoundItem as UsuarioDTO;
            if (seleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return seleccionado;
        }

        private void dgvUsuarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Rol" && e.Value is int rol)
            {
                // 1=Administrador, 2=Cliente (Domain.Model.RolUsuario).
                e.Value = rol == 1 ? "Administrador" : "Cliente";
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
