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
	public partial class GenerateMultiDimForm : Form {
		public GenerateMultiDimForm(List<double[]> arr) {
			InitializeComponent();
			this.arr = arr;
		}
		int n, N;
		List<double[]> arr;
		private void build_Click(object sender, EventArgs e) {
			E.Columns.Clear();
			r.Columns.Clear();
			r.Rows.Clear();
			r.Columns.Clear();
			sig.Rows.Clear();
			sig.Columns.Clear();
			n = (int)n_.Value;
			N = (int)Num_.Value;
			for (int i = 0; i < n; i++) {
				E.Columns.Add("", "");
				E.Columns[i].Width = 30;
				r.Columns.Add("", "");
				r.Columns[i].Width = 30;
				sig.Columns.Add("", "");
				sig.Columns[i].Width = 30;
			}
			sig.Rows.Add(1);
			E.Rows.Add(1);
			r.Rows.Add(n);
		}


		private void build1_Click(object sender, EventArgs e) {
			avec.Columns.Clear();
			avec.Rows.Clear();
			min_.Columns.Clear();
			min_.Rows.Clear();
			max_.Columns.Clear();
			max_.Rows.Clear();
			n = (int)n__.Value;
			N = (int)Num__.Value;
			for (int i = 0; i < n-1; i++) {
				avec.Columns.Add("", "");
				avec.Columns[i].Width = 30;
				min_.Columns.Add("", "");
				min_.Columns[i].Width = 30;
				max_.Columns.Add("", "");
				max_.Columns[i].Width = 30;
			}
			avec.Rows.Add(1);
			min_.Rows.Add(1);
			max_.Rows.Add(1);
		}


		private void ok_regr_Click(object sender, EventArgs e) {
			double[] a = new double[n-1], min = new double[n-1], max = new double[n-1], step = new double[n-1];
			double sig = double.Parse(sig_regr.Text.Replace(".", ",")), a0 = double.Parse(a0_.Text.Replace(".", ","));
			for (int i = 0; i < n-1; i++) {
				a[i] = Convert.ToDouble(((string)avec[i, 0].Value).Replace('.', ','));
				min[i] = Convert.ToDouble(((string)min_[i, 0].Value).Replace('.', ','));
				max[i] = Convert.ToDouble(((string)max_[i, 0].Value).Replace('.', ','));
				step[i] = Math.Abs(max[i] - min[i]) / N;
			}
			Random r = new Random();
			/*string str = "";
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < n; j++) {
					str += A[i, j] + " ";
				}
				str += "\n";
			}
			MessageBox.Show(str);

			for (int l = 0; l < N; l++) {
				double[] vec = new double[n];
				for (int i = 0; i < n; i++) {
					double au = 0;
					for (int j = 0; j < n; j++)
						au += A[i, j] * MainForm.norm_F_rev(r.NextDouble());
					vec[i] = m[i] + au;
				}
				arr.Add(vec);
			}*/
			for(int i = 0; i < N; i++) {
				double[] vec = new double[n];
				double eps = MainForm.norm_F_rev(r.NextDouble(), 0, sig);
				for (int j = 0; j < n-1; j++) {
					vec[j] = min[j];
					min[j] += step[j];
 				}
				vec[n - 1] = a0 + eps;
				for (int j = 0; j < n - 1; j++) {
					vec[n - 1] += a[j] * vec[j];
				}
				arr.Add(vec);
			}
		}

		private void ok_Click(object sender, EventArgs e) {
			double[] m = new double[n], s = new double[n];
			double[,] A = new double[n, n], cov = new double[n, n];
			for (int i = 0; i < n; i++) {
				m[i] = Convert.ToDouble(((string)E[i, 0].Value).Replace('.', ','));
				s[i] = Convert.ToDouble(((string)sig[i, 0].Value).Replace('.', ','));
				for (int j = 0; j < n; j++) {
					cov[i, j] = Convert.ToDouble(((string)this.r[j, i].Value).Replace('.', ','));
				}
			}
			for(int i = 0;i < n; i++) {
				for(int j = 0; j < n; j++) {
					cov[i, j] *= s[i] * s[j];
				}
			}
			Random r = new Random();
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < n; j++) {
					if (i == j) {
						double a = 0;
						for (int w = 0; w < i; w++)
							a += Math.Pow(A[i, w], 2);
						A[i, j] = Math.Sqrt(cov[i, i] - a);
					}
					else if (i > j) {
						double a = 0;
						for (int w = 0; w < j; w++)
							a += A[i, w] * A[j, w];
						A[i, j] = (cov[i, j] - a) / A[j, j];
					}
					else
						A[i, j] = 0;
				}
			}
			/*string str = "";
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < n; j++) {
					str += A[i, j] + " ";
				}
				str += "\n";
			}
			MessageBox.Show(str);*/

			for (int l = 0; l < N; l++) {
				double[] vec = new double[n];
				for (int i = 0; i < n; i++) {
					double au = 0;
					for (int j = 0; j < n; j++)
						au += A[i, j] * MainForm.norm_F_rev(r.NextDouble());
					vec[i] = m[i] + au;
				}
				arr.Add(vec);
			}
		}
	}
}
