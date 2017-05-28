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
	public partial class CheckSelParamForm : Form {
		public CheckSelParamForm(Dictionary<string, List<double[]>> var_rows_, Dictionary<string, int> N) {
			InitializeComponent();
			this.var_rows_old = var_rows_;
			this.N_old = N;
			foreach (KeyValuePair<string, List<double[]>> row in var_rows_) {
				addList.Items.Add(row.Key);
			}
			var_rows = new Dictionary<string, List<double[]>>();
			N_dict = new Dictionary<string, int>();
			methList.Items.AddRange(new string[] {
					"Збіг дисперсій",
					"Збіг середніх(залежні)",
					"Збіг середніх(незалежні)",
					"Критерій Бартлетта",
					"Однофакторний дисперсійний аналіз",
					"Критерій Вілкоксона",
					"Критерій Манна - Уітні",
					"Критерій Кохрена",
					"Критерій Смирнова",
					"Критерій різниці рангів",
					"H - критерій",
					"Критерій Аббе)"
				});
			multidim = false;
		}
		public CheckSelParamForm(Dictionary<string, double[][]> var_rows_, Dictionary<string, int> N) {
			InitializeComponent();
			this.var_rowsm_old = var_rows_;
			this.N_old = N;
			foreach (KeyValuePair<string, double[][]> row in var_rows_) {
				addList.Items.Add(row.Key);
			}
			var_rowsm = new Dictionary<string, double[][]>();
			N_dict = new Dictionary<string, int>();
			methList.Items.AddRange(new string[] {
					"Збіг ДКМ",
					"Збіг середніх(ДКМ збігаються)",
					"Збіг середніх(ДКМ не збігаються)"
				});
			multidim = true;
		}
		Dictionary<string, List<double[]>> var_rows_old, var_rows;
		Dictionary<string, double[][]> var_rowsm_old, var_rowsm;
		Dictionary<string, int> N_old, N_dict;
		bool multidim;
		#region Критерии
		void mid_dep() {
			if(var_rows.Count != 2) {
				MessageBox.Show("Для даного критерію потрібно обрати 2 вибірки.");
				return;
			}
			KeyValuePair<string, List<double[]>>[] kvp = var_rows.ToArray();
			List<double[]> x_ = kvp[0].Value, y_ = kvp[1].Value;
			if(N_dict[kvp[0].Key] != N_dict[kvp[1].Key]) {
				MessageBox.Show("Обсяги вибірок мають бути однаковими.");
				return;
			}
			int N = N_dict[kvp[0].Key];
			double[] z = new double[N], x = new double[N], y = new double[N];
			int l = 0;
			for(int i = 0; i < x_.Count; i++) {
				int j = (int)(N * x_[i][1]);
				for (int k = 0; k < j; k++) {
					x[l] = x_[i][0];
				}
				l+= j;
			}
			l = 0;
			for (int i = 0; i < y_.Count; i++) {
				int j = (int)(N * y_[i][1]);
				for (int k = 0; k < j; k++) {
					y[l] = y_[i][0];
				}
				l += j;
			}
			double z_mid = 0, S = 0;
			for(int i = 0; i < N; i++) {
				z[i] = x[i] - y[i];
				z_mid += z[i];
			}
			z_mid /= N;
			for (int i = 0; i < N; i++) {
				S += Math.Pow(z[i] - z_mid, 2);
			}
			S = Math.Sqrt(S / (N - 1));
			double t = z_mid * Math.Sqrt(N) / S, crit = MainForm.crit_ttest(N - 2);
			string s = Math.Abs(t) <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("t(a) = " + Math.Round(t, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nТест збігу середніх " + s);
		}
		void mid_indep() {
			if (var_rows.Count != 2) {
				MessageBox.Show("Для даного критерію потрібно обрати 2 вибірки.");
				return;
			}
			KeyValuePair<string, List<double[]>>[] kvp = var_rows.ToArray();
			List<double[]> x = kvp[0].Value, y = kvp[1].Value;
			int Nx = N_dict[kvp[0].Key], Ny = N_dict[kvp[1].Key];
			if(Nx + Ny > 25) {
				double Sx = 0, Sy = 0, Mx = 0, My = 0;
				for (int i = 0; i < x.Count; i++)
					Mx += x[i][0]*x[i][1];
				for (int i = 0; i < y.Count; i++)
					My += y[i][0] * y[i][1];
				for (int i = 0; i < x.Count; i++)
					Sx += Math.Pow(x[i][0] - Mx, 2) * x[i][1];
				for (int i = 0; i < y.Count; i++)
					Sy += Math.Pow(y[i][0] - My, 2) * y[i][1];
				double t = (Mx - My) / Math.Sqrt(Sx / Nx + Sy / Ny), crit = MainForm.crit_ttest(Nx + Ny - 2);
				string s = Math.Abs(t) <= crit ? "пройдений" : "не пройдений";
				MessageBox.Show("t(a) = " + Math.Round(t, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nТест збігу середніх " + s);
			}
			else {
				double Sx = 0, Sy = 0, Mx = 0, My = 0;
				for (int i = 0; i < x.Count; i++)
					Mx += x[i][0] * x[i][1];
				for (int i = 0; i < y.Count; i++)
					My += y[i][0] * y[i][1];
				for (int i = 0; i < x.Count; i++)
					Sx += Math.Pow(x[i][0] - Mx, 2) * x[i][1];
				for (int i = 0; i < y.Count; i++)
					Sy += Math.Pow(y[i][0] - My, 2) * y[i][1];
				double Sxm = Sx / Nx;
				double Sym = Sy / Ny;
				double t = (Mx - My) * Math.Sqrt(Nx * Ny / (Nx + Ny)) / Math.Sqrt(((Nx - 1) * Sxm + (Ny - 1) * Sym) / (Nx + Ny - 2)), crit = MainForm.crit_ttest(Nx + Ny - 2);
				string s = Math.Abs(t) <= crit ? "пройдений" : "не пройдений";
				MessageBox.Show("t(a) = " + Math.Round(t, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nТест збігу середніх " + s);
			}
		}

		void disp() {
			if (var_rows.Count != 2) {
				MessageBox.Show("Для даного критерію потрібно обрати 2 вибірки.");
				return;
			}
			KeyValuePair<string, List<double[]>>[] kvp = var_rows.ToArray();
			List<double[]> x = kvp[0].Value, y = kvp[1].Value;
			int Nx = N_dict[kvp[0].Key], Ny = N_dict[kvp[1].Key];
			double Sx = 0, Sy = 0, Mx = 0, My = 0;
			for (int i = 0; i < x.Count; i++)
				Mx += x[i][0] * x[i][1];
			for (int i = 0; i < y.Count; i++)
				My += y[i][0] * y[i][1];
			for (int i = 0; i < x.Count; i++)
				Sx += Math.Pow(x[i][0] - Mx, 2) * x[i][1];
			for (int i = 0; i < y.Count; i++)
				Sy += Math.Pow(y[i][0] - My, 2) * y[i][1];
			double f = Sx >= Sy ? Sx / Sy : Sy / Sx, crit = MainForm.crit_fisher(Nx - 1, Ny - 1);
			string s = Math.Abs(f) <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("t(a) = " + Math.Round(f, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nТест збігу дисперсій " + s);
		}

		void bartlett() {
			if (var_rows.Count <= 1) {
				MessageBox.Show("Для даного критерію потрібно не менш 2 вибірок.");
				return;
			}
			List<double[]>[] x = new List<double[]>[var_rows.Count];
			double[] xm = new double[var_rows.Count], N = new double[var_rows.Count], S_ = new double[var_rows.Count];
			double S = 0, S1 = 0;
			int t1 = 0;
			foreach(KeyValuePair<string, List<double[]>> kvp in var_rows) {
				x[t1] = kvp.Value;
				N[t1] = N_dict[kvp.Key];
				t1++;
			}
			for(int i = 0; i < x.Length; i++) {
				xm[i] = 0;
				for (int j = 0; j < x[i].Count; j++)
					xm[i] += x[i][j][0]*x[i][j][1];
			}
			for (int i = 0; i < x.Length; i++) {
				S_[i] = 0;
				for (int j = 0; j < x[i].Count; j++)
					S_[i] += Math.Pow(x[i][j][0] - xm[i], 2) * x[i][j][1];
				S_[i] *= N[i] / (N[i] - 1);
			}
			for(int i = 0; i < x.Length; i++) {
				S += (N[i] - 1) * S_[i];
				S1 += (N[i] - 1);
			}
			S /= S1;
			double B = 0, C = 0, C1 = 0, C2 = 0;
			for(int i = 0; i < x.Length; i++) {
				B -= (N[i] - 1) * Math.Log(S_[i]/S);
				C1 += 1 / (N[i] - 1);
				C2 += N[i] - 1;
			}
			C = 1 + 1 / (3 * (x.Length - 1)) * (C1 - 1 / C2);
			double HiQuad = B / C, crit = MainForm.crit_pirs(x.Length - 1);
			string s = Math.Abs(HiQuad) <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("X^2 = " + Math.Round(HiQuad, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій Бартлетта " + s);
		}
		void onefactor() {
			if (var_rows.Count <= 1) {
				MessageBox.Show("Для даного критерію потрібно не менш 2 вибірок.");
				return;
			}
			List<double[]>[] x = new List<double[]>[var_rows.Count];
			double[] xm = new double[var_rows.Count], S_ = new double[var_rows.Count];
			int[] N = new int[var_rows.Count];
			int t1 = 0, N_sum = 0;
			foreach (KeyValuePair<string, List<double[]>> kvp in var_rows) {
				x[t1] = kvp.Value;
				N[t1] = N_dict[kvp.Key];
				t1++;
			}
			int k = x.Length;
			for (int i = 0; i < k; i++) {
				xm[i] = 0;
				for (int j = 0; j < x[i].Count; j++)
					xm[i] += x[i][j][0] * x[i][j][1];
			}
			double mid = 0, Sm = 0, Sb = 0, F, crit;
			for(int i = 0; i < k; i++) {
				mid += N[i] * xm[i];
			}
			for (int i = 0; i < k; i++) {
				N_sum += N[i];
			}
			mid /= N_sum;
			for (int i = 0; i < x.Length; i++) {
				S_[i] = 0;
				for (int j = 0; j < x[i].Count; j++)
					S_[i] += Math.Pow(x[i][j][0] - xm[i], 2) * x[i][j][1];
				S_[i] *= N[i] / (N[i] - 1);
			}
			for (int i = 0; i < k; i++) {
				Sm += N[i] * Math.Pow(xm[i] - mid, 2);
				Sb = (N[i] - 1) * S_[i];
			}
			Sm /= (k - 1);
			Sb /= (N_sum - k);
			F = Sm / Sb;
			crit = MainForm.crit_fisher(k - 1, N_sum - k);
			string s = F <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("F = " + Math.Round(F, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nОднофакторний дисперсійний аналіз " + s);
		}
		void wilkokson() {
			if (var_rows.Count != 2) {
				MessageBox.Show("Для даного критерію потрібно обрати 2 вибірки.");
				return;
			}
			KeyValuePair<string, List<double[]>>[] kvp = var_rows.ToArray();
			List<double[]> x = kvp[0].Value, y = kvp[1].Value;
			int Nx = N_dict[kvp[0].Key], Ny = N_dict[kvp[1].Key];
			int W = 0;
			for (int i = 0; i < Nx; i++) {
				for (int j = 0; j < Ny; j++) {
					W += x[i][0] > y[j][0] ? 1 : 0;
				}
			}
			W = Nx * Ny + Nx * (Nx - 1) / 2 - W;
			double E = Nx * (Nx + Ny + 1) / 2, D = Nx * Ny * (Nx + Ny + 1) / 12;
			double w = (W - E) / Math.Sqrt(D);
			string s = w <= 1.645 ? "пройдений" : "не пройдений";
			MessageBox.Show("w = " + Math.Round(w, 4) + "\nКритичне значення " + 1.645 + "\nКритерій Вілкоксона " + s);
		}
		void mannwhitney() {
			if (var_rows.Count != 2) {
				MessageBox.Show("Для даного критерію потрібно обрати 2 вибірки.");
				return;
			}
			KeyValuePair<string, List<double[]>>[] kvp = var_rows.ToArray();
			List<double[]> x = kvp[0].Value, y = kvp[1].Value;
			int Nx = N_dict[kvp[0].Key], Ny = N_dict[kvp[1].Key];
			int U = 0;
			for(int i = 0; i < Nx; i++) {
				for(int j = 0; j < Ny; j++) {
					U += x[i][0] > y[j][0] ? 1 : 0;
				}
			}
			double E = Nx * Ny / 2, D = Nx * Ny * (Nx + Ny + 1) / 12;
			double u = (U - E)/Math.Sqrt(D);
			string s = u <= 1.645 ? "пройдений" : "не пройдений";
			MessageBox.Show("u = " + Math.Round(u, 4) + "\nКритичне значення " + 1.645 + "\nКритерій Манна-Уітні " + s);
		}

		void cohren() {
			if (var_rows.Count <= 1) {
				MessageBox.Show("Для даного критерію потрібно не менш 2 вибірок.");
				return;
			}
			List<double[]>[] x = new List<double[]>[var_rows.Count];
			double[] xm = new double[var_rows.Count], N = new double[var_rows.Count], S_ = new double[var_rows.Count];
			double res = 0, crit = 0;
			int t1 = 0;
			foreach (KeyValuePair<string, List<double[]>> kvp in var_rows) {
				x[t1] = kvp.Value;
				N[t1] = N_dict[kvp.Key];
				t1++;
			}
			int count = x[0].Count;
			for (int i = 0; i < x.Length; i++) {
				xm[i] = 0;
				if (x[i].Count != count) {
					MessageBox.Show("Обсяги вибірок мають бути однаковими.");
					return;
				}
				for (int j = 0; j < count; j++)
					xm[i] += x[i][j][0] * x[i][j][1];
			}
			for (int i = 0; i < x.Length; i++) {
				S_[i] = 0;
				for (int j = 0; j < x[i].Count; j++)
					S_[i] += Math.Pow(x[i][j][0] - xm[i], 2) * x[i][j][1];
				S_[i] *= N[i] / (N[i] - 1);
			}
			res = S_.Max() / S_.Sum();
			crit = MainForm.crit_pirs(count - 1);
			string s = Math.Abs(res) <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("Q = " + Math.Round(res, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій Кохрена " + s);
		}
		void smirnov() {
			if (var_rows.Count != 2) {
				MessageBox.Show("Для даного критерію потрібно обрати 2 вибірки.");
				return;
			}
			KeyValuePair<string, List<double[]>>[] kvp = var_rows.ToArray();
			List<double[]> x = kvp[0].Value, y = kvp[1].Value;
			int Nx = N_dict[kvp[0].Key], Ny = N_dict[kvp[1].Key];
			double Dplus = double.MinValue, Dminus = double.MinValue;
			for(int i = 0; i < Nx; i++) {
				double pl = (i + 1) / Nx - x[i][2];
				double min = x[i][2] - i;
				if (pl > Dplus)
					Dplus = pl;
				if (min > Dminus)
					Dminus = min;
			}
			double D = Dplus > Dminus ? Dplus : Dminus;
			string s = D <= MainForm.set.al ? "пройдений" : "не пройдений";
			MessageBox.Show("z = " + Math.Round(D,4) + "\nКритичне значення " + Math.Round(MainForm.set.al, 4) + "\nКритерій Смирнова " + s);

		}
		void H() {
			if (var_rows.Count <= 1) {
				MessageBox.Show("Для даного критерію потрібно не менш 2 вибірок.");
				return;
			}
			List<double[]>[] x = new List<double[]>[var_rows.Count];
			double[] xm = new double[var_rows.Count], S_ = new double[var_rows.Count];
			int[] N = new int[var_rows.Count];
			int t1 = 0, N_sum = 0;
			foreach (KeyValuePair<string, List<double[]>> kvp in var_rows) {
				x[t1] = kvp.Value;
				N[t1] = N_dict[kvp.Key];
				N_sum += N[t1];
				t1++;
			}
			List<double[]> common = new List<double[]>();
			int[][] r = new int[x.Length][];
			for (int i = 0; i < x.Length; i++) {
				for (int j = 0; j < x[i].Count; j++) {
					int l = (int)(N[i] * x[i][j][1]);
					for (int k = 0; k < l; k++)
						common.Add(new double[] { x[i][j][0], 0 , i, j});
				}
				r[i] = new int[x[i].Count];
			}
			common.Sort(new VarRowComp());
			for (int i = 0; i < common.Count; i++) {
				common[i][1] = i + 1;
			}
			for (int i = 0; i < common.Count; i++) {
				if (i < common.Count - 1 && common[i + 1][0] == common[i][0]) {
					int sum = i + 1;
					int k = 1;
					for (; common[i + k][0] == common[i][0]; k++)
						sum += i + k + 1;
					common[i][1] = sum / k;
				}
				r[(int)common[i][2]][(int)common[i][3]] = (int)common[i][1];
			}
			double H = 0;
			for(int i = 0; i < x.Length; i++) {
				int R = 0;
				for (int j = 0; j < r[i].Length; j++)
					R += r[i][j];
				R /= N[i];
				H += N[i] * Math.Pow(R - (N_sum + 1) / 2, 2);//(1 + N[i] / N_sum) * Math.Pow((R + (N_sum + 1) / 2) / Math.Sqrt((N_sum - N[i]) * (N_sum - 1) / (12 * N[i])), 2);
			}
			H *= 12 / (N_sum * (N_sum + 1));
			double crit = MainForm.crit_pirs(x.Length - 1);
			string s = H <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("H = " + Math.Round(H, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nH - критерій " + s);
		}
		void ranks() {
			if (var_rows.Count != 2) {
				MessageBox.Show("Для даного критерію потрібно 2 вибірки.");
				return;
			}
			List<double[]>[] x = new List<double[]>[var_rows.Count];
			double[] xm = new double[var_rows.Count], S_ = new double[var_rows.Count];
			int[] N = new int[var_rows.Count];
			int t1 = 0, N_sum = 0;
			foreach (KeyValuePair<string, List<double[]>> kvp in var_rows) {
				x[t1] = kvp.Value;
				N[t1] = N_dict[kvp.Key];
				N_sum += N[t1];
				t1++;
			}
			List<double[]> common = new List<double[]>();
			double[][] r = new double[x.Length][];
			for (int i = 0; i < x.Length; i++) {
				for (int j = 0; j < x[i].Count; j++) {
					int l = (int)(N[i] * x[i][j][1]);
					for (int k = 0; k < l; k++)
						common.Add(new double[] { x[i][j][0], 0, i, j });
				}
				r[i] = new double[x[i].Count];
			}
			common.Sort(new VarRowComp());
			for (int i = 0; i < common.Count; i++) {
				common[i][1] = i + 1;
			}
			for (int i = 0; i < common.Count; i++) {
				if (i < common.Count - 1 && common[i + 1][0] == common[i][0]) {
					int sum = i + 1;
					int k = 1;
					for (; common[i + k][0] == common[i][0]; k++)
						sum += i + k + 1;
					common[i][1] = sum / k;
				}
				r[(int)common[i][2]][(int)common[i][3]] = common[i][1];
				//MessageBox.Show(common[i][2] + " " + common[i][3] + " " + r[(int)common[i][2]][(int)common[i][3]]);
			}
			double H = 0, rx = 0, ry = 0;
			for (int j = 0; j < r[0].Length; j++)
				rx += r[0][j];
			for (int j = 0; j < r[1].Length; j++)
				ry += r[1][j];
			//MessageBox.Show((N_sum + 1) / (12 * N[0] * N[1]) + "");
			H = (rx - ry) / (N_sum * Math.Sqrt((double)(N_sum+1)/(double)(12*N[0]*N[1])));
			double crit = 1.645;
			string s = H <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("H = " + Math.Round(H, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій різниці рангів " + s);
		}

		void abbe() {
			if (var_rows.Count != 1) {
				MessageBox.Show("Для даного критерію потрібно обрати 1 вибірку.");
				return;
			}
			KeyValuePair<string, List<double[]>>[] kvp = var_rows.ToArray();
			List<double[]> x = kvp[0].Value;
			int Nx = N_dict[kvp[0].Key];
			double xm = 0;
			for (int i = 0; i < x.Count; i++) {
				xm += x[i][0] * x[i][1];
			}
			double q = 0, s = 0;
			for(int i = 0; i < x.Count - 1; i++) {
				q += Math.Pow(x[i + 1][0]* x[i+1][1] * Nx - x[i][0]* x[i][1] * Nx, 2);
			}
			q /= 2 * (Nx - 1);
			//double sum = 0;
			for (int i = 0; i < x.Count; i++) {
				s += Math.Pow((x[i][0] - xm)*x[i][1]*Nx, 2);
				//sum += x[i][0];
				//MessageBox.Show(x[i][0] + " " + xm + " " + q1);
			}
			//MessageBox.Show(sum + " " + xm*x.Count);
			s /= Nx - 1;
			q /= s;
			//q = -(1 - q) * Math.Sqrt((2*x.Count + 1)/(2 - Math.Pow(1-q,2)));
			double crit = 1.645;
			string a = q <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("q = " + Math.Round(q, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій Аббе " + a);
		}
		void mid_multidim_yes() {
			if (var_rowsm.Count != 2) {
				MessageBox.Show("Для даного критерію потрібно обрати 2 вибірки.");
				return;
			}
			KeyValuePair<string, double[][]>[] kvp = var_rowsm.ToArray();
			double[][] x = kvp[0].Value, y = kvp[1].Value;
			int N1 = N_dict[kvp[0].Key], N2 = N_dict[kvp[1].Key], dim = x.Length;
			double[][] S0_ = new double[dim][], S1_ = new double[dim][];
			for (int i = 0; i < dim; i++) {
				S0_[i] = new double[dim];
				S1_[i] = new double[dim];
			}
			for (int i = 0; i < dim; i++) {
				for (int j = 0; j < dim; j++) {
					double a = 0, b = 0, c = 0, d = 0, e = 0, f = 0;
					for (int l = 0; l < N1; l++) {
						a += x[i][l] * x[j][l];
						b += x[i][l];
						c += x[j][l];
					}
					for (int l = 0; l < N2; l++) {
						d += y[i][l] * y[j][l];
						e += y[i][l];
						f += y[j][l];
					}
					S0_[i][j] = (a + d - (b + e) * (c + f) / (N1 + N2)) / (N1 + N2 - 2);
					S1_[i][j] = (a + d - b * c / N1 - e * f / N2) / (N1 + N2 - 2);
				}
			}
			double S0 = Matrix.DetCount(S0_), S1 = Matrix.DetCount(S1_), V = -(N1 + N2 - 2 - dim/2)*Math.Log(S1/S0), crit = MainForm.crit_pirs(dim);
			string s = V <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("H = " + Math.Round(V, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nH - критерій " + s);
		}

		void mid_multidim_no() {
			KeyValuePair<string, double[][]>[] kvp = var_rowsm.ToArray();
			int K = kvp.Length; //количество выборок
			double[][][] X = new double[K][][], S = new double[K][][];
			double[] N = new double[K];
			double[][] Xmid = new double[K][];
			for(int i = 0; i < K; i++) {
				X[i] = kvp[i].Value;
				N[i] = N_dict[kvp[i].Key];
			}
			int dim = X[0].Length;//размерность выборок
			double[][] MidCommon;
			double[][] a = new double[dim][], b = new double[dim][];
			for (int i = 0; i < K; i++) {
				double[][] Xcur = X[i];
				if(Xcur.Length != dim) {
					MessageBox.Show("Усі вибірки мають бути однакової розмірності");
					return;
				}
				Xmid[i] = new double[dim];
				for(int j = 0; j < Xcur.Length; j++) {
					for(int k = 0; k < N[i]; k++) {
						Xmid[i][j] += Xcur[j][k];
					}
					Xmid[i][j] /= N[i];
				}
			}
			for(int i = 0; i < K; i++) {
				double[][] Xcur = new double[dim][];
				S[i] = new double[dim][];
				for(int j = 0; j < dim; j++) {
					//Создание матриц и векторов
					S[i][j] = new double[dim];
					a[j] = new double[dim];
					b[j] = new double[1];
					b[i][0] = 0.0;
					Xcur[j] = new double[X[i][j].Length];
					for (int k = 0; k < dim; k++) {
						S[i][j][k] = 0.0;
						a[j][k] = 0.0;
					}
					////////////////////////////
					for (int k = 0; k < X[i][j].Length; k++) {
						Xcur[j][k] = X[i][j][k] - Xmid[i][j];
					}
				}
				for(int l = 0; l < Xcur.Length; l++) {
					double[][] Xl = new double[][] { Xcur[l] }, XlT = Matrix.Transposition(Xl);
					S[i] = Matrix.Add(S[i], Matrix.Multiply(XlT, Xl));
				}
				Matrix.MultiplyNumber(S[i], (1 / (N[i] - 1)));
			}
			for(int d = 0; d < K; d++) {
				double[][] Scur = Matrix.Invert(S[d]), Midcur = Matrix.Transposition(new double[][] { Xmid[d] });
				Matrix.MultiplyNumber(Scur, N[d]);
				a = Matrix.Add(a, Scur);
				b = Matrix.Add(b, Matrix.Multiply(Scur, Midcur));
			}
			a = Matrix.Invert(a);
			MidCommon = Matrix.Transposition(Matrix.Multiply(a, b));
			Matrix.MultiplyNumber(MidCommon, -1);
			double V = 0.0, crit = MainForm.crit_pirs(dim*(K-1));
			for (int d = 0; d < K; d++) {
				string str = "";
				double[][] c = Matrix.Add(new double[][] { Xmid[d] }, MidCommon),
					Scur = S[d]/*Matrix.Invert(S[d])*/,
					D = Matrix.Multiply(c, Matrix.Multiply(Scur, Matrix.Transposition(c)));
				V += N[d] * D[0][0];
				//for (int i = 0; i < S[d].Length; i++) {
				//	for (int j = 0; j < S[d].Length; j++) {
				//		str += S[d][i][j] + " ";
				//	}
				//	str += "\n";
				//}
				//MessageBox.Show(str);
			}
			string s = V <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("V = " + Math.Round(V, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nH - критерій " + s);
		}

		void dkm() {
			KeyValuePair<string, double[][]>[] kvp = var_rowsm.ToArray();
			int K = kvp.Length; //количество выборок
			double[][][] X = new double[K][][], S = new double[K][][];
			double[] N = new double[K];
			double[][] Xmid = new double[K][];
			double NCommon = 0;
			for (int i = 0; i < K; i++) {
				X[i] = kvp[i].Value;
				N[i] = N_dict[kvp[i].Key];
				NCommon += N[i];
			}
			int dim = X[0].Length;//размерность выборок
			double[][] a = new double[dim][], b = new double[dim][], SCommon = new double[dim][];
			for (int i = 0; i < K; i++) {
				double[][] Xcur = X[i];
				if (Xcur.Length != dim) {
					MessageBox.Show("Усі вибірки мають бути однакової розмірності");
					return;
				}
				Xmid[i] = new double[dim];
				for (int j = 0; j < Xcur.Length; j++) {
					for (int k = 0; k < N[i]; k++) {
						Xmid[i][j] += Xcur[j][k];
					}
					Xmid[i][j] /= N[i];
				}
			}
			for (int i = 0; i < K; i++) {
				double[][] Xcur = new double[dim][];
				S[i] = new double[dim][];
				for (int j = 0; j < dim; j++) {
					//Создание матриц и векторов
					S[i][j] = new double[dim];
					a[j] = new double[dim];
					SCommon[j] = new double[dim];
					b[j] = new double[1];
					b[i][0] = 0.0;
					Xcur[j] = new double[X[i][j].Length];
					for (int k = 0; k < dim; k++) {
						S[i][j][k] = 0.0;
						a[j][k] = 0.0;
						SCommon[j][k] = 0.0;
					}
					////////////////////////////
					for (int k = 0; k < X[i][j].Length; k++) {
						Xcur[j][k] = X[i][j][k] - Xmid[i][j];
					}
				}
				for (int l = 0; l < Xcur.Length; l++) {
					double[][] Xl = new double[][] { Xcur[l] }, XlT = Matrix.Transposition(Xl);
					S[i] = Matrix.Add(S[i], Matrix.Multiply(XlT, Xl));
				}
				Matrix.MultiplyNumber(S[i], (1 / (N[i] - 1)));
			}
			for(int d = 0; d < K; d++) {
				double[][] Scur = new double[dim][];
				for(int j = 0; j < dim; j++) {
					Scur[j] = new double[dim];
					for (int k = 0; k < dim; k++)
						Scur[j][k] = S[d][j][k];
				}
				Matrix.MultiplyNumber(Scur, (N[d] - 1));
				SCommon = Matrix.Add(SCommon, Scur);
			}
			Matrix.MultiplyNumber(SCommon, 1 / ((NCommon - K)*1.5));
			double DetCommon = Matrix.DetCount(SCommon), V = 0.0, crit = MainForm.crit_pirs(dim * (dim + 1) * (K - 1) / 2);
			for(int d = 0; d < K; d++) {
				double DetCur = Matrix.DetCount(S[d]),
					D = Math.Log(DetCommon / DetCur) * (N[d] - 1) / 2;
				V += D;
				MessageBox.Show(DetCommon + " " + DetCur + " " + Math.Log(DetCommon / DetCur));
			}
			string s = V <= crit ? "пройдений" : "не пройдений";
			MessageBox.Show("V = " + Math.Round(V, 4) + "\nКритичне значення " + Math.Round(crit, 4) + "\nКритерій " + s);
		}
		#endregion

		#region Добавление, удаление, выбор метода
		private void ok_Click(object sender, EventArgs e) {
			if (methList.Text == "")
				return;
			switch (methList.Text) {
				case "Збіг середніх(залежні)": {
						mid_dep();
					}
					break;
				case "Збіг середніх(незалежні)": {
						mid_indep();
					}
					break;
				case "Збіг дисперсій": {
						disp();
					}
					break;
				case "Критерій Бартлетта": {
						bartlett();
					}
					break;
				case "Однофакторний дисперсійний аналіз": {
						onefactor();
					}
					break;
				case "Критерій Вілкоксона": {
						wilkokson();
					}
					break;
				case "Критерій Манна-Уітні": {
						mannwhitney();
					}
					break;
				case "Критерій Кохрена": {
						cohren();
					}
					break;
				case "Критерій Смирнова": {
						smirnov();
					}
					break;
				case "H-критерій": {
						H();
					}
					break;
				case "Критерій різниці рангів": {
						ranks();
					}
					break;
				case "Критерій Аббе": {
						abbe();
					}
					break;
				case "Збіг ДКМ": {
						dkm();
					}
					break;
				case "Збіг середніх(ДКМ збігаються)": {
						mid_multidim_yes();
					}
					break;
				case "Збіг середніх(ДКМ не збігаються)": {
						mid_multidim_no();
					}
					break;

			}
		}
		private void add_Click(object sender, EventArgs e) {
			if (addList.Text == "")
				return;
			int n = 1;
			string temp = addList.Text;
			if (!multidim) {
				if (var_rows.ContainsKey(addList.Text)) {
					for (; var_rows.ContainsKey(temp); n++, temp = addList.Text + "(" + n + ")") ;
				}
				delList.Items.Add(temp);
				N_dict.Add(temp, N_old[addList.Text]);
				var_rows.Add(temp, var_rows_old[addList.Text]);
			}
			else {
				if (var_rowsm.ContainsKey(addList.Text)) {
					for (; var_rowsm.ContainsKey(temp); n++, temp = addList.Text + "(" + n + ")") ;
				}
				delList.Items.Add(temp);
				N_dict.Add(temp, N_old[addList.Text]);
				var_rowsm.Add(temp, var_rowsm_old[addList.Text]);
			}
		}

		private void delete_Click(object sender, EventArgs e) {
			if (delList.Text == "")
				return;
			string temp = delList.Text;
			if (!multidim) {
				for (int i = 0; i < delList.Items.Count; i++) {
					if ((string)delList.Items[i] == temp) {
						delList.Items.RemoveAt(i);
						break;
					}
				}
				N_dict.Remove(temp);
				var_rows.Remove(temp);
			}
			else {
				for (int i = 0; i < delList.Items.Count; i++) {
					if ((string)delList.Items[i] == temp) {
						delList.Items.RemoveAt(i);
						break;
					}
				}
				N_dict.Remove(temp);
				var_rowsm.Remove(temp);
			}
		}
		#endregion

	}
}
