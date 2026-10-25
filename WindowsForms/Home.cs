namespace WindowsForms
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            lblBienvenida.Text = $"Conectado como: {SesionActual.Usuario?.NombreUsuario}";

            // Autorización básica por rol (1=Administrador, 2=Cliente, ver Domain.Model.RolUsuario).
            // Clientes, Canchas, Turnos y Reservas son pantallas de administración: solo las ve el
            // Administrador. Esto solo oculta el menú; la WebAPI valida el rol de verdad con el token.
            bool esAdministrador = SesionActual.Usuario?.Rol == 1;
            menuClientes.Visible = esAdministrador;
            menuCanchas.Visible = esAdministrador;
            menuTurnos.Visible = esAdministrador;
            menuReservas.Visible = esAdministrador;
            menuAdministradores.Visible = esAdministrador;
            menuUsuarios.Visible = esAdministrador;

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

        private void menuTurnos_Click(object sender, EventArgs e)
        {
            var existente = MdiChildren.OfType<TurnoLista>().FirstOrDefault();
            if (existente != null)
            {
                existente.Activate();
                return;
            }

            var form = new TurnoLista { MdiParent = this };
            form.Show();
        }

        private void menuReservas_Click(object sender, EventArgs e)
        {
            var existente = MdiChildren.OfType<ReservaLista>().FirstOrDefault();
            if (existente != null)
            {
                existente.Activate();
                return;
            }

            var form = new ReservaLista { MdiParent = this };
            form.Show();
        }

        private void menuAdministradores_Click(object sender, EventArgs e)
        {
            AbrirLista<AdministradorLista>();
        }

        private void menuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirLista<UsuarioLista>();
        }

        // Igual que los demás menús: si la lista ya está abierta la trae al frente, si no la abre
        // como ventana hija (MDI).
        private void AbrirLista<T>() where T : Form, new()
        {
            var existente = MdiChildren.OfType<T>().FirstOrDefault();
            if (existente != null)
            {
                existente.Activate();
                return;
            }

            var form = new T { MdiParent = this };
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
