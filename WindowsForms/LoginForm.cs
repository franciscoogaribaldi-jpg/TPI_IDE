using API.Clients;

namespace WindowsForms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtContrasena.Text))
            {
                MessageBox.Show("Completá usuario y contraseña.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnIngresar.Enabled = false;
            btnIngresar.Text = "Ingresando...";

            try
            {
                var respuesta = await AuthApiClient.LoginAsync(txtUsuario.Text.Trim(), txtContrasena.Text);

                if (respuesta == null)
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de autenticación",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtContrasena.Clear();
                    txtContrasena.Focus();
                    return;
                }

                SesionActual.IniciarSesion(respuesta);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo conectar con el servidor.\n\n{ex.Message}",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnIngresar.Enabled = true;
                btnIngresar.Text = "Ingresar";
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtContrasena_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnIngresar_Click(sender, EventArgs.Empty);
            }
        }
    }
}
