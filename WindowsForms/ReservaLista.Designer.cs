namespace WindowsForms
{
    partial class ReservaLista
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
            this.dgvReservas = new System.Windows.Forms.DataGridView();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCancha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTurno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImporteTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSena = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas)).BeginInit();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();
            //
            // dgvReservas
            //
            this.dgvReservas.AllowUserToAddRows = false;
            this.dgvReservas.AllowUserToDeleteRows = false;
            this.dgvReservas.AutoGenerateColumns = false;
            this.dgvReservas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFecha,
            this.colCliente,
            this.colCancha,
            this.colTurno,
            this.colEstado,
            this.colImporteTotal,
            this.colSena});
            this.dgvReservas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReservas.Location = new System.Drawing.Point(0, 40);
            this.dgvReservas.MultiSelect = false;
            this.dgvReservas.Name = "dgvReservas";
            this.dgvReservas.ReadOnly = true;
            this.dgvReservas.RowHeadersWidth = 25;
            this.dgvReservas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservas.Size = new System.Drawing.Size(960, 460);
            this.dgvReservas.TabIndex = 1;
            this.dgvReservas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReservas_CellDoubleClick);
            this.dgvReservas.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvReservas_CellFormatting);
            //
            // colFecha
            //
            this.colFecha.DataPropertyName = "Fecha";
            this.colFecha.DefaultCellStyle.Format = "dd/MM/yyyy";
            this.colFecha.HeaderText = "Fecha";
            this.colFecha.Name = "colFecha";
            this.colFecha.ReadOnly = true;
            this.colFecha.Width = 90;
            //
            // colCliente
            //
            this.colCliente.DataPropertyName = "NombreCliente";
            this.colCliente.HeaderText = "Cliente";
            this.colCliente.Name = "colCliente";
            this.colCliente.ReadOnly = true;
            this.colCliente.Width = 160;
            //
            // colCancha
            //
            this.colCancha.DataPropertyName = "NombreCancha";
            this.colCancha.HeaderText = "Cancha";
            this.colCancha.Name = "colCancha";
            this.colCancha.ReadOnly = true;
            this.colCancha.Width = 120;
            //
            // colTurno
            //
            this.colTurno.DataPropertyName = "HorarioTurno";
            this.colTurno.HeaderText = "Turno";
            this.colTurno.Name = "colTurno";
            this.colTurno.ReadOnly = true;
            this.colTurno.Width = 110;
            //
            // colEstado
            //
            this.colEstado.DataPropertyName = "EstadoReserva";
            this.colEstado.HeaderText = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            this.colEstado.Width = 100;
            //
            // colImporteTotal
            //
            this.colImporteTotal.DataPropertyName = "ImporteTotal";
            this.colImporteTotal.DefaultCellStyle.Format = "C2";
            this.colImporteTotal.HeaderText = "Importe total";
            this.colImporteTotal.Name = "colImporteTotal";
            this.colImporteTotal.ReadOnly = true;
            this.colImporteTotal.Width = 110;
            //
            // colSena
            //
            this.colSena.DataPropertyName = "Sena";
            this.colSena.DefaultCellStyle.Format = "C2";
            this.colSena.HeaderText = "Seña";
            this.colSena.Name = "colSena";
            this.colSena.ReadOnly = true;
            this.colSena.Width = 100;
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
            this.pnlAcciones.Size = new System.Drawing.Size(960, 40);
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
            // ReservaLista
            //
            this.ClientSize = new System.Drawing.Size(960, 500);
            this.Controls.Add(this.dgvReservas);
            this.Controls.Add(this.pnlAcciones);
            this.Name = "ReservaLista";
            this.Text = "Reservas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas)).EndInit();
            this.pnlAcciones.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dgvReservas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCancha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTurno;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImporteTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSena;
        private System.Windows.Forms.FlowLayoutPanel pnlAcciones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
    }
}