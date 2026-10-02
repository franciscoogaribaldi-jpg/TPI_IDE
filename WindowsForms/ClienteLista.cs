using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class ClienteLista : Form
    {
        public ClienteLista()
        {
            InitializeComponent();
            ConfigurarGrilla();
            Load += async (_, _) => await CargarTodosAsync();
        }

        // nuevo metodo
        private void ConfigurarGrilla()
        {
            dgvClientes.AutoGenerateColumns = false;
            dgvClientes.Columns.Clear();

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdCliente", HeaderText = "ID", DataPropertyName = "IdCliente", Width = 50 });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nombre", HeaderText = "Nombre", DataPropertyName = "Nombre" });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Apellido", HeaderText = "Apellido", DataPropertyName = "Apellido" });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Dni", HeaderText = "DNI", DataPropertyName = "Dni" });
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Telefono", HeaderText = "Teléfono", DataPropertyName = "Telefono" });

            var colFecha = new DataGridViewTextBoxColumn { Name = "FechaNacimiento", HeaderText = "F. Nacimiento", DataPropertyName = "FechaNacimiento" };
            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvClientes.Columns.Add(colFecha);

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Estado", HeaderText = "Estado", DataPropertyName = "Estado" });

            
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn { Name = "NombreUsuario", HeaderText = "Registrado Por", DataPropertyName = "NombreUsuario" });
        }



        private async Task CargarTodosAsync()
        {
            await EjecutarConEsperaAsync(async () =>
            {
                var clientes = (await ClienteApiClient.GetAllAsync()).ToList();
                dgvClientes.DataSource = clientes;
                
            });
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await EjecutarConEsperaAsync(async () =>
            {
                var clientes = (await ClienteApiClient.BuscarAsync(txtBuscar.Text.Trim())).ToList();
                dgvClientes.DataSource = clientes;
               
            });
        }

        private async void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            await CargarTodosAsync();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using var form = new ClienteDetalle();
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
                MessageBox.Show("Seleccioná un cliente de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var form = new ClienteDetalle(seleccionado);
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
                MessageBox.Show("Seleccioná un cliente de la lista primero.", "Nada seleccionado",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmacion = MessageBox.Show(
                $"¿Eliminar a {seleccionado.Nombre} {seleccionado.Apellido} (DNI {seleccionado.Dni})?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes) return;

            await EjecutarConEsperaAsync(async () =>
            {
                await ClienteApiClient.DeleteAsync(seleccionado.IdCliente);
                await CargarTodosAsync();
            });
        }

        private void dgvClientes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnEditar_Click(sender, e);
        }

        private ClienteDTO? ObtenerSeleccionado()
        {
            return dgvClientes.CurrentRow?.DataBoundItem as ClienteDTO;
        }

       

        private void dgvClientes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvClientes.Columns[e.ColumnIndex].Name == "Estado" && e.Value is int estado)
            {
                // 1=Activo, 0=Inactivo (Domain.Model.Estado). No referenciamos Domain.Model
                // desde la UI a propósito, para no acoplar el escritorio al dominio.
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
