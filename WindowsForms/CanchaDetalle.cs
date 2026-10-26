using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class CanchaDetalle : Form
    {
        // Valores del enum TipoFutbol (Domain.Model) en el mismo orden que el combo: la posición
        // 0 es Fútbol 5, la 1 es Fútbol 7 y la 2 es Fútbol 11. Superficie y Pared no lo
        // necesitan porque sus valores son 1, 2, 3... (alcanza con SelectedIndex + 1, como el
        // estado en ReservaDetalle).
        private static readonly int[] ValoresTipoFutbol = { 5, 7, 11 };

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

            cmbTipoFutbol.Items.AddRange(new object[] { "Fútbol 5", "Fútbol 7", "Fútbol 11" });
            cmbSuperficie.Items.AddRange(new object[] { "Césped sintético", "Césped natural", "Cemento" });
            cmbTipoPared.Items.AddRange(new object[] { "Vidrio", "Muro" });
            cmbTipoFutbol.SelectedIndex = 0;
            cmbSuperficie.SelectedIndex = 0;
            cmbTipoPared.SelectedIndex = 0;

            if (EsAlta)
            {
                radFutbol.Checked = true;
                ActualizarVisibilidadPaneles();
            }
            else if (_canchaOriginal != null)
            {
                txtNombre.Text = _canchaOriginal.Nombre;
                numPrecioPorHora.Value = _canchaOriginal.PrecioPorHora;
                chkTechada.Checked = _canchaOriginal.EsTechada;

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
                    cmbTipoPared.SelectedIndex = (_canchaOriginal.TipoPared ?? 1) - 1;
                }
                else
                {
                    cmbTipoFutbol.SelectedIndex = Array.IndexOf(ValoresTipoFutbol, _canchaOriginal.TipoFutbol ?? 5);
                    cmbSuperficie.SelectedIndex = (_canchaOriginal.TipoSuperficie ?? 1) - 1;
                }

                ActualizarVisibilidadPaneles();
            }
        }

        private void radTipo_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarVisibilidadPaneles();
        }

        // Cada tipo de cancha muestra solo sus datos propios.
        private void ActualizarVisibilidadPaneles()
        {
            pnlFutbol.Visible = radFutbol.Checked;
            pnlPadel.Visible = radPadel.Checked;
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

            bool esPadel = radPadel.Checked;
            var dto = new CanchaDTO
            {
                IdCancha = _canchaOriginal?.IdCancha ?? 0,
                Nombre = txtNombre.Text.Trim(),
                PrecioPorHora = numPrecioPorHora.Value,
                TipoCancha = esPadel ? "Padel" : "Futbol",
                EsTechada = chkTechada.Checked,
                TipoFutbol = esPadel ? null : ValoresTipoFutbol[cmbTipoFutbol.SelectedIndex],
                TipoSuperficie = esPadel ? null : cmbSuperficie.SelectedIndex + 1,
                TipoPared = esPadel ? cmbTipoPared.SelectedIndex + 1 : null
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
