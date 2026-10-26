using API.Clients;
using DTOs;

namespace WindowsForms
{
    /// <summary>
    /// Reporte 1 (con gráfico): ocupación de cada cancha en un período. Los datos ya vienen
    /// calculados de la API (/reportes/ocupacion); acá solo se muestran.
    /// </summary>
    public partial class ReporteOcupacion : Form
    {
        // Ancho en píxeles de la barra más larga (la cancha con más reservas).
        private const int AnchoMaximoBarra = 400;

        public ReporteOcupacion()
        {
            InitializeComponent();
            ConfigurarColumnas();

            // Período por defecto: el último mes y las próximas dos semanas.
            dtpDesde.Value = DateTime.Today.AddDays(-30);
            dtpHasta.Value = DateTime.Today.AddDays(14);

            Load += async (_, _) => await GenerarAsync();
        }

        // Gráfico de barras armado con controles comunes: por cada cancha, una fila con un
        // Label (nombre), un Panel de color cuyo ancho es proporcional a las reservas, y otro
        // Label con el número. La cancha con más reservas tiene la barra de AnchoMaximoBarra.
        private void ArmarGrafico(List<ReporteOcupacionDTO> datos)
        {
            pnlGrafico.Controls.Clear();
            pnlGrafico.Controls.Add(new Label
            {
                Text = "Reservas por cancha  (verde: fútbol - naranja: pádel)",
                Font = new Font(Font, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            });

            int maximo = datos.Count == 0 ? 0 : datos.Max(d => d.CantidadReservas);

            foreach (var d in datos)
            {
                var fila = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0, 2, 0, 2) };

                var lblCancha = new Label
                {
                    Text = $"{d.NombreCancha} ({d.TipoCancha})",
                    Width = 200,
                    Height = 22,
                    TextAlign = ContentAlignment.MiddleRight
                };

                // Regla de tres: si 'maximo' reservas ocupan AnchoMaximoBarra píxeles, ¿cuántos ocupa esta cancha?
                int ancho = maximo == 0 ? 0 : d.CantidadReservas * AnchoMaximoBarra / maximo;
                var barra = new Panel
                {
                    Width = Math.Max(ancho, 2),
                    Height = 22,
                    BackColor = d.TipoCancha == "Pádel" ? Color.DarkOrange : Color.SeaGreen
                };

                var lblValor = new Label
                {
                    Text = $"{d.CantidadReservas} reservas ({d.PorcentajeOcupacion:0.0} %)",
                    AutoSize = true,
                    Margin = new Padding(6, 4, 0, 0)
                };

                fila.Controls.Add(lblCancha);
                fila.Controls.Add(barra);
                fila.Controls.Add(lblValor);
                pnlGrafico.Controls.Add(fila);
            }
        }

        private void ConfigurarColumnas()
        {
            dgvOcupacion.AutoGenerateColumns = false;
            dgvOcupacion.Columns.Clear();

            dgvOcupacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreCancha", HeaderText = "Cancha", DataPropertyName = "NombreCancha", Width = 200
            });
            dgvOcupacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TipoCancha", HeaderText = "Tipo", DataPropertyName = "TipoCancha", Width = 100
            });
            dgvOcupacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CantidadReservas", HeaderText = "Reservas", DataPropertyName = "CantidadReservas", Width = 100,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvOcupacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TurnosDisponibles", HeaderText = "Turnos disponibles", DataPropertyName = "TurnosDisponibles", Width = 140,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvOcupacion.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PorcentajeOcupacion", HeaderText = "% Ocupación", DataPropertyName = "PorcentajeOcupacion", Width = 110,
                DefaultCellStyle = { Format = "0.0' %'", Alignment = DataGridViewContentAlignment.MiddleRight }
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
                var datos = (await ReporteApiClient.GetOcupacionAsync(dtpDesde.Value.Date, dtpHasta.Value.Date)).ToList();

                dgvOcupacion.DataSource = datos;
                ArmarGrafico(datos);

                lblResumen.Text = $"Total: {datos.Sum(d => d.CantidadReservas)} reservas (sin contar las canceladas)";
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
