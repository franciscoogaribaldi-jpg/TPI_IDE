using API.Clients;
using DTOs;

namespace WindowsForms
{
    public partial class UsuarioDetalle : Form
    {
        private readonly UsuarioDTO? _usuarioOriginal;
        private bool EsAlta => _usuarioOriginal == null;

        public UsuarioDetalle()
        {
            InitializeComponent();
            _usuarioOriginal = null;
        }

        public UsuarioDetalle(UsuarioDTO usuario)
        {
            InitializeComponent();
            _usuarioOriginal = usuario;
        }

        private void UsuarioDetalle_Load(object sender, EventArgs e)
        {
            Text = EsAlta ? "Nuevo usuario" : "Editar usuario";

            // Índice 0 = Administrador (1), índice 1 = Cliente (2), como Domain.Model.RolUsuario.
            cmbRol.DataSource = new[] { "Administrador", "Cliente" };

            if (EsAlta)
            {
                cmbRol.SelectedIndex = 1;
            }
            else if (_usuarioOriginal != null)
            {
                txtUsuario.Text = _usuarioOriginal.NombreUsuario;
                txtEmail.Text = _usuarioOriginal.Email;
                cmbRol.SelectedIndex = _usuarioOriginal.Rol == 1 ? 0 : 1;

                // La contraseña no se cambia desde acá (el servidor tampoco la toca en la edición).
                lblContrasena.Visible = false;
                txtContrasena.Visible = false;
                btnGuardar.Top -= 40;
                btnCancelar.Top -= 40;
                ClientSize = new Size(ClientSize.Width, ClientSize.Height - 40);
            }
        }

        private bool ValidarCampos()
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

            if (EsAlta && string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("La contraseña es obligatoria.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContrasena.Focus();
                return false;
            }

            return true;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            var dto = new UsuarioDTO
            {
                IdUsuario = _usuarioOriginal?.IdUsuario ?? 0,
                NombreUsuario = txtUsuario.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Rol = cmbRol.SelectedIndex == 0 ? 1 : 2,
                Contrasena = EsAlta ? txtContrasena.Text : string.Empty
            };

            btnGuardar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                if (EsAlta)
                {
                    var creado = await UsuarioApiClient.AddAsync(dto);

                    // Si el nombre de usuario era de un usuario eliminado, el servidor lo recuperó en vez de crear otro.
                    if (creado.Recuperado)
                        MessageBox.Show("Ese nombre de usuario era de un usuario eliminado: se lo recuperó con los datos nuevos.",
                            "Usuario recuperado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await UsuarioApiClient.UpdateAsync(dto);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                // Por ejemplo: usuario o email repetido, o cambiar el rol de alguien con ficha.
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
