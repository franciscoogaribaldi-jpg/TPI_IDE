namespace WindowsForms
{
    partial class CanchaDetalle
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
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblPrecioPorHora = new System.Windows.Forms.Label();
            this.numPrecioPorHora = new System.Windows.Forms.NumericUpDown();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.grpTipo = new System.Windows.Forms.GroupBox();
            this.radPadel = new System.Windows.Forms.RadioButton();
            this.radFutbol = new System.Windows.Forms.RadioButton();
            this.pnlRaquetas = new System.Windows.Forms.Panel();
            this.lblPrecioTotalRaquetas = new System.Windows.Forms.Label();
            this.numPrecioTotalRaquetas = new System.Windows.Forms.NumericUpDown();
            this.lblCantidadRaquetas = new System.Windows.Forms.Label();
            this.numCantidadRaquetas = new System.Windows.Forms.NumericUpDown();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecioPorHora)).BeginInit();
            this.grpTipo.SuspendLayout();
            this.pnlRaquetas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecioTotalRaquetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidadRaquetas)).BeginInit();
            this.SuspendLayout();
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(20, 23);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(58, 15);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre:";
            //
            // txtNombre
            //
            this.txtNombre.Location = new System.Drawing.Point(150, 20);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(210, 23);
            this.txtNombre.TabIndex = 1;
            //
            // lblPrecioPorHora
            //
            this.lblPrecioPorHora.AutoSize = true;
            this.lblPrecioPorHora.Location = new System.Drawing.Point(20, 63);
            this.lblPrecioPorHora.Name = "lblPrecioPorHora";
            this.lblPrecioPorHora.Size = new System.Drawing.Size(95, 15);
            this.lblPrecioPorHora.TabIndex = 2;
            this.lblPrecioPorHora.Text = "Precio por hora:";
            //
            // numPrecioPorHora
            //
            this.numPrecioPorHora.DecimalPlaces = 2;
            this.numPrecioPorHora.Location = new System.Drawing.Point(150, 60);
            this.numPrecioPorHora.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.numPrecioPorHora.Name = "numPrecioPorHora";
            this.numPrecioPorHora.Size = new System.Drawing.Size(150, 23);
            this.numPrecioPorHora.TabIndex = 3;
            //
            // lblEstado
            //
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(20, 103);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(45, 15);
            this.lblEstado.TabIndex = 4;
            this.lblEstado.Text = "Estado:";
            //
            // cmbEstado
            //
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Location = new System.Drawing.Point(150, 100);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(210, 23);
            this.cmbEstado.TabIndex = 5;
            //
            // grpTipo
            //
            this.grpTipo.Controls.Add(this.radPadel);
            this.grpTipo.Controls.Add(this.radFutbol);
            this.grpTipo.Location = new System.Drawing.Point(20, 140);
            this.grpTipo.Name = "grpTipo";
            this.grpTipo.Size = new System.Drawing.Size(340, 60);
            this.grpTipo.TabIndex = 6;
            this.grpTipo.TabStop = false;
            this.grpTipo.Text = "Tipo de cancha";
            //
            // radFutbol
            //
            this.radFutbol.AutoSize = true;
            this.radFutbol.Location = new System.Drawing.Point(20, 25);
            this.radFutbol.Name = "radFutbol";
            this.radFutbol.Size = new System.Drawing.Size(65, 19);
            this.radFutbol.TabIndex = 0;
            this.radFutbol.TabStop = true;
            this.radFutbol.Text = "Fútbol";
            this.radFutbol.UseVisualStyleBackColor = true;
            this.radFutbol.CheckedChanged += new System.EventHandler(this.radTipo_CheckedChanged);
            //
            // radPadel
            //
            this.radPadel.AutoSize = true;
            this.radPadel.Location = new System.Drawing.Point(150, 25);
            this.radPadel.Name = "radPadel";
            this.radPadel.Size = new System.Drawing.Size(63, 19);
            this.radPadel.TabIndex = 1;
            this.radPadel.TabStop = true;
            this.radPadel.Text = "Pádel";
            this.radPadel.UseVisualStyleBackColor = true;
            this.radPadel.CheckedChanged += new System.EventHandler(this.radTipo_CheckedChanged);
            //
            // pnlRaquetas
            //
            this.pnlRaquetas.Controls.Add(this.lblCantidadRaquetas);
            this.pnlRaquetas.Controls.Add(this.numCantidadRaquetas);
            this.pnlRaquetas.Controls.Add(this.lblPrecioTotalRaquetas);
            this.pnlRaquetas.Controls.Add(this.numPrecioTotalRaquetas);
            this.pnlRaquetas.Location = new System.Drawing.Point(20, 210);
            this.pnlRaquetas.Name = "pnlRaquetas";
            this.pnlRaquetas.Size = new System.Drawing.Size(340, 80);
            this.pnlRaquetas.TabIndex = 7;
            //
            // lblCantidadRaquetas
            //
            this.lblCantidadRaquetas.AutoSize = true;
            this.lblCantidadRaquetas.Location = new System.Drawing.Point(0, 8);
            this.lblCantidadRaquetas.Name = "lblCantidadRaquetas";
            this.lblCantidadRaquetas.Size = new System.Drawing.Size(130, 15);
            this.lblCantidadRaquetas.TabIndex = 0;
            this.lblCantidadRaquetas.Text = "Cantidad de raquetas:";
            //
            // numCantidadRaquetas
            //
            this.numCantidadRaquetas.Location = new System.Drawing.Point(160, 5);
            this.numCantidadRaquetas.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numCantidadRaquetas.Name = "numCantidadRaquetas";
            this.numCantidadRaquetas.Size = new System.Drawing.Size(90, 23);
            this.numCantidadRaquetas.TabIndex = 1;
            //
            // lblPrecioTotalRaquetas
            //
            this.lblPrecioTotalRaquetas.AutoSize = true;
            this.lblPrecioTotalRaquetas.Location = new System.Drawing.Point(0, 43);
            this.lblPrecioTotalRaquetas.Name = "lblPrecioTotalRaquetas";
            this.lblPrecioTotalRaquetas.Size = new System.Drawing.Size(140, 15);
            this.lblPrecioTotalRaquetas.TabIndex = 2;
            this.lblPrecioTotalRaquetas.Text = "Precio total raquetas:";
            //
            // numPrecioTotalRaquetas
            //
            this.numPrecioTotalRaquetas.DecimalPlaces = 2;
            this.numPrecioTotalRaquetas.Location = new System.Drawing.Point(160, 40);
            this.numPrecioTotalRaquetas.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.numPrecioTotalRaquetas.Name = "numPrecioTotalRaquetas";
            this.numPrecioTotalRaquetas.Size = new System.Drawing.Size(150, 23);
            this.numPrecioTotalRaquetas.TabIndex = 3;
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(190, 320);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(85, 30);
            this.btnGuardar.TabIndex = 8;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(280, 320);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(85, 30);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // CanchaDetalle
            //
            this.AcceptButton = this.btnGuardar;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(385, 375);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblPrecioPorHora);
            this.Controls.Add(this.numPrecioPorHora);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.cmbEstado);
            this.Controls.Add(this.grpTipo);
            this.Controls.Add(this.pnlRaquetas);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CanchaDetalle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CanchaDetalle";
            this.Load += new System.EventHandler(this.CanchaDetalle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPrecioPorHora)).EndInit();
            this.grpTipo.ResumeLayout(false);
            this.grpTipo.PerformLayout();
            this.pnlRaquetas.ResumeLayout(false);
            this.pnlRaquetas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrecioTotalRaquetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidadRaquetas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblPrecioPorHora;
        private System.Windows.Forms.NumericUpDown numPrecioPorHora;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.GroupBox grpTipo;
        private System.Windows.Forms.RadioButton radPadel;
        private System.Windows.Forms.RadioButton radFutbol;
        private System.Windows.Forms.Panel pnlRaquetas;
        private System.Windows.Forms.Label lblCantidadRaquetas;
        private System.Windows.Forms.NumericUpDown numCantidadRaquetas;
        private System.Windows.Forms.Label lblPrecioTotalRaquetas;
        private System.Windows.Forms.NumericUpDown numPrecioTotalRaquetas;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
