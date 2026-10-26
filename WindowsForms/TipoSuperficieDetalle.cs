using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class TipoSuperficieDetalle : Form
    {
        private readonly TipoSuperficieDTO? _tipoOriginal;
        private bool EsAlta => _tipoOriginal == null;

        public TipoSuperficieDetalle()
        {
            InitializeComponent();
            _tipoOriginal = null;
        }

        public TipoSuperficieDetalle(TipoSuperficieDTO tipo)
        {
            InitializeComponent();
            _tipoOriginal = tipo;
        }

        private void TipoSuperficieDetalle_Load(object sender, EventArgs e)
        {
            Text = EsAlta ? "Nuevo tipo de cancha" : "Editar tipo de cancha";

            cmbDeporte.Items.AddRange(new object[] { "Futbol", "Padel" });

            if (EsAlta)
            {
                cmbDeporte.SelectedIndex = 0;
            }
            else if (_tipoOriginal != null)
            {
                txtNombre.Text = _tipoOriginal.Nombre;
                cmbDeporte.SelectedItem = _tipoOriginal.Deporte;
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (cmbDeporte.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná el deporte.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDeporte.Focus();
                return;
            }

            var dto = new TipoSuperficieDTO
            {
                IdTipoSuperficie = _tipoOriginal?.IdTipoSuperficie ?? 0,
                Nombre = txtNombre.Text.Trim(),
                Deporte = cmbDeporte.SelectedItem.ToString()!,
                Estado = _tipoOriginal?.Estado ?? 1
            };

            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                if (EsAlta)
                {
                    await TipoSuperficieApiClient.AddAsync(dto);
                }
                else
                {
                    await TipoSuperficieApiClient.UpdateAsync(dto);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ErroresUI.Mostrar(this, ex, "No se pudo guardar");
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
