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
	public partial class SettingsForm : Form {
		public SettingsForm(Settings set_) {
			InitializeComponent();
			set = set_;
			this.ControlBox = false;
		}
		Settings set;
		private void ok_Click(object sender, EventArgs e) {
			try {
				if (sigLevel.Text != "") {
					set.sigLevel = double.Parse(sigLevel.Text.Replace('.', ','));
					if (set.sigLevel <= 0 || set.sigLevel >= 1) {
						MessageBox.Show("Невірно заданий рівень значущості(0 < α < 1)");
						return;
					}
				}
				if (alpha.Text != "") {
					set.alpha = double.Parse(alpha.Text.Replace('.', ','));
					if (set.alpha < 0 || set.alpha >= 0.5) {
						set.alpha = -1;
						MessageBox.Show("Невірно заданий порядок усіченого середнього(0 < α < 0.5)");
						return;
					}
				}
				if (shiftDist.Text != "") {
					set.shiftDist = double.Parse(shiftDist.Text.Replace('.', ','));
				}
				if (logBase.Text != "") {
					set.logBase = double.Parse(logBase.Text.Replace('.', ','));
					if (set.logBase <= 1 || set.logBase > 10) {
						MessageBox.Show("Невірно задана основа логарифмування(0 < n <= 10)");
						return;
					}
				}
				if (pgrad_t.Text != "") {
					set.pgrad = int.Parse(pgrad_t.Text.Replace('.', ','));
					if (set.pgrad < 2 || set.pgrad > 15) {
						MessageBox.Show("Невірно задана кількість класів ймовірності(2 < n <= 15)");
						return;
					}
				}
				if (Mx__t.Text != "") {
					set.Mx = int.Parse(Mx__t.Text);
					if (set.Mx < 2 || set.Mx > 100) {
						set.Mx = -1;
						MessageBox.Show("Невірно задана кількість класів (2 <= Mx <= 100)");
						return;
					}
				}
				if (My__t.Text != "") {
					set.My = int.Parse(My__t.Text);
					if (set.My < 2 || set.My > 100) {
						set.My = -1;
						MessageBox.Show("Невірно задана кількість класів (2 <= Mx <= 100)");
						return;
					}
				}
				if (m_t.Text != "") {
					set.m = double.Parse(m_t.Text.Replace('.', ','));
				}
				if (lm_t.Text != "") {
					set.lm = double.Parse(lm_t.Text.Replace('.', ','));
				}
				if (sig_t.Text != "") {
					set.sig = double.Parse(sig_t.Text.Replace('.', ','));
				}
				if (al_t.Text != "") {
					set.al = double.Parse(al_t.Text.Replace('.', ','));
				}
				if (bet_t.Text != "") {
					set.bet = double.Parse(bet_t.Text.Replace('.', ','));
				}
				if (classNum_t.Text != "") {
					set.classNum = int.Parse(classNum_t.Text);
				}
				if (mx_t.Text != "") {
					set.mx = double.Parse(mx_t.Text.Replace('.', ','));
				}
				if (my_t.Text != "") {
					set.my = double.Parse(my_t.Text.Replace('.', ','));
				}
				if (sigx_t.Text != "") {
					set.sigx = double.Parse(sigx_t.Text.Replace('.', ','));
				}
				if (sigy_t.Text != "") {
					set.sigy = double.Parse(sigy_t.Text.Replace('.', ','));
				}
				if (r_t.Text != "") {
					set.r = double.Parse(r_t.Text.Replace('.', ','));
				}
				if (at.Text != "") {
					set.ar = double.Parse(at.Text.Replace('.', ','));
				}
				if (bt.Text != "") {
					set.br = double.Parse(bt.Text.Replace('.', ','));
				}
				if (ct.Text != "") {
					set.cr = double.Parse(ct.Text.Replace('.', ','));
				}
			}
			catch(Exception) {
				MessageBox.Show("Некоректно введені дані");
				ok.DialogResult = DialogResult.Cancel;
			}
		}
	}
}
