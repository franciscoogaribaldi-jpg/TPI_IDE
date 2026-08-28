using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class CanchaDetalle : Form
    {
        private readonly CanchaDTO? _canchaOriginal;
        private bool EsAlta => _canchaOriginal == null;

        public CanchaDetalle()
        {
            InitializeComponent();
            _canchaOriginal = null;
        }

        public CanchaDetalle(CanchaDTO cancha)
        {
            InitializeComponent();
            _canchaOriginal = cancha;
        }

        private void CanchaDetalle_Load(object sender, EventArgs e)
        {
            Text = EsAlta ? "Nueva cancha" : "Editar cancha";

            cmbEstado.DataSource = new[] { "Activo", "Inactivo" };
            cmbEstado.SelectedIndex = 0;

            if (EsAlta)
            {
                radFutbol.Checked = true;
                ActualizarVisibilidadRaquetas();
            }
            else if (_canchaOriginal != null)
            {
                txtNombre.Text = _canchaOriginal.Nombre;
                numPrecioPorHora.Value = _canchaOriginal.PrecioPorHora;
                cmbEstado.SelectedIndex = _canchaOriginal.Estado == 1 ? 0 : 1;

                bool esPadel = _canchaOriginal.TipoCancha == "Padel";
                radFutbol.Checked = !esPadel;
                radPadel.Checked = esPadel;

                // No se puede cambiar el tipo de una cancha ya creada (limitación conocida
                // del repositorio, ver comentario en CanchaRepository.UpdateAsync). Se
                // muestran los radios así el usuario ve el tipo, pero no los puede tocar.
                radFutbol.Enabled = false;
                radPadel.Enabled = false;

                if (esPadel)
                {
                    numCantidadRaquetas.Value = _canchaOriginal.CantidadRaquetas ?? 0;
                    numPrecioTotalRaquetas.Value = _canchaOriginal.PrecioTotalRaquetas ?? 0;
                }

                ActualizarVisibilidadRaquetas();
            }
        }

        private void radTipo_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarVisibilidadRaquetas();
        }

        private void ActualizarVisibilidadRaquetas()
        {
            pnlRaquetas.Visible = radPadel.Checked;
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (numPrecioPorHora.Value <= 0)
            {
                MessageBox.Show("El precio por hora debe ser mayor a cero.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numPrecioPorHora.Focus();
                return false;
            }

            return true;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var dto = new CanchaDTO
            {
                IdCancha = _canchaOriginal?.IdCancha ?? 0,
                Nombre = txtNombre.Text.Trim(),
                Estado = cmbEstado.SelectedIndex == 0 ? 1 : 0,
                PrecioPorHora = numPrecioPorHora.Value,
                TipoCancha = radPadel.Checked ? "Padel" : "Futbol",
                CantidadRaquetas = radPadel.Checked ? (int)numCantidadRaquetas.Value : null,
                PrecioTotalRaquetas = radPadel.Checked ? numPrecioTotalRaquetas.Value : null
            };

            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                if (EsAlta)
                {
                    await CanchaApiClient.AddAsync(dto);
                }
                else
                {
                    await CanchaApiClient.UpdateAsync(dto);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGuardar.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
