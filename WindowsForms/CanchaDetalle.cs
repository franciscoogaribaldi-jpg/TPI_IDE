using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class CanchaDetalle : Form
    {
        // Opciones de los combos: el valor es el int del enum de Domain.Model (WinForms no lo
        // referencia a propósito) y el texto es lo que ve el usuario.
        private static readonly (int Valor, string Texto)[] TiposFutbol =
            { (5, "Fútbol 5"), (7, "Fútbol 7"), (11, "Fútbol 11") };
        private static readonly (int Valor, string Texto)[] Superficies =
            { (1, "Césped sintético"), (2, "Césped natural"), (3, "Cemento") };
        private static readonly (int Valor, string Texto)[] Paredes =
            { (1, "Vidrio"), (2, "Muro") };

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

            CargarCombo(cmbTipoFutbol, TiposFutbol);
            CargarCombo(cmbSuperficie, Superficies);
            CargarCombo(cmbTipoPared, Paredes);

            if (EsAlta)
            {
                radFutbol.Checked = true;
                ActualizarVisibilidadPaneles();
            }
            else if (_canchaOriginal != null)
            {
                txtNombre.Text = _canchaOriginal.Nombre;
                numPrecioPorHora.Value = _canchaOriginal.PrecioPorHora;
                cmbEstado.SelectedIndex = _canchaOriginal.Estado == 1 ? 0 : 1;
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
                    SeleccionarValor(cmbTipoPared, Paredes, _canchaOriginal.TipoPared);
                }
                else
                {
                    SeleccionarValor(cmbTipoFutbol, TiposFutbol, _canchaOriginal.TipoFutbol);
                    SeleccionarValor(cmbSuperficie, Superficies, _canchaOriginal.TipoSuperficie);
                }

                ActualizarVisibilidadPaneles();
            }
        }

        private static void CargarCombo(ComboBox combo, (int Valor, string Texto)[] opciones)
        {
            combo.Items.Clear();
            foreach (var opcion in opciones)
                combo.Items.Add(opcion.Texto);
            combo.SelectedIndex = 0;
        }

        private static void SeleccionarValor(ComboBox combo, (int Valor, string Texto)[] opciones, int? valor)
        {
            int indice = Array.FindIndex(opciones, o => o.Valor == valor);
            combo.SelectedIndex = indice >= 0 ? indice : 0;
        }

        private static int ValorSeleccionado(ComboBox combo, (int Valor, string Texto)[] opciones)
        {
            return opciones[combo.SelectedIndex].Valor;
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
                Estado = cmbEstado.SelectedIndex == 0 ? 1 : 0,
                PrecioPorHora = numPrecioPorHora.Value,
                TipoCancha = esPadel ? "Padel" : "Futbol",
                EsTechada = chkTechada.Checked,
                TipoFutbol = esPadel ? null : ValorSeleccionado(cmbTipoFutbol, TiposFutbol),
                TipoSuperficie = esPadel ? null : ValorSeleccionado(cmbSuperficie, Superficies),
                TipoPared = esPadel ? ValorSeleccionado(cmbTipoPared, Paredes) : null
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
