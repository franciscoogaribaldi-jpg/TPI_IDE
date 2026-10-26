namespace WindowsForms
{
    partial class MisReservas
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
            this.dgvReservas = new System.Windows.Forms.DataGridView();
            this.pnlAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnReservar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();
            //
            // dgvReservas
            //
            this.dgvReservas.AllowUserToAddRows = false;
            this.dgvReservas.AllowUserToDeleteRows = false;
            this.dgvReservas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReservas.MultiSelect = false;
            this.dgvReservas.Name = "dgvReservas";
            this.dgvReservas.ReadOnly = true;
            this.dgvReservas.RowHeadersWidth = 25;
            this.dgvReservas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservas.TabIndex = 1;
            this.dgvReservas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvReservas_CellFormatting);
            //
            // pnlAcciones
            //
            this.pnlAcciones.AutoSize = true;
            this.pnlAcciones.Controls.Add(this.btnReservar);
            this.pnlAcciones.Controls.Add(this.btnCancelar);
            this.pnlAcciones.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Padding = new System.Windows.Forms.Padding(8);
            this.pnlAcciones.Size = new System.Drawing.Size(760, 40);
            this.pnlAcciones.TabIndex = 0;
            //
            // btnReservar
            //
            this.btnReservar.Name = "btnReservar";
            this.btnReservar.Size = new System.Drawing.Size(120, 27);
            this.btnReservar.TabIndex = 0;
            this.btnReservar.Text = "Reservar turno";
            this.btnReservar.UseVisualStyleBackColor = true;
            this.btnReservar.Click += new System.EventHandler(this.btnReservar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 27);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar reserva";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // MisReservas
            //
            this.ClientSize = new System.Drawing.Size(760, 500);
            this.Controls.Add(this.dgvReservas);
            this.Controls.Add(this.pnlAcciones);
            this.Name = "MisReservas";
            this.Text = "Mis reservas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReservas;
        private System.Windows.Forms.FlowLayoutPanel pnlAcciones;
        private System.Windows.Forms.Button btnReservar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
