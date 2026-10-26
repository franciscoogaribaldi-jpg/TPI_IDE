namespace WindowsForms
{
    partial class ReporteOcupacion
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
            this.lblResumen = new System.Windows.Forms.Label();
            this.pnlGrafico = new System.Windows.Forms.FlowLayoutPanel();
            this.dgvOcupacion = new System.Windows.Forms.DataGridView();
            this.pnlFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOcupacion)).BeginInit();
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
            this.pnlFiltros.Controls.Add(this.lblResumen);
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
            // lblResumen
            //
            this.lblResumen.AutoSize = true;
            this.lblResumen.Margin = new System.Windows.Forms.Padding(20, 8, 3, 0);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.TabIndex = 5;
            //
            // pnlGrafico
            //
            this.pnlGrafico.AutoScroll = true;
            this.pnlGrafico.BackColor = System.Drawing.Color.White;
            this.pnlGrafico.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGrafico.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlGrafico.Name = "pnlGrafico";
            this.pnlGrafico.Padding = new System.Windows.Forms.Padding(10);
            this.pnlGrafico.Size = new System.Drawing.Size(860, 300);
            this.pnlGrafico.TabIndex = 1;
            this.pnlGrafico.WrapContents = false;
            //
            // dgvOcupacion
            //
            this.dgvOcupacion.AllowUserToAddRows = false;
            this.dgvOcupacion.AllowUserToDeleteRows = false;
            this.dgvOcupacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOcupacion.MultiSelect = false;
            this.dgvOcupacion.Name = "dgvOcupacion";
            this.dgvOcupacion.ReadOnly = true;
            this.dgvOcupacion.RowHeadersWidth = 25;
            this.dgvOcupacion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOcupacion.TabIndex = 2;
            //
            // ReporteOcupacion
            //
            this.ClientSize = new System.Drawing.Size(860, 600);
            this.Controls.Add(this.dgvOcupacion);
            this.Controls.Add(this.pnlGrafico);
            this.Controls.Add(this.pnlFiltros);
            this.Name = "ReporteOcupacion";
            this.Text = "Reporte - Ocupación de canchas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOcupacion)).EndInit();
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
        private System.Windows.Forms.Label lblResumen;
        private System.Windows.Forms.FlowLayoutPanel pnlGrafico;
        private System.Windows.Forms.DataGridView dgvOcupacion;
    }
}
