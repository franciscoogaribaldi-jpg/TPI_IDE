using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class AdministradorDetalle : Form
    {
        private readonly AdministradorDTO? _administradorOriginal;
        private bool EsAlta => _administradorOriginal == null;

        public AdministradorDetalle()
        {
            InitializeComponent();
            _administradorOriginal = null;
        }

        public AdministradorDetalle(AdministradorDTO administrador)
        {
            InitializeComponent();
            _administradorOriginal = administrador;
        }

        private void AdministradorDetalle_Load(object sender, EventArgs e)
        {
            Text = EsAlta ? "Nuevo administrador" : "Editar administrador";


            if (!EsAlta && _administradorOriginal != null)
            {
                txtNombre.Text = _administradorOriginal.Nombre;
                txtApellido.Text = _administradorOriginal.Apellido;
                txtTelefono.Text = _administradorOriginal.Telefono;

                // Como en Cliente: en la edición el usuario de acceso solo se muestra
                // (se gestiona desde Usuarios) y no se pide contraseña.
                txtUsuario.Text = _administradorOriginal.NombreUsuario;
                txtEmail.Text = _administradorOriginal.Email;
                txtUsuario.Enabled = false;
                txtEmail.Enabled = false;
                lblContrasena.Visible = false;
                txtContrasena.Visible = false;

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

            // Solo en el alta: se crea también el usuario de acceso (rol Administrador).
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

            var dto = new AdministradorDTO
            {
                IdAdministrador = _administradorOriginal?.IdAdministrador ?? 0,
                IdUsuario = _administradorOriginal?.IdUsuario ?? 0,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                NombreUsuario = EsAlta ? txtUsuario.Text.Trim() : _administradorOriginal!.NombreUsuario,
                Email = EsAlta ? txtEmail.Text.Trim() : _administradorOriginal!.Email,
                Contrasena = EsAlta ? txtContrasena.Text : null
            };

            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                if (EsAlta)
                {
                    var creado = await AdministradorApiClient.AddAsync(dto);

                    // Si el usuario era de un administrador eliminado, el servidor lo recuperó en vez de crear otro.
                    if (creado.Recuperado)
                        MessageBox.Show("Ese usuario era de un administrador eliminado: se lo recuperó con los datos nuevos y las reservas que había registrado siguen a su nombre.",
                            "Administrador recuperado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await AdministradorApiClient.UpdateAsync(dto);
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
