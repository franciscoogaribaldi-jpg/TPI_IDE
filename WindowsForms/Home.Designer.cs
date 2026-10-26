namespace WindowsForms
{
    partial class Home
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.menuPrincipal = new System.Windows.Forms.MenuStrip();
            this.menuClientes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCanchas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTiposCancha = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTurnos = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReservas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAdministradores = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReportes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReporteOcupacion = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReporteRecaudacion = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCerrarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSalir = new System.Windows.Forms.ToolStripMenuItem();
            this.lblBienvenida = new System.Windows.Forms.ToolStripLabel();
            this.menuPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPrincipal
            // 
            this.menuPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuClientes,
            this.menuCanchas,
            this.menuTiposCancha,
            this.menuTurnos,
            this.menuReservas,
            this.menuAdministradores,
            this.menuUsuarios,
            this.menuReportes,
            this.menuSesion,
            this.lblBienvenida});
            this.menuPrincipal.Location = new System.Drawing.Point(0, 0);
            this.menuPrincipal.Name = "menuPrincipal";
            this.menuPrincipal.Size = new System.Drawing.Size(984, 24);
            this.menuPrincipal.TabIndex = 0;
            // 
            // menuClientes
            // 
            this.menuClientes.Name = "menuClientes";
            this.menuClientes.Size = new System.Drawing.Size(66, 20);
            this.menuClientes.Text = "Clientes";
            this.menuClientes.Click += new System.EventHandler(this.menuClientes_Click);
            // 
            // menuCanchas
            // 
            this.menuCanchas.Name = "menuCanchas";
            this.menuCanchas.Size = new System.Drawing.Size(70, 20);
            this.menuCanchas.Text = "Canchas";
            this.menuCanchas.Click += new System.EventHandler(this.menuCanchas_Click);
            // 
            // menuTiposCancha
            // 
            this.menuTiposCancha.Name = "menuTiposCancha";
            this.menuTiposCancha.Size = new System.Drawing.Size(110, 20);
            this.menuTiposCancha.Text = "Tipos de Cancha";
            this.menuTiposCancha.Click += new System.EventHandler(this.menuTiposCancha_Click);
            // 
            // menuTurnos
            // 
            this.menuTurnos.Name = "menuTurnos";
            this.menuTurnos.Size = new System.Drawing.Size(60, 20);
            this.menuTurnos.Text = "Turnos";
            this.menuTurnos.Click += new System.EventHandler(this.menuTurnos_Click);
            // 
            // menuReservas
            // 
            this.menuReservas.Name = "menuReservas";
            this.menuReservas.Size = new System.Drawing.Size(75, 20);
            this.menuReservas.Text = "Reservas";
            this.menuReservas.Click += new System.EventHandler(this.menuReservas_Click);
            // 
            // menuAdministradores
            // 
            this.menuAdministradores.Name = "menuAdministradores";
            this.menuAdministradores.Size = new System.Drawing.Size(110, 20);
            this.menuAdministradores.Text = "Administradores";
            this.menuAdministradores.Click += new System.EventHandler(this.menuAdministradores_Click);
            // 
            // menuUsuarios
            // 
            this.menuUsuarios.Name = "menuUsuarios";
            this.menuUsuarios.Size = new System.Drawing.Size(64, 20);
            this.menuUsuarios.Text = "Usuarios";
            this.menuUsuarios.Click += new System.EventHandler(this.menuUsuarios_Click);
            // 
            // menuReportes
            // 
            this.menuReportes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuReporteOcupacion,
            this.menuReporteRecaudacion});
            this.menuReportes.Name = "menuReportes";
            this.menuReportes.Size = new System.Drawing.Size(65, 20);
            this.menuReportes.Text = "Reportes";
            // 
            // menuReporteOcupacion
            // 
            this.menuReporteOcupacion.Name = "menuReporteOcupacion";
            this.menuReporteOcupacion.Size = new System.Drawing.Size(220, 22);
            this.menuReporteOcupacion.Text = "Ocupación de canchas";
            this.menuReporteOcupacion.Click += new System.EventHandler(this.menuReporteOcupacion_Click);
            // 
            // menuReporteRecaudacion
            // 
            this.menuReporteRecaudacion.Name = "menuReporteRecaudacion";
            this.menuReporteRecaudacion.Size = new System.Drawing.Size(220, 22);
            this.menuReporteRecaudacion.Text = "Recaudación";
            this.menuReporteRecaudacion.Click += new System.EventHandler(this.menuReporteRecaudacion_Click);
            // 
            // menuSesion
            // 
            this.menuSesion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCerrarSesion,
            this.menuSalir});
            this.menuSesion.Name = "menuSesion";
            this.menuSesion.Size = new System.Drawing.Size(58, 20);
            this.menuSesion.Text = "Sesión";
            // 
            // menuCerrarSesion
            // 
            this.menuCerrarSesion.Name = "menuCerrarSesion";
            this.menuCerrarSesion.Size = new System.Drawing.Size(180, 22);
            this.menuCerrarSesion.Text = "Cerrar sesión";
            this.menuCerrarSesion.Click += new System.EventHandler(this.menuCerrarSesion_Click);
            // 
            // menuSalir
            // 
            this.menuSalir.Name = "menuSalir";
            this.menuSalir.Size = new System.Drawing.Size(180, 22);
            this.menuSalir.Text = "Salir";
            this.menuSalir.Click += new System.EventHandler(this.menuSalir_Click);
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(0, 20);
            // 
            // Home
            // 
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.menuPrincipal);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuPrincipal;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Complejo Deportivo - Panel principal";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuPrincipal.ResumeLayout(false);
            this.menuPrincipal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuPrincipal;
        private System.Windows.Forms.ToolStripMenuItem menuClientes;
        private System.Windows.Forms.ToolStripMenuItem menuCanchas;
        private System.Windows.Forms.ToolStripMenuItem menuTiposCancha;
        private System.Windows.Forms.ToolStripMenuItem menuTurnos;
        private System.Windows.Forms.ToolStripMenuItem menuReservas;
        private System.Windows.Forms.ToolStripMenuItem menuAdministradores;
        private System.Windows.Forms.ToolStripMenuItem menuUsuarios;
        private System.Windows.Forms.ToolStripMenuItem menuReportes;
        private System.Windows.Forms.ToolStripMenuItem menuReporteOcupacion;
        private System.Windows.Forms.ToolStripMenuItem menuReporteRecaudacion;
        private System.Windows.Forms.ToolStripMenuItem menuSesion;
        private System.Windows.Forms.ToolStripMenuItem menuCerrarSesion;
        private System.Windows.Forms.ToolStripMenuItem menuSalir;
        private System.Windows.Forms.ToolStripLabel lblBienvenida;
    }
}
