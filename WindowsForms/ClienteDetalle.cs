using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class ClienteDetalle : Form
    {
        private readonly ClienteDTO? _clienteOriginal;
        private bool EsAlta => _clienteOriginal == null;

        public ClienteDetalle()
        {
            InitializeComponent();
            _clienteOriginal = null;
        }

        public ClienteDetalle(ClienteDTO cliente)
        {
            InitializeComponent();
            _clienteOriginal = cliente;
        }

        private void ClienteDetalle_Load(object sender, EventArgs e)
        {
            Text = EsAlta ? "Nuevo cliente" : "Editar cliente";

            cmbEstado.DataSource = new[] { "Activo", "Inactivo" };
            cmbEstado.SelectedIndex = 0;

            if (!EsAlta && _clienteOriginal != null)
            {
                txtNombre.Text = _clienteOriginal.Nombre;
                txtApellido.Text = _clienteOriginal.Apellido;
                txtDni.Text = _clienteOriginal.Dni;
                txtTelefono.Text = _clienteOriginal.Telefono;
                dtpFechaNacimiento.Value = _clienteOriginal.FechaNacimiento;
                cmbEstado.SelectedIndex = _clienteOriginal.Estado == 1 ? 0 : 1;
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDni.Text))
            {
                MessageBox.Show("El DNI es obligatorio.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return false;
            }

            if (dtpFechaNacimiento.Value.Date >= DateTime.Today)
            {
                MessageBox.Show("La fecha de nacimiento debe ser anterior a hoy.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpFechaNacimiento.Focus();
                return false;
            }

            

            return true;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var dto = new ClienteDTO
            {
                IdCliente = _clienteOriginal?.IdCliente ?? 0,
                IdUsuario = SesionActual.Usuario.IdUsuario,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Dni = txtDni.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                FechaNacimiento = dtpFechaNacimiento.Value.Date,
                Estado = cmbEstado.SelectedIndex == 0 ? 1 : 0
            };

            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                if (EsAlta)
                {
                    await ClienteApiClient.AddAsync(dto);
                }
                else
                {
                    await ClienteApiClient.UpdateAsync(dto);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                // Acá llegan tanto los ArgumentException (datos inválidos) como los
                // ReglaDeNegocioException (ej. DNI duplicado) que arma el servidor:
                // el mensaje ya viene armado y listo para mostrar.
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

