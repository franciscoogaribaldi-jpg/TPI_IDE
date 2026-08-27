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
        private System.Windows.Forms.ToolStripMenuItem menuSesion;
        private System.Windows.Forms.ToolStripMenuItem menuCerrarSesion;
        private System.Windows.Forms.ToolStripMenuItem menuSalir;
        private System.Windows.Forms.ToolStripLabel lblBienvenida;
    }
}
