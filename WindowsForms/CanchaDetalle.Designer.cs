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
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblPrecioPorHora = new Label();
            numPrecioPorHora = new NumericUpDown();
            lblEstado = new Label();
            cmbEstado = new ComboBox();
            grpTipo = new GroupBox();
            radPadel = new RadioButton();
            radFutbol = new RadioButton();
            pnlRaquetas = new Panel();
            lblCantidadRaquetas = new Label();
            numCantidadRaquetas = new NumericUpDown();
            lblPrecioTotalRaquetas = new Label();
            numPrecioTotalRaquetas = new NumericUpDown();
            btnGuardar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)numPrecioPorHora).BeginInit();
            grpTipo.SuspendLayout();
            pnlRaquetas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCantidadRaquetas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrecioTotalRaquetas).BeginInit();
            SuspendLayout();
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(20, 23);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(54, 15);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(150, 20);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(210, 23);
            txtNombre.TabIndex = 1;
            // 
            // lblPrecioPorHora
            // 
            lblPrecioPorHora.AutoSize = true;
            lblPrecioPorHora.Location = new Point(20, 63);
            lblPrecioPorHora.Name = "lblPrecioPorHora";
            lblPrecioPorHora.Size = new Size(91, 15);
            lblPrecioPorHora.TabIndex = 2;
            lblPrecioPorHora.Text = "Precio por hora:";
            // 
            // numPrecioPorHora
            // 
            numPrecioPorHora.DecimalPlaces = 2;
            numPrecioPorHora.Location = new Point(150, 60);
            numPrecioPorHora.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numPrecioPorHora.Name = "numPrecioPorHora";
            numPrecioPorHora.Size = new Size(150, 23);
            numPrecioPorHora.TabIndex = 3;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(20, 103);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(45, 15);
            lblEstado.TabIndex = 4;
            lblEstado.Text = "Estado:";
            // 
            // cmbEstado
            // 
            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstado.Location = new Point(150, 100);
            cmbEstado.Name = "cmbEstado";
            cmbEstado.Size = new Size(210, 23);
            cmbEstado.TabIndex = 5;
            // 
            // grpTipo
            // 
            grpTipo.Controls.Add(radPadel);
            grpTipo.Controls.Add(radFutbol);
            grpTipo.Location = new Point(20, 140);
            grpTipo.Name = "grpTipo";
            grpTipo.Size = new Size(340, 60);
            grpTipo.TabIndex = 6;
            grpTipo.TabStop = false;
            grpTipo.Text = "Tipo de cancha";
            // 
            // radPadel
            // 
            radPadel.AutoSize = true;
            radPadel.Location = new Point(150, 25);
            radPadel.Name = "radPadel";
            radPadel.Size = new Size(54, 19);
            radPadel.TabIndex = 1;
            radPadel.TabStop = true;
            radPadel.Text = "Pádel";
            radPadel.UseVisualStyleBackColor = true;
            radPadel.CheckedChanged += radTipo_CheckedChanged;
            // 
            // radFutbol
            // 
            radFutbol.AutoSize = true;
            radFutbol.Location = new Point(20, 25);
            radFutbol.Name = "radFutbol";
            radFutbol.Size = new Size(59, 19);
            radFutbol.TabIndex = 0;
            radFutbol.TabStop = true;
            radFutbol.Text = "Fútbol";
            radFutbol.UseVisualStyleBackColor = true;
            radFutbol.CheckedChanged += radTipo_CheckedChanged;
            // 
            // pnlRaquetas
            // 
            pnlRaquetas.Controls.Add(lblCantidadRaquetas);
            pnlRaquetas.Controls.Add(numCantidadRaquetas);
            pnlRaquetas.Controls.Add(lblPrecioTotalRaquetas);
            pnlRaquetas.Controls.Add(numPrecioTotalRaquetas);
            pnlRaquetas.Location = new Point(20, 210);
            pnlRaquetas.Name = "pnlRaquetas";
            pnlRaquetas.Size = new Size(340, 80);
            pnlRaquetas.TabIndex = 7;
            // 
            // lblCantidadRaquetas
            // 
            lblCantidadRaquetas.AutoSize = true;
            lblCantidadRaquetas.Location = new Point(0, 8);
            lblCantidadRaquetas.Name = "lblCantidadRaquetas";
            lblCantidadRaquetas.Size = new Size(122, 15);
            lblCantidadRaquetas.TabIndex = 0;
            lblCantidadRaquetas.Text = "Cantidad de raquetas:";
            // 
            // numCantidadRaquetas
            // 
            numCantidadRaquetas.Location = new Point(160, 5);
            numCantidadRaquetas.Name = "numCantidadRaquetas";
            numCantidadRaquetas.Size = new Size(90, 23);
            numCantidadRaquetas.TabIndex = 1;
            // 
            // lblPrecioTotalRaquetas
            // 
            lblPrecioTotalRaquetas.AutoSize = true;
            lblPrecioTotalRaquetas.Location = new Point(0, 43);
            lblPrecioTotalRaquetas.Name = "lblPrecioTotalRaquetas";
            lblPrecioTotalRaquetas.Size = new Size(118, 15);
            lblPrecioTotalRaquetas.TabIndex = 2;
            lblPrecioTotalRaquetas.Text = "Precio total raquetas:";
            // 
            // numPrecioTotalRaquetas
            // 
            numPrecioTotalRaquetas.DecimalPlaces = 2;
            numPrecioTotalRaquetas.Location = new Point(160, 40);
            numPrecioTotalRaquetas.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            numPrecioTotalRaquetas.Name = "numPrecioTotalRaquetas";
            numPrecioTotalRaquetas.Size = new Size(150, 23);
            numPrecioTotalRaquetas.TabIndex = 3;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(190, 320);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(85, 30);
            btnGuardar.TabIndex = 8;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(280, 320);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(85, 30);
            btnCancelar.TabIndex = 9;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // CanchaDetalle
            // 
            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;
            ClientSize = new Size(449, 375);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblPrecioPorHora);
            Controls.Add(numPrecioPorHora);
            Controls.Add(lblEstado);
            Controls.Add(cmbEstado);
            Controls.Add(grpTipo);
            Controls.Add(pnlRaquetas);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CanchaDetalle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CanchaDetalle";
            Load += CanchaDetalle_Load;
            ((System.ComponentModel.ISupportInitialize)numPrecioPorHora).EndInit();
            grpTipo.ResumeLayout(false);
            grpTipo.PerformLayout();
            pnlRaquetas.ResumeLayout(false);
            pnlRaquetas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCantidadRaquetas).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrecioTotalRaquetas).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
