using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DataProcessing {
	public partial class MainForm : Form {
		#region Конструктор
		public MainForm() {
			InitializeComponent();
			var_rows_tree = new Dictionary<string, List<double[]>>();
			N_tree = new Dictionary<string, int>();
			egf = new EmpirGraphFlags(true, true, false, false);
			gf = new GistFlags(true, false);
			corff = new CorfFlags(true, false, false, false, false, false, false, false, false, false, false, false, false);
			var_row = null;
			var_row_cl = null;
			_N = new List<int>();
			set = new Settings();
		}
		#endregion

		#region Одномерный анализ
		#region Переменные
		List<List<double[]>> var_rows;

		List<double[]> var_row_cl, var_row;

		Dictionary<string, List<double[]>> var_rows_tree;

		Dictionary<string, int> N_tree;

		NumF f, F, mqd;

		Eval ev;
		EmpirGraphFlags egf;
		GistFlags gf;
		public static Settings set;

		static public int v_count = 0, l_count = 0, st_count = 0, sh_count = 0, d_count = 0, N, max_col = 0, row_count = 0;

		static double step, m, lm, sig;//, m_mod = 0, sig_mod = 1, lm_mod = 1, al_mod = 1, bet_mod = 2;

		public List<int> _N;

		static public double[][] dist_f, dist_F;

		double[] dist_mqd;
		#endregion

		#region Контекстные меню

		#region AddTableCM

		private void AddTableCMManual_Click(object sender, EventArgs e) {
			if (var_rows == null)
				var_rows = new List<List<double[]>>();
			List<List<double>> arr = new List<List<double>>();
			new GenerateForm(arr).ShowDialog();
			if (arr[0]!=null) {
				MakeAddTable(arr);
			}
		}

		private void AddTableCMFile_Click(object sender, EventArgs e) {
			if (var_rows == null)
				var_rows = new List<List<double[]>>();
			OpenFileDialog od = new OpenFileDialog();
			if (od.ShowDialog() == DialogResult.OK) {
				StreamReader sr = new StreamReader(od.FileName);
				List<List<double>> arr = new List<List<double>>();
				string[] txtData = null;
				string s = null;
				for (int i = 0; (s = sr.ReadLine()) != null; i++) {
					txtData = s.Replace('.', ',').Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					arr.Add(new List<double>());
					for (int j = 0; j < txtData.Length; j++) {
						double temp = 0;
						if (double.TryParse(txtData[j], out temp)) {
							arr[i].Add(temp);
						}
					}
				}
				MakeAddTable(arr);
			}
		}

		private void DeleteVectorToolStripMenuItem_Click(object sender, EventArgs e) {
			int row = AddTable.SelectedCells[0].RowIndex;
			string key = (string)AddTable.Rows[row].HeaderCell.Value;
			AddTable.Rows.RemoveAt(row);
			var_rows.RemoveAt(row);
			row_count--;
		}

		private void DeleteValueToolStripMenuItem_Click(object sender, EventArgs e) {
			if (AddTable.SelectedCells[0].Value == null)
				return;
			double[] val = new double[] { (double)AddTable.SelectedCells[0].Value };
			int row = AddTable.SelectedCells[0].RowIndex;
			int delInd = var_rows[row].BinarySearch(val, new VarRowComp());
			var_rows[row].RemoveAt(delInd);
			for (int i = delInd + 1; i <= var_rows[row].Count; i++) {
				AddTable[i - 1, row].Value = AddTable[i, row].Value;
			}
			AddTable[var_rows[row].Count, row].Value = null;
			if(var_rows[row].Count == 0) {
				DeleteVectorToolStripMenuItem_Click(new object(), new EventArgs());
			}
		}

		private void AddTreeToolStripMenuItem_Click(object sender, EventArgs e) {
			if (AddTable.SelectedCells.Count != 0) {
				string nodeName = (string)AddTable.SelectedCells[0].OwningRow.HeaderCell.Value;
				if (Tree.Nodes.ContainsKey(nodeName)) {
					int n = 1;
					string temp = nodeName + "(1)";
					for (; Tree.Nodes.ContainsKey(temp); n++, temp = nodeName + "(" + n + ")") ;
					nodeName = temp;
				}
				Tree.Nodes.Add(nodeName, nodeName);
				List<double[]> old_ = var_rows[AddTable.SelectedCells[0].RowIndex];
				List<double[]> new_ = new List<double[]>();
				for (int i = 0; i < old_.Count; i++) {
					new_.Add(new double[3]);
					for (int j = 0; j < 3; j++)
						new_[i][j] = old_[i][j];
				}
				N_tree.Add(nodeName, _N[AddTable.SelectedCells[0].RowIndex]);
				var_rows_tree.Add(nodeName, new_);
			}
		}

		private void AddTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
			AddTreeToolStripMenuItem_Click(new object(), new EventArgs());
		}

		#endregion

		#region TreeCM

		private void chooseToolStripMenuItem_Click(object sender, EventArgs e) {
			if (Tree.SelectedNode != null) {
				var_row_cl = new List<double[]>();
				dist_F = null;
				dist_f = null;
				dist_mqd = null;
				egf.F = FMenuItem.Checked = false;
				egf.interv = intervMenuItem.Checked = false;
				gf.f = f_MenuItem.Checked = false;
				string name = Tree.SelectedNode.Text;
				var_row = var_rows_tree[name];
				N = N_tree[name];
				double M;
				if (set.classNum == -1) {
					if (N <= 100) {
						M = Math.Ceiling(Math.Sqrt(N));
						if (M % 2 == 0)
							M--;
					}
					else {
						M = Math.Ceiling(Math.Pow(N, 1.0 / 3.0));
						if (M % 2 == 0)
							M--;
					}
				}
				else
					M = set.classNum;
				step = (var_row[var_row.Count - 1][0] - var_row[0][0]) / M;
				//if (step < 1)
				//	step = 1;
				int j = 0, l = 0;
				for (double i = var_row[0][0], empir = 0; i < var_row[var_row.Count - 1][0]; i += step, l++) {
					int k = 0;
					double p = 0;
					for (; j + k < var_row.Count && var_row[j + k][0] < i + step; p += var_row[j + k][1], k++) ;
					var_row_cl.Add(new double[3]);
					var_row_cl[l][0] = i;
					var_row_cl[l][1] = p;
					empir += p;
					var_row_cl[l][2] = empir;
					j += k;
				}
				MakeGistogram(var_row_cl);
				MakeEmpirGraph(var_row, var_row_cl);
				MakeEvalTable(var_row);
			}
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
			var_rows_tree.Remove(Tree.SelectedNode.Name);
			N_tree.Remove(Tree.SelectedNode.Name);
			Tree.Nodes.Remove(Tree.SelectedNode);
		}

		private void shiftStripMenuItem_Click(object sender, EventArgs e) {
			if (Tree.SelectedNode != null) {
				List<double[]> old_ = var_rows_tree[Tree.SelectedNode.Name], new_ = new List<double[]>();
				for (int i = 0; i < old_.Count; i++) {
					new_.Add(new double[3]);
					new_[i][0] = old_[i][0] + set.shiftDist;
					new_[i][1] = old_[i][1];
					new_[i][2] = old_[i][2];
				}
				var_rows_tree.Add("SH" + sh_count, new_);
				N_tree.Add("SH" + sh_count, N);
				Tree.Nodes.Add("SH" + sh_count, "SH" + sh_count);
				sh_count++;
			}
		}

		private void deleteAnomStripMenuItem_Click(object sender, EventArgs e) {
			if (Tree.SelectedNode != null) {
				List<double[]> old_ = var_rows_tree[Tree.SelectedNode.Name], new_ = new List<double[]>();
				double a = ev.MID - ev.MQD * (1.2 + 3.6 * (1 - ev.ContrE) * Math.Log10(N / 10)),
					b = ev.MID + ev.MQD * (1.2 + 3.6 * (1 - ev.ContrE) * Math.Log10(N / 10)), empir = 0;
				for (int i = 0, j = 0; i < old_.Count; i++) {
					if (old_[i][0] > a && old_[i][0] < b) {
						new_.Add(new double[3]);
						new_[j][0] = old_[i][0];
						new_[j][1] = old_[j][1] * N;
						empir += new_[j][1];
						new_[j][2] = empir;
						j++;
					}
				}
				for (int i = 0; i < new_.Count; i++) {
					new_[i][1] /= empir;
					new_[i][2] /= empir;
				}
				var_rows_tree.Add("D" + d_count, new_);
				N_tree.Add("D" + d_count, (int)empir);
				Tree.Nodes.Add("D" + d_count, "D" + d_count);
				d_count++;
			}
		}

		private void logToolStripMenuItem_Click(object sender, EventArgs e) {
			if (Tree.SelectedNode != null) {
				List<double[]> old_ = var_rows_tree[Tree.SelectedNode.Name], new_ = new List<double[]>();
				double shift = old_[0][0] < 0 ? old_[0][0] : 0;
				for (int i = 0; i < old_.Count; i++) {
					new_.Add(new double[3]);
					new_[i][0] = Math.Log(old_[i][0] + shift, set.logBase);
					new_[i][1] = old_[i][1];
					new_[i][2] = old_[i][2];
				}
				var_rows_tree.Add("L" + l_count, new_);
				N_tree.Add("L" + l_count, N);
				Tree.Nodes.Add("L" + l_count, "L" + l_count);
				l_count++;
			}
		}

		private void standToolStripMenuItem_Click(object sender, EventArgs e) {
			if (Tree.SelectedNode != null) {
				List<double[]> old_ = var_rows_tree[Tree.SelectedNode.Name], new_ = new List<double[]>();
				double MID = 0, MQD = 0;
				for (int i = 0; i < old_.Count; i++) {
					MID += old_[i][0] * old_[i][1];
					MQD += Math.Pow(old_[i][0] - MID, 2) * old_[i][1];
				}
				MQD = Math.Sqrt(MQD);
				for (int i = 0; i < old_.Count; i++) {
					new_.Add(new double[3]);
					new_[i][0] = (old_[i][0] + MID) / MQD;
					new_[i][1] = old_[i][1];
					new_[i][2] = old_[i][2];
				}
				var_rows_tree.Add("ST" + st_count, new_);
				N_tree.Add("ST" + st_count, N);
				Tree.Nodes.Add("ST" + st_count, "ST" + st_count);
				st_count++;
			}
		}

		private void Tree_DoubleClick(object sender, EventArgs e) {
			chooseToolStripMenuItem_Click(new object(), new EventArgs());
		}

		private void twoSelMenuItem_Click(object sender, EventArgs e) {
			new CheckSelParamForm(var_rows_tree, N_tree).ShowDialog();
		}
		#endregion

		#region EmpirGraphCM

		private void graph_clMenuItem_Click(object sender, EventArgs e) {
			egf.graph_cl = graph_clMenuItem.Checked;
			if (var_row != null && var_row_cl != null)
				MakeEmpirGraph(var_row, var_row_cl);
		}

		private void graphMenuItem_Click(object sender, EventArgs e) {
			egf.graph = graphMenuItem.Checked;
			if (var_row != null && var_row_cl != null)
				MakeEmpirGraph(var_row, var_row_cl);
		}

		private void FMenuItem_Click(object sender, EventArgs e) {
			egf.F = FMenuItem.Checked;
			if (var_row != null && var_row_cl != null)
				MakeEmpirGraph(var_row, var_row_cl);
		}

		private void інтервалToolStripMenuItem_Click(object sender, EventArgs e) {
			egf.interv = intervMenuItem.Checked;
			if (var_row != null && var_row_cl != null)
				MakeEmpirGraph(var_row, var_row_cl);
		}
		#endregion

		#region GistogramCM

		private void gistMenuItem_Click(object sender, EventArgs e) {
			gf.gist = gistMenuItem.Checked;
			if (var_row_cl != null)
				MakeGistogram(var_row_cl);
		}

		private void f_MenuItem_Click(object sender, EventArgs e) {
			gf.f = f_MenuItem.Checked;
			if (var_row_cl != null)
				MakeGistogram(var_row_cl);
		}
		#endregion

		private void settingsMenuItem_Click(object sender, EventArgs e) {
			new SettingsForm(set).ShowDialog();
		}

		#endregion

		#region Графики и таблицы

		void MakeAddTable(List<List<double>> arr) {
			int cur_max = 0;
			for (int i = 0; i < arr.Count; i++) {
				arr[i].Sort();
				_N.Add(arr[i].Count);
				var_rows.Add(new List<double[]>());
				double empir = 0;
				int l = 0;
				int i_ = i + row_count;
				for (int j = 0; j < arr[i].Count; j++, l++) {
					var_rows[i_].Add(new double[3]);
					var_rows[i_][l][0] = arr[i][j];
					double k = 1;
					for (; j + k < arr[i].Count && arr[i][j + (int)k] == var_rows[i_][l][0]; k++) ;
					var_rows[i_][l][1] = k / arr[i].Count;
					empir += var_rows[i_][l][1];
					var_rows[i_][l][2] = empir;
					j += (int)k - 1;
				}
				if (l > cur_max)
					cur_max = l;
			}
			if (cur_max > max_col) {
				int add = cur_max - max_col;
				for (int i = 0; i < add; i++) {
					AddTable.Columns.Add(i.ToString(), i.ToString());
					AddTable.Columns[i].Width = 60;
				}
				max_col = cur_max;
			}
			AddTable.Rows.Add(arr.Count);
			for (int i = row_count; i < AddTable.Rows.Count; i++, v_count++) {
				AddTable.Rows[i].HeaderCell.Value = "V" + v_count;
				for (int j = 0; j < var_rows[i].Count; j++) {
					AddTable[j, i].Value = var_rows[i][j][0];
				}
			}
			row_count = AddTable.Rows.Count;
		}

		void MakeEvalTable(List<double[]> var_row) {
			ev = new Eval(var_row);
			EvalTable.Rows.Clear();
			EvalTable.Columns.Clear();
			EvalTable.Columns.Add("Min", "Min");
			EvalTable.Columns[0].Width = 125;
			EvalTable.Columns.Add("Eval", "θ^");
			EvalTable.Columns[1].Width = 125;
			EvalTable.Columns.Add("Max", "Max");
			EvalTable.Columns[2].Width = 125;
			EvalTable.Rows.Add(12);
			EvalTable.Rows[0].HeaderCell.Value = "MID";
			EvalTable[0, 0].Value = Math.Round(ev.MID_min, 4);
			EvalTable[1, 0].Value = Math.Round(ev.MID, 4);
			EvalTable[2, 0].Value = Math.Round(ev.MID_max, 4);
			EvalTable.Rows[1].HeaderCell.Value = "σ";
			EvalTable[0, 1].Value = Math.Round(ev.MQD_min, 4);
			EvalTable[1, 1].Value = Math.Round(ev.MQD, 4);
			EvalTable[2, 1].Value = Math.Round(ev.MQD_max, 4);
			EvalTable.Rows[2].HeaderCell.Value = "MED";
			EvalTable[1, 2].Value = Math.Round(ev.MED, 4);
			EvalTable.Rows[3].HeaderCell.Value = "MED Walsh";
			EvalTable[1, 3].Value = Math.Round(ev.MED_W, 4);
			EvalTable.Rows[4].HeaderCell.Value = "MAD";
			EvalTable[1, 4].Value = Math.Round(ev.MAD, 4);
			EvalTable.Rows[5].HeaderCell.Value = "A";
			EvalTable[0, 5].Value = Math.Round(ev.A_min, 4);
			EvalTable[1, 5].Value = Math.Round(ev.A, 4);
			EvalTable[2, 5].Value = Math.Round(ev.A_max, 4);
			EvalTable.Rows[6].HeaderCell.Value = "E";
			EvalTable[0, 6].Value = Math.Round(ev.E_min, 4);
			EvalTable[1, 6].Value = Math.Round(ev.E, 4);
			EvalTable[2, 6].Value = Math.Round(ev.E_max, 4);
			EvalTable.Rows[7].HeaderCell.Value = "χ";
			EvalTable[0, 7].Value = Math.Round(ev.ContrE_min, 4);
			EvalTable[1, 7].Value = Math.Round(ev.ContrE, 4);
			EvalTable[2, 7].Value = Math.Round(ev.ContrE_max, 4);
			EvalTable.Rows[8].HeaderCell.Value = "W";
			EvalTable[0, 8].Value = Math.Round(ev.VAR_min, 4);
			EvalTable[1, 8].Value = Math.Round(ev.VAR, 4);
			EvalTable[2, 8].Value = Math.Round(ev.VAR_max, 4);
			EvalTable.Rows[9].HeaderCell.Value = "Wн";
			EvalTable[1, 9].Value = Math.Round(ev.VAR_NP, 4);
			EvalTable.Rows[9].HeaderCell.Value = "MIDα(" + set.alpha + ")";
			EvalTable[1, 9].Value = Math.Round(ev.MID_CUT, 4);
		}

		void MakeGistogram(List<double[]> var_row_cl) {
			gist_ch.Series.Clear();
			gist_ch.ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
			gist_ch.ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
			if (gf.gist && var_row_cl!=null) {
				gist_ch.Series.Add("g");
				gist_ch.Series["g"].Color = Color.Blue;
				gist_ch.Series["g"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
				gist_ch.Series["g"]["PointWidth"] = "1";
				gist_ch.Series["g"].IsVisibleInLegend = false;
				gist_ch.Series["g"].BorderColor = Color.Black;
				gist_ch.ChartAreas[0].AxisX.Minimum = var_row_cl[0][0] - step;
				gist_ch.ChartAreas[0].AxisX.Maximum = var_row_cl[var_row_cl.Count - 1][0] + step;
				gist_ch.ChartAreas[0].AxisX.Interval = step;
				for (int i = 0; i < var_row_cl.Count; i++) {
					gist_ch.Series["g"].Points.AddXY(Math.Round(var_row_cl[i][0], 4), Math.Round(var_row_cl[i][1], 4));
				}
			}
			if (gf.f && var_row_cl != null && dist_f != null) {
				gist_ch.Series.Add("f");
				gist_ch.Series["f"].Color = Color.Red;
				gist_ch.Series["f"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				gist_ch.ChartAreas[0].AxisX.Minimum = var_row_cl[0][0] - step;
				gist_ch.ChartAreas[0].AxisX.Maximum = var_row_cl[var_row_cl.Count - 1][0] + step;
				gist_ch.ChartAreas[0].AxisX.Interval = step;
				gist_ch.Series["f"].IsVisibleInLegend = false;
				gist_ch.Series["f"].BorderWidth = 2;
				for(int i = 0; i < dist_f.Length; i++) {
					gist_ch.Series["f"].Points.AddXY(dist_f[i][0], dist_f[i][1]);
					//MessageBox.Show(dist_f[i][0] + " " + dist_f[i][1]);
				}
			}
		}

		void MakeEmpirGraph(List<double[]> var_row, List<double[]> var_row_cl) {
			graph_ch.Series.Clear();
			graph_ch.ChartAreas[0].CursorX.IsUserEnabled = true;
			graph_ch.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
			graph_ch.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
			graph_ch.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
			graph_ch.ChartAreas[0].CursorY.IsUserEnabled = true;
			graph_ch.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
			graph_ch.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
			graph_ch.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
			graph_ch.ChartAreas[0].AxisX.Minimum = var_row[0][0] - step;
			graph_ch.ChartAreas[0].AxisX.Maximum = var_row[var_row.Count - 1][0] + step;
			graph_ch.ChartAreas[0].AxisY.Minimum = 0.0;
			graph_ch.ChartAreas[0].AxisY.Maximum = 1.0;
			graph_ch.ChartAreas[0].CursorX.Interval = 0.001;
			graph_ch.ChartAreas[0].CursorY.Interval = 0.001;
			graph_ch.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 0.01;
			graph_ch.ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
			graph_ch.ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
			if (egf.graph_cl && var_row_cl != null) {
				for (int i = 0; i < var_row_cl.Count - 1; i++) {
					graph_ch.Series.Add(i.ToString());
					graph_ch.Series[i.ToString()].Color = Color.Red;
					graph_ch.Series[i.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
					graph_ch.Series[i.ToString()].IsVisibleInLegend = false;
					graph_ch.Series[i.ToString()].Points.AddXY(var_row_cl[i][0], var_row_cl[i][2]);
					graph_ch.Series[i.ToString()].Points.AddXY(var_row_cl[i + 1][0], var_row_cl[i][2]);
				}
				graph_ch.Series.Add((var_row_cl.Count - 1).ToString());
				graph_ch.Series[(var_row_cl.Count - 1).ToString()].Color = Color.Red;
				graph_ch.Series[(var_row_cl.Count - 1).ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				graph_ch.Series[(var_row_cl.Count - 1).ToString()].IsVisibleInLegend = false;
				graph_ch.Series[(var_row_cl.Count - 1).ToString()].Points.AddXY(var_row_cl[(var_row_cl.Count - 1)][0], var_row_cl[(var_row_cl.Count - 1)][2]);
				graph_ch.Series[(var_row_cl.Count - 1).ToString()].Points.AddXY(var_row_cl[(var_row_cl.Count - 1)][0] + step, var_row_cl[(var_row_cl.Count - 1)][2]);
			}
			if (egf.graph && var_row != null) {
				graph_ch.Series.Add("Point");
				graph_ch.Series["Point"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
				graph_ch.Series["Point"].IsVisibleInLegend = false;
				graph_ch.Series["Point"].MarkerSize = 3;
				for (int i = 0; i < var_row.Count; i++) {
					graph_ch.Series["Point"].Points.AddXY(var_row[i][0], var_row[i][2]);
				}
			}
			if (egf.F && dist_F != null) {
				graph_ch.Series.Add("F");
				graph_ch.Series["F"].Color = Color.Lime;
				graph_ch.Series["F"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				graph_ch.ChartAreas[0].AxisX.Interval = step;
				graph_ch.Series["F"].IsVisibleInLegend = false;
				graph_ch.Series["F"].BorderWidth = 2;
				for (int i = 0; i < dist_f.Length; i++) {
					graph_ch.Series["F"].Points.AddXY(dist_F[i][0], dist_F[i][1]);
					//MessageBox.Show(dist_F[i][0] + " " + dist_F[i][1]);
				}
			}
			if (egf.interv && dist_F != null && dist_mqd != null) {
				graph_ch.Series.Add("Min");
				graph_ch.Series["Min"].Color = Color.Red;
				graph_ch.Series["Min"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				graph_ch.ChartAreas[0].AxisX.Interval = step;
				graph_ch.Series["Min"].IsVisibleInLegend = false;
				graph_ch.Series["Min"].BorderWidth = 2;
				graph_ch.Series.Add("Max");
				graph_ch.Series["Max"].Color = Color.Red;
				graph_ch.Series["Max"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				graph_ch.Series["Max"].IsVisibleInLegend = false;
				graph_ch.Series["Max"].BorderWidth = 2;
				for (int i = 0; i < dist_f.Length; i++) {
					if (dist_F[i][1] - dist_mqd[i] < 0)
						graph_ch.Series["Min"].Points.AddXY(dist_F[i][0], 0);
					else if (dist_F[i][1] - dist_mqd[i] > 1)
						graph_ch.Series["Min"].Points.AddXY(dist_F[i][0], 1);
					else
						graph_ch.Series["Min"].Points.AddXY(dist_F[i][0], dist_F[i][1] - dist_mqd[i]);
					if (dist_F[i][1] + dist_mqd[i] < 0)
						graph_ch.Series["Max"].Points.AddXY(dist_F[i][0], 0);
					else if (dist_F[i][1] + dist_mqd[i] > 1)
						graph_ch.Series["Max"].Points.AddXY(dist_F[i][0], 1);
					else
						graph_ch.Series["Max"].Points.AddXY(dist_F[i][0], dist_F[i][1] + dist_mqd[i]);
				}
			}
		}
		#endregion

		#region Распределения
		private void нормальнийToolStripMenuItem_Click(object sender, EventArgs e) {
			if (var_row != null) {
				m = ev.MID;
				sig = N * Math.Sqrt(ev.MID_QUAD - Math.Pow(ev.MID, 2)) / (N - 1);
				f = norm_f;
				F = norm_F;
				mqd = norm_mqd;
				EvalTable.Rows[11].HeaderCell.Value = "m";
				EvalTable[0, 11].Value = Math.Round(m - 1.645 * sig / Math.Sqrt(N), 4);
				EvalTable[1, 11].Value = Math.Round(m, 4);
				EvalTable[2, 11].Value = Math.Round(m + 1.645 * sig / Math.Sqrt(N), 4);
				EvalTable.Rows[12].HeaderCell.Value = "σ";
				EvalTable[0, 12].Value = Math.Round(sig - 1.645 * sig / Math.Sqrt(2 * N), 4);
				EvalTable[1, 12].Value = Math.Round(sig, 4);
				EvalTable[2, 12].Value = Math.Round(sig + 1.645 * sig / Math.Sqrt(2 * N), 4);
				MakeDistr();
			}
		}

		private void експоненціальнийToolStripMenuItem_Click(object sender, EventArgs e) {
			if(var_row != null) {
				lm = 1 / ev.MID;
				f = exp_f;
				F = exp_F;
				mqd = exp_mqd;
				EvalTable.Rows[11].HeaderCell.Value = "λ";
				EvalTable[0, 11].Value = Math.Round(lm - 1.645 / (lm * Math.Sqrt(N)), 4);
				EvalTable[1, 11].Value = Math.Round(lm, 4);
				EvalTable[2, 11].Value = Math.Round(lm + 1.645 / (lm * Math.Sqrt(N)), 4);
				EvalTable.Rows[12].HeaderCell.Value = "";
				EvalTable[0, 12].Value = "";
				EvalTable[1, 12].Value = "";
				EvalTable[2, 12].Value = "";
				MakeDistr();
			}
		}

		private void арксинусаToolStripMenuItem_Click(object sender, EventArgs e) {
			if (var_row != null) {
				m = Math.Sqrt(2) * Math.Sqrt(ev.MID_QUAD - Math.Pow(ev.MID, 2));
				//MessageBox.Show(m + "");
				f = arcsin_f;
				F = arcsin_F;
				mqd = arcsin_mqd;
				EvalTable.Rows[11].HeaderCell.Value = "a";
				EvalTable[0, 11].Value = Math.Round(m - 1.645 / (lm * Math.Sqrt(N)), 4);
				EvalTable[1, 11].Value = Math.Round(m, 4);
				EvalTable[2, 11].Value = Math.Round(m + 1.645 / (lm * Math.Sqrt(N)), 4);
				EvalTable.Rows[12].HeaderCell.Value = "";
				EvalTable[0, 12].Value = "";
				EvalTable[1, 12].Value = "";
				EvalTable[2, 11].Value = "";
				MakeDistr();
			}
		}

		void MakeDistr() {
			double st = (var_row[var_row.Count - 1][0] - var_row[0][0]) / 200, par = var_row[0][0];
			dist_f = new double[200][];
			dist_F = new double[200][];
			dist_mqd = new double[200];
			for (int i = 0; i < 200; par += st, i++) {
				dist_f[i] = new double[2];
				dist_F[i] = new double[2];
				dist_f[i][0] = par;
				dist_F[i][0] = par;
				dist_f[i][1] = f(par)*step;
				dist_F[i][1] = F(par);
				if (dist_f[i][1] < 0)
					dist_f[i][1] = 0;
				else if (dist_f[i][1] > 1)
					dist_f[i][1] = 1;
				if (dist_F[i][1] < 0)
					dist_F[i][1] = 0;
				else if (dist_F[i][1] > 1)
					dist_F[i][1] = 1;
				dist_mqd[i] = mqd(par);
			}
			f_MenuItem.Checked = true;
			gf.f = true;
			FMenuItem.Checked = true;
			egf.F = true;
			intervMenuItem.Checked = true;
			egf.interv = true;
			MakeGistogram(var_row_cl);
			MakeEmpirGraph(var_row, var_row_cl);
		}
		#endregion

		#endregion

		#region Двухмерный анализ

		#region Переменные
		Dictionary<string, double[][]> arrs2d;

		public static double[][] arr;
		public static double[,,] var_row_cl2d;

		static public List<double>[] arr_cl;

		static public int v_count2d = 0, Mx, My, N2d;

		static double stepx, stepy, maxp, maxx, maxy, minx, miny, Sa, Sb, S_rem = 0, t_mid;
		static double? ar = null, br = null, cr = null;

		bool gist_paint = false;

		Regression regr;

		Graphics gi;

		CorfFlags corff;

		static public Eval2D ev2d;
		#endregion

		#region Контекстные меню

		#region Tree2DCM

		private void addFile2DClick(object sender, EventArgs e) {
			if (arrs2d == null)
				arrs2d = new Dictionary<string, double[][]>();
			OpenFileDialog od = new OpenFileDialog();
			if (od.ShowDialog() == DialogResult.OK) {
				StreamReader sr = new StreamReader(od.FileName);
				string[][] txtData = new string[2][];
				txtData[0] = sr.ReadLine().Replace('.', ',').Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				txtData[1] = sr.ReadLine().Replace('.', ',').Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				if(txtData[0].Length != txtData[1].Length) {
					MessageBox.Show("Некоректно введені дані.");
					return;
				}
				double[][] arr = new double[2][];
				arr[0] = new double[txtData[0].Length];
				arr[1] = new double[txtData[0].Length];
				for (int j = 0; j < txtData[0].Length; j++) {
					double[] temp = new double[2];
					if (double.TryParse(txtData[0][j], out temp[0]) && double.TryParse(txtData[1][j], out temp[1])) {
						arr[0][j] = temp[0];
						arr[1][j] = temp[1];
					}
				}
				//Array.Sort(arr, new VarRowComp());
				arrs2d.Add("V" + v_count2d, arr);
				Tree2D.Nodes.Add("V" + v_count2d, "V" + v_count2d);
				v_count2d++;
			}
		}

		private void generateMenuItem_Click(object sender, EventArgs e) {
			if (arrs2d == null)
				arrs2d = new Dictionary<string, double[][]>();
			double[][] arr = new double[2][];
			new GenerateForm(arr).ShowDialog();
			//Array.Sort(arr, new VarRowComp());
			if (arr[0] != null) {
				arrs2d.Add("V" + v_count2d, arr);
				Tree2D.Nodes.Add("V" + v_count2d, "V" + v_count2d);
				v_count2d++;
			}
		}

		private void selectMenuItem_Click(object sender, EventArgs e) {
			if(Tree2D.SelectedNode != null) {
				string name = Tree2D.SelectedNode.Text;
				arr = arrs2d[name];
				minx = double.MaxValue;
				miny = double.MaxValue;
				maxx = double.MinValue;
				maxy = double.MinValue;
				maxp = double.MinValue;
				N2d = arr[0].Length;
				if (set.Mx == -1) {
					if (N2d <= 100) {
						Mx = (int)Math.Ceiling(Math.Sqrt(N2d));
						if (Mx % 2 == 0)
							Mx--;
					}
					else {
						Mx = (int)Math.Ceiling(Math.Pow(N2d, 1.0 / 3.0));
						if (Mx % 2 == 0)
							Mx--;
					}
				}
				else
					Mx = set.Mx;
				if (set.My == -1) {
					if (N2d <= 100) {
						My = (int)Math.Ceiling(Math.Sqrt(N2d));
						if (My % 2 == 0)
							My--;
					}
					else {
						My = (int)Math.Ceiling(Math.Pow(N2d, 1.0 / 3.0));
						if (My % 2 == 0)
							My--;
					}
				}
				else
					My = set.My;
				for (int i = 0; i < N2d; i++) {
					//MessageBox.Show(minx + " " + miny + "\n" + maxx + " " + maxy);
					if (arr[0][i] < minx)
						minx = arr[0][i];
					if (arr[1][i] < miny)
						miny = arr[1][i];
					if (arr[0][i] > maxx)
						maxx = arr[0][i];
					if (arr[1][i] > maxy)
						maxy = arr[1][i];
				}
				stepx = (maxx - minx) / Mx;
				stepy = (maxy - miny) / My;
				var_row_cl2d = new double[Mx, My, 4];
				arr_cl = new List<double>[Mx];
				double x = minx + stepx / 2, y = miny + stepy / 2, empir = 0;
				for (int i = 0; i < Mx; i++, x+=stepx) {
					y = miny + stepy / 2;
					for (int j = 0; j < My; j++, y+=stepy) {
						var_row_cl2d[i, j, 0] = x;
						var_row_cl2d[i, j, 1] = y;
						int p = 0;
						for(int k = 0; k < N2d; k++) {
							if ((arr[0][k] < x + stepx / 2 && arr[0][k] >= x - stepx / 2 || x + stepx / 2 == maxx && arr[0][k] == maxx) && (arr[1][k] < y + stepy / 2 && arr[1][k] >= y - stepy / 2 || y + stepy / 2 == maxy && arr[1][k] == maxy)) {
								//MessageBox.Show((x - stepx / 2) + " " + arr[0][k] + " " + (x + stepx / 2) + "\n" + (y - stepy / 2) + " " + arr[1][k] + " " + (y + stepy / 2) + "\nok");
								p++;
							}
						}
						
						var_row_cl2d[i, j, 2] = p / (double)N2d;
						if (var_row_cl2d[i, j, 2] > maxp)
							maxp = var_row_cl2d[i, j, 2];
						empir = 0;
						for (int k = 0; k <= i; k++) {
							for (int l = 0; l <= j; l++)
								empir += var_row_cl2d[k, l, 2];
						}
						var_row_cl2d[i, j, 3] = empir;
						//MessageBox.Show(i + " " + j + "\n" + var_row_cl2d[i, j, 0] + " " + var_row_cl2d[i, j, 1] + " " + var_row_cl2d[i, j, 2] + " " + var_row_cl2d[i, j, 3]);
					}
				}
				x = minx + stepx / 2;
				for (int i = 0; i < Mx; i++, x += stepx) {
					arr_cl[i] = new List<double>();
					for(int j = 0; j < N2d; j++) {
						if(arr[0][j] < x + stepx / 2 && arr[0][j] >= x - stepx / 2) {
							arr_cl[i].Add(arr[1][j]);
						}
					}
				}
				regr = Regression.None;
				ev2d = new Eval2D(arr);
				MakeEvalTable2D();
				gist_paint = true;
				gist2d.Invalidate();
				MakeCorField();
			}
		}

		private void deleteMenuItem_Click(object sender, EventArgs e) {
			if (Tree2D.SelectedNode != null) {
				string name = Tree2D.SelectedNode.Text;
				Tree2D.Nodes.Remove(Tree2D.SelectedNode);
				arrs2d.Remove(name);
			}
		}

		#endregion

		#region CorFieldCM

		private void corf_chs_Click(object sender, EventArgs e) {
			corff.corf = corf_chs.Checked;
			MakeCorField();
		}

		private void regr_chs_Click(object sender, EventArgs e) {
			switch (regr) {
				case Regression.Linear: {
						corff.regr_l = regr_chs.Checked;
					}
					break;
				case Regression.Parabolic: {
						corff.regr_p = regr_chs.Checked;
					}
					break;
				case Regression.Qlinear: {
						corff.regr_q = regr_chs.Checked;
					}
					break;
				default:  {
						regr_chs.Checked = false;
					}
					break;
			}
			MakeCorField();
		}

		private void toler_chs_Click(object sender, EventArgs e) {
			switch (regr) {
				case Regression.Linear: {
						corff.toler_l = toler_chs.Checked;
					}
					break;
				case Regression.Parabolic: {
						corff.toler_p = toler_chs.Checked;
					}
					break;
				case Regression.Qlinear: {
						corff.toler_q = toler_chs.Checked;
					}
					break;
				default: {
						toler_chs.Checked = false;
					}
					break;
			}
			MakeCorField();
		}

		private void interv_chs_Click(object sender, EventArgs e) {
			switch (regr) {
				case Regression.Linear: {
						corff.interv_l = interv_chs.Checked;
					}
					break;
				case Regression.Parabolic: {
						corff.interv_p = interv_chs.Checked;
					}
					break;
				case Regression.Qlinear: {
						corff.interv_q = toler_chs.Checked;
					}
					break;
				default: {
						interv_chs.Checked = false;
					}
					break;
			}
			MakeCorField();
		}

		private void intervprogn_chs_Click(object sender, EventArgs e) {
			switch (regr) {
				case Regression.Linear: {
						corff.progn_l = intervprogn_chs.Checked;
					}
					break;
				case Regression.Parabolic: {
						corff.progn_p = intervprogn_chs.Checked;
					}
					break;
				case Regression.Qlinear: {
						corff.progn_q = toler_chs.Checked;
					}
					break;
				default: {
						intervprogn_chs.Checked = false;
					}
					break;
			}
			MakeCorField();
		}
		#endregion

		#endregion

		#region Графики и таблицы

		void MakeEvalTable2D() {
			EvalTable2D.Rows.Clear();
			EvalTable2D.Columns.Clear();
			EvalTable2D.Columns.Add("Eval", "θ^");
			EvalTable2D.Columns[0].Width = 125;
			EvalTable2D.Rows.Add(18);
			EvalTable2D.Rows[0].HeaderCell.Value = "MID_X";
			EvalTable2D[0, 0].Value = Math.Round(ev2d.MID_X, 4);
			EvalTable2D.Rows[1].HeaderCell.Value = "MID_Y";//σ
			EvalTable2D[0, 1].Value = Math.Round(ev2d.MID_Y, 4);
			EvalTable2D.Rows[2].HeaderCell.Value = "MID_XY";
			EvalTable2D[0, 2].Value = Math.Round(ev2d.MID_XY, 4);
			EvalTable2D.Rows[3].HeaderCell.Value = "σx";
			EvalTable2D[0, 3].Value = Math.Round(ev2d.MQD_X, 4);
			EvalTable2D.Rows[4].HeaderCell.Value = "σy";
			EvalTable2D[0, 4].Value = Math.Round(ev2d.MQD_Y, 4);
			EvalTable2D.Rows[5].HeaderCell.Value = "r(x,y)н";
			EvalTable2D[0, 5].Value = Math.Round(ev2d.COR_MIN, 4);
			EvalTable2D.Rows[6].HeaderCell.Value = "r(x,y)";
			EvalTable2D[0, 6].Value = Math.Round(ev2d.COR, 4);
			EvalTable2D.Rows[7].HeaderCell.Value = "r(x,y)в";
			EvalTable2D[0, 7].Value = Math.Round(ev2d.COR_MAX, 4);
			EvalTable2D.Rows[8].HeaderCell.Value = "ρ(ζ,η)";
			EvalTable2D[0, 8].Value = Math.Round(ev2d.COR_RATIO, 4);
			EvalTable2D.Rows[9].HeaderCell.Value = "Cпірмена";
			EvalTable2D[0, 9].Value = Math.Round(ev2d.SPIR, 4);
			EvalTable2D.Rows[10].HeaderCell.Value = "Кендалла";
			EvalTable2D[0, 10].Value = Math.Round(ev2d.KEND, 4);
			EvalTable2D.Rows[11].HeaderCell.Value = "Фехнера";
			EvalTable2D[0, 11].Value = Math.Round(ev2d.FEHN, 4);
			EvalTable2D.Rows[12].HeaderCell.Value = "Ф";
			EvalTable2D[0, 12].Value = double.IsNaN(ev2d.FI) ? "Не визначено" : Math.Round(ev2d.FI, 4).ToString();
			EvalTable2D.Rows[13].HeaderCell.Value = "Y";
			EvalTable2D[0, 13].Value = double.IsNaN(ev2d.YUL_Y) ? "Не визначено" : Math.Round(ev2d.YUL_Y, 4).ToString();
			EvalTable2D.Rows[14].HeaderCell.Value = "Q";
			EvalTable2D[0, 14].Value = double.IsNaN(ev2d.YUL_Q) ? "Не визначено" : Math.Round(ev2d.YUL_Q, 4).ToString();
			EvalTable2D.Rows[15].HeaderCell.Value = "Сполучень Пірсона";
			EvalTable2D[0, 15].Value = Math.Round(ev2d.PIRS, 4);
			EvalTable2D.Rows[16].HeaderCell.Value = "Міра зв'язку";
			EvalTable2D[0, 16].Value = double.IsNaN(ev2d.KEND2) ? "Не визначено" : Math.Round(ev2d.KEND2, 4).ToString();
			EvalTable2D.Rows[17].HeaderCell.Value = "Стюарда";
			EvalTable2D[0, 17].Value = double.IsNaN(ev2d.STEW) ? "Не визначено" : Math.Round(ev2d.STEW, 4).ToString();

		}

		private void gist2d_Paint(object sender, PaintEventArgs e) {
			if (gist_paint) {
				gi = gist2d.CreateGraphics();
				double pstep = maxp / set.pgrad;
				double[] prob = new double[set.pgrad + 1];
				Color[] color = new Color[set.pgrad + 1];
				for (int i = 1, c = 0; i <= set.pgrad; i++, c += (int)Math.Truncate(256.0 / set.pgrad)) {
					prob[set.pgrad - i] = i * pstep;
					color[i-1] = Color.FromArgb(c, c, c);
				}
				prob[set.pgrad] = 0;
				color[set.pgrad] = Color.FromArgb(255, 255, 255);
				gi.DrawLines(Pens.Black, new Point[] { new Point(40, 365), new Point(530, 365), new Point(530, 5)});
				for(int i = 0, ypos = 5; i < set.pgrad; i++, ypos += 25) {
					gi.FillRectangle(new SolidBrush(color[i]), 535, ypos, 40, 20);
					gi.DrawString(Math.Round(prob[i],3).ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.White, new Point(536, ypos + 1));
				}
				float w = 490f / Mx, h = 360f / My;
				for (int i = 0; i < Mx; i++) {
					for(int j = 0; j < My; j++) {
						int k = set.pgrad;
						for (; k >= 0; k--) {
							if(var_row_cl2d[i,j,2] <= prob[k]) {
								gi.FillRectangle(new SolidBrush(color[k]), 40 + 490f * i / Mx, 5 + 360f * (My - j - 1) / My, w, h);
								break;
							}
						}
						//MessageBox.Show(i + " " + j + " " + var_row_cl2d[i, j, 2] + " " + k);
					}
				}
				for (int i = 0; i < Mx; i++) {
					gi.DrawLine(Pens.Black, 40 + 490f * i / Mx, 5, 40 + 490f * i / Mx, 365);
					gi.DrawString(Math.Round(var_row_cl2d[i, 0, 0], 3).ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 40 + 490f * i / Mx, 365);

				}
				for (int i = 0; i < My; i++) {
					gi.DrawLine(Pens.Black, 40, 5 + 360f * i / My, 530, 5 + 360f * i / My);
					gi.DrawString(Math.Round(var_row_cl2d[0, My - i - 1, 1], 3).ToString(), new Font("Arial", 10, FontStyle.Bold), Brushes.Black, 0, 5 + 360f * (i + 0.8f) / My);
				}
			}
		}

		void MakeCorField() {
			if (arr == null)
				return;
			corf_ch.Series.Clear();
			corf_ch.ChartAreas[0].CursorX.IsUserEnabled = true;
			corf_ch.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
			corf_ch.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
			corf_ch.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
			corf_ch.ChartAreas[0].CursorY.IsUserEnabled = true;
			corf_ch.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
			corf_ch.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
			corf_ch.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
			corf_ch.ChartAreas[0].CursorX.Interval = 0.001;
			corf_ch.ChartAreas[0].CursorY.Interval = 0.001;
			corf_ch.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 0.01;
			corf_ch.ChartAreas[0].AxisX.LabelStyle.Format = "0.000";
			corf_ch.ChartAreas[0].AxisY.LabelStyle.Format = "0.000";
			double difx, dify;
			if (maxx - minx >= maxy - miny) {
				difx = 0;
				dify = ((maxx - minx) - (maxy - miny)) / 2;
			}
			else {
				difx = ((maxy - miny) - (maxx - minx)) / 2; ;
				dify = 0;
			}
			/*corf_ch.ChartAreas[0].AxisX.Maximum = maxx + difx;
			corf_ch.ChartAreas[0].AxisX.Minimum = minx - difx;
			corf_ch.ChartAreas[0].AxisY.Maximum = maxy + dify;
			corf_ch.ChartAreas[0].AxisY.Minimum = miny - dify;*/
			if (arr != null && corff.corf) {
				corf_ch.Series.Add("S");
				corf_ch.Series["S"].Color = Color.Black;
				corf_ch.Series["S"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
				corf_ch.Series["S"].MarkerSize = 4;
				corf_ch.Series["S"].IsVisibleInLegend = false;
				for (int i = 0; i < N2d; i++) {
					corf_ch.Series["S"].Points.AddXY(arr[0][i], arr[1][i]);
				}
			}
			if(regr == Regression.Linear && corff.regr_l) {
				corf_ch.Series.Add("regr_l");
				corf_ch.Series["regr_l"].Color = Color.Red;
				corf_ch.Series["regr_l"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["regr_l"].BorderWidth = 2;
				corf_ch.Series["regr_l"].IsVisibleInLegend = false;
				corf_ch.Series["regr_l"].Points.AddXY(minx, br * minx + ar);
				corf_ch.Series["regr_l"].Points.AddXY(maxx, br * maxx + ar);
			}
			if (regr == Regression.Linear && corff.toler_l) {
				double t = crit_ttest(N2d - 2);
				corf_ch.Series.Add("tol_min");
				corf_ch.Series["tol_min"].Color = Color.Blue;
				corf_ch.Series["tol_min"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["tol_min"].BorderWidth = 2;
				corf_ch.Series["tol_min"].IsVisibleInLegend = false;
				double step_ = (maxx - minx) / 100, x, y;
				corf_ch.Series["tol_min"].Points.AddXY(minx, br * minx + ar - t * S_rem);
				corf_ch.Series["tol_min"].Points.AddXY(maxx, br * maxx + ar - t * S_rem);
				//MessageBox.Show(minx + " " + (br * minx + ar - t * S_rem));
				//MessageBox.Show(maxx + " " + (br * maxx + ar - t * S_rem));
				corf_ch.Series.Add("tol_max");
				corf_ch.Series["tol_max"].Color = Color.Blue;
				corf_ch.Series["tol_max"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["tol_max"].BorderWidth = 2;
				corf_ch.Series["tol_max"].IsVisibleInLegend = false;
				corf_ch.Series["tol_max"].Points.AddXY(minx, br * minx + ar + t * S_rem);
				corf_ch.Series["tol_max"].Points.AddXY(maxx, br * maxx + ar + t * S_rem);
			}
			if (regr == Regression.Linear && corff.progn_l) {
				double t = crit_ttest(N2d - 2);
				double[,] Syx0 = new double[2, 100]; //Math.Sqrt(Math.Pow(S_rem,2)*(1 + 1/N2d) + Math.Pow(Sb, 2)*Math.Pow(x - ev2d.MID_X,2));
				double step_ = (maxx - minx) / 100;
				//MessageBox.Show(maxx + " " + minx + " " + step_);
				for(int i = 0; i < 100; i++) {
					Syx0[0, i] = minx + i * step_;
					Syx0[1, i] = Math.Sqrt(Math.Pow(S_rem, 2) * (1 + 1 / N2d) + Math.Pow(Sb, 2) * Math.Pow(Syx0[0, i] - ev2d.MID_X, 2));
					//MessageBox.Show(Syx0[0, i] + " " + Syx0[1, i]);
				}
				corf_ch.Series.Add("progn_min");
				corf_ch.Series["progn_min"].Color = Color.Green;
				corf_ch.Series["progn_min"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["progn_min"].BorderWidth = 2;
				corf_ch.Series["progn_min"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					corf_ch.Series["progn_min"].Points.AddXY(Syx0[0, i], br * Syx0[0, i] + ar - t * Syx0[1, i]);
					//MessageBox.Show(Syx0[0, i] + " " + (br * Syx0[0, i] + ar - t * Syx0[1, i]));
				}
				corf_ch.Series.Add("progn_max");
				corf_ch.Series["progn_max"].Color = Color.Green;
				corf_ch.Series["progn_max"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["progn_max"].BorderWidth = 2;
				corf_ch.Series["progn_max"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					corf_ch.Series["progn_max"].Points.AddXY(Syx0[0, i], br * Syx0[0, i] + ar + t * Syx0[1, i]);
					//MessageBox.Show(Syx0[0, i] + " " + (br * minx + ar + t * Syx0[1, i]));

				}
			}
			if (regr == Regression.Linear && corff.interv_l) {
				double t = crit_ttest(N2d - 2);
				double[,] Syx = new double[2, 100]; //Math.Sqrt(Math.Pow(S_rem,2)*(1 + 1/N2d) + Math.Pow(Sb, 2)*Math.Pow(x - ev2d.MID_X,2));
				double step_ = (maxx - minx) / 100;
				//MessageBox.Show(maxx + " " + minx + " " + step_);
				for (int i = 0; i < 100; i++) {
					Syx[0, i] = minx + i * step_;
					Syx[1, i] = Math.Sqrt(Math.Pow(S_rem,2)/N2d + Math.Pow(Sb, 2)* Math.Pow(Syx[0, i] - ev2d.MID_X, 2));
					//MessageBox.Show(Syx[0, i] + " " + Syx[1, i]);
				}
				corf_ch.Series.Add("interv_min");
				corf_ch.Series["interv_min"].Color = Color.Fuchsia;
				corf_ch.Series["interv_min"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["interv_min"].BorderWidth = 2;
				corf_ch.Series["interv_min"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					corf_ch.Series["interv_min"].Points.AddXY(Syx[0, i], br * Syx[0, i] + ar - t * Syx[1, i]);
					//MessageBox.Show(Syx0[0, i] + " " + (br * Syx0[0, i] + ar - t * Syx0[1, i]));
				}
				corf_ch.Series.Add("interv_max");
				corf_ch.Series["interv_max"].Color = Color.Fuchsia;
				corf_ch.Series["interv_max"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["interv_max"].BorderWidth = 2;
				corf_ch.Series["interv_max"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					corf_ch.Series["interv_max"].Points.AddXY(Syx[0, i], br * Syx[0, i] + ar + t * Syx[1, i]);
					//MessageBox.Show(Syx[0, i] + " " + (br * minx + ar + t * Syx[1, i]));

				}
			}
			if (regr == Regression.Parabolic && corff.regr_p) {
				double[,] Syx = new double[2, 100]; //Math.Sqrt(Math.Pow(S_rem,2)*(1 + 1/N2d) + Math.Pow(Sb, 2)*Math.Pow(x - ev2d.MID_X,2));
				double step_ = (maxx - minx) / 100;
				//MessageBox.Show(maxx + " " + minx + " " + step_);
				for (int i = 0; i < 100; i++) {
					Syx[0, i] = minx + i * step_;
					double FI1 = Syx[0, i] - ev2d.MID_X,
					FI2 = Math.Pow(Syx[0, i], 2) - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X) * (Syx[0, i] - ev2d.MID_X) / Math.Pow(ev2d.MQD_X, 2) - ev2d.MID_X_QUAD;
					Syx[1, i] = cr.Value * FI2 + br.Value * FI1 + ar.Value;
					//MessageBox.Show(Syx0[0, i] + " " + Syx0[1, i]);
				}
				corf_ch.Series.Add("regr_p");
				corf_ch.Series["regr_p"].Color = Color.Red;
				corf_ch.Series["regr_p"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["regr_p"].BorderWidth = 2;
				corf_ch.Series["regr_p"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					corf_ch.Series["regr_p"].Points.AddXY(Syx[0, i], Syx[1, i]);
					//MessageBox.Show(Syx0[0, i] + " " + (br * Syx0[0, i] + ar - t * Syx0[1, i]));
				}
			}
			if (regr == Regression.Parabolic && corff.toler_p) {
				double t = crit_ttest(N2d - 2);
				corf_ch.Series.Add("tol_min");
				corf_ch.Series["tol_min"].Color = Color.Blue;
				corf_ch.Series["tol_min"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["tol_min"].BorderWidth = 2;
				corf_ch.Series["tol_min"].IsVisibleInLegend = false;
				double step_ = (maxx - minx) / 100, x, y;
				double sqrt_rem = Math.Sqrt(S_rem);
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					double FI1 = x - ev2d.MID_X,
					FI2 = Math.Pow(x, 2) - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X) * (x - ev2d.MID_X) / Math.Pow(ev2d.MQD_X, 2) - ev2d.MID_X_QUAD;
					y = cr.Value * FI2 + br.Value * FI1 + ar.Value;
					corf_ch.Series["tol_min"].Points.AddXY(x, y - t * sqrt_rem);
				}
				corf_ch.Series.Add("tol_max");
				corf_ch.Series["tol_max"].Color = Color.Blue;
				corf_ch.Series["tol_max"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["tol_max"].BorderWidth = 2;
				corf_ch.Series["tol_max"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					double FI1 = x - ev2d.MID_X,
					FI2 = Math.Pow(x, 2) - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X) * (x - ev2d.MID_X) / Math.Pow(ev2d.MQD_X, 2) - ev2d.MID_X_QUAD;
					y = cr.Value * FI2 + br.Value * FI1 + ar.Value;
					corf_ch.Series["tol_max"].Points.AddXY(x, y + t * sqrt_rem);
					//MessageBox.Show(x + " " + (y + t * S_rem));
				}
			}
			if (regr == Regression.Parabolic && corff.interv_p) {
				double t = crit_ttest(N2d - 2);
				corf_ch.Series.Add("interv_min");
				corf_ch.Series["interv_min"].Color = Color.Fuchsia;
				corf_ch.Series["interv_min"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["interv_min"].BorderWidth = 2;
				corf_ch.Series["interv_min"].IsVisibleInLegend = false;
				double step_ = (maxx - minx) / 100, x, y;
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					double FI1 = x - ev2d.MID_X,
					FI2 = Math.Pow(x, 2) - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X) * (x - ev2d.MID_X) / Math.Pow(ev2d.MQD_X, 2) - ev2d.MID_X_QUAD;
					double Syx = Math.Sqrt((S_rem / N2d) * (1 + Math.Pow(FI1 / ev2d.MQD_X, 2) + Math.Pow(FI2, 2) / ev2d.MID_X_QUAD));
					y = cr.Value * FI2 + br.Value * FI1 + ar.Value;
					corf_ch.Series["interv_min"].Points.AddXY(x, y - t * Syx);
				}
				corf_ch.Series.Add("interv_max");
				corf_ch.Series["interv_max"].Color = Color.Fuchsia;
				corf_ch.Series["interv_max"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["interv_max"].BorderWidth = 2;
				corf_ch.Series["interv_max"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					double FI1 = x - ev2d.MID_X,
					FI2 = Math.Pow(x, 2) - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X) * (x - ev2d.MID_X) / Math.Pow(ev2d.MQD_X, 2) - ev2d.MID_X_QUAD;
					double Syx = Math.Sqrt((S_rem / N2d) * (1 + Math.Pow(FI1 / ev2d.MQD_X, 2) + Math.Pow(FI2, 2) / ev2d.MID_X_QUAD));
					y = cr.Value * FI2 + br.Value * FI1 + ar.Value;
					corf_ch.Series["interv_max"].Points.AddXY(x, y + t * Syx);
					//MessageBox.Show(x + " " + (y + t * S_rem));
				}
			}
			if(regr == Regression.Parabolic && corff.progn_p) {
				double t = crit_ttest(N2d - 2);
				corf_ch.Series.Add("progn_min");
				corf_ch.Series["progn_min"].Color = Color.Green;
				corf_ch.Series["progn_min"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["progn_min"].BorderWidth = 2;
				corf_ch.Series["progn_min"].IsVisibleInLegend = false;
				double step_ = (maxx - minx) / 100, x, y;
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					double FI1 = x - ev2d.MID_X,
					FI2 = Math.Pow(x, 2) - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X) * (x - ev2d.MID_X) / Math.Pow(ev2d.MQD_X, 2) - ev2d.MID_X_QUAD;
					double Syx = Math.Sqrt((S_rem / N2d) * (N2d + 1 + Math.Pow(FI1 / ev2d.MQD_X, 2) + Math.Pow(FI2, 2) / ev2d.MID_X_QUAD));
					y = cr.Value * FI2 + br.Value * FI1 + ar.Value;
					corf_ch.Series["progn_min"].Points.AddXY(x, y - t * Syx);
				}
				corf_ch.Series.Add("progn_max");
				corf_ch.Series["progn_max"].Color = Color.Green;
				corf_ch.Series["progn_max"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["progn_max"].BorderWidth = 2;
				corf_ch.Series["progn_max"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					double FI1 = x - ev2d.MID_X,
					FI2 = Math.Pow(x, 2) - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X) * (x - ev2d.MID_X) / Math.Pow(ev2d.MQD_X, 2) - ev2d.MID_X_QUAD;
					double Syx = Math.Sqrt((S_rem / N2d) * (N2d + 1 + Math.Pow(FI1 / ev2d.MQD_X, 2) + Math.Pow(FI2, 2) / ev2d.MID_X_QUAD));
					y = cr.Value * FI2 + br.Value * FI1 + ar.Value;
					corf_ch.Series["progn_max"].Points.AddXY(x, y + t * Syx);
					//MessageBox.Show(x + " " + (y + t * S_rem));
				}
			}
			if (regr == Regression.Qlinear && corff.regr_q) {
				double[,] Syx = new double[2, 100]; //Math.Sqrt(Math.Pow(S_rem,2)*(1 + 1/N2d) + Math.Pow(Sb, 2)*Math.Pow(x - ev2d.MID_X,2));
				double step_ = (maxx - minx) / 100, a = ar.Value / br.Value, b = 1 / br.Value ;
				//MessageBox.Show(maxx + " " + minx + " " + step_);
				for (int i = 0; i < 100; i++) {
					Syx[0, i] = minx + i * step_;
					Syx[1, i] = b / (Syx[0, i] + a);
					//MessageBox.Show(Syx0[0, i] + " " + Syx0[1, i]);
				}
				corf_ch.Series.Add("regr_q");
				corf_ch.Series["regr_q"].Color = Color.Red;
				corf_ch.Series["regr_q"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["regr_q"].BorderWidth = 2;
				corf_ch.Series["regr_q"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					corf_ch.Series["regr_q"].Points.AddXY(Syx[0, i], Syx[1, i]);
					//MessageBox.Show(Syx[0, i] + " " + Syx[1, i]);
					//MessageBox.Show(Syx0[0, i] + " " + (br * Syx0[0, i] + ar - t * Syx0[1, i]));
				}
			}
			if(regr == Regression.Qlinear && corff.toler_q) {
				double t = crit_ttest(N2d - 2), a = ar.Value / br.Value, b = 1 / br.Value;
				corf_ch.Series.Add("tol_min");
				corf_ch.Series["tol_min"].Color = Color.Blue;
				corf_ch.Series["tol_min"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["tol_min"].BorderWidth = 2;
				corf_ch.Series["tol_min"].IsVisibleInLegend = false;
				double step_ = (maxx - minx) / 100, x, y;
				double sqrt_rem = Math.Sqrt(S_rem);
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					y = b / (x + a);
					corf_ch.Series["tol_min"].Points.AddXY(x, y - t * sqrt_rem);
				}
				corf_ch.Series.Add("tol_max");
				corf_ch.Series["tol_max"].Color = Color.Blue;
				corf_ch.Series["tol_max"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["tol_max"].BorderWidth = 2;
				corf_ch.Series["tol_max"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					y = b / (x + a);
					corf_ch.Series["tol_max"].Points.AddXY(x, y + t * sqrt_rem);
				//	MessageBox.Show(x + " " + (y - t * sqrt_rem));

				}
			}
			if(regr == Regression.Qlinear && corff.interv_q) {
				double t = crit_ttest(N2d - 2), a = ar.Value / br.Value, b = 1 / br.Value;
				corf_ch.Series.Add("interv_min");
				corf_ch.Series["interv_min"].Color = Color.Fuchsia;
				corf_ch.Series["interv_min"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["interv_min"].BorderWidth = 2;
				corf_ch.Series["interv_min"].IsVisibleInLegend = false;
				double step_ = (maxx - minx) / 100, x, y;
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					double Syx = Math.Sqrt(S_rem / N2d + Math.Pow(Sb, 2) * (x - t_mid));
					y = b / (x + a);
					corf_ch.Series["interv_min"].Points.AddXY(x, y - t * Syx);
				}
				corf_ch.Series.Add("interv_max");
				corf_ch.Series["interv_max"].Color = Color.Fuchsia;
				corf_ch.Series["interv_max"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
				corf_ch.Series["interv_max"].BorderWidth = 2;
				corf_ch.Series["interv_max"].IsVisibleInLegend = false;
				for (int i = 0; i < 100; i++) {
					x = minx + i * step_;
					double Syx = Math.Sqrt(S_rem / N2d + Math.Pow(Sb, 2) * (x - t_mid));
					y = b / (x + a);
					corf_ch.Series["interv_max"].Points.AddXY(x, y + t * Syx);
					//MessageBox.Show(x + " " + (y + t * S_rem));
				}
			}
		}

		#endregion

		#region Регрессии
		private void перевіркаУмовToolStripMenuItem_Click(object sender, EventArgs e) {
			double C = 0, L = 0, S = 0;
			double[] S_arr = new double[Mx];
			for (int i = 0; i < Mx; i++)
				C += arr_cl[i].Count;
			C = 1 + (C - 1/N2d) / (3 * (Mx - 1));
			for(int i = 0; i < Mx; i++) {
				double Ymid = 0;
				for (int j = 0; j < arr_cl[i].Count; j++)
					Ymid += arr_cl[i][j];
				Ymid /= arr_cl[i].Count;
				S_arr[i] = 0;
				for (int j = 0; j < arr_cl[i].Count; j++)
					S_arr[i] += Math.Pow(arr_cl[i][j] - Ymid, 2);
				S_arr[i]/= (arr_cl[i].Count - 1);
			}
			for (int i = 0; i < Mx; i++) {
				S += (arr_cl[i].Count - 1) * S_arr[i];
			}
			S /= (N2d - Mx);
			for(int i = 0; i < Mx; i++) {
				L += arr_cl[i].Count * Math.Log(S_arr[i] / S);
			}
			L /= (-C);
			double crit = crit_pirs(Mx - 1);
			if (L <= crit) {
				MessageBox.Show("Λ = " + L + "\nКритичне значення = " + crit + "\nДисперсія є стала.");
			}
			else {
				MessageBox.Show("Λ = " + L + "\nКритичне значення = " + crit + "\nДисперсія не є стала.");
			}
		}

		private void LinearToolStripMenuItem_Click(object sender, EventArgs e) {
			regr = Regression.Linear;
			br = ev2d.COR * ev2d.MQD_Y / ev2d.MQD_X;
			ar = ev2d.MID_Y - br * ev2d.MID_X;
			corf_chs.Checked = true;
			corff.regr_l = true;
			S_rem = ev2d.MQD_Y * Math.Sqrt((1 - Math.Pow(ev2d.COR,2))*(N2d - 1)/(N2d - 2));
			Sa = S_rem * Math.Sqrt(1 / N2d + Math.Pow(ev2d.MID_X, 2) / (Math.Pow(ev2d.MQD_X, 2) * (N2d - 1)));
			Sb = S_rem / (ev2d.MQD_X * Math.Sqrt(N2d - 1));
			double t = crit_ttest(N2d - 2);
			if (EvalTable2D.Rows.Count != 26)
				EvalTable2D.Rows.Add(8);
			EvalTable2D.Rows[18].HeaderCell.Value = "a min";
			EvalTable2D[0, 18].Value = Math.Round(ar.Value - t*Sa, 4);
			EvalTable2D.Rows[19].HeaderCell.Value = "a";
			EvalTable2D[0, 19].Value = Math.Round(ar.Value, 4);
			EvalTable2D.Rows[20].HeaderCell.Value = "a max";
			EvalTable2D[0, 20].Value = Math.Round(ar.Value + t * Sa, 4);
			EvalTable2D.Rows[21].HeaderCell.Value = "b min";
			EvalTable2D[0, 21].Value = Math.Round(br.Value - t * Sb, 4);
			EvalTable2D.Rows[22].HeaderCell.Value = "b";
			EvalTable2D[0, 22].Value = Math.Round(br.Value, 4);
			EvalTable2D.Rows[23].HeaderCell.Value = "b max";
			EvalTable2D[0, 23].Value = Math.Round(br.Value + t * Sb, 4);
			EvalTable2D.Rows[24].HeaderCell.Value = "R^2";
			EvalTable2D[0, 24].Value = Math.Round(Math.Pow(ev2d.COR, 2), 4);
			EvalTable2D.Rows[25].HeaderCell.Value = "(S^2)зал";
			EvalTable2D[0, 25].Value = Math.Round(S_rem, 4);
			MakeCorField();
		}

		private void ParabolicToolStripMenuItem_Click(object sender, EventArgs e) {
			/*	br = ((ev2d.MID_X_FOUR - Math.Pow(ev2d.MID_X_QUAD, 2)) * ev2d.COR * ev2d.MQD_X * ev2d.MQD_Y - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD*ev2d.MID_X) * ev2d.MID_X_QUAD_Y) / (Math.Pow(ev2d.MQD_X,2) * (ev2d.MID_X_FOUR - Math.Pow(ev2d.MID_X_QUAD, 2))*Math.Pow((ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X), 2));
				cr = (Math.Pow(ev2d.MQD_X, 2) * ev2d.MID_X_QUAD_Y - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X) * ev2d.COR * ev2d.MQD_X * ev2d.MQD_Y) / (Math.Pow(ev2d.MQD_X, 2) * (ev2d.MID_X_FOUR - Math.Pow(ev2d.MID_X_QUAD, 2)) * Math.Pow((ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X), 2));
				ar = ev2d.MID_Y - br * ev2d.MID_X - cr * ev2d.MID_X_QUAD;*/
			regr = Regression.Parabolic;
			ar = ev2d.MID_Y;
			br = ev2d.MID1 / Math.Pow(ev2d.MQD_X, 2);
			cr = ev2d.MID_FI2_Y / ev2d.MID_FI2_QUAD;
			corf_chs.Checked = true;
			corff.regr_p = true;
			S_rem = 0;
			for (int i = 0; i < N2d; i++) {
				double FI1 = arr[0][i] - ev2d.MID_X,
					FI2 = Math.Pow(arr[0][i], 2) - (ev2d.MID_X_CUBE - ev2d.MID_X_QUAD * ev2d.MID_X) * (arr[0][i] - ev2d.MID_X) / Math.Pow(ev2d.MQD_X, 2) - ev2d.MID_X_QUAD;
				S_rem += Math.Pow(arr[1][i] - ar.Value - br.Value*FI1 - cr.Value*FI2,2);
			}
			double t = crit_ttest(N2d - 3),
				at = t * S_rem / Math.Sqrt(N2d),
				bt = t * S_rem / (ev2d.MQD_Y * Math.Sqrt(N2d)),
				ct = t * S_rem / Math.Sqrt(N2d * ev2d.MID_FI2_QUAD);
			//MessageBox.Show(S_rem + "");
			if (EvalTable2D.Rows.Count != 29)
				EvalTable2D.Rows.Add(11);
			EvalTable2D.Rows[18].HeaderCell.Value = "a min";
			EvalTable2D[0, 18].Value = Math.Round(ar.Value - at, 4);
			EvalTable2D.Rows[19].HeaderCell.Value = "a";
			EvalTable2D[0, 19].Value = Math.Round(ar.Value, 4);
			EvalTable2D.Rows[20].HeaderCell.Value = "a max";
			EvalTable2D[0, 20].Value = Math.Round(ar.Value + at, 4);
			EvalTable2D.Rows[21].HeaderCell.Value = "b min";
			EvalTable2D[0, 21].Value = Math.Round(br.Value - bt, 4);
			EvalTable2D.Rows[22].HeaderCell.Value = "b";
			EvalTable2D[0, 22].Value = Math.Round(br.Value, 4);
			EvalTable2D.Rows[23].HeaderCell.Value = "b max";
			EvalTable2D[0, 23].Value = Math.Round(br.Value + bt, 4);
			EvalTable2D.Rows[24].HeaderCell.Value = "c min";
			EvalTable2D[0, 24].Value = Math.Round(cr.Value - ct, 4);
			EvalTable2D.Rows[25].HeaderCell.Value = "c";
			EvalTable2D[0, 25].Value = Math.Round(cr.Value, 4);
			EvalTable2D.Rows[26].HeaderCell.Value = "c max";
			EvalTable2D[0, 26].Value = Math.Round(cr.Value + ct, 4);
			EvalTable2D.Rows[27].HeaderCell.Value = "R^2";
			EvalTable2D[0, 27].Value = Math.Round(Math.Pow(ev2d.COR, 2), 4);
			EvalTable2D.Rows[28].HeaderCell.Value = "(S^2)зал";
			EvalTable2D[0, 28].Value = Math.Round(S_rem, 4);
			MakeCorField();
		}

		private void quazilinToolStripMenuItem_Click(object sender, EventArgs e) {
			regr = Regression.Qlinear;
			/*double mid_fi = 0, mid_psi = 0, mid_fi_sqr = 0, mid_fi_psi = 0, sum_w = 0;
			for(int i = 0; i < N2d; i++) {
				sum_w += Math.Pow(arr[1][i], 3) / Math.Pow(arr[0][i], 4);
			}
			for (int i = 0; i < N2d; i++) {
				mid_fi += Math.Pow(arr[1][i], 3) / (Math.Pow(arr[0][i], 5));
				mid_psi += Math.Pow(arr[1][i], 3) * Math.Log(arr[1][i]) / Math.Pow(arr[0][i], 4);
				mid_fi_sqr += Math.Pow(arr[1][i], 3) / (Math.Pow(arr[0][i], 6));
				mid_fi_psi += Math.Pow(arr[1][i], 3) * Math.Log(arr[1][i]) / Math.Pow(arr[0][i], 5);
			}
			mid_fi /= sum_w;
			mid_fi_psi /= sum_w;
			mid_fi_sqr /= sum_w;
			mid_psi /= sum_w;
			br = (mid_fi_psi - mid_fi * mid_psi) / (mid_fi_sqr - Math.Pow(mid_fi,2));
			ar = mid_psi - br.Value * mid_fi;*/
			double[][] new_arr = new double[2][];
			double Sa = 0, Sb = 0;
			new_arr[0] = new double[N2d];
			new_arr[1] = new double[N2d];
			for (int i = 0; i < N2d; i++) {
				new_arr[0][i] = arr[0][i];
				new_arr[1][i] = 1/arr[1][i];
			}
			Eval2D _ev2d = new Eval2D(new_arr);
			br = _ev2d.COR * _ev2d.MQD_Y / _ev2d.MQD_X;
			ar = _ev2d.MID_Y - br.Value * _ev2d.MID_X;
			corf_chs.Checked = true;
			corff.regr_q = true;
			S_rem = 0;
			for (int i = 0; i < N2d; i++) {
				S_rem += Math.Pow(new_arr[1][i] - ar.Value - br.Value * new_arr[0][i], 2);
			}
			S_rem /= N2d - 2;
			Sa = Math.Sqrt(S_rem) * Math.Sqrt(1 / N2d + Math.Pow(_ev2d.MID_X, 2) / (Math.Pow(_ev2d.MQD_X, 2) * (N2d - 1)));
			Sb = Math.Sqrt(S_rem) / (_ev2d.MQD_X * Math.Sqrt(N2d - 1));
			t_mid = _ev2d.MID_X;
			double t = crit_ttest(N2d - 2);
			if (EvalTable2D.Rows.Count != 26)
				EvalTable2D.Rows.Add(8);
			EvalTable2D.Rows[18].HeaderCell.Value = "a min";
			EvalTable2D[0, 18].Value = Math.Round(ar.Value - t * Sa, 4);
			EvalTable2D.Rows[19].HeaderCell.Value = "a";
			EvalTable2D[0, 19].Value = Math.Round(ar.Value, 4);
			EvalTable2D.Rows[20].HeaderCell.Value = "a max";
			EvalTable2D[0, 20].Value = Math.Round(ar.Value + t * Sa, 4);
			EvalTable2D.Rows[21].HeaderCell.Value = "b min";
			EvalTable2D[0, 21].Value = Math.Round(br.Value - t * Sb, 4);
			EvalTable2D.Rows[22].HeaderCell.Value = "b";
			EvalTable2D[0, 22].Value = Math.Round(br.Value, 4);
			EvalTable2D.Rows[23].HeaderCell.Value = "b max";
			EvalTable2D[0, 23].Value = Math.Round(br.Value + t * Sb, 4);
			EvalTable2D.Rows[24].HeaderCell.Value = "R^2";
			EvalTable2D[0, 24].Value = Math.Round(Math.Pow(ev2d.COR_RATIO, 2), 4);
			EvalTable2D.Rows[25].HeaderCell.Value = "(S^2)зал";
			EvalTable2D[0, 25].Value = Math.Round(S_rem, 4);
			MakeCorField();
		}

		private void ttestLinearRegression(object sender, EventArgs e) {
			if (ar != null && br!= null) {
				bool res = true;
				double crit = crit_ttest(N2d - 2);
				double[] par = new double[2];
				new TtestForm(3, par, ref res).ShowDialog();
				if (res) {
					double t1 = (double)(ar - par[0])/Sa,
						t2 = (double)(br - par[1]) / Sa;
					//MessageBox.Show(par[0] + " " + ar + " " + Sa);
					string s1 = Math.Abs(t1) <= crit ? "пройдений" : "не пройдений", s2 = Math.Abs(t2) <= crit ? "пройдений" : "не пройдений" ;
					MessageBox.Show("t(a) = " + t1 + "\nКритичне значення " + crit + " t-тест " + s1);
					MessageBox.Show("t(b) = " + t2 + "\nКритичне значення " + crit + " t-тест " + s2);
				}
			}
		}

		private void ttestParabolicRegression(object sender, EventArgs e) {
			if (ar != null && br != null) {
				bool res = true;
				double crit = crit_ttest(N2d - 3);
				double[] par = new double[3];
				new TtestForm(4, par, ref res).ShowDialog();
				if (res) {
					double t1 = (double)(ar - par[0]) * Math.Sqrt(N2d / S_rem),
						t2 = (double)(br - par[1]) * Math.Sqrt(N2d / S_rem),
						t3 = (double)(cr - par[2]) * Math.Sqrt(N2d / S_rem);
					//MessageBox.Show(par[0] + " " + ar + " " + Sa);
					string s1 = Math.Abs(t1) <= crit ? "пройдений" : "не пройдений", s2 = Math.Abs(t2) <= crit ? "пройдений" : "не пройдений", s3 = Math.Abs(t3) <= crit ? "пройдений" : "не пройдений";
					MessageBox.Show("t(a) = " + t1 + "\nКритичне значення " + crit + " t-тест " + s1);
					MessageBox.Show("t(b) = " + t2 + "\nКритичне значення " + crit + " t-тест " + s2);
					MessageBox.Show("t(c) = " + t3 + "\nКритичне значення " + crit + " t-тест " + s3);
				}
			}
		}
		#endregion

		#endregion

		#region Многомерный анализ

		#region Переменные
		Dictionary<string, double[][]> arrsNd = null;
		double v_countNd = 0;
		static public int Nnd, dim;
		EvalND evnd;
		List<int> Cpart = null;
		#endregion

		#region Контекстные меню

		#region TreeNDCM

		private void TreeND_ReadFile_Click(object sender, EventArgs e) {
			if (arrsNd == null)
				arrsNd = new Dictionary<string, double[][]>();
			OpenFileDialog od = new OpenFileDialog();
			if (od.ShowDialog() == DialogResult.OK) {
				StreamReader sr = new StreamReader(od.FileName);
				List<string[]> txtData = new List<string[]>();
				string str = "";
				while ((str = sr.ReadLine())!=null) {
					txtData.Add(str.Replace('.', ',').Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
				}
				double[][] arr = new double[txtData.Count][];
				for (int i = 0; i < txtData.Count; i++) {
					arr[i] = new double[txtData[i].Length];
				}
				for (int j = 0; j < txtData[0].Length; j++) {
					double[] temp = new double[txtData.Count];
					bool err = false;
					for(int i = 0; i < txtData.Count; i++) {
						if(!double.TryParse(txtData[i][j], out temp[i])) {
							err = true;
							break;
						}
					}
					if (!err) {
						for (int i = 0; i < txtData.Count; i++) {
							arr[i][j] = temp[i];
						}
					}
				}
				//Array.Sort(arr, new VarRowComp());
				arrsNd.Add("V" + v_countNd, arr);
				TreeND.Nodes.Add("V" + v_countNd, "V" + v_countNd);
				N_tree.Add("V" + v_countNd, Nnd);
				v_countNd++;
			}
		}

		private void TreeND_Generate_Click(object sender, EventArgs e) {
			if (arrsNd == null)
				arrsNd = new Dictionary<string, double[][]>();
			List<double[]> temp_arr = new List<double[]>();
			new GenerateMultiDimForm(temp_arr).ShowDialog();
			dim = temp_arr[0].Length;
			Nnd = temp_arr.Count;
			arr = new double[dim][];
			for (int i = 0; i < dim; i++) {
				arr[i] = new double[Nnd];
				for (int j = 0; j < Nnd; j++) {
					arr[i][j] = temp_arr[j][i];
				}
			}
			/*for(int i = 0; i < dim; i++) {
				str = "";
				for(int j = 0; j < Nnd; j++) {
					str += arr[i][j] + " ";
				}
				MessageBox.Show(str);
			}*/
			arrsNd.Add("V" + v_countNd, arr);
			TreeND.Nodes.Add("V" + v_countNd, "V" + v_countNd);
			N_tree.Add("V" + v_countNd, Nnd);
			v_countNd++;
		}

		private void TreeND_Select_Click(object sender, EventArgs e) {
			if (TreeND.SelectedNode != null) {
				string name = TreeND.SelectedNode.Text;
				arr = arrsNd[name];
				minx = double.MaxValue;
				miny = double.MaxValue;
				maxx = double.MinValue;
				maxy = double.MinValue;
				maxp = double.MinValue;
				dim = arr.Length;
				Nnd = arr[0].Length;
				for (int i = 0; i < Nnd; i++) {
					//MessageBox.Show(minx + " " + miny + "\n" + maxx + " " + maxy);
					if (arr[0][i] < minx)
						minx = arr[0][i];
					if (arr[1][i] < miny)
						miny = arr[1][i];
					if (arr[0][i] > maxx)
						maxx = arr[0][i];
					if (arr[1][i] > maxy)
						maxy = arr[1][i];
				}
				regr = Regression.None;
				evnd = new EvalND(arr);
				DCtab.Rows.Clear();
				DCtab.Columns.Clear();
				for (int i = 0; i < dim; i++) {
					DCtab.Columns.Add("", "");
					DCtab.Columns[i].Width = 50;
				}
				DCtab.Rows.Add(dim);
				for (int i = 0; i < dim; i++) {
					for (int j = 0; j < dim; j++) {
						DCtab[i, j].Value = Math.Round(evnd.DCmat[i, j], 4);
					}
				}
				part_cor_c.Items.Clear();
				part_cor_i.Items.Clear();
				part_cor_j.Items.Clear();
				many_cor_i.Items.Clear();
				pair_cor_i.Items.Clear();
				pair_cor_j.Items.Clear();
				regr_i.Items.Clear();
				Cpart = null;
				for (int i = 0; i < arr.Length; i++) {
					part_cor_c.Items.Add(i);
					part_cor_i.Items.Add(i);
					part_cor_j.Items.Add(i);
					many_cor_i.Items.Add(i);
					pair_cor_i.Items.Add(i);
					pair_cor_j.Items.Add(i);
					regr_i.Items.Add(i);
				}
				ir = -1;
			}
		}
		private void TreeND_Standartize_Click(object sender, EventArgs e) {
			/*if (arr == null)
				return;
			for(int i = 0; i < arr.Length; i++) {
				for(int j = 0; j < arr[i].Length; j++) {
					arr[i][j] = (arr[i][j] + evnd.MID[i]) / evnd.MQD[i];
				}
			}
			arrsNd.Add("V" + v_countNd, arr);
			TreeND.Nodes.Add("V" + v_countNd, "V" + v_countNd);
			v_countNd++;*/
			double[] A = new double[] { 1, 2, 3 };
			MultiDimArray md = new MultiDimArray(new int[] { 3, 3, 3 });
			md.SetVal(new int[] { 1, 1, 1 }, A);
			double[] B = md.GetVal(new int[] { 1, 1, 1 });
			MessageBox.Show(B[1] + "");

		}


		private void var_del_btn_Click(object sender, EventArgs e) {
			int del;
			if(!int.TryParse(var_del.Text, out del)) {
				return;
			}
			if(del >= dim || del < 0) {
				return;
			}
			double[][] new_arr = new double[dim - 1][];
			for(int i = 0; i < del; i++) {
				new_arr[i] = new double[Nnd]; 
				for(int j = 0; j < Nnd; j++) {
					new_arr[i][j] = arr[i][j];
				}
			}
			for (int i = del; i < dim-1; i++) {
				//MessageBox.Show(i + "");
				new_arr[i] = new double[Nnd];
				for (int j = 0; j < Nnd; j++) {
					new_arr[i][j] = arr[i+1][j];
				}
			}
			arrsNd.Add("V" + v_countNd, new_arr);
			TreeND.Nodes.Add("V" + v_countNd, "V" + v_countNd);
			N_tree.Add("V" + v_countNd, Nnd);
			v_countNd++;
		}


		private void TreeND_SelectionAnalysis_Click(object sender, EventArgs e) {
			new CheckSelParamForm(arrsNd, N_tree).ShowDialog();
		}

		private void TreeND_Delete_Click(object sender, EventArgs e) {
			if (TreeND.SelectedNode != null) {
				string name = TreeND.SelectedNode.Text;
				TreeND.Nodes.Remove(TreeND.SelectedNode);
				arrsNd.Remove(name);
			}
		}

		#endregion

		#endregion

		#region Кореляционный анализ

		private void part_cor_add_Click(object sender, EventArgs e) {
			if (arr == null || part_cor_c.Text == "")
				return;
			int num = int.Parse(part_cor_c.Text);
			if (Cpart == null)
				Cpart = new List<int>();
			if (Cpart.IndexOf(num) == -1)
				Cpart.Add(num);
		}

		private void part_cor_clear_Click(object sender, EventArgs e) {
			Cpart = null;
		}


		private void pair_cor_count_Click(object sender, EventArgs e) {
			if(pair_cor_i.Text == "" || pair_cor_j.Text == "") {
				return;
			}
			double mid_ij = 0, cor;
			int i = int.Parse(pair_cor_i.Text), j = int.Parse(pair_cor_j.Text);
			for (int k = 0; k < Nnd; k++)
				mid_ij += arr[i][k] * arr[j][k];
			mid_ij /= Nnd;
			cor = Nnd * (mid_ij - evnd.MID[i] * evnd.MID[j]) / (evnd.MQD[i] * evnd.MQD[j] * (Nnd - 1));
			pair_cor.Text = cor.ToString();
		}


		private void part_cor_count_Click(object sender, EventArgs e) {
			if (part_cor_i.Text == "" || part_cor_j.Text == "")
				return;
			int i = int.Parse(part_cor_i.Text), j = int.Parse(part_cor_j.Text);
			if (Cpart == null || Cpart.IndexOf(i) != -1 || Cpart.IndexOf(j) != -1 || i == j) {
				MessageBox.Show("Невірно задані вибірки");
				return;
			}
			double partcor = PartCor(i, j, Cpart);
			part_cor.Text = partcor.ToString();
			double T = partcor * Math.Sqrt(Nnd - Cpart.Count - 2) / Math.Sqrt(1 - Math.Pow(partcor, 2)),
				crit = crit_ttest(Nnd - Cpart.Count - 2);
			part_cor_t.Text = T.ToString();
			part_cor_tcrit.Text = crit.ToString();
			double v1 = 0.5 * Math.Log((1 + partcor) / (1 - partcor)) - 1.645 / (Nnd - Cpart.Count - 3),
				v2 = 0.5 * Math.Log((1 + partcor) / (1 - partcor)) + 1.645 / (Nnd - Cpart.Count - 3),
				low = (Math.Exp(2 * v1) - 1) / (Math.Exp(2 * v1) + 1),
				up = (Math.Exp(2 * v2) - 1) / (Math.Exp(2 * v2) + 1);
			tup.Text = up.ToString();
			tlow.Text = low.ToString();
		}

		double PartCor(int i, int j, List<int> C) {
			if(C.Count == 1) {
				int d = C[0];
				double res, mid_i = evnd.MID[i], mid_j = evnd.MID[j], mid_d = evnd.MID[d],
					mqd_i = evnd.MQD[j], mqd_j = evnd.MQD[j], mqd_d = evnd.MQD[d],
					mid_ij = 0, mid_id = 0, mid_jd = 0,
					rij, rid, rjd;
				for(int k = 0; k < Nnd; k++) {
					mid_ij += arr[i][k] * arr[j][k];
					mid_id += arr[i][k] * arr[d][k];
					mid_jd += arr[j][k] * arr[d][k];
				}
				mid_ij /= Nnd;
				mid_id /= Nnd;
				mid_jd /= Nnd;
				rij = Nnd * (mid_ij - mid_i * mid_j) / ((Nnd - 1) * mqd_i * mqd_j);
				rid = Nnd * (mid_id - mid_i * mid_d) / ((Nnd - 1) * mqd_i * mqd_d);
				rjd = Nnd * (mid_jd - mid_j * mid_d) / ((Nnd - 1) * mqd_j * mqd_d);
				res = (rij - rid * rjd) / Math.Sqrt((1 - Math.Pow(rid, 2)) * (1 - Math.Pow(rjd, 2)));
				return res;
			}
			else {
				int d = C[0];
				List<int> Cnew = new List<int>();
				for(int k = 1; k < C.Count; k++) {
					Cnew.Add(C[k]);
				}
				double rij, rid, rjd, res;
				rij = PartCor(i, j, Cnew);
				rid = PartCor(i, d, Cnew);
				rjd = PartCor(j, d, Cnew);
				res = (rij - rid * rjd) / Math.Sqrt((1 - Math.Pow(rid, 2)) * (1 - Math.Pow(rjd, 2)));
				return res;
			}
		}


		private void many_cor_count_Click(object sender, EventArgs e) {
			if(many_cor_i.Text == "") {
				MessageBox.Show("Невірно задані вибірки");
				return;
			}
			int k = int.Parse(many_cor_i.Text);
			double manycor = ManyCor(k);
			many_cor.Text = manycor.ToString();
			double f = (Nnd - dim - 1) * Math.Pow(manycor, 2) / (dim * (1 - Math.Pow(manycor, 2))),
				crit = crit_fisher(dim, Nnd - dim - 1);
			many_cor_f.Text = f.ToString();
			many_cor_fcrit.Text = crit.ToString();
		}


		#endregion

		#region Визуализация


		private void DiagMatrixMenuItem_Click(object sender, EventArgs e) {
			if (arr == null || Nnd == 0)
				return;
			new DrawMultiDimForm(0).ShowDialog();
		}


		private void BubblesMenuItem_Click(object sender, EventArgs e) {
			if (arr == null || Nnd == 0)
				return;
			if(dim != 3) {
				MessageBox.Show("Бульбашкова діаграма тільки для 3-вимірних даних");
				return;
			}
			new DrawMultiDimForm(1).ShowDialog();
		}

		private void ParalelCoordsMenuItem_Click(object sender, EventArgs e) {
			if (arr == null || Nnd == 0)
				return;
			new DrawMultiDimForm(2).ShowDialog();
		}

		#endregion

		#region Регрессии
		static public int ir = -1;
		static public double DetCoef, a0;
		public static double[][] Ckk, x_mult, y_mult, Ckk_;
		static public double[] a_mult;
		private void regr_count_Click(object sender, EventArgs e) {
			if (regr_i.Text == "") {
				return;
			}
			ir = int.Parse(regr_i.Text);
			x_mult = new double[dim - 1][];
			y_mult = Matrix.Transposition(new double[][] { arr[ir] });
			double[][] x_ = new double[x_mult.Length][], y_ = new double[y_mult.Length][], x_t, x_mult_t;
			for (int i = 0; i < ir; i++ ) {
				x_mult[i] = new double[Nnd];
				x_[i] = new double[Nnd];
				for (int j = 0; j < Nnd; j++) {
					x_mult[i][j] = arr[i][j];
					x_[i][j] = arr[i][j] - evnd.MID[i];
				}
			}
			for (int i = ir; i < dim-1; i++) {
				x_mult[i] = new double[Nnd];
				x_[i] = new double[Nnd];
				for (int j = 0; j < Nnd; j++) {
					x_mult[i][j] = arr[i+1][j];
					x_[i][j] = arr[i+1][j] - evnd.MID[i];
				}
			}
			for (int i = 0; i < Nnd; i++) {
				y_[i] = new double[1];
				y_[i][0] = y_mult[i][0] - evnd.MID[ir];
			}
			x_mult_t = x_mult;
			string str = "";
			/*for (int i = 0; i < x_mult.Length; i++) {
				for (int j = 0; j < x_mult[i].Length; j++) {
					str += x_mult[i][j] + " ";
				}
				str += "\n";
			}
			MessageBox.Show(str);*/
			x_mult = Matrix.Transposition(x_mult);
			x_t = x_;
			x_ = Matrix.Transposition(x_t);
			
			/*str = "";
			for (int i = 0; i < y_.Length; i++) {
				for (int j = 0; j < y_[i].Length; j++) {
					str += y_[i][j] + " ";
				}
				str += "\n";
			}
			MessageBox.Show(str);*/
			double[][] t1 = Matrix.Invert(Matrix.Multiply(x_t, x_)), t2 = Matrix.Multiply(x_t, y_);
			a_mult = Matrix.Transposition(Matrix.Multiply(t1, t2))[0];
			a0 = evnd.MID[ir];
			for (int i = 0, i_ = 0; i < dim; i++, i_++) {
				if (i_ == ir) {
					i_--;
					continue;
				}
				a_check.Items.Add(i + 1);
				a0 -= evnd.MID[i] * a_mult[i_];
			}
			DetCoef = Math.Pow(ManyCor(ir), 2);
			R_.Text = DetCoef.ToString();
			a0_.Text = a0.ToString();
			Ckk = Matrix.Invert(Matrix.Multiply(x_mult_t, x_mult));
			Ckk_ = Matrix.Invert(Matrix.Multiply(arr, Matrix.Transposition(arr)));
		/*	string str = "";
			for (int i = 0; i < Ckk_.Length; i++) {
				for (int j = 0; j < Ckk_[i].Length; j++) {
					str += Ckk_[i][j] + " ";
				}
				str += "\n";
			}
			MessageBox.Show(str);*/
			double[] a_min = new double[a_mult.Length], a_max = new double[a_mult.Length];
			double t = crit_ttest(Nnd - dim);
			S_rem = (Nnd - 1) * Math.Pow(evnd.MQD[ir],2)*(1-DetCoef)/(Nnd - dim);
			S_rem_.Text = S_rem.ToString();
			if (CoefTab.Columns.Count == 0) {
				CoefTab.Columns.Add("min", "min");
				CoefTab.Columns[0].Width = 50;
				CoefTab.Columns.Add("a", "a");
				CoefTab.Columns[1].Width = 50;
				CoefTab.Columns.Add("max", "max");
				CoefTab.Columns[2].Width = 50;
				CoefTab.Rows.Add(a_mult.Length);
			}
			for (int k = 0; k < a_mult.Length; k++) {
				a_max[k] = a_mult[k] + t * Math.Sqrt(S_rem * Math.Sqrt(Math.Abs(Ckk[k][k])));
				a_min[k] = a_mult[k] - t * Math.Sqrt(S_rem * Math.Sqrt(Math.Abs(Ckk[k][k])));
				CoefTab.Rows[k].HeaderCell.Value = "a" + (k + 1);
				CoefTab[0, k].Value = Math.Round(a_min[k], 4);
				CoefTab[1, k].Value = Math.Round(a_mult[k], 4);
				CoefTab[2, k].Value = Math.Round(a_max[k], 4);
			}
			
		}

		private void regr_check_Click(object sender, EventArgs e) {
			if(ir == -1) {
				return;
			}
			double f = (Nnd - dim - 1) * (1 / (1 - Math.Pow(DetCoef, 2)) - 1) / dim,
				crit = crit_fisher(dim, Nnd - dim - 1);
			string s = f > crit ? "пройдений" : "не пройдений";
			MessageBox.Show("f = " + Math.Round(f, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій " + s);
		}


		private void check_coef_regr_Click(object sender, EventArgs e) {
			if(a_check.Text == "") {
				return;
			}
			int n = int.Parse(a_check.Text)-1;
			double a = a_mult[n], t = a/(S_rem * Math.Sqrt(Ckk_[n][n])), crit = crit_ttest(Nnd - dim);
			string s = Math.Abs(t) <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("t = " + Math.Round(t, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій " + s);
		}


		private void emp_line_check_Click(object sender, EventArgs e) {
			double xa = Matrix.Multiply(x_mult, Matrix.Transposition(new double[][] { a_mult }))[0][0],
				xcx = Matrix.Multiply(x_mult, Matrix.Multiply(Ckk, Matrix.Transposition(x_mult)))[0][0];
			double t = xa - evnd.MID[ir], crit = crit_ttest(Nnd - dim);
			string s = Math.Abs(t) <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("t = " + Math.Round(t, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій " + s);
		}


		private void DiagDiagramMenuItem_Click(object sender, EventArgs e) {
			new DrawMultiDimForm(3).ShowDialog();
		}

		#endregion

		#endregion

		#region Численные функции
		double f1(double k) {
			return (Math.Pow(k, 2)) - 0.5 * (1 - Math.Pow(-1, k));
		}

		double f2(double k) {
			return 5 * Math.Pow(k, 2) + 22 - 7.5 * (1 - Math.Pow(-1, k));
		}

		double K(double z) {
			double res = 0;
			for (int k = 1; k < 5; k++) {
				res += Math.Pow(-1, k) * Math.Exp(-2 * Math.Pow(k * z, 2));// * (1 - 2 * Math.Pow(k, 2) * z / (3 * Math.Sqrt(N)) - ((f1(k) - 4 * (f1(k) + 3)) * Math.Pow(k * z, 2) + 8 * Math.Pow(k * z, 4)) / (18 * N) + Math.Pow(k, 2) * z * (Math.Pow(f2(k), 2) / 5 - 4 * (f2(k) + 45) * Math.Pow(k * z, 2) / 15 + 8 * Math.Pow(k * z, 4)) * 27 * Math.Pow(N, 1.5));
			}
			//	MessageBox.Show(res + " "+z);
			res = 1 + 2 * res;
			return res;
		}

		double Hi_quad() {
			double res = 0;
			for (int i = 0; i < var_row_cl.Count; i++) {
				double n0 = F(var_row_cl[i][0] + step) - F(var_row_cl[i][0]);
				res += Math.Pow(var_row_cl[i][1] - n0, 2) / n0;
			}
			return res;
		}

		double exp_f(double x) {
			double res = lm * Math.Exp(-lm * x);
			if (!double.IsNaN(res) && !double.IsInfinity(res))
				return x < 0 ? 0 : res;
			else
				return 0;
		}

		double exp_F(double x) {
			double res = 1 - Math.Exp(-lm * x);
			if (!double.IsNaN(res) && !double.IsInfinity(res))
				return x < 0 ? 0 : res;
			else
				return 0;
		}

		double exp_mqd(double x) {
			double res = Math.Sqrt(Math.Pow(x, 2) * Math.Exp(-2 * lm * x) * Math.Pow(lm, 2) / N);
			if (!double.IsNaN(res) && !double.IsInfinity(res))
				return res;
			else
				return 0;
		}

		double arcsin_f(double x) {
			double res = 1 / (Math.PI * Math.Sqrt(Math.Pow(m, 2) - Math.Pow(x, 2)));
			if (!double.IsNaN(res) && !double.IsInfinity(res)) {
				return Math.Abs(x) < m ? res : 0;
			}
			else if (double.IsInfinity(res))
				return 1;
			else
				return 0;
		}

		double arcsin_F(double x) {
			double res = 0.5 + Math.Asin(x / m) / Math.PI;
			if (!double.IsNaN(res) && !double.IsInfinity(res)) {
				return Math.Abs(x) < m ? res : 0;
			}
			else if (double.IsInfinity(res))
				return 1;
			else
				return 0;
		}

		double arcsin_mqd(double x) {
			double res = Math.Abs(-x / (Math.PI * m * Math.Sqrt(Math.Pow(m, 2) - Math.Pow(x, 2)))) * Math.Pow(m, 2) / Math.Sqrt(8 * N);
			if (!double.IsNaN(res) && !double.IsInfinity(res))
				return res;
			else if (double.IsInfinity(res))
				return 1;
			else
				return 0;
		}



		double norm_f(double x) {
			double res = Math.Exp(-Math.Pow(x - m, 2) / (2 * Math.Pow(sig, 2))) / (sig * Math.Sqrt(2 * Math.PI));
			if (!double.IsNaN(res) && !double.IsInfinity(res))
				return res;
			else
				return 0;
		}

		double norm_F(double x) {
			double u = (x - m) / sig;
			double
				t = u >= 0 ? 1 / (1 + 0.2316419 * u) : 1 / (1 - 0.2316419 * u),
				b1 = 0.31938153,
				b2 = -0.356563782,
				b3 = 1.781477937,
				b4 = -1.821255978,
				b5 = 1.330274429;
			double res = 1 - Math.Exp(-Math.Pow(u, 2) / 2) * (Math.Pow(t, 5) * b5 + Math.Pow(t, 4) * b4 + Math.Pow(t, 3) * b3 + Math.Pow(t, 2) * b2 + t * b1) / Math.Sqrt(2 * Math.PI);
			if (!double.IsNaN(res) && !double.IsInfinity(res))
				return u >= 0 ? res : 1 - res;
			else
				return 0;
		}

		double norm_mqd(double x) {
			double res = Math.Sqrt(Math.Exp(-Math.Pow((x - m) / sig, 2)) * (2 * Math.Pow(sig, 2) + Math.Pow(x - m, 2)) / (4 * Math.PI * N * Math.Pow(sig, 2)));
			if (!double.IsNaN(res) && !double.IsInfinity(res))
				return res;
			else
				return 0;
		}

		public static double crit_pirs(int Nu) {
			double crit;
			if (Nu >= 90)
				crit = 113.3;
			else if (Nu <= 90 && Nu > 80)
				crit = 101.9;
			else if (Nu <= 80 && Nu > 70)
				crit = 90.5;
			else if (Nu <= 70 && Nu > 60)
				crit = 79.1;
			else if (Nu <= 60 && Nu > 50)
				crit = 67.5;
			else if (Nu <= 50 && Nu > 40)
				crit = 55.6;
			else if (Nu <= 40 && Nu > 30)
				crit = 43.8;
			else {
				double[] temp = new double[] { 3.84,5.99,7.81,9.49,11.1,12.6,14.1,15.5,16.9,18.3,19.7,21.0,22.4,23.7,25.0,26.3,27.6,28.9,30.1,31.4,32.7,33.9,35.2,36.4,37.7,38.9,40.1,41.3,42.6,43.8 };
				crit = temp[Nu - 1];
			}
			return crit;
		}
		public static double crit_ttest(int Nu) {
			double crit;
			if (Nu >= 120)
				crit = 1.96;
			else if (Nu <= 120 && Nu > 60)
				crit = 1.98;
			else if (Nu <= 60 && Nu > 40)
				crit = 2;
			else if (Nu <= 40 && Nu > 30)
				crit = 2.02;
			else {
				double[] temp = new double[] { 12.7, 4.30, 3.18, 2.78, 2.57, 2.45, 2.36, 2.31, 2.26, 2.23, 2.20, 2.18, 2.16, 2.14, 2.13, 2.12, 2.11, 2.10, 2.09, 2.09, 2.08, 2.07, 2.07, 2.06, 2.06, 2.06, 2.05, 2.05, 2.05, 2.04 };
				crit = temp[Nu - 1];
			}
			return crit;
		}

		public static double crit_fisher(int Nu1, int Nu2) {
			double[][] f = new double[][] {
				new double[] { 161.4, 18.51, 10.13, 7.71, 6.61, 5.99, 5.59, 5.32, 5.12, 4.96, 4.84, 4.75, 4.67, 4.60, 4.54, 4.49, 4.45, 4.41, 4.38, 4.35, 4.32, 4.30, 4.28, 4.26, 4.24, 4.23, 4.21, 4.20, 4.18, 4.17, 4.08, 4.00, 3.92, 3.84 } ,
				new double[] {199.5,19.00,9.55,6.94,5.79,5.14,4.74,4.46,4.26,4.10,3.98,3.89,3.81,3.74,3.68,3.63,3.59,3.55,3.52,3.49,3.47,3.44,3.42,3.40,3.39,3.37,3.35,3.34,3.33,3.32,3.23,3.15,3.07,3,00 },
				new double[] {215.7,19.16,9.28,6.59,5.41,4.76,4.35,4.07,3.86,3.71,3.59,3.49,3.41,3.34,3.29,3.24,3.20,3.16,3.13,3.10,3.07,3.05,3.03,3.01,2.99,2.98,2.96,2.95,2.93,2.92,2.84,2.76,2.68,2.60},
				new double[] {224.6,19.25,9.12,6.39,5.19,4.53,4.12,3.84,3.63,3.48,3.36,3.26,3.18,3.11,3.06,3.01,2.96,2.93,2.90,2.87,2.84,2.82,2.80,2.78,2.76,2.74,2.73,2.71,2.70,2.69,2.61,2.53,2.45,2.37},
				new double[] {230.2,19.3,9.01,6.26,5.05,4.39,3.97,3.69,3.48,3.33,3.20,3.11,3.03,2.96,2.90,2.85,2.81,2.77,2.74,2.71,2.68,2.66,2.64,2.62,2.60,2.59,2.57,2.56,2.55,2.53,2.45,2.37,2.29,2.21},
				new double[] {234.0,19.33,8.94,6.16,4.95,4.28,3.87,3.58,3.37,3.22,3.09,3.00,2.92,2.85,2.79,2.74,2.70,2.66,2.63,2.60,2.57,2.55,2.53,2.51,2.49,2.47,2.46,2.45,2.43,2.42,2.34,2.25,2.17,2.10},
				new double[] {236.8,19.35,8.89,6.09,4.88,4.21,3.79,3.50,3.29,3.14,3.01,2.91,2.83,2.76,2.71,2.66,2.61,2.58,2.54,2.51,2.49,2.46,2.44,2.42,2.40,2.39,2.37,2.36,2.35,2.33,2.25,2.17,2.09,2.01},
				new double[] {238.9,19.37,8.85,6.04,4.82,4.15,3.73,3.44,3.23,3.07,2.95,2.85,2.77,2.70,2.64,2.59,2.55,2.51,2.48,2.45,2.42,2.40,2.37,2.36,2.34,2.32,2.31,2.29,2.28,2.27,2.18,2.10,2.02,1.94},
				new double[] {240.5,19.38,8.81,6.00,4.77,4.10,3.68,3.39,3.18,3.02,2.90,2.80,2.71,2.65,2.59,2.54,2.49,2.46,2.42,2.39,2.37,2.34,2.32,2.30,2.28,2.27,2.25,2.24,2.22,2.21,2.12,2.04,1.96,1.88},
				new double[] {241.9,19.40,8.79,5.96,4.74,4.06,3.64,3.35,3.14,2.98,2.85,2.75,2.67,2.60,2.54,2.49,2.45,2.41,2.38,2.35,2.32,2.30,2.27,2.25,2.24,2.22,2.20,2.19,2.18,2.16,2.08,1.99,1.91,1.83},
				new double[] {243.9,19.41,8.78,5.91,4.68,4.00,3.57,3.28,3.07,2.91,2.79,2.69,2.60,2.53,2.48,2.42,2.38,2.34,2.31,2.28,2.25,2.23,2.20,2.18,2.16,2.15,2.13,2.12,2.10,2.09,2.00,1.92,1.83,1.75},
				new double[] {245.9,19.43,8.70,5.86,4.62,3.94,3.51,3.22,3.01,2.85,2.72,2.62,2.53,2.46,2.40,2.35,2.31,2.27,2.23,2.20,2.18,2.15,2.13,2.11,2.09,2.07,2.06,2.04,2.03,2.01,1.92,1.84,1.75,1.67},
				new double[] {248.0,19.45,8.66,5.80,4.56,3.87,3.44,3.15,2.94,2.77,2.65,2.54,2.46,2.39,2.33,2.28,2.23,2.19,2.16,2.12,2.10,2.07,2.05,2.03,2.01,1.99,1.97,1.96,1.94,1.93,1.84,1.75,1.66,1.57},
				new double[] {249.1,19.45,8.64,5.77,4.53,3.84,3.41,3.12,2.90,2.74,2.61,2.51,2.42,2.35,2.29,2.24,2.19,2.15,2.11,2.08,2.05,2.03,2.01,1.98,1.96,1.95,1.93,1.91,1.90,1.89,1.79,1.70,1.61,1.52},
				new double[] {250.1,19.46,8.62,5.75,4.50,3.81,3.38,3.08,2.86,2.70,2.57,2.47,2.38,2.31,2.25,2.19,2.15,2.11,2.07,2.04,2.01,1.98,1.96,1.94,1.92,1.90,1.88,1.87,1.85,1.84,1.74,1.65,1.55,1.46},
				new double[] {251.1,19.47,8.59,5.72,4.46,3.77,3.34,3.04,2.83,2.66,2.53,2.43,2.34,2.27,2.20,2.15,2.10,2.06,2.03,1.99,1.96,1.94,1.91,1.89,1.87,1.85,1.84,1.82,1.81,1.79,1.69,1.59,1.50,1.39},
				new double[] {252.2,19.48,8.57,5.69,4.43,3.74,3.30,3.01,2.79,2.62,2.49,2.38,2.30,2.22,2.16,2.11,2.06,2.02,1.98,1.95,1.92,1.89,1.86,1.84,1.82,1.80,1.79,1.77,1.75,1.74,1.64,1.53,1.43,1.32},
				new double[] {253.3,19.49,8.55,5.66,4.40,3.70,3.27,2.97,2.75,2.58,2.45,2.34,2.25,2.18,2.11,2.06,2.01,1.97,1.93,1.90,1.87,1.84,1.81,1.79,1.77,1.75,1.73,1.71,1.70,1.68,1.58,1.47,1.35,1.22},
				new double[] {254.3,19.50,8.53,5.63,4.36,3.67,3.23,2.93,2.71,2.54,2.40,2.30,2.21,2.13,2.07,2.01,1.96,1.92,1.88,1.84,1.81,1.78,1.76,1.73,1.71,1.69,1.67,1.65,1.64,1.62,1.51,1.39,1.25,1.00}
			};
			double[] lim = new double[19];
			lim[18] = double.PositiveInfinity;
			lim[17] = 120;
			lim[16] = 60;
			lim[15] = 40;
			lim[14] = 30;
			lim[13] = 24;
			lim[12] = 20;
			lim[11] = 15;
			lim[10] = 12;
			for (int i = 0; i < 10; i++) {
				lim[i] = i + 1;
			}
			int indR = -1;
			for (int i = 0; i < 33; i++) {
				if (Nu2 >= lim[i] && Nu2 < lim[i + 1]) {
					indR = i;
					break;
				}
			}
			int indC = crit_fisher2(Nu2, f[indR]);
			return f[indR][indC];
		}
		private static int crit_fisher2(int Nu2, double[] f) {
			double[] lim = new double[34];
			lim[33] = double.PositiveInfinity;
			lim[32] = 120;
			lim[31] = 60;
			lim[30] = 40;
			for (int i = 0; i < 30; i++) {
				lim[i] = i + 1;
			}
			int ind = -1;
			for (int i = 0; i < 33; i++) {
				if (Nu2 >= lim[i] && Nu2 < lim[i + 1]) {
					ind = i;
					break;
				}
			}
			return ind;
		}

		public static double norm_F_rev(double x, double m = 0, double sig = 1) {
			bool b = x < 0.5;
			if (b) {
				x = 1 - x;
				b = true;
			}
			double t = Math.Sqrt(-2 * Math.Log(1 - x)),
				c0 = 2.515517,
				c1 = 0.802853,
				c2 = 0.010328,
				d1 = 1.432788,
				d2 = 0.189269,
				d3 = 0.001308;
			double res = (t - (c0 + c1 * t + c2 * Math.Pow(t, 2)) / (d1 * t + d2 * Math.Pow(t, 2) + d3 * Math.Pow(t, 3)));
			if (b)
				res = -res;
			res = m + sig * res;
			return res;
		}
		public static double exp_F_rev(double x) {
			double res = -Math.Log(1 - x) / set.lm;
			return res;
		}
		public static double veib_F_rev(double x) {
			double res = Math.Pow(-set.al * Math.Log(1 - x), 1 / set.bet);
			//Math.Pow(Math.Log(1/(1 - x)) / set.al, 1 / set.bet);//set.al * Math.Pow(-Math.Log(1 - x), 1 / set.bet);
			return res;
		}

		public static double norm2d_f(double x, double y) {
			double res = Math.Exp(-(Math.Pow((x-ev2d.MID_X)/ev2d.MQD_X,2) + Math.Pow((y - ev2d.MID_Y) / ev2d.MQD_Y, 2) - 2*ev2d.COR*(x - ev2d.MID_X)*(y - ev2d.MID_Y)/(ev2d.MQD_X*ev2d.MQD_Y)) /(2* (1 - Math.Pow(ev2d.COR,2))))/(2*Math.PI*ev2d.MQD_X*ev2d.MQD_Y * Math.Sqrt(1-Math.Pow(ev2d.COR, 2)));
			return !double.IsNaN(res) ? res: 0;
		}

		public double ManyCor(int k) {
			double[][] Delta = new double[dim - 1][], DeltaStar = new double[dim][];
			for (int i = 0; i < dim; i++) {
				DeltaStar[i] = new double[dim];
				for (int j = 0; j < dim; j++) {
					double mid_ij = 0;
					for (int l = 0; l < Nnd; l++)
						mid_ij += arr[i][l] * arr[j][l];
					mid_ij /= Nnd;
					DeltaStar[i][j] = Nnd * (mid_ij - evnd.MID[i] * evnd.MID[j]) / (evnd.MQD[i] * evnd.MQD[j] * (Nnd - 1));
				}
			}
			for (int i = 0, i_ = 0; i < dim; i++, i_++) {
				if (i == k) {
					i_--;
					continue;
				}
				Delta[i_] = new double[dim - 1];
				for (int j = 0, j_ = 0; j < dim; j++, j_++) {
					if (j == k) {
						j_--;
						continue;
					}
					Delta[i_][j_] = DeltaStar[i][j];
				}
			}

			/*string str = "";
			for (int i = 0; i < DeltaStar.Length; i++) {
				for (int j = 0; j < DeltaStar[i].Length; j++) {
					str += DeltaStar[i][j] + " ";
				}
				str += "\n";
			}
			MessageBox.Show(str);
			str = "";
			for (int i = 0; i < Delta.Length; i++) {
				for (int j = 0; j < Delta[i].Length; j++) {
					str += Delta[i][j] + " ";
				}
				str += "\n";
			}
			MessageBox.Show(str);*/


			/*for (int i = 0, i_ = 0; i < dim; i++, i_++) {
				if (i == k) {
					i_--;
					continue;
				}
				Delta[i_] = new double[dim - 1];
				for (int j = 0, j_ = 0; j < dim; j++, j_++) {
					if (j == k) {
						j_--;
						continue;
					}
					if (i == j)
						Delta[i_][j_] = 1;
					else {
						Delta[i_][j_] = evnd.DCmat[i, j] / evnd.MQD[i] * evnd.MQD[j];
					}
				}
			}
			DeltaStar[0] = new double[dim];
			for (int i = 0; i < dim - 1; i++) {
				DeltaStar[i + 1] = new double[dim];
				for (int j = 0; j < dim - 1; j++) {
					DeltaStar[i + 1][j] = Delta[i][j];
				}
			}
			DeltaStar[0][dim - 1] = 0;
			double[][] arrt = new double[dim][];
			for (int i = 0; i < dim; i++) {
				arrt[i] = new double[Nnd];
				for (int j = 0; j < Nnd; j++) {
					arrt[i][j] = (arr[i][j] - evnd.MID[i]) / evnd.MQD[i];
				}
			}
			EvalND evndt = new EvalND(arrt);
			double mid_k = evnd.MID[k], mqd_k = evnd.MQD[k];
			for (int i = 0, i_ = 0; i < dim; i++, i_++) {
				if (i == k) {
					i_--;
					continue;
				}
				double mid_i = evndt.MID[i], mid_ik = 0, mqd_i = evnd.MQD[i];
				for (int l = 0; l < Nnd; l++)
					mid_ik += arr[i][l] * arr[k][l];
				mid_ik /= Nnd;
				DeltaStar[0][i_] = Nnd * (mid_ik - mid_i * mid_k) / ((Nnd - 1) * mqd_i * mqd_k);
				DeltaStar[i_ + 1][dim - 1] = DeltaStar[0][i_];
			}*/
			MessageBox.Show(Matrix.DetCount(DeltaStar) + " " + Matrix.DetCount(Delta));
			double manycor = Math.Sqrt(Math.Abs(1 - Matrix.DetCount(DeltaStar) / Matrix.DetCount(Delta)));
			return manycor;
		}
		#endregion

		#region Критерии
		private void tTestMenuItem_Click(object sender, EventArgs e) {
			if (dist_f != null) {
				bool res = false;
				double crit = crit_ttest(N - 1);
				double[] par = new double[2];
				int n = -1;
				if (f == norm_f)
					n = 0;
				else if (f == exp_f)
					n = 1;
				else if (f == arcsin_f)
					n = 2;
				new TtestForm(n, par, ref res).ShowDialog();
				if (res) {
					if (f == norm_f) {
						double t1 = (par[0] - sig) / ev.MQD, t2 = (par[1] - m) / ev.MQD;
						string s1 = Math.Abs(t1) <= crit ? "пройдений" : "не пройдений", s2 = Math.Abs(t2) <= crit ? "пройдений" : "не пройдений";
						MessageBox.Show("t(σ) = " + t1 + "\nКритичне значення" + crit + "t-тест " + s1);
						MessageBox.Show("t(m) = " + t2 + "\nКритичне значення" + crit + "t-тест " + s2);
					}
					else if (f == exp_f) {
						double t1 = (par[0] + sig) / ev.MQD;
						string s1 = Math.Abs(t1) <= crit ? "пройдений" : "не пройдений";
						MessageBox.Show("t(λ) = " + t1 + "\nКритичне значення" + crit + "t-тест " + s1);
					}
					else if (f == arcsin_f) {
						double t1 = (par[0] + sig) / ev.MQD;
						string s1 = Math.Abs(t1) <= crit ? "пройдений" : "не пройдений";
						MessageBox.Show("t(a) = " + t1 + "\nКритичне значення" + crit + "t-тест " + s1);
					}
				}
			}
		}

		private void pirsMenuItem_Click(object seNuder, EventArgs e) {
			if (dist_f != null) {
				double X2 = Hi_quad();
				int Nu = var_row_cl.Count - 1;
				double crit = crit_pirs(Nu);
				if (X2 <= crit) {
					MessageBox.Show("X^2 = " + X2 + "\nКритичне значення = " + crit + "\nЕмпірична і теоретична функції збігаються.");
				}
				else {
					MessageBox.Show("X^2 = " + X2 + "\nКритичне значення = " + crit + "\nЕмпірична і теоретична функції не збігаються.");

				}
			}
			else {
				MessageBox.Show("Немає відтвореного розподілу");
			}
		}

		private void kolmMenuItem_Click(object sender, EventArgs e) {
			if (dist_F != null) {
				double Dpl = double.MinValue, Dmin = double.MinValue;
				double[] F_temp = new double[var_row.Count];
				for (int i = 0; i < var_row.Count; i++) {
					F_temp[i] = F(var_row[i][0]);
					double temp = Math.Abs(var_row[i][2] - F_temp[i]);
					if (Dpl < temp)
						Dpl = temp;
				}
				for (int i = 1; i < var_row.Count; i++) {
					double temp = Math.Abs(var_row[i][2] - F_temp[i - 1]);
					if (Dmin < temp)
						Dmin = temp;
				}
				//MessageBox.Show(Dpl + " " + Dmin);
				double z = Math.Sqrt(N) * Math.Max(Dpl, Dmin), P = 1 - K(z);
				if (P >= set.sigLevel) {
					MessageBox.Show("P = " + P + "\nα = " + set.sigLevel + "\nЕмпірична і теоретична функції збігаються.");
				}
				else {
					MessageBox.Show("P = " + P + "\nα = " + set.sigLevel + "\nЕмпірична і теоретична функції не збігаються.");

				}
			}
			else {
				MessageBox.Show("Немає відтвореного розподілу");
			}
		}

		private void x22вимірнийToolStripMenuItem_Click(object sender, EventArgs e) {
			if (var_row_cl2d != null) {
				double HiQuad = 0;
				for(int i = 0; i < Mx; i++) {
					for(int j = 0; j < My; j++) {
						double p = norm2d_f(var_row_cl2d[i, j, 0], var_row_cl2d[i, j, 1]) * stepx * stepy;
						if (p != 0)
							HiQuad += Math.Pow(var_row_cl2d[i, j, 2] - p, 2) / p;
						//MessageBox.Show(var_row_cl2d[i, j, 2] + " " + p);

					}
				}
				double crit = crit_pirs(arr[0].Length - 2);
				string a = HiQuad <= crit ? "пройдений" : "не пройдений";
				MessageBox.Show("q = " + Math.Round(HiQuad, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій X^2 " + a);
			}
		}

		private void ттесткоефіцієнтКореляціїToolStripMenuItem_Click(object sender, EventArgs e) {
			double t = ev2d.COR * Math.Sqrt(arr[0].Length - 2) / Math.Sqrt(1 - Math.Pow(ev2d.COR,2));
			double crit = crit_ttest(arr[0].Length - 2);
			string a = t <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("t = " + Math.Round(t, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nТ - тест " + a);
		}

		private void tTestCorRatioMenuItem_Click(object sender, EventArgs e) {
			double t = ev2d.COR_RATIO * Math.Sqrt(arr[0].Length - 2) / Math.Sqrt(1 - Math.Pow(ev2d.COR_RATIO, 2));
			double crit = crit_ttest(arr[0].Length - 2);
			string a = t <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("t = " + Math.Round(t, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nТ - тест " + a);
		}
		private void збігКоефіцієнтівКореляціїToolStripMenuItem_Click(object sender, EventArgs e) {
			if (arrs2d.Count < 2)
				return;
			else if (arrs2d.Count == 2) {
				KeyValuePair<string, double[][]>[] kvp = arrs2d.ToArray();
				double[][] x = kvp[0].Value, y = kvp[1].Value;
				Eval2D ev_x = new Eval2D(x, true), ev_y = new Eval2D(y, true);
				double zx = Math.Log((1 + ev_x.COR) / (1 - ev_x.COR)) / 2.0;
				double zy = Math.Log((1 + ev_y.COR) / (1 - ev_y.COR)) / 2.0;
				double u = (zx - zy) / Math.Sqrt(1.0/(x[0].Length - 3) + 1.0 / (y[0].Length - 3));
				//MessageBox.Show(x[0].Length + " " + y[0].Length);
				double crit = 1.645;
				string a = Math.Abs(u) <= crit ? "пройдений" : "не пройдений";
				MessageBox.Show("u = " + Math.Round(u, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій збігу коефіцієнтів кореляції " + a);
			}
			else {
				KeyValuePair<string, double[][]>[] kvp = arrs2d.ToArray();
				List<double[][]> x = new List<double[][]>();
				foreach(KeyValuePair<string, double[][]> k in kvp) {
					x.Add(k.Value);
				}
				Eval2D[] ev = new Eval2D[x.Count];
				double[] z = new double[x.Count];
				for(int i = 0; i < x.Count; i++) {
					ev[i] = new Eval2D(x[i], true);
					z[i] = Math.Log((1 + ev[i].COR) / (1 - ev[i].COR)) / 2.0;
				}
				double S1 = 0, S2 = 0, S3 = 0;
				for(int i = 0; i < x.Count; i++) {
					S1 += (x[i][0].Length - 3) * Math.Pow(z[i], 2);
					S2 += Math.Pow((x[i][0].Length - 3) * z[i], 2);
					S3 += (x[i][0].Length - 3);
				}
				double HiQuad = S1 - S2 / S3;
				double crit = crit_pirs(x.Count - 1);
				string a = HiQuad <= crit ? "пройдений" : "не пройдений";
				MessageBox.Show("X^2 = " + Math.Round(HiQuad, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій збігу коефіцієнтів кореляції " + a);
			}
		}

		private void значущістьКоефіцієнтаСпірменаToolStripMenuItem_Click(object sender, EventArgs e) {
			double t = ev2d.SPIR * Math.Sqrt(N2d - 2) / Math.Sqrt(1 - Math.Pow(ev2d.SPIR, 2)), crit = crit_ttest(N2d - 2);
			string a = t <= crit ? "не значущий" : "значущий";
			MessageBox.Show("t = " + Math.Round(t, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКоефіцієнт Спірмана " + a);
		}

		private void значущістьКоефіцієнтаКендаллаToolStripMenuItem_Click(object sender, EventArgs e) {
			double u = 3 * ev2d.KEND * Math.Sqrt(N2d * (N2d - 1)) / Math.Sqrt(4*N2d + 10);
			string a = Math.Abs(u) <= 1.645 ? "не значущий" : "значущий";
			MessageBox.Show("u = " + Math.Round(u, 4) + "\nКритичне значення " + 1.645 + "\nКоефіцієнт Кендалла " + a);
		}

		private void значущістьКоефіцієнтаФToolStripMenuItem_Click(object sender, EventArgs e) {
			double X, crit = crit_pirs(1);
			if (N2d > 40)
				X = N2d * Math.Pow(ev2d.FI, 2);
			else
				X = N2d * Math.Pow(ev2d.N00 * ev2d.N11 - ev2d.N10 * ev2d.N01 - 0.5, 2) / ((ev2d.N00 + ev2d.N01) + (ev2d.N00 + ev2d.N10) + (ev2d.N10 + ev2d.N11) + (ev2d.N01 + ev2d.N11));
			string a = Math.Abs(X) <= 1.645 ? "не значущий" : "значущий";
			MessageBox.Show("X^2 = " + Math.Round(X, 4) + "\nКритичне значення " + 1.645 + "\nКоефіцієнт Ф " + a);
		}

		private void значущістьКоефіцієнтєвЮлаToolStripMenuItem_Click(object sender, EventArgs e) {
			double uq = 2 * ev2d.YUL_Q / ((1 - Math.Pow(ev2d.YUL_Q, 2)) * Math.Sqrt(1 / ev2d.N00 + 1 / ev2d.N01 + 1 / ev2d.N10 + 1 / ev2d.N11)), 
				uy = 2 * ev2d.YUL_Y / ((1 - Math.Pow(ev2d.YUL_Y, 2)) * Math.Sqrt(1 / ev2d.N00 + 1 / ev2d.N01 + 1 / ev2d.N10 + 1 / ev2d.N11));
			string a1 = Math.Abs(uq) <= 1.645 ? "не значущий" : "значущий", 
				a2 = Math.Abs(uy) <= 1.645 ? "не значущий" : "значущий";
			MessageBox.Show("Uq = " + Math.Round(uq, 4) + "\nКритичне значення " + 1.645 + "\nКоефіцієнт Q " + a1 + "\nUq = " + Math.Round(uy, 4) + "\nКритичне значення " + 1.645 + "\nКоефіцієнт Y " + a2);
		}

		private void незалежністьХТаYToolStripMenuItem_Click(object sender, EventArgs e) {
			double X = 0, crit = crit_pirs((Mx-1)*(My-1));
			double[] n = new double[Mx], m = new double[My];
			for (int i = 0; i < Mx;i++) {
				n[i] = 0;
				for (int j = 0; i < My; i++) {
					n[i] += var_row_cl2d[i, j, 2] * N2d;
				}
			}
			for (int i = 0; i < My; i++) {
				m[i] = 0;
				for (int j = 0; i < Mx; i++) {
					m[i] += var_row_cl2d[j,i, 2] * N2d;
				}
			}
			for (int i = 0; i < Mx; i++) {
				for(int j = 0; j  <My; j++) {
					double Nij = m[i] * n[j] / N2d;
					if (Nij != 0)
						X += Math.Pow(var_row_cl2d[i, j, 2]*N2d  - Nij, 2) / Nij;
					MessageBox.Show(var_row_cl2d[i, j, 2] * N2d + " " + Nij);
				}
			}
			string a = X <= crit ? "немає зв'язку" : "є зв'язок";
			MessageBox.Show("X^2 = " + Math.Round(X, 4) + "\nКритичне значення " + crit + "\nМіж Х та Y " + a);
		}

		private void адекватністьВідтворенняРегресіїToolStripMenuItem_Click(object sender, EventArgs e) {
			if (regr == Regression.Linear || regr == Regression.Parabolic) {
				double f = Math.Pow(S_rem / ev2d.MQD_Y, 2), crit = crit_fisher(N2d - 1, N2d - 3);
				string a = f <= crit ? "значуща" : "не значуща";
				MessageBox.Show("f = " + Math.Round(f, 4) + "\nКритичне значення " + crit + "\nВідтворена регресія " + a);
			}
			else {
				double[][] new_arr = new double[2][];
				new_arr[0] = new double[N2d];
				new_arr[1] = new double[N2d];
				for (int i = 0; i < N2d; i++) {
					new_arr[0][i] = arr[0][i];
					new_arr[1][i] = 1 / arr[1][i];
				}
				Eval2D _ev2d = new Eval2D(new_arr);
				double f = Math.Pow(S_rem / _ev2d.MQD_Y, 2), crit = crit_fisher(N2d - 1, N2d - 3);
				string a = f <= crit ? "значуща" : "не значуща";
				MessageBox.Show("f = " + Math.Round(f, 4) + "\nКритичне значення " + crit + "\nВідтворена регресія " + a);
			}
		}

		private void адекватністьВідтворенняРегресіїToolStripMenuItem1_Click(object sender, EventArgs e) {
			double t1 = 0, t2 = 0;
			for (int i = 0; i < Mx; i++) {
				double Ymid = 0;
				for (int j = 0; j < arr_cl[i].Count; j++)
					Ymid += arr_cl[i][j];
				Ymid /= arr_cl[i].Count;
				t1 += arr_cl[i].Count * Math.Pow((double)(Ymid - br*var_row_cl2d[i,0,0] - ar),2);
				for (int j = 0; j < arr_cl[i].Count; j++)
					t2 += Math.Pow(arr_cl[i][j] - Ymid, 2);
			}
			double f = (N2d - Mx) * t1 / (Mx - 3) * t2, crit = crit_fisher(Mx - 3, N2d - Mx);
			
			string a = f <= crit ? "значуща" : "не значуща";
			MessageBox.Show("f = " + Math.Round(f, 4) + "\nКритичне значення " + crit + "\nВідтворена регресія " + a);
		}

		private void збігРегресійToolStripMenuItem_Click(object sender, EventArgs e) {
			if (arrs2d.Count < 2)
				return;
			KeyValuePair<string, double[][]>[] kvp = arrs2d.ToArray();
			double[][] x = kvp[0].Value, y = kvp[1].Value;
			Eval2D ev_x = new Eval2D(x, true), ev_y = new Eval2D(y, true);
			double b1 = ev_x.COR * ev_x.MQD_Y / ev_x.MQD_X, a1 = ev_x.MID_Y - b1 * ev_x.MID_X;
			double b2 = ev_y.COR * ev_y.MQD_Y / ev_y.MQD_X, a2 = ev_y.MID_Y - b2 * ev_y.MID_X;
			double Xmid1 = 0, Ymid1 = 0, Xmid2 = 0, Ymid2 = 0, S1 = 0, S2 = 0;
			for(int i = 0; i < x[0].Length; i++) {
				Xmid1 += x[0][i];
			}
			Xmid1 /= x[0].Length;
			for (int i = 0; i < x[0].Length; i++) {
				Ymid1 += x[1][i];
			}
			Ymid1 /= x[0].Length;
			for (int i = 0; i < y[0].Length; i++) {
				Xmid2 += y[0][i];
			}
			Xmid2 /= y[0].Length;
			for (int i = 0; i < y[0].Length; i++) {
				Ymid2 += y[1][i];
			}
			Ymid2 /= y[0].Length;
			for (int i = 0; i < x[0].Length; i++) {
				S1 += Math.Pow(x[1][i] - a1 - b1*(x[1][i] - Xmid1),2);
			}
			S1 /= x[0].Length - 2;
			for (int i = 0; i < y[0].Length; i++) {
				S2 += Math.Pow(y[1][i] - a1 - b1 * (y[1][i] - Xmid2), 2);
			}
			S2 /= y[0].Length - 2;
			double f = S1 > S2 ? S1/S2 : S2/S1;
			double S = Math.Sqrt(((x[0].Length - 2) * S1 + (y[0].Length - 2) * S2) / (x[0].Length + y[0].Length - 4));
			if (f < crit_fisher(x[0].Length - 2, y[0].Length - 2)) {
				double t = (b1 - b2) / (S * Math.Sqrt(1 / ((x[0].Length - 1) * Math.Pow(ev_x.MQD_X, 2)) + 1 / ((y[0].Length - 1) * Math.Pow(ev_y.MQD_X, 2))));
				if (Math.Abs(t) < crit_ttest(x[0].Length + y[0].Length - 2)) {
					double b = ((x[0].Length - 1) * Math.Pow(ev_x.MQD_X * b1, 2) + (y[0].Length - 1) * Math.Pow(ev_y.MQD_X * b2, 2)) / ((x[0].Length - 1) * Math.Pow(ev_x.MQD_X, 2) + (y[0].Length - 1) * Math.Pow(ev_y.MQD_X, 2)),
						S0 = S * Math.Sqrt(1 / ((x[0].Length - 1) * Math.Pow(ev_x.MQD_X, 2) + (y[0].Length - 1) * Math.Pow(ev_y.MQD_X, 2)) + (1 / x[0].Length + 1 / y[0].Length) / Math.Pow(Xmid1 - Xmid2, 2)),
						b0 = (Ymid1 - Ymid2) / (Xmid1 - Xmid2),
						t1 = (b - b0) / S0;
					if (Math.Abs(t1) < crit_ttest(x[0].Length + y[0].Length - 4))
						MessageBox.Show("t = " + Math.Round(t1, 4) + "\nКритичне значення " + crit_ttest(x[0].Length + y[0].Length - 4) + "\nРегресії ідентичні.");
					else
						MessageBox.Show("t = " + Math.Round(t1, 4) + "\nКритичне значення " + crit_ttest(x[0].Length + y[0].Length - 4) + "\nРегресії мають значущий незбіг.");
				}
				else {
					MessageBox.Show("t = " + Math.Round(t, 4) + "\nКритичне значення " + crit_ttest(x[0].Length + y[0].Length - 2) + "\nКути нахилу регресій різні.");
				}
			}
			else {
				double t = (b1 - b2) / (S * Math.Sqrt(S1 / (x[0].Length * Math.Pow(ev_x.MQD_X, 2)) + S2 / (y[0].Length * Math.Pow(ev_y.MQD_X, 2)))),
					C0 = (S1 / (x[0].Length * Math.Pow(ev_x.MQD_X, 2))) / (S1 / (x[0].Length * Math.Pow(ev_x.MQD_X, 2)) + S2 / (y[0].Length * Math.Pow(ev_y.MQD_X, 2)));
				int nu = (int)Math.Truncate(Math.Pow(Math.Pow(C0, 2) / (x[0].Length - 2) + Math.Pow(1 - C0, 2) / (y[0].Length - 2), -1));
				if(Math.Abs(t) < crit_ttest(nu)) {
					double b0 = (Ymid1 - Ymid2) / (Xmid1 - Xmid2),
						b = (b1 * (x[0].Length * Math.Pow(ev_x.MQD_X, 2)) / S1 + b2 * (y[0].Length * Math.Pow(ev_y.MQD_X, 2)) / S2) / ((x[0].Length * Math.Pow(ev_x.MQD_X, 2)) / S1 + (y[0].Length * Math.Pow(ev_y.MQD_X, 2)) / S2),
						S10 = (y[0].Length * S1 + x[0].Length * S2) / x[0].Length * y[0].Length * Math.Pow(Xmid1 - Xmid2, 2) + S1 * S2 / (x[0].Length * Math.Pow(ev_x.MQD_X, 2) * S2 + y[0].Length * Math.Pow(ev_y.MQD_X, 2) * S1);
					double t1 = (b - b0) / S10;
					if (Math.Abs(t1) < crit_ttest(x[0].Length + y[0].Length - 4))
						MessageBox.Show("t = " + Math.Round(t1, 4) + "\nКритичне значення " + crit_ttest(x[0].Length + y[0].Length - 4) + "\nРегресії ідентичні");
					else
						MessageBox.Show("t = " + Math.Round(t1, 4) + "\nКритичне значення " + crit_ttest(x[0].Length + y[0].Length - 4) + "\nРегресії мають значущий незбіг");
				}
				else {
					MessageBox.Show("t = " + Math.Round(t, 4) + "\nКритичне значення " + crit_ttest(nu) + "\nКути нахилу регресій різні");
				}
			}
		}
		#endregion
	}
}
 