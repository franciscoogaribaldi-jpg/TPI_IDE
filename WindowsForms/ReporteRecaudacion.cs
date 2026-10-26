using API.Clients;
using DTOs;

namespace WindowsForms
{
    /// <summary>
    /// Reporte 2: recaudación por mes y por tipo de cancha (solo reservas Finalizadas, que son
    /// las ya cobradas). Los datos vienen calculados de la API (/reportes/recaudacion).
    /// </summary>
    public partial class ReporteRecaudacion : Form
    {
        // Fila que se muestra en la grilla: el DTO más el período ya formateado ("09/2026").
        private class FilaRecaudacion
        {
            public string Periodo { get; set; } = string.Empty;
            public string TipoCancha { get; set; } = string.Empty;
            public int CantidadReservas { get; set; }
            public decimal ImporteCanchas { get; set; }
            public decimal ImporteAdicionales { get; set; }
            public decimal ImporteTotal { get; set; }
        }

        public ReporteRecaudacion()
        {
            InitializeComponent();
            ConfigurarColumnas();

            // Período por defecto: desde el primer día de hace dos meses hasta hoy.
            var haceDosMeses = DateTime.Today.AddMonths(-2);
            dtpDesde.Value = new DateTime(haceDosMeses.Year, haceDosMeses.Month, 1);
            dtpHasta.Value = DateTime.Today;

            Load += async (_, _) => await GenerarAsync();
        }

        private void ConfigurarColumnas()
        {
            dgvRecaudacion.AutoGenerateColumns = false;
            dgvRecaudacion.Columns.Clear();

            dgvRecaudacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Periodo", HeaderText = "Mes", DataPropertyName = "Periodo", Width = 90
            });
            dgvRecaudacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TipoCancha", HeaderText = "Tipo de cancha", DataPropertyName = "TipoCancha", Width = 120
            });
            dgvRecaudacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CantidadReservas", HeaderText = "Reservas", DataPropertyName = "CantidadReservas", Width = 90,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvRecaudacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ImporteCanchas", HeaderText = "Alquiler de canchas", DataPropertyName = "ImporteCanchas", Width = 150,
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvRecaudacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ImporteAdicionales", HeaderText = "Adicionales", DataPropertyName = "ImporteAdicionales", Width = 130,
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvRecaudacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ImporteTotal", HeaderText = "Total", DataPropertyName = "ImporteTotal", Width = 130,
                DefaultCellStyle = { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private async void btnGenerar_Click(object sender, EventArgs e)
        {
            await GenerarAsync();
        }

        private async Task GenerarAsync()
        {
            if (dtpDesde.Value.Date > dtpHasta.Value.Date)
            {
                MessageBox.Show("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.", "Período inválido",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cursor = Cursors.WaitCursor;
            btnGenerar.Enabled = false;
            try
            {
                var datos = (await ReporteApiClient.GetRecaudacionAsync(dtpDesde.Value.Date, dtpHasta.Value.Date)).ToList();

                dgvRecaudacion.DataSource = datos.Select(d => new FilaRecaudacion
                {
                    Periodo = $"{d.Mes:00}/{d.Anio}",
                    TipoCancha = d.TipoCancha,
                    CantidadReservas = d.CantidadReservas,
                    ImporteCanchas = d.ImporteCanchas,
                    ImporteAdicionales = d.ImporteAdicionales,
                    ImporteTotal = d.ImporteTotal
                }).ToList();

                decimal futbol = datos.Where(d => d.TipoCancha == "Fútbol").Sum(d => d.ImporteTotal);
                decimal padel = datos.Where(d => d.TipoCancha == "Pádel").Sum(d => d.ImporteTotal);
                lblTotales.Text = $"Total del período: {datos.Sum(d => d.ImporteTotal):C2}   " +
                                  $"(Fútbol: {futbol:C2}  -  Pádel: {padel:C2}  -  {datos.Sum(d => d.CantidadReservas)} reservas finalizadas)";
            }
            catch (Exception ex)
            {
                ErroresUI.Mostrar(this, ex, "No se pudo generar el reporte");
            }
            finally
            {
                btnGenerar.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }
}
