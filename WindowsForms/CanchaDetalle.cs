using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class CanchaDetalle : Form
    {
        private static readonly int[] ValoresTipoFutbol = { 5, 7, 11 };

        private readonly CanchaDTO? _canchaOriginal;
        private bool EsAlta => _canchaOriginal == null;

        private List<TipoSuperficieDTO> _tiposFutbol = new();
        private List<TipoSuperficieDTO> _tiposPadel = new();

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

        private async void CanchaDetalle_Load(object sender, EventArgs e)
        {
            Text = EsAlta ? "Nueva cancha" : "Editar cancha";

            cmbTipoFutbol.Items.AddRange(new object[] { "Fútbol 5", "Fútbol 7", "Fútbol 11" });
            cmbTipoFutbol.SelectedIndex = 0;

            try
            {
                var todosTipos = (await TipoSuperficieApiClient.GetAllAsync()).ToList();
                _tiposFutbol = todosTipos.Where(t => t.Deporte.Equals("Futbol", StringComparison.OrdinalIgnoreCase)).ToList();
                _tiposPadel = todosTipos.Where(t => t.Deporte.Equals("Padel", StringComparison.OrdinalIgnoreCase)).ToList();

                cmbSuperficie.DataSource = _tiposFutbol;
                cmbSuperficie.DisplayMember = "Nombre";
                cmbSuperficie.ValueMember = "IdTipoSuperficie";

                cmbTipoPared.DataSource = _tiposPadel;
                cmbTipoPared.DisplayMember = "Nombre";
                cmbTipoPared.ValueMember = "IdTipoSuperficie";
            }
            catch (Exception ex)
            {
                ErroresUI.Mostrar(this, ex, "Error al cargar tipos de cancha");
            }

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

                radFutbol.Enabled = false;
                radPadel.Enabled = false;

                int idSup = _canchaOriginal.IdTipoSuperficie > 0 ? _canchaOriginal.IdTipoSuperficie : (_canchaOriginal.TipoSuperficie ?? 0);
                if (esPadel)
                {
                    if (idSup > 0) cmbTipoPared.SelectedValue = idSup;
                }
                else
                {
                    cmbTipoFutbol.SelectedIndex = Array.IndexOf(ValoresTipoFutbol, _canchaOriginal.TipoFutbol ?? 5);
                    if (idSup > 0) cmbSuperficie.SelectedValue = idSup;
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

            bool esPadel = radPadel.Checked;
            if (esPadel && cmbTipoPared.SelectedValue == null)
            {
                MessageBox.Show("Seleccioná el tipo de cancha/pared.", "Dato incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!esPadel && cmbSuperficie.SelectedValue == null)
            {
                MessageBox.Show("Seleccioná la superficie de fútbol.", "Dato incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            bool esPadel = radPadel.Checked;
            int idSuperficie = esPadel ? (int)cmbTipoPared.SelectedValue! : (int)cmbSuperficie.SelectedValue!;

            var dto = new CanchaDTO
            {
                IdCancha = _canchaOriginal?.IdCancha ?? 0,
                Nombre = txtNombre.Text.Trim(),
                PrecioPorHora = numPrecioPorHora.Value,
                TipoCancha = esPadel ? "Padel" : "Futbol",
                EsTechada = chkTechada.Checked,
                IdTipoSuperficie = idSuperficie,
                TipoFutbol = esPadel ? null : ValoresTipoFutbol[cmbTipoFutbol.SelectedIndex]
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
