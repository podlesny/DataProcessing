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
	public partial class GenerateForm : Form {
		public GenerateForm(List<List<double>> arr_) {
			InitializeComponent();
			arr = arr_;
			this.ControlBox = false;
			_2d = false;
		}
		public GenerateForm(double[][] arr_) {
			InitializeComponent();
			arr2d = arr_;
			this.ControlBox = false;
			_2d = true;
		}
		List<List<double>> arr;
		double[][] arr2d;
		bool _2d;
		private void generate_Click(object sender, EventArgs e) {
			input.Text = "";
			int m = 0, n = 0;
			double l = 0, u = 0;
			Random r = new Random();
			if (!int.TryParse(M.Text, out m) && m < 1) {
				m = 1;
			}
			if (!int.TryParse(N.Text, out n) && n < 1) {
				n = 100;
			}
			if (!double.TryParse(low.Text, out l)) {
				l = 0;
			}
			if (!double.TryParse(up.Text, out u)) {
				u = 100;
			}
			string str = "";
			if (chs.Text == "")
				chs.Text = "—";
			switch (chs.Text) {
				case "—": {
						for (int i = 0; i < m; i++) {
							for (int j = 0; j < n; j++) {
								str += r.Next((int)l, (int)u) + " ";
							}
							str += Environment.NewLine + Environment.NewLine;
						}
					}
					break;
				case "Нормальний": {
						for (int i = 0; i < m; i++) {
							for (int j = 0; j < n; j++) {
								str += MainForm.norm_F_rev(r.NextDouble(), MainForm.set.m, MainForm.set.sig) + " ";
							}
							str += Environment.NewLine + Environment.NewLine;
						}
					}
					break;
				case "Експоненціальний": {
						for (int i = 0; i < m; i++) {
							for (int j = 0; j < n; j++) {
								str += MainForm.exp_F_rev(r.NextDouble()) + " ";
							}
							str += Environment.NewLine + Environment.NewLine;
						}
					}
					break;
				case "Вейбулла": {
						for (int i = 0; i < m; i++) {
							for (int j = 0; j < n; j++) {
								str += MainForm.veib_F_rev(r.NextDouble()) + " ";
							}
							str += Environment.NewLine + Environment.NewLine;
						}
					}
					break;
				case "Нормальний 2-вимірний": {
						double[] x = new double[n], y = new double[n];
						Settings set = MainForm.set;
						for (int i = 0; i < m; i++) {
							for (int j = 0; j < n; j++) {
								double z1 = MainForm.norm_F_rev(r.NextDouble());
								double z2 = (MainForm.norm_F_rev(r.NextDouble()) - set.r * (MainForm.norm_F_rev(r.NextDouble()))) / Math.Sqrt(1 - Math.Pow(set.r, 2));
								//MessageBox.Show(z1 + " " + z2 + " " + set.r);
								x[j] = set.mx + set.sigx * z1;
								y[j] = set.my + set.sigy * (z2 * Math.Sqrt(1 - Math.Pow(set.r, 2)) + z1 * set.r);
								str += x[j] + " ";
							}
							str += Environment.NewLine + Environment.NewLine;
							for (int j = 0; j < n; j++)
								str += y[j] + " ";
							str += Environment.NewLine + Environment.NewLine;
						}
					}
					break;
				case "Рівномірний 2-вимірний(параб.регр)": {
						double[] x = new double[n], y = new double[n];
						Settings set = MainForm.set;
						double step = (double)(u - l) / (n - 1), cur_x = l;
						//MessageBox.Show(set.ar + "");
						for (int i = 0; i < n; i++, cur_x += step) {
							x[i] = cur_x;
							y[i] = set.cr * Math.Pow(cur_x, 2) + set.br * cur_x + set.ar + MainForm.norm_F_rev(r.NextDouble(), MainForm.set.m, MainForm.set.sig);
							//MessageBox.Show(x[i] + " " + y[i]);
							str += x[i] + " ";
						}
						str += Environment.NewLine + Environment.NewLine;
						for (int j = 0; j < n; j++)
							str += y[j] + " ";
					}
					break;
				case "Рівномірний 2-вимірний(квазіл.регр)": {
						double[] x = new double[n], y = new double[n];
						Settings set = MainForm.set;
						double step = (double)(u - l) / (n - 1), cur_x = l;
						//MessageBox.Show(set.ar + "");
						for (int i = 0; i < n; i++, cur_x += step) {
							x[i] = cur_x;
							y[i] = set.br / (set.ar + cur_x) + MainForm.norm_F_rev(r.NextDouble(), MainForm.set.m, MainForm.set.sig);
							//MessageBox.Show(x[i] + " " + y[i]);
							str += x[i] + " ";
						}
						str += Environment.NewLine + Environment.NewLine;
						for (int j = 0; j < n; j++)
							str += y[j] + " ";
					}
					break;
			}
			input.Text = str;
		}

		private void ok_Click(object sender, EventArgs e) {
			if (input.Text == "") {
				if(_2d)
					arr2d[0] = null;
				else
					arr.Add(null);
			}
			else {
				if (_2d) {
					string[] vectors = input.Text.Replace(Environment.NewLine, "\n").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
					int l = vectors.Length;
					if (vectors.Length != 2) {
						arr2d[0] = null;
						return;
					}
					string[] txtData1 = vectors[0].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					string[] txtData2 = vectors[1].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
					if (txtData1.Length != txtData2.Length) {
						MessageBox.Show("Некоректно введені дані. Обсяги компонент х та у потрібні бути однаковими.");
						arr2d[0] = null;
						return;
					}
					arr2d[0] = new double[txtData1.Length];
					arr2d[1] = new double[txtData2.Length];
					for (int j = 0; j < txtData1.Length; j++) {
						double temp = 0;
						if (double.TryParse(txtData1[j], out temp)) {
							arr2d[0][j] = temp;
							
						}
					}
					for (int j = 0; j < txtData2.Length; j++) {
						double temp = 0;
						if (double.TryParse(txtData2[j], out temp)) {
							arr2d[1][j] = temp;
						}
					}
				}
				else {
					string[] vectors = input.Text.Replace(Environment.NewLine, "\n").Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < vectors.Length; i++) {
						string[] txtData = vectors[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
						arr.Add(new List<double>());
						for (int j = 0; j < txtData.Length; j++) {
							double temp = 0;
							if (double.TryParse(txtData[j], out temp)) {
								arr[i].Add(temp);
							}
						}
					}
				}
			}
		}

		private void cancel_btn_Click(object sender, EventArgs e) {
			if (_2d)
				arr2d[0] = null;
			else
				arr.Add(null);
			
		}
	}
}