namespace WindowsForms
{
    partial class AdministradorLista
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
            this.dgvAdministradores = new System.Windows.Forms.DataGridView();
            this.pnlAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdministradores)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();
            //
            // dgvAdministradores
            //
            this.dgvAdministradores.AllowUserToAddRows = false;
            this.dgvAdministradores.AllowUserToDeleteRows = false;
            this.dgvAdministradores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAdministradores.Location = new System.Drawing.Point(0, 40);
            this.dgvAdministradores.MultiSelect = false;
            this.dgvAdministradores.Name = "dgvAdministradores";
            this.dgvAdministradores.ReadOnly = true;
            this.dgvAdministradores.RowHeadersWidth = 25;
            this.dgvAdministradores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdministradores.Size = new System.Drawing.Size(860, 460);
            this.dgvAdministradores.TabIndex = 1;
            this.dgvAdministradores.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAdministradores_CellDoubleClick);
            //
            // pnlAcciones
            //
            this.pnlAcciones.AutoSize = true;
            this.pnlAcciones.Controls.Add(this.btnNuevo);
            this.pnlAcciones.Controls.Add(this.btnEditar);
            this.pnlAcciones.Controls.Add(this.btnEliminar);
            this.pnlAcciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAcciones.Location = new System.Drawing.Point(0, 0);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Padding = new System.Windows.Forms.Padding(8);
            this.pnlAcciones.Size = new System.Drawing.Size(860, 40);
            this.pnlAcciones.TabIndex = 0;
            //
            // btnNuevo
            //
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(80, 27);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            //
            // btnEditar
            //
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(80, 27);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            //
            // btnEliminar
            //
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(80, 27);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            //
            // AdministradorLista
            //
            this.ClientSize = new System.Drawing.Size(860, 500);
            this.Controls.Add(this.dgvAdministradores);
            this.Controls.Add(this.pnlAcciones);
            this.Name = "AdministradorLista";
            this.Text = "Administradores";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdministradores)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAdministradores;
        private System.Windows.Forms.FlowLayoutPanel pnlAcciones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
    }
}
