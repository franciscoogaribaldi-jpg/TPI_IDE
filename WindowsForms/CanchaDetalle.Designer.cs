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
            chkTechada = new CheckBox();
            grpTipo = new GroupBox();
            radPadel = new RadioButton();
            radFutbol = new RadioButton();
            pnlFutbol = new Panel();
            lblTipoFutbol = new Label();
            cmbTipoFutbol = new ComboBox();
            lblSuperficie = new Label();
            cmbSuperficie = new ComboBox();
            pnlPadel = new Panel();
            lblTipoPared = new Label();
            cmbTipoPared = new ComboBox();
            btnGuardar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)numPrecioPorHora).BeginInit();
            grpTipo.SuspendLayout();
            pnlFutbol.SuspendLayout();
            pnlPadel.SuspendLayout();
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
            txtNombre.Location = new Point(155, 15);
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
            // chkTechada
            //
            chkTechada.AutoSize = true;
            chkTechada.Location = new Point(150, 97);
            chkTechada.Name = "chkTechada";
            chkTechada.Size = new Size(108, 19);
            chkTechada.TabIndex = 6;
            chkTechada.Text = "Cancha techada";
            chkTechada.UseVisualStyleBackColor = true;
            //
            // grpTipo
            //
            grpTipo.Controls.Add(radPadel);
            grpTipo.Controls.Add(radFutbol);
            grpTipo.Location = new Point(20, 130);
            grpTipo.Name = "grpTipo";
            grpTipo.Size = new Size(340, 60);
            grpTipo.TabIndex = 7;
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
            // pnlFutbol
            //
            pnlFutbol.Controls.Add(lblTipoFutbol);
            pnlFutbol.Controls.Add(cmbTipoFutbol);
            pnlFutbol.Controls.Add(lblSuperficie);
            pnlFutbol.Controls.Add(cmbSuperficie);
            pnlFutbol.Location = new Point(20, 200);
            pnlFutbol.Name = "pnlFutbol";
            pnlFutbol.Size = new Size(345, 75);
            pnlFutbol.TabIndex = 8;
            //
            // lblTipoFutbol
            //
            lblTipoFutbol.AutoSize = true;
            lblTipoFutbol.Location = new Point(0, 8);
            lblTipoFutbol.Name = "lblTipoFutbol";
            lblTipoFutbol.Size = new Size(86, 15);
            lblTipoFutbol.TabIndex = 0;
            lblTipoFutbol.Text = "Tipo de fútbol:";
            //
            // cmbTipoFutbol
            //
            cmbTipoFutbol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoFutbol.Location = new Point(130, 5);
            cmbTipoFutbol.Name = "cmbTipoFutbol";
            cmbTipoFutbol.Size = new Size(210, 23);
            cmbTipoFutbol.TabIndex = 1;
            //
            // lblSuperficie
            //
            lblSuperficie.AutoSize = true;
            lblSuperficie.Location = new Point(0, 43);
            lblSuperficie.Name = "lblSuperficie";
            lblSuperficie.Size = new Size(62, 15);
            lblSuperficie.TabIndex = 2;
            lblSuperficie.Text = "Superficie:";
            //
            // cmbSuperficie
            //
            cmbSuperficie.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSuperficie.Location = new Point(130, 40);
            cmbSuperficie.Name = "cmbSuperficie";
            cmbSuperficie.Size = new Size(210, 23);
            cmbSuperficie.TabIndex = 3;
            //
            // pnlPadel
            //
            pnlPadel.Controls.Add(lblTipoPared);
            pnlPadel.Controls.Add(cmbTipoPared);
            pnlPadel.Location = new Point(20, 200);
            pnlPadel.Name = "pnlPadel";
            pnlPadel.Size = new Size(345, 40);
            pnlPadel.TabIndex = 9;
            //
            // lblTipoPared
            //
            lblTipoPared.AutoSize = true;
            lblTipoPared.Location = new Point(0, 8);
            lblTipoPared.Name = "lblTipoPared";
            lblTipoPared.Size = new Size(52, 15);
            lblTipoPared.TabIndex = 0;
            lblTipoPared.Text = "Paredes:";
            //
            // cmbTipoPared
            //
            cmbTipoPared.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoPared.Location = new Point(130, 5);
            cmbTipoPared.Name = "cmbTipoPared";
            cmbTipoPared.Size = new Size(210, 23);
            cmbTipoPared.TabIndex = 1;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(190, 295);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(85, 30);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelar
            //
            btnCancelar.Location = new Point(280, 295);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(85, 30);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            //
            // CanchaDetalle
            //
            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;
            ClientSize = new Size(449, 350);
            Controls.Add(lblNombre);
            Controls.Add(txtNombre);
            Controls.Add(lblPrecioPorHora);
            Controls.Add(numPrecioPorHora);
            Controls.Add(chkTechada);
            Controls.Add(grpTipo);
            Controls.Add(pnlFutbol);
            Controls.Add(pnlPadel);
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
            pnlFutbol.ResumeLayout(false);
            pnlFutbol.PerformLayout();
            pnlPadel.ResumeLayout(false);
            pnlPadel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblPrecioPorHora;
        private System.Windows.Forms.NumericUpDown numPrecioPorHora;
        private System.Windows.Forms.CheckBox chkTechada;
        private System.Windows.Forms.GroupBox grpTipo;
        private System.Windows.Forms.RadioButton radPadel;
        private System.Windows.Forms.RadioButton radFutbol;
        private System.Windows.Forms.Panel pnlFutbol;
        private System.Windows.Forms.Label lblTipoFutbol;
        private System.Windows.Forms.ComboBox cmbTipoFutbol;
        private System.Windows.Forms.Label lblSuperficie;
        private System.Windows.Forms.ComboBox cmbSuperficie;
        private System.Windows.Forms.Panel pnlPadel;
        private System.Windows.Forms.Label lblTipoPared;
        private System.Windows.Forms.ComboBox cmbTipoPared;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
