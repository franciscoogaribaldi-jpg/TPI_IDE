namespace WindowsForms
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            lblBienvenida.Text = $"Conectado como: {SesionActual.Usuario?.NombreUsuario}";
        }

        private void menuClientes_Click(object sender, EventArgs e)
        {
            // Próximo paso: abrir ClienteLista como hijo MDI, por ejemplo:
            //   var form = new ClienteLista { MdiParent = this };
            //   form.Show();
            MessageBox.Show("El formulario de Clientes se agrega en el próximo paso.",
                "En construcción", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuCanchas_Click(object sender, EventArgs e)
        {
            MessageBox.Show("El formulario de Canchas se agrega en el próximo paso.",
                "En construcción", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
