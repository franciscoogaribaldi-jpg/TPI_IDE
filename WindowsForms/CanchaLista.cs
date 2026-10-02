using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class CanchaLista : Form
    {
        public CanchaLista()
        {
            InitializeComponent();
            ConfigurarGrilla();
            Load += async (_, _) => await CargarTodosAsync();
        }

        private async Task CargarTodosAsync()
        {
            await EjecutarConEsperaAsync(async () =>
            {
                var canchas = (await CanchaApiClient.GetAllAsync()).ToList();
                dgvCanchas.DataSource = canchas;
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

        private void ConfigurarGrilla()
        {
            dgvCanchas.AutoGenerateColumns = false;
            dgvCanchas.Columns.Clear();

            dgvCanchas.Columns.Add(new DataGridViewTextBoxColumn { 
                Name = "IdCancha", HeaderText = "ID", DataPropertyName = "IdCancha", Width = 50 
            });
            dgvCanchas.Columns.Add(new DataGridViewTextBoxColumn { 
                Name = "Nombre", HeaderText = "Nombre", DataPropertyName = "Nombre" 
            });
            dgvCanchas.Columns.Add(new DataGridViewTextBoxColumn { 
                Name = "TipoCancha", HeaderText = "Tipo de Cancha", DataPropertyName = "TipoCancha" 
            });

            var colPrecioHora = new DataGridViewTextBoxColumn { 
                Name = "PrecioPorHora", HeaderText = "Precio p/Hora", DataPropertyName = "PrecioPorHora" 
            };
            colPrecioHora.DefaultCellStyle.Format = "C2";
            dgvCanchas.Columns.Add(colPrecioHora);

            dgvCanchas.Columns.Add(new DataGridViewTextBoxColumn { 
                Name = "CantidadRaquetas", HeaderText = "Cant. Raquetas", DataPropertyName = "CantidadRaquetas" 
            });

            var colPrecioRaquetas = new DataGridViewTextBoxColumn { 
                Name = "PrecioTotalRaquetas", HeaderText = "Total Raquetas", DataPropertyName = "PrecioTotalRaquetas" 
            };
            colPrecioRaquetas.DefaultCellStyle.Format = "C2";
            dgvCanchas.Columns.Add(colPrecioRaquetas);

            dgvCanchas.Columns.Add(new DataGridViewTextBoxColumn { 
                Name = "Estado", HeaderText = "Estado", DataPropertyName = "Estado" 
            });
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
