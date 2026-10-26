namespace WindowsForms
{
    partial class ReservarTurno
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
            this.pnlFiltros = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.dgvLibres = new System.Windows.Forms.DataGridView();
            this.pnlBotones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnReservar = new System.Windows.Forms.Button();
            this.pnlFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLibres)).BeginInit();
            this.pnlBotones.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlFiltros
            //
            this.pnlFiltros.AutoSize = true;
            this.pnlFiltros.Controls.Add(this.lblFecha);
            this.pnlFiltros.Controls.Add(this.dtpFecha);
            this.pnlFiltros.Controls.Add(this.btnBuscar);
            this.pnlFiltros.Controls.Add(this.lblResultado);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Padding = new System.Windows.Forms.Padding(8);
            this.pnlFiltros.Size = new System.Drawing.Size(640, 40);
            this.pnlFiltros.TabIndex = 0;
            //
            // lblFecha
            //
            this.lblFecha.AutoSize = true;
            this.lblFecha.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.TabIndex = 0;
            this.lblFecha.Text = "Fecha:";
            //
            // dtpFecha
            //
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(110, 23);
            this.dtpFecha.TabIndex = 1;
            //
            // btnBuscar
            //
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(120, 27);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Ver turnos libres";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            //
            // lblResultado
            //
            this.lblResultado.AutoSize = true;
            this.lblResultado.Margin = new System.Windows.Forms.Padding(10, 8, 3, 0);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.TabIndex = 3;
            //
            // dgvLibres
            //
            this.dgvLibres.AllowUserToAddRows = false;
            this.dgvLibres.AllowUserToDeleteRows = false;
            this.dgvLibres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLibres.MultiSelect = false;
            this.dgvLibres.Name = "dgvLibres";
            this.dgvLibres.ReadOnly = true;
            this.dgvLibres.RowHeadersWidth = 25;
            this.dgvLibres.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLibres.TabIndex = 1;
            //
            // pnlBotones
            //
            this.pnlBotones.AutoSize = true;
            this.pnlBotones.Controls.Add(this.btnCerrar);
            this.pnlBotones.Controls.Add(this.btnReservar);
            this.pnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Padding = new System.Windows.Forms.Padding(8);
            this.pnlBotones.Size = new System.Drawing.Size(640, 46);
            this.pnlBotones.TabIndex = 2;
            //
            // btnCerrar
            //
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(90, 30);
            this.btnCerrar.TabIndex = 1;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // btnReservar
            //
            this.btnReservar.Name = "btnReservar";
            this.btnReservar.Size = new System.Drawing.Size(90, 30);
            this.btnReservar.TabIndex = 0;
            this.btnReservar.Text = "Reservar";
            this.btnReservar.UseVisualStyleBackColor = true;
            this.btnReservar.Click += new System.EventHandler(this.btnReservar_Click);
            //
            // ReservarTurno
            //
            this.CancelButton = this.btnCerrar;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.dgvLibres);
            this.Controls.Add(this.pnlBotones);
            this.Controls.Add(this.pnlFiltros);
            this.MinimizeBox = false;
            this.Name = "ReservarTurno";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reservar turno";
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLibres)).EndInit();
            this.pnlBotones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlFiltros;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.DataGridView dgvLibres;
        private System.Windows.Forms.FlowLayoutPanel pnlBotones;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnReservar;
    }
}
