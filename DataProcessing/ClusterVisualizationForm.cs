using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataProcessing {
    public partial class ClusterVisualizationForm : Form {
        public ClusterVisualizationForm(List<List<double[]>> Clusters) {
            InitializeComponent();
            this.Clusters = Clusters;
            int dim = Clusters[0][0].Length;
            for (int i = 0; i < dim; i++)  {
                x_select.Items.Add(i + 1);
                y_select.Items.Add(i + 1);
            }
        }
        List<List<double[]>> Clusters;

        private void ok_Click(object sender, EventArgs e) {
            if(x_select.Text == "" || y_select.Text == "") {
                return;
            }
            int x = int.Parse(x_select.Text) - 1, y = int.Parse(y_select.Text) - 1;
            chart.Series.Clear();
            chart.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            chart.ChartAreas[0].CursorX.Interval = 0.001;
            chart.ChartAreas[0].CursorY.Interval = 0.001;
            chart.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 0.01;
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
            for(int i = 0; i < Clusters.Count; i++) {
                List<double[]> Cluster = Clusters[i];
                chart.Series.Add(i+"");
                chart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                chart.Series[i].MarkerSize = 6;
                chart.Series[i].IsVisibleInLegend = false;
                for (int j = 0; j < Cluster.Count; j++) {
                    chart.Series[i].Points.AddXY(Cluster[j][x], Cluster[j][y]);
                }
            }
        }
    }
}
