using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataProcessing {
	delegate double NumF(double par);

	public struct EmpirGraphFlags {
		public bool graph_cl;
		public bool graph;
		public bool F;
		public bool interv;
		public EmpirGraphFlags(bool a, bool b, bool c, bool d) {
			graph_cl = a;
			graph = b;
			F = c;
			interv = d;
		}
	}

	public class Settings {
		public double sigLevel;
		public double alpha;
		public double shiftDist;
		public double logBase;
		public int classNum;
		public double m;
		public double mx;
		public double my;
		public double lm;
		public double sig;
		public double sigx;
		public double sigy;
		public double r;
		public double al;
		public double bet;
		public double ar, br, cr;
		public int pgrad, Mx, My;
		public Settings() {
			sigLevel = 0.05;
			alpha = 0.25;
			shiftDist = 50;
			logBase = 2;
			classNum = -1;
			pgrad = 6;
			Mx = -1;
			My = -1;
			m = 0;
			sig = 1;
			lm = 1;
			al = 1;
			bet = 1;
			mx = 0;
			my = 0;
			r = 0;
			sigx = 1;
			sigy = 1;
			ar = 0;
			br = 0;
			cr = 1;
		}
	}

	public struct GistFlags {
		public bool gist;
		public bool f;
		public GistFlags(bool a, bool b) {
			gist = a;
			f = b;
		}
	}

	public struct CorfFlags {
		public bool corf, regr_l, toler_l, interv_l, progn_l, regr_p, toler_p, interv_p, progn_p, regr_q, toler_q, interv_q, progn_q;
		public CorfFlags(bool c, bool rl, bool tl, bool il, bool pl, bool rp, bool tp, bool ip, bool pp, bool rq, bool tq, bool iq, bool pq) {
			corf = c;
			regr_l = rl;
			toler_l = tl;
			interv_l = il;
			progn_l = pl;
			regr_p = rp;
			toler_p = tp;
			interv_p = ip;
			progn_p = pp;
			regr_q = rq;
			toler_q = tq;
			interv_q = iq;
			progn_q = pq;
		}
	}

	public class VarRowComp : IComparer<double[]> {
		public int Compare(double[] a, double[] b) {
			if (a[0] > b[0])
				return 1;
			else if (a[0] < b[0])
				return -1;
			else return 0;
		}
	}

	public class WalshComp : IComparer<KeyValuePair<double, int>> {
		public int Compare(KeyValuePair<double, int> a, KeyValuePair<double, int> b) {
			if (a.Key > b.Key)
				return 1;
			else if (a.Key < b.Key)
				return -1;
			else return 0;
		}
	}

	public class Eval {
		public double MID = 0;
		public double MID_QUAD = 0;
		public double MID_max = 0;
		public double MID_min = 0;
		public double MQD = 0;
		public double MQD_max = 0;
		public double MQD_min = 0;
		public double MED = 0;
		public double MED_W = 0;
		public double MAD = 0;
		public double A_S = 0;
		public double A = 0;
		public double A_max = 0;
		public double A_min = 0;
		public double E_S = 0;
		public double E = 0;
		public double E_max = 0;
		public double E_min = 0;
		public double ContrE = 0;
		public double ContrE_max = 0;
		public double ContrE_min = 0;
		public double VAR = 0;
		public double VAR_max = 0;
		public double VAR_min = 0;
		public double VAR_NP = 0;
		public double MID_CUT = 0;
		const double u = 1.645;
		public double moment(int n, List<double[]> var_row) {
			double temp = 0;
			for (int i = 0; i < var_row.Count; i++)
				temp += Math.Pow(var_row[i][0] - MID, n) * var_row[i][1];
			return temp;

		}
		public double beta(int n, List<double[]> v) {
			if (n % 2 == 0) {
				int k = n / 2;
				return (moment(2 * k + 2, v) / Math.Pow(moment(2, v), k + 1));
			}
			else {
				int k = (n - 1) / 2;
				return (moment(3, v) * moment(2 * k + 3, v) / Math.Pow(moment(2, v), k + 3));
			}
		}
		public Eval(List<double[]> v) {
			int N = MainForm.N;
			double tMED = 0;
			bool tMED1 = true;
			for (int i = 0; i < v.Count; i++) {
				MID += v[i][0] * v[i][1];
				MID_QUAD += Math.Pow(v[i][0], 2) * v[i][1];
				tMED += v[i][1] * N;
				if (tMED > N / 2 && tMED1) {
					MED = v[i][0];
					tMED1 = false;
				}
				else if (tMED == N / 2 && tMED1) {
					MED = (v[i][0] + v[i + 1][0]) / 2;
					tMED1 = false;
				}
			}
			MQD = Math.Sqrt(moment(2, v));
			MID_max = MID + u * MQD / Math.Sqrt(N);
			MID_min = MID - u * MQD / Math.Sqrt(N);
			MQD_max = MQD + u * MQD / Math.Sqrt(2 * N);
			MQD_min = MQD - u * MQD / Math.Sqrt(2 * N);
			Dictionary<double, int> tWalsh = new Dictionary<double, int>();
			for (int i = 1; i < v.Count; i++) {
				double c = 0;
				for (int j = 1; j < v[i][1] * N; j++)
					c += j;
				if (tWalsh.ContainsKey(v[i][0])) {
					tWalsh[v[i][0]] += (int)c;
				}
				else {
					tWalsh.Add(v[i][0], (int)c);
				}
				for (int j = 0; j < i; j++) {
					c = v[i][1] * N * v[j][1] * N;
					if (tWalsh.ContainsKey((v[j][0] + v[i][0]) / 2)) {
						tWalsh[(v[j][0] + v[i][0]) / 2] += (int)c;
					}
					else {
						tWalsh.Add((v[j][0] + v[i][0]) / 2, (int)c);
					}
				}
			}
			KeyValuePair<double, int>[] Walsh = tWalsh.ToArray();
			tMED = 0;
			Array.Sort(Walsh, new WalshComp());
			for (int i = 0; i < Walsh.Length; i++) {
				tMED += Walsh[i].Value;
				if (tMED > N * (N - 1) / 4) {
					MED_W = Walsh[i].Key;
					break;
				}
				else if (tMED == N * (N - 1) / 4) {
					MED_W = (Walsh[i].Key + Walsh[i + 1].Key) / 2.0;
					break;
				}
			}
			double[] MADarr = new double[v.Count];
			for (int i = 0; i < v.Count; i++)
				MADarr[i] = Math.Abs(v[i][0] - MED);
			tMED = 0;
			for (int i = 0; i < v.Count; i++) {
				tMED += v[i][1] * N;
				if (tMED > N / 2) {
					MAD = MADarr[i];
					break;
				}
				else if (tMED == N / 2) {
					MAD = (MADarr[i] + MADarr[i + 1]) / 2;
					break;
				}
			}
			for (int i = 0; i < v.Count; i++) {
				MID_CUT += v[i][0] * v[i][1] * N;
			}
			MID_CUT /= (N - 2 * MainForm.set.alpha * N);
			MAD *= 1.483;
			A_S = moment(3, v) / Math.Pow(MQD, 3);
			A = Math.Sqrt(N * (N - 1)) * A_S / (N - 2);
			A_max = A + u * Math.Sqrt(6.0 * (1.0 - 12.0 / (2.0 * N + 7.0)) / N);
			A_min = A - u * Math.Sqrt(6.0 * (1.0 - 12.0 / (2.0 * N + 7.0)) / N);//(6 * (N - 2) / ((N + 1) * (N + 3)));
			E_S = moment(4, v) / Math.Pow(MQD, 4);
			E = (Math.Pow(N, 2) - 1) * ((E_S - 3) + 6 / (N + 1)) / ((N - 2) * (N - 3));
			E_max = E + u * Math.Sqrt(24.0 * N * (N - 2) * (N - 3) / (Math.Pow(N + 1, 2) * (N + 3) * (N + 5)));
			E_min = E - u * Math.Sqrt(24.0 * N * (N - 2) * (N - 3) / (Math.Pow(N + 1, 2) * (N + 3) * (N + 5)));
			ContrE = 1 / Math.Sqrt(Math.Abs(E));
			VAR = MQD / MID;
			VAR_NP = MAD / MED;
			VAR_max = VAR + u * VAR * Math.Sqrt((1.0 + 2 * Math.Pow(VAR, 2)) / 2.0 * N);
			VAR_min = VAR - u * VAR * Math.Sqrt((1.0 + 2 * Math.Pow(VAR, 2)) / 2.0 * N);
			ContrE_max = ContrE + u * Math.Sqrt(Math.Abs(E_S) / (29.0 * N)) * Math.Pow(Math.Abs(Math.Pow(E_S, 2)) - 1, 3.0 / 4.0);
			ContrE_min = ContrE * u * Math.Sqrt(Math.Abs(E_S) / (29.0 * N)) * Math.Pow(Math.Abs(Math.Pow(E_S, 2)) - 1, 3.0 / 4.0);
		}
	}

	public class Eval2D {
		public double MID_X = 0, MID_Y = 0, MID_XY = 0, MID_X_QUAD = 0, MID_X_CUBE = 0, MID1 = 0, MID_FI2_Y = 0, MID_FI2_QUAD = 0;
		public double MQD_X = 0, MQD_Y = 0;
		public double COR = 0, COR_MAX = 0, COR_MIN = 0, COR_RATIO = 0;
		public double SPIR = 0, KEND = 0;
		public double FEHN, FI, YUL_Y, YUL_Q;
		public double PIRS, KEND2, STEW;
		public int N00 = 0, N01 = 0, N10 = 0, N11 = 0;
		public Eval2D(double[][] arr, bool lite = false) {
			int N2d = arr[0].Length;
			for (int i = 0; i < N2d; i++) {
				MID_X += arr[0][i];
				MID_Y += arr[1][i];
				MID_XY += arr[0][i] * arr[1][i];
				MID_X_QUAD += Math.Pow(arr[0][i], 2);
				MID_X_CUBE += Math.Pow(arr[0][i], 3);
			}
			MID_X /= N2d;
			MID_Y /= N2d;
			MID_XY /= N2d;
			MID_X_QUAD /= N2d;
			MID_X_CUBE /= N2d;
			for (int i = 0; i < N2d; i++) {
				MQD_X += Math.Pow(arr[0][i] - MID_X, 2);
				MQD_Y += Math.Pow(arr[1][i] - MID_Y, 2);
				MID1 += (arr[0][i] - MID_X) * arr[1][i];
				double FI2 = Math.Pow(arr[0][i], 2) - (MID_X_CUBE - MID_X_QUAD * MID_X) * (arr[0][i] - MID_X) / Math.Pow(MQD_X, 2) - MID_X_QUAD;
				MID_FI2_Y += FI2 * arr[1][i];
				MID_FI2_QUAD += Math.Pow(FI2, 2);
			}
			MID_FI2_Y /= N2d;
			MID_FI2_QUAD /= N2d;
			MID1 /= N2d;
			MQD_X = Math.Sqrt(MQD_X / (N2d - 1));
			MQD_Y = Math.Sqrt(MQD_Y / (N2d - 1));
			COR = N2d/ (N2d - 1) * (MID_XY - MID_X * MID_Y) / (MQD_X * MQD_Y);
			COR_MAX = COR + COR * (1 - Math.Pow(COR, 2)) / (2 * N2d) + 1.645 * (1 - Math.Pow(COR, 2)) / Math.Sqrt(N2d - 1);
			COR_MIN = COR + COR * (1 - Math.Pow(COR, 2)) / (2 * N2d) - 1.645 * (1 - Math.Pow(COR, 2)) / Math.Sqrt(N2d - 1);
			#region COR_RATIO
			if (!lite) {
				double[] mid_arr = new double[MainForm.Mx];
				double mid = 0, S1 = 0, S2 = 0;
				for (int i = 0; i < MainForm.Mx; i++) {
					mid_arr[i] = 0;
					if (MainForm.arr_cl[i].Count != 0) {
						for (int j = 0; j < MainForm.arr_cl[i].Count; j++)
							mid_arr[i] += MainForm.arr_cl[i][j];
						mid_arr[i] /= MainForm.arr_cl[i].Count;
					}
				}
				for (int i = 0; i < MainForm.Mx; i++) {
					mid += mid_arr[i] * MainForm.arr_cl[i].Count;
				}
				mid /= N2d;
				for (int i = 0; i < MainForm.Mx; i++) {
					S1 += MainForm.arr_cl[i].Count * Math.Pow(mid_arr[i] - mid, 2);
					for (int j = 0; j < MainForm.arr_cl[i].Count; j++)
						S2 += Math.Pow(MainForm.arr_cl[i][j] - mid, 2);
				}
				COR_RATIO = S1 / S2;
			}
			#endregion
			#region RANKS
			double[][] rx = new double[N2d][], ry = new double[N2d][];
			for (int i = 0; i < N2d; i++) {
				rx[i] = new double[2];
				ry[i] = new double[3];
				rx[i][0] = arr[0][i];
				ry[i][0] = arr[1][i];
				ry[i][2] = arr[0][i];
			}
			List<int> A = new List<int>(), B = new List<int>();
			Array.Sort(rx, new VarRowComp());
			Array.Sort(ry, new VarRowComp());
			for (int i = 0; i < N2d; i++) {
				rx[i][1] = i + 1;
				ry[i][1] = i + 1;
			}
			for (int i = 0; i < N2d; i++) {
				if (i < N2d - 1 && rx[i + 1][0] == rx[i][0]) {
					int sum = i + 1;
					int k = 1;
					for (; i + k < N2d && rx[i + k][0] == rx[i][0]; k++)
						sum += i + k + 1;
					A.Add(k);
					double q = sum / k;
					for (int j = 0; j < k; j++)
						rx[i + j][1] = q;
				}
				//MessageBox.Show(MainForm.Mx + " " + MainForm.My);
				if (i < N2d - 1 && ry[i + 1][0] == ry[i][0]) {
					int sum = i + 1;
					int k = 1;
					for (; i + k < N2d && ry[i + k][0] == ry[i][0]; k++)
						sum += i + k + 1;
					B.Add(k);
					double q = sum / k;
					for (int j = 0; j < k; j++)
						ry[i + j][1] = q;
				}
			}
			double[,] r = new double[N2d, 2];
			for (int i = 0; i < N2d; i++) {
				r[i, 0] = rx[i][1];
				for (int j = 0; j < N2d; j++) {
					if (rx[i][0] == ry[j][2]) {
						r[i, 1] = ry[j][1];
					}
				}
			}
			for (int i = 0; i < N2d; i++) {
				SPIR += Math.Pow(r[i, 0] - r[i, 1], 2);
			}
			if (A.Count == 0 && B.Count == 0) {
				SPIR = 1 - 6 * SPIR / (N2d * (Math.Pow(N2d, 2) - 1));
			}
			else {
				double A_ = 0, B_ = 0;
				for (int i = 0; i < A.Count; i++)
					A_ += Math.Pow(A[i], 3) - A[i];
				for (int i = 0; i < B.Count; i++)
					B_ += Math.Pow(B[i], 3) - B[i];
				A_ /= 12;
				B_ /= 12;
				SPIR = ((N2d * (Math.Pow(N2d, 2) - 1)) - SPIR - A_ - B_) / Math.Sqrt(((N2d * (Math.Pow(N2d, 2) - 1)) / 6 - 2 * A_) * ((N2d * (Math.Pow(N2d, 2) - 1)) / 6 - 2 * B_));
			}
			for(int i = 0; i < N2d - 1; i++) {
				int v = 0;
				for(int j = i + 1; j < N2d; j++) {
					v += r[i, 1] < r[j, 1] ? 1 : -1;
				}
				KEND += v;
			}
			KEND = 2 * KEND / (N2d * (N2d - 1));
			#endregion
			#region Коефіцієнти таблиць
			double V = 0, W = 0;
			for(int i = 0; i < arr[0].Length; i++) {
				double _x = arr[0][i] - MID_X;
				double _y = arr[1][i] - MID_Y;
				if (_x >= 0 && _y >= 0 || _x < 0 && _y < 0)
					V++;
				else
					W++;
			}
			FEHN = (V - W) / (V + W);
			bool err = false;
			for(int i = 0; i < arr[0].Length; i++) {
				if (arr[0][i] == 0 && arr[1][i] == 0)
					N00++;
				else if (arr[0][i] == 0 && arr[1][i] == 1)
					N01++;
				else if (arr[0][i] == 1 && arr[1][i] == 0)
					N10++;
				else if (arr[0][i] == 1 && arr[1][i] == 1)
					N11++;
				else {
					err = true;
					break;
				}
			}
			if (err) {
				FI = double.NaN;
				YUL_Q = double.NaN;
				YUL_Y = double.NaN;
			}
			else {
				FI = (N00 * N11 - N01 * N10) / Math.Sqrt((N00 + N01) * (N10 + N11) * (N00 + N10) * (N01 + N11));
				YUL_Y = (Math.Sqrt(N00 * N11) - Math.Sqrt(N01 * N10)) / (Math.Sqrt(N00 * N11) + Math.Sqrt(N01 * N10));
				YUL_Q = 2 * YUL_Y / (1 + Math.Pow(YUL_Y, 2));
			}
			double[,,] var_row_cl2d = MainForm.var_row_cl2d;
			int Mx = MainForm.Mx, My = MainForm.My;
			double X = 0;
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
				}
			}
			PIRS = Math.Sqrt(X / (N2d + X));
			double P = 0, Q = 0, T1 = 0, T2 = 0;
			for (int i = 0; i < Mx; i++) {
				for (int j = 0; j < My; j++) {
					double temp = var_row_cl2d[i, j, 2] * N2d;
					double temp1 = 0, temp2 = 0;
					for (int k = i + 1; k < Mx; k++) {
						for (int l = j + 1; l < My; l++)
							temp1 += var_row_cl2d[k, l, 2] * N2d;
						for (int l = 1; l < j - 1; l++)
							temp2 += var_row_cl2d[k, l, 2] * N2d;
					}
					P += temp * temp1;
					Q += temp * temp2;
				}
			}
			if (Mx == My) {				
				for (int i = 0; i < Mx; i++)
					T1 += var_row_cl2d[i, 0, 0] * (var_row_cl2d[i, 0, 0] - 1);
				for (int i = 0; i < My; i++)
					T2 += var_row_cl2d[0, i, 0] * (var_row_cl2d[0, i, 0] - 1);
				KEND2 = (P - Q) / Math.Sqrt((N2d * (N2d - 1) / 2 - T1) * (N2d * (N2d - 1) / 2 - T2));
				STEW = double.NaN;
			}
			else {
				KEND2 = double.NaN;
				double min = Mx > My ? My : Mx;
				STEW = 2 * (P - Q) * min / (Math.Pow(Mx, 2) * (min - 1));
			}

			#endregion

		}
		
	}

	public class EvalND {
		public double[] MID, MQD;
		public double[,] DCmat;
		public EvalND(double[][] arr) {
			int Nnd = MainForm.Nnd, dim = MainForm.dim;
			MID = new double[dim];
			MQD = new double[dim];
			DCmat = new double[dim, dim];
			for(int i = 0; i < arr.Length; i++) {
				for(int j = 0; j < arr[i].Length; j++) {
					MID[i] += arr[i][j];
				}
				MID[i] /= Nnd;
			}
			for (int i = 0; i < arr.Length; i++) {
				for (int j = 0; j < arr[i].Length; j++) {
					MQD[i] += Math.Pow(arr[i][j] - MID[i], 2);
				}
				MQD[i] = Math.Sqrt(MQD[i] / (Nnd - 1));
			}
			for(int i = 0; i < arr.Length; i++) {
				for(int j = 0; j < arr.Length; j++) {
					if (i == j) {
						DCmat[i, j] = Math.Pow(MID[i],2);
						continue;
					}
					double MID_xi_xj = 0;
					for (int k = 0; k < arr[j].Length; k++)
						MID_xi_xj += arr[i][k] * arr[j][k];
					MID_xi_xj /= Nnd;
					DCmat[i,j] = Nnd * (MID_xi_xj - MID[i] * MID[j]) / (Nnd - 1);
				}
			}
		}
	}

	public class MultiDimArray {
		List<object> data;
		int d;
		public MultiDimArray(int[] sizes) {
			data = new List<object>();
			d = sizes.Length;
			Create(data, sizes, 0);
		}
		void Create(List<object> arr, int[] sizes, int dim) {
			if (dim == d) {
				return;
			}
			int size = sizes[dim];
			for (int i = 0; i < size; i++) {
				arr.Add(new List<object>());
				Create((List<object>)arr[i], sizes, dim + 1);
			}
		}
		public void SetVal(int[] indexes, double[] val) {
			List<object> temp_arr = (List<object>)data[indexes[0]];
			double[] temp;
			for (int i = 1; i < indexes.Length-1; i++) {
				temp_arr = (List<object>)temp_arr[indexes[i]];
			}
			temp = (double[])temp_arr[indexes.Length - 1];
			for (int i = 0; i < val.Length; i++) {
				temp[i] = val[i];
			}
		}
		public double[] GetVal(int[] indexes) {
			List<object> temp_arr = (List<object>)data[indexes[0]];
			double[] temp;
			for (int i = 1; i < indexes.Length - 1; i++) {
				temp_arr = (List<object>)temp_arr[indexes[i]];
			}
			temp = (double[])temp_arr[indexes.Length - 1];
			return temp;
		}
	}

	public static class Matrix {

		public static double[][] Invert(double[][] A_) {
			double[][] A = new double[A_.Length][];
			for(int i = 0; i < A_.Length; i++) {
				A[i] = new double[A_[i].Length];
				for(int j = 0; j < A_[i].Length; j++) {
					A[i][j] = A_[i][j];
				}
			}
			int n = A.Length;
			int[] row = new int[n];
			int[] col = new int[n];
			double[] temp = new double[n];
			int hold, I_pivot, J_pivot;
			double pivot, abs_pivot;
			// установиим row и column как вектор изменений.
			for (int k = 0; k < n; k++) {
				row[k] = k;
				col[k] = k;
			}
			// начало главного цикла
			for (int k = 0; k < n; k++) {
				// найдем наибольший элемент для основы
				pivot = A[row[k]][col[k]];
				I_pivot = k;
				J_pivot = k;
				for (int i = k; i < n; i++) {
					for (int j = k; j < n; j++) {
						abs_pivot = Math.Abs(pivot);
						if (Math.Abs(A[row[i]][col[j]]) > abs_pivot) {
							I_pivot = i;
							J_pivot = j;
							pivot = A[row[i]][col[j]];
						}
					}
				}
				//перестановка к-ой строки и к-ого столбца с стобцом и строкой, содержащий основной элемент(pivot основу)
				hold = row[k];
				row[k] = row[I_pivot];
				row[I_pivot] = hold;
				hold = col[k];
				col[k] = col[J_pivot];
				col[J_pivot] = hold;
				// k-ую строку с учетом перестановок делим на основной элемент
				A[row[k]][col[k]] = 1.0 / pivot;
				for (int j = 0; j < n; j++) {
					if (j != k) {
						A[row[k]][col[j]] = A[row[k]][col[j]] * A[row[k]][col[k]];
					}
				}
				// внутренний цикл
				for (int i = 0; i < n; i++) {
					if (k != i) {
						for (int j = 0; j < n; j++) {
							if (k != j) {
								A[row[i]][col[j]] = A[row[i]][col[j]] - A[row[i]][col[k]] *
										A[row[k]][col[j]];
							}
						}
						A[row[i]][col[k]] = -A[row[i]][col[k]] * A[row[k]][col[k]];
					}
				}
			}
			// конец главного цикла

			// переставляем назад rows
			for (int j = 0; j < n; j++) {
				for (int i = 0; i < n; i++) {
					temp[col[i]] = A[row[i]][j];
				}
				for (int i = 0; i < n; i++) {
					A[i][j] = temp[i];
				}
			}
			// переставляем назад columns
			for (int i = 0; i < n; i++) {
				for (int j = 0; j < n; j++) {
					temp[row[j]] = A[i][col[j]];
				}
				for (int j = 0; j < n; j++) {
					A[i][j] = temp[j];
				}
			}
			return A;
		}

		public static void MultiplyNumber(double[][] A, double b) {
			for(int i = 0; i < A.Length; i++) {
				for(int j = 0; j < A[i].Length; j++) {
					A[i][j] *= b;
				}
			}
		}

		public static double DetCount(double[][] M) {
			double det = 1; // Хранит определитель, который вернёт функция
			int n = M.Length; // Размерность матрицы
			int k = 0;
			const double E = 1E-9; // Погрешность вычислений

			for (int i = 0; i < n; i++) {
				k = i;
				for (int j = i + 1; j < n; j++)
					if (Math.Abs(M[j][i]) > Math.Abs(M[k][i]))
						k = j;

				if (Math.Abs(M[k][i]) < E) {
					det = 0;
					break;
				}
				Swap(ref M, i, k);

				if (i != k) det *= -1;

				det *= M[i][i];

				for (int j = i + 1; j < n; j++)
					M[i][j] /= M[i][i];

				for (int j = 0; j < n; j++)
					if ((j != i) && (Math.Abs(M[j][i]) > E))
						for (k = i + 1; k < n; k++)
							M[j][k] -= M[i][k] * M[j][i];
			}
			return det;
		}

		public static void Swap(ref double[][] M, int row1, int row2) {
			double s = 0;

			for (int i = 0; i < M[row1].Length; i++) {
				s = M[row1][i];
				M[row1][i] = M[row2][i];
				M[row2][i] = s;
			}
		}
		public static double[][] Multiply(double[][] A, double[][] B) {
			int ni = A.Length;
			int nk = A[0].Length;
			int nj = B[0].Length;
			double[][] C = new double[ni][];
			for (int i = 0; i < ni; i++) {
				C[i] = new double[nj];
			}
			for (int i = 0; i < ni; i++) {
				for (int j = 0; j < nj; j++) {
					C[i][j] = 0.0;
					for (int k = 0; k < nk; k++)
						C[i][j] += A[i][k] * B[k][j];
				}
			}
			return C;
		}

		public static double[][] Transposition(double[][] A) {
			int ni = A[0].Length, nj = A.Length;
			double[][] B = new double[ni][];
			for(int i = 0; i < ni; i++) {
				B[i] = new double[nj];
				for(int j = 0; j < nj; j++) {
					B[i][j] = A[j][i];
				}
			}
			return B;
		}

		public static double[][] Add(double[][] A, double[][] B) {
			int ni = A.Length;
			int nj = A[0].Length;
			double[][] C = new double[ni][];
			for (int i = 0; i < ni; i++) {
				C[i] = new double[nj];
				for (int j = 0; j < nj; j++)
					C[i][j] = A[i][j] + B[i][j];
			}
			return C;
		}
	}

	public enum Regression {
		None, Linear, Parabolic, Qlinear
	}
}
