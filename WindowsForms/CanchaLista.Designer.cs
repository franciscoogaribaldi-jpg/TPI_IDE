namespace WindowsForms
{
    partial class CanchaLista
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
            this.dgvCanchas = new System.Windows.Forms.DataGridView();
            this.pnlAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCanchas)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();
            //
            // dgvCanchas
            //
            this.dgvCanchas.AllowUserToAddRows = false;
            this.dgvCanchas.AllowUserToDeleteRows = false;
            this.dgvCanchas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCanchas.Location = new System.Drawing.Point(0, 40);
            this.dgvCanchas.MultiSelect = false;
            this.dgvCanchas.Name = "dgvCanchas";
            this.dgvCanchas.ReadOnly = true;
            this.dgvCanchas.RowHeadersWidth = 25;
            this.dgvCanchas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCanchas.Size = new System.Drawing.Size(860, 460);
            this.dgvCanchas.TabIndex = 1;
            this.dgvCanchas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCanchas_CellDoubleClick);
            this.dgvCanchas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCanchas_CellFormatting);
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
            this.btnNuevo.Location = new System.Drawing.Point(11, 8);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(80, 27);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nueva";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            //
            // btnEditar
            //
            this.btnEditar.Location = new System.Drawing.Point(97, 8);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(80, 27);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            //
            // btnEliminar
            //
            this.btnEliminar.Location = new System.Drawing.Point(183, 8);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(80, 27);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            //
            // CanchaLista
            //
            this.ClientSize = new System.Drawing.Size(860, 500);
            this.Controls.Add(this.dgvCanchas);
            this.Controls.Add(this.pnlAcciones);
            this.Name = "CanchaLista";
            this.Text = "Canchas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvCanchas)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCanchas;
        private System.Windows.Forms.FlowLayoutPanel pnlAcciones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
    }
}
