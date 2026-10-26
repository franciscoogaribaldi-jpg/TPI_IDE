namespace WindowsForms
{
    partial class ReporteRecaudacion
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
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.dgvRecaudacion = new System.Windows.Forms.DataGridView();
            this.lblTotales = new System.Windows.Forms.Label();
            this.pnlFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecaudacion)).BeginInit();
            this.SuspendLayout();
            //
            // pnlFiltros
            //
            this.pnlFiltros.AutoSize = true;
            this.pnlFiltros.Controls.Add(this.lblDesde);
            this.pnlFiltros.Controls.Add(this.dtpDesde);
            this.pnlFiltros.Controls.Add(this.lblHasta);
            this.pnlFiltros.Controls.Add(this.dtpHasta);
            this.pnlFiltros.Controls.Add(this.btnGenerar);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Padding = new System.Windows.Forms.Padding(8);
            this.pnlFiltros.Size = new System.Drawing.Size(860, 40);
            this.pnlFiltros.TabIndex = 0;
            //
            // lblDesde
            //
            this.lblDesde.AutoSize = true;
            this.lblDesde.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.TabIndex = 0;
            this.lblDesde.Text = "Desde:";
            //
            // dtpDesde
            //
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(110, 23);
            this.dtpDesde.TabIndex = 1;
            //
            // lblHasta
            //
            this.lblHasta.AutoSize = true;
            this.lblHasta.Margin = new System.Windows.Forms.Padding(10, 8, 3, 0);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.TabIndex = 2;
            this.lblHasta.Text = "Hasta:";
            //
            // dtpHasta
            //
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(110, 23);
            this.dtpHasta.TabIndex = 3;
            //
            // btnGenerar
            //
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(90, 27);
            this.btnGenerar.TabIndex = 4;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            //
            // dgvRecaudacion
            //
            this.dgvRecaudacion.AllowUserToAddRows = false;
            this.dgvRecaudacion.AllowUserToDeleteRows = false;
            this.dgvRecaudacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecaudacion.MultiSelect = false;
            this.dgvRecaudacion.Name = "dgvRecaudacion";
            this.dgvRecaudacion.ReadOnly = true;
            this.dgvRecaudacion.RowHeadersWidth = 25;
            this.dgvRecaudacion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecaudacion.TabIndex = 1;
            //
            // lblTotales
            //
            this.lblTotales.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTotales.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotales.Name = "lblTotales";
            this.lblTotales.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.lblTotales.Size = new System.Drawing.Size(860, 36);
            this.lblTotales.TabIndex = 2;
            this.lblTotales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // ReporteRecaudacion
            //
            this.ClientSize = new System.Drawing.Size(860, 500);
            this.Controls.Add(this.dgvRecaudacion);
            this.Controls.Add(this.lblTotales);
            this.Controls.Add(this.pnlFiltros);
            this.Name = "ReporteRecaudacion";
            this.Text = "Reporte - Recaudación";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecaudacion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlFiltros;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.DataGridView dgvRecaudacion;
        private System.Windows.Forms.Label lblTotales;
    }
}
