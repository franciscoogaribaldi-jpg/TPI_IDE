namespace WindowsForms
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            lblBienvenida.Text = $"Conectado como: {SesionActual.Usuario?.NombreUsuario}";

            // Autorización básica por rol (1=Administrador, 2=Cliente, ver Domain.Model.RolUsuario).
            // Por ahora la gestión de Clientes/Canchas es solo para el Administrador; un usuario
            // Cliente todavía no tiene pantallas propias en el escritorio (eso llega con las
            // reservas). OJO: esto solo oculta el menú del lado de la UI. La WebAPI todavía no
            // valida el rol de quien llama a cada endpoint — eso requiere tokens, que son
            // requisito recién de la Entrega 3.
            bool esAdministrador = SesionActual.Usuario?.Rol == 1;
            menuClientes.Visible = esAdministrador;
            menuCanchas.Visible = esAdministrador;

            if (!esAdministrador)
            {
                MessageBox.Show(
                    "Tu usuario no tiene funcionalidades de administración disponibles en esta versión.",
                    "Acceso limitado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            var existente = MdiChildren.OfType<ClienteLista>().FirstOrDefault();
            if (existente != null)
            {
                existente.Activate();
                return;
            }

            var form = new ClienteLista { MdiParent = this };
            form.Show();
        }

        private void menuCanchas_Click(object sender, EventArgs e)
        {
            var existente = MdiChildren.OfType<CanchaLista>().FirstOrDefault();
            if (existente != null)
            {
                existente.Activate();
                return;
            }

            var form = new CanchaLista { MdiParent = this };
            form.Show();
        }

        private void menuCerrarSesion_Click(object sender, EventArgs e)
        {
            SesionActual.CerrarSesion();
            Close(); // Program.cs detecta que no hay sesión y vuelve a mostrar el login
        }

        private void menuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
