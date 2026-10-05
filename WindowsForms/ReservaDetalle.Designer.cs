namespace WindowsForms
{
    partial class ReservaDetalle
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

        private void InitializeComponent()
        {
            this.lblCliente = new System.Windows.Forms.Label();
            this.cmbCliente = new System.Windows.Forms.ComboBox();
            this.lblCancha = new System.Windows.Forms.Label();
            this.cmbCancha = new System.Windows.Forms.ComboBox();
            this.lblTurno = new System.Windows.Forms.Label();
            this.cmbTurno = new System.Windows.Forms.ComboBox();
            this.lblFecha = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cmbEstado = new System.Windows.Forms.ComboBox();
            this.lblSena = new System.Windows.Forms.Label();
            this.numSena = new System.Windows.Forms.NumericUpDown();
            this.lblDetalle = new System.Windows.Forms.Label();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.colConcepto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecioUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTotalEstimado = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numSena)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            this.SuspendLayout();
            //
            // lblCliente
            //
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(20, 23);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(47, 15);
            this.lblCliente.TabIndex = 0;
            this.lblCliente.Text = "Cliente:";
            //
            // cmbCliente
            //
            this.cmbCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCliente.Location = new System.Drawing.Point(140, 20);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(300, 23);
            this.cmbCliente.TabIndex = 1;
            //
            // lblCancha
            //
            this.lblCancha.AutoSize = true;
            this.lblCancha.Location = new System.Drawing.Point(20, 63);
            this.lblCancha.Name = "lblCancha";
            this.lblCancha.Size = new System.Drawing.Size(50, 15);
            this.lblCancha.TabIndex = 2;
            this.lblCancha.Text = "Cancha:";
            //
            // cmbCancha
            //
            this.cmbCancha.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCancha.Location = new System.Drawing.Point(140, 60);
            this.cmbCancha.Name = "cmbCancha";
            this.cmbCancha.Size = new System.Drawing.Size(300, 23);
            this.cmbCancha.TabIndex = 3;
            this.cmbCancha.SelectedIndexChanged += new System.EventHandler(this.cmbCancha_SelectedIndexChanged);
            //
            // lblTurno
            //
            this.lblTurno.AutoSize = true;
            this.lblTurno.Location = new System.Drawing.Point(20, 103);
            this.lblTurno.Name = "lblTurno";
            this.lblTurno.Size = new System.Drawing.Size(42, 15);
            this.lblTurno.TabIndex = 4;
            this.lblTurno.Text = "Turno:";
            //
            // cmbTurno
            //
            this.cmbTurno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTurno.Location = new System.Drawing.Point(140, 100);
            this.cmbTurno.Name = "cmbTurno";
            this.cmbTurno.Size = new System.Drawing.Size(300, 23);
            this.cmbTurno.TabIndex = 5;
            //
            // lblFecha
            //
            this.lblFecha.AutoSize = true;
            this.lblFecha.Location = new System.Drawing.Point(20, 143);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(42, 15);
            this.lblFecha.TabIndex = 6;
            this.lblFecha.Text = "Fecha:";
            //
            // dtpFecha
            //
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(140, 140);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(150, 23);
            this.dtpFecha.TabIndex = 7;
            //
            // lblEstado
            //
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(320, 143);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(45, 15);
            this.lblEstado.TabIndex = 8;
            this.lblEstado.Text = "Estado:";
            //
            // cmbEstado
            //
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.Location = new System.Drawing.Point(400, 140);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(150, 23);
            this.cmbEstado.TabIndex = 9;
            //
            // lblSena
            //
            this.lblSena.AutoSize = true;
            this.lblSena.Location = new System.Drawing.Point(20, 183);
            this.lblSena.Name = "lblSena";
            this.lblSena.Size = new System.Drawing.Size(39, 15);
            this.lblSena.TabIndex = 10;
            this.lblSena.Text = "Seña:";
            //
            // numSena
            //
            this.numSena.DecimalPlaces = 2;
            this.numSena.Location = new System.Drawing.Point(140, 180);
            this.numSena.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
            this.numSena.Name = "numSena";
            this.numSena.Size = new System.Drawing.Size(150, 23);
            this.numSena.TabIndex = 11;
            //
            // lblDetalle
            //
            this.lblDetalle.AutoSize = true;
            this.lblDetalle.Location = new System.Drawing.Point(20, 220);
            this.lblDetalle.Name = "lblDetalle";
            this.lblDetalle.Size = new System.Drawing.Size(140, 15);
            this.lblDetalle.TabIndex = 12;
            this.lblDetalle.Text = "Detalle de la reserva:";
            //
            // dgvDetalle
            //
            this.dgvDetalle.AllowUserToAddRows = true;
            this.dgvDetalle.AllowUserToDeleteRows = true;
            this.dgvDetalle.AutoGenerateColumns = false;
            this.dgvDetalle.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colConcepto,
            this.colCantidad,
            this.colPrecioUnitario,
            this.colSubtotal});
            this.dgvDetalle.Location = new System.Drawing.Point(20, 245);
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.RowHeadersWidth = 25;
            this.dgvDetalle.Size = new System.Drawing.Size(590, 220);
            this.dgvDetalle.TabIndex = 13;
            this.dgvDetalle.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDetalle_CellEndEdit);
            this.dgvDetalle.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvDetalle_UserDeletedRow);
            this.dgvDetalle.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDetalle_DataError);
            //
            // colConcepto
            //
            this.colConcepto.DataPropertyName = "Concepto";
            this.colConcepto.HeaderText = "Concepto";
            this.colConcepto.Name = "colConcepto";
            this.colConcepto.Width = 250;
            //
            // colCantidad
            //
            this.colCantidad.DataPropertyName = "Cantidad";
            this.colCantidad.HeaderText = "Cantidad";
            this.colCantidad.Name = "colCantidad";
            this.colCantidad.Width = 90;
            //
            // colPrecioUnitario
            //
            this.colPrecioUnitario.DataPropertyName = "PrecioUnitario";
            this.colPrecioUnitario.HeaderText = "Precio unitario";
            this.colPrecioUnitario.Name = "colPrecioUnitario";
            this.colPrecioUnitario.Width = 110;
            //
            // colSubtotal
            //
            this.colSubtotal.DataPropertyName = "Subtotal";
            this.colSubtotal.DefaultCellStyle.Format = "C2";
            this.colSubtotal.HeaderText = "Subtotal";
            this.colSubtotal.Name = "colSubtotal";
            this.colSubtotal.ReadOnly = true;
            this.colSubtotal.Width = 110;
            //
            // lblTotalEstimado
            //
            this.lblTotalEstimado.AutoSize = true;
            this.lblTotalEstimado.Location = new System.Drawing.Point(20, 478);
            this.lblTotalEstimado.Name = "lblTotalEstimado";
            this.lblTotalEstimado.Size = new System.Drawing.Size(130, 15);
            this.lblTotalEstimado.TabIndex = 14;
            this.lblTotalEstimado.Text = "Total estimado: $0,00";
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(435, 505);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(85, 30);
            this.btnGuardar.TabIndex = 15;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(525, 505);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(85, 30);
            this.btnCancelar.TabIndex = 16;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // ReservaDetalle
            //
            this.AcceptButton = this.btnGuardar;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(650, 555);
            this.Controls.Add(this.lblCliente);
            this.Controls.Add(this.cmbCliente);
            this.Controls.Add(this.lblCancha);
            this.Controls.Add(this.cmbCancha);
            this.Controls.Add(this.lblTurno);
            this.Controls.Add(this.cmbTurno);
            this.Controls.Add(this.lblFecha);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.cmbEstado);
            this.Controls.Add(this.lblSena);
            this.Controls.Add(this.numSena);
            this.Controls.Add(this.lblDetalle);
            this.Controls.Add(this.dgvDetalle);
            this.Controls.Add(this.lblTotalEstimado);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.MinimizeBox = false;
            this.Name = "ReservaDetalle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ReservaDetalle";
            this.Load += new System.EventHandler(this.ReservaDetalle_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numSena)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.ComboBox cmbCliente;
        private System.Windows.Forms.Label lblCancha;
        private System.Windows.Forms.ComboBox cmbCancha;
        private System.Windows.Forms.Label lblTurno;
        private System.Windows.Forms.ComboBox cmbTurno;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.ComboBox cmbEstado;
        private System.Windows.Forms.Label lblSena;
        private System.Windows.Forms.NumericUpDown numSena;
        private System.Windows.Forms.Label lblDetalle;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConcepto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecioUnitario;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubtotal;
        private System.Windows.Forms.Label lblTotalEstimado;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}