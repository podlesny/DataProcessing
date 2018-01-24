using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DataProcessing {
	public partial class DrawMultiDimForm : Form {
		public DrawMultiDimForm(int option) {
			InitializeComponent();
			this.option = option;
			Nnd = MainForm.Nnd;
			dim = MainForm.dim;
			arr = MainForm.arr;
			if (option == 0) {
				sizex = 1225 / dim;// - 6 * (dim + 1);
				sizey = 690 / dim;// - 6 * (dim + 1);
			}
			if(option == 3) {
				x_mult = MainForm.x_mult;
				y_mult = MainForm.x_mult;
				a0 = MainForm.a0;
				a_mult = MainForm.a_mult;
			}
		}
		int option;
		int Nnd, dim;
		int sizex, sizey;
		double a0;
		double[][] arr, x_mult, y_mult;
		double[] a_mult;

		private void DrawMultiDimForm_Load(object sender, EventArgs e) {
			switch (option) {
				#region Кореляционная матрица
				case 0: {
						Chart[][] charts = new Chart[dim][];
						//MessageBox.Show(Nnd + " " + dim);

						for (int i = 0; i < dim; i++) {
							charts[i] = new Chart[dim];
							for (int j = 0; j <dim; j++) {
								charts[i][j] = new Chart();
								Controls.Add(charts[i][j]);
								charts[i][j].Series.Add(i + " " + j);
								if (i == j)
									charts[i][j].Series[0].ChartType = SeriesChartType.Column;
								else
									charts[i][j].Series[0].ChartType = SeriesChartType.Point;
								charts[i][j].Size = new Size(sizex, sizey);
								//charts[i][j].Location = new Point(sizex * i + 6 * (i + 1), sizey * j + 6 * (j + 1));
								charts[i][j].Location = new Point(sizex * i, sizey * j);
								//MessageBox.Show(charts[i][j].Location + "\n" + i + " " + j);
								#region Гистограмма
								if (i == j) {
									List<double> temp = new List<double>();
									for (int k = 0; k < Nnd; k++)
										temp.Add(arr[i][k]);
									temp.Sort();
									List<double[]> var_row = new List<double[]>();
									double M, step;
									double cur = temp[0], counter = 0;
									if (Nnd <= 100) {
										M = Math.Ceiling(Math.Sqrt(Nnd));
										if (M % 2 == 0)
											M--;
									}
									else {
										M = Math.Ceiling(Math.Pow(Nnd, 1.0 / 3.0));
										if (M % 2 == 0)
											M--;
									}
									step = (temp[temp.Count - 1] - temp[0]) / M;
									for (int k = 0; k < Nnd; k++) {
										if (temp[k] >= cur + step) {
											var_row.Add(new double[] { cur, counter / Nnd });
											cur = temp[k];
											counter = 0;
										}
										else {
											counter++;
										}
									}
									charts[i][j].ChartAreas.Add(i + " " + j);
									charts[i][j].ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
									charts[i][j].ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
									charts[i][j].Series[0].Color = Color.Blue;
									charts[i][j].Series[0].ChartType = SeriesChartType.Column;
									charts[i][j].Series[0]["PointWidth"] = "1";
									charts[i][j].Series[0].IsVisibleInLegend = false;
									charts[i][j].Series[0].BorderColor = Color.Black;
									charts[i][j].ChartAreas[0].AxisX.Minimum = var_row[0][0] - step;
									charts[i][j].ChartAreas[0].AxisX.Maximum = var_row[var_row.Count - 1][0] + step;
									charts[i][j].ChartAreas[0].AxisX.Interval = step;
									for (int k = 0; k < var_row.Count; k++) {
										charts[i][j].Series[0].Points.AddXY(Math.Round(var_row[k][0], 4), Math.Round(var_row[k][1], 4));
									}
								}
								#endregion


								#region Кореляционное поле
								else {
									double[] x = arr[i], y = arr[j];
									charts[i][j].ChartAreas.Add(i + " " + j);
									charts[i][j].ChartAreas[0].CursorX.IsUserEnabled = true;
									charts[i][j].ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
									charts[i][j].ChartAreas[0].AxisX.ScaleView.Zoomable = true;
									charts[i][j].ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
									charts[i][j].ChartAreas[0].CursorY.IsUserEnabled = true;
									charts[i][j].ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
									charts[i][j].ChartAreas[0].AxisY.ScaleView.Zoomable = true;
									charts[i][j].ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
									charts[i][j].ChartAreas[0].CursorX.Interval = 0.001;
									charts[i][j].ChartAreas[0].CursorY.Interval = 0.001;
									charts[i][j].ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 0.01;
									charts[i][j].ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
									charts[i][j].ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
									double difx, dify, maxx = x.Max(), maxy = y.Max(), minx = x.Min(), miny = y.Min();
									if (maxx - minx >= maxy - miny) {
										difx = 0;
										dify = ((maxx - minx) - (maxy - miny)) / 2;
									}
									else {
										difx = ((maxy - miny) - (maxx - minx)) / 2; ;
										dify = 0;
									}
									charts[i][j].ChartAreas[0].AxisX.Maximum = maxx + difx;
									charts[i][j].ChartAreas[0].AxisX.Minimum = minx - difx;
									charts[i][j].ChartAreas[0].AxisY.Maximum = maxy + dify;
									charts[i][j].ChartAreas[0].AxisY.Minimum = miny - dify;
									charts[i][j].Series[0].Color = Color.Black;
									charts[i][j].Series[0].MarkerSize = 4;
									charts[i][j].Series[0].IsVisibleInLegend = false;
									for (int k = 0; k < Nnd; k++) {
										charts[i][j].Series[0].Points.AddXY(x[k], y[k]);
									}
								}
								#endregion
							}
						}
					}
					break;
				#endregion

				#region Бульбашки
				case 1: {
						Chart chart = new Chart();
						Controls.Add(chart);
						chart.Size = new Size(1225, 716);
						chart.Location = new Point(0, 0);
						chart.ChartAreas.Add("");
						chart.ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
						chart.ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
						chart.Series.Add("");
						chart.Series[0].ChartType = SeriesChartType.Bubble;
						chart.Series[0].MarkerStyle = MarkerStyle.Circle;
						double difx, dify, maxx = arr[0].Max(), maxy = arr[1].Max(), minx = arr[0].Min(), miny = arr[1].Min();
						/*if (maxx - minx >= maxy - miny) {
							difx = 0;
							dify = ((maxx - minx) - (maxy - miny)) / 2;
						}
						else {
							difx = ((maxy - miny) - (maxx - minx)) / 2; ;
							dify = 0;
						}
						chart.ChartAreas[0].AxisX.Maximum = maxx + difx;
						chart.ChartAreas[0].AxisX.Minimum = minx - difx;
						chart.ChartAreas[0].AxisY.Maximum = maxy + dify;
						chart.ChartAreas[0].AxisY.Minimum = miny - dify;*/
						chart.Series[0].Color = Color.Transparent;
						chart.Series[0].BorderColor = Color.Black;
						for (int i = 0; i < Nnd; i++) {
							chart.Series[0].Points.AddXY(arr[0][i], arr[1][i], arr[2][i]);
						}
					}
					break;
				#endregion

				#region Параллельные координаты
				case 2: {
						Chart chart = new Chart();
						Controls.Add(chart);
						chart.Size = new Size(1225, 716);
						chart.Location = new Point(0, 0);
						chart.ChartAreas.Add("");
						chart.ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
						chart.ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
						chart.ChartAreas[0].AxisX.Minimum = 0;
						chart.ChartAreas[0].AxisX.Maximum = dim-1;
						double[] max = new double[dim], min = new double[dim];
						for(int i = 0; i < dim; i++) {
							max[i] = arr[i].Max();
							min[i] = arr[i].Min();
						}
						for (int i = 0; i < Nnd; i++) {
							chart.Series.Add(i.ToString());
							chart.Series[i].ChartType = SeriesChartType.Line;
							for(int j = 0; j < dim; j++) {
								chart.Series[i].Points.AddXY(j, (arr[j][i] - min[j])/max[j]);
							}
						}
						
					}
					break;
				#endregion

				#region Диагностичеcкая диаграмма
				case 3: {
						Chart chart = new Chart();
						Controls.Add(chart);
						chart.Size = new Size(1225, 716);
						chart.Location = new Point(0, 0);
						chart.ChartAreas.Add("");
						chart.ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
						chart.ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
						chart.Series.Add("");
						chart.Series[0].ChartType = SeriesChartType.Point;
						double[] x = new double[Nnd], y = new double[Nnd], ymult = Matrix.Transposition(y_mult)[0];
						int ir = MainForm.ir;
						for (int i = 0; i < Nnd; i++) {
							x[i] = arr[ir][i];
							y[i] = arr[ir][i] - a0;
							for (int j = 0; j < dim - 1; j++) {
								if (j != ir)
									y[i] -= a_mult[j] * arr[j][i];
							}
						}
						double difx, dify, maxx = x.Max(), maxy = y.Max(), minx = x.Min(), miny = y.Min();
						if (maxx - minx >= maxy - miny) {
							difx = 0;
							dify = ((maxx - minx) - (maxy - miny)) / 2;
						}
						else {
							difx = ((maxy - miny) - (maxx - minx)) / 2; ;
							dify = 0;
						}
						chart.ChartAreas[0].AxisX.Maximum = maxx + difx;
						chart.ChartAreas[0].AxisX.Minimum = minx - difx;
						chart.ChartAreas[0].AxisY.Maximum = maxy + dify;
						chart.ChartAreas[0].AxisY.Minimum = miny - dify;
						chart.Series[0].Color = Color.Transparent;
						chart.Series[0].BorderColor = Color.Black;
						for (int i = 0; i < Nnd; i++) {
							chart.Series[0].Points.AddXY(x[i], y[i]);
						}
					}
					break;
				#endregion
			}
		}
	}
}
