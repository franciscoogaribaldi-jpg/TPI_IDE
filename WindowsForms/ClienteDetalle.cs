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


            if (!EsAlta && _clienteOriginal != null)
            {
                txtNombre.Text = _clienteOriginal.Nombre;
                txtApellido.Text = _clienteOriginal.Apellido;
                txtDni.Text = _clienteOriginal.Dni;
                txtTelefono.Text = _clienteOriginal.Telefono;
                dtpFechaNacimiento.Value = _clienteOriginal.FechaNacimiento;

                // En la edición el usuario de acceso no se modifica: se muestra solo para consulta
                // y no se pide contraseña.
                txtUsuario.Text = _clienteOriginal.NombreUsuario;
                txtEmail.Text = _clienteOriginal.Email;
                txtUsuario.Enabled = false;
                txtEmail.Enabled = false;
                lblContrasena.Visible = false;
                txtContrasena.Visible = false;

                // Se acomoda el formulario para que no quede el hueco de la contraseña.
                btnGuardar.Top -= 40;
                btnCancelar.Top -= 40;
                ClientSize = new Size(ClientSize.Width, ClientSize.Height - 40);
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

            // Solo en el alta: se crea también el usuario de acceso del cliente.
            if (EsAlta)
            {
                if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    MessageBox.Show("El nombre de usuario es obligatorio.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsuario.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains('@'))
                {
                    MessageBox.Show("Ingresá un email válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtContrasena.Text))
                {
                    MessageBox.Show("La contraseña inicial es obligatoria.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtContrasena.Focus();
                    return false;
                }
            }

            return true;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var dto = new ClienteDTO
            {
                IdCliente = _clienteOriginal?.IdCliente ?? 0,
                // En el alta es 0: el servidor crea el usuario junto con el cliente.
                IdUsuario = _clienteOriginal?.IdUsuario ?? 0,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Dni = txtDni.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                FechaNacimiento = dtpFechaNacimiento.Value.Date,
                NombreUsuario = EsAlta ? txtUsuario.Text.Trim() : _clienteOriginal!.NombreUsuario,
                Email = EsAlta ? txtEmail.Text.Trim() : _clienteOriginal!.Email,
                Contrasena = EsAlta ? txtContrasena.Text : null
            };

            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                if (EsAlta)
                {
                    var creado = await ClienteApiClient.AddAsync(dto);

                    // Si el DNI era de un cliente eliminado, el servidor lo recuperó en vez de crear otro.
                    if (creado.Recuperado)
                        MessageBox.Show("Ese DNI era de un cliente eliminado: se lo recuperó con los datos nuevos y conserva sus reservas anteriores.",
                            "Cliente recuperado", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                // ReglaDeNegocioException (ej. DNI o usuario duplicado) que arma el servidor:
                // el mensaje ya viene armado y listo para mostrar.
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
