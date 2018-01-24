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
	public partial class TtestForm : Form {
		public TtestForm(int n, double[]par, ref bool b) {
			InitializeComponent();
			this.b = b;
			this.n = n;
			this.par = par;
			switch (n) {
				case 0: {
						t2.Size = new Size(100, 20);
						t2.Location = new Point(74, 19);
						t1.Size = new Size(100, 20);
						t1.Location = new Point(74, 45);
						l1.Location = new Point(23, 22);
						l1.Text = "σ";
						l2.Location = new Point(23, 48);
						l2.Text = "m";
						Controls.Add(t1);
						Controls.Add(t2);
						Controls.Add(l1);
						Controls.Add(l2);
					}
					break;
				case 1: {
						t1.Size = new Size(100, 20);
						t1.Location = new Point(74, 19);
						l1.Location = new Point(23, 22);
						l1.Text = "λ";
						Controls.Add(t1);
						Controls.Add(l1);
					}
					break;
				case 2: {
						t1.Size = new Size(100, 20);
						t1.Location = new Point(74, 19);
						l1.Location = new Point(23, 22);
						l1.Text = "a";
						Controls.Add(t1);
						Controls.Add(l1);
					}
					break;
				case 3: {
						t1.Location = new Point(74, 19);
						t1.Size = new Size(100, 20);
						t2.Location = new Point(74, 45);
						t2.Size = new Size(100, 20);
						l1.Location = new Point(23, 22);
						l1.Text = "a";
						l2.Location = new Point(23, 48);
						l2.Text = "b";
						Controls.Add(t1);
						Controls.Add(t2);
						Controls.Add(l1);
						Controls.Add(l2);
					}
					break;
				case 4: {
						t1.Location = new Point(74, 19);
						t1.Size = new Size(100, 20);
						t2.Location = new Point(74, 45);
						t2.Size = new Size(100, 20);
						t3.Location = new Point(74, 71);
						t3.Size = new Size(100, 20);
						l1.Location = new Point(23, 22);
						l1.Text = "a";
						l2.Location = new Point(23, 48);
						l2.Text = "b";
						l3.Location = new Point(23, 74);
						l3.Text = "c";
						Controls.Add(t1);
						Controls.Add(t2);
						Controls.Add(t3);
						Controls.Add(l1);
						Controls.Add(l2);
						Controls.Add(l3);
					}
					break;
			}
		}
		TextBox t1 = new TextBox(), t2 = new TextBox(), t3 = new TextBox();

		Label l1 = new Label(), l2 = new Label(), l3 = new Label();

		int n;

		bool b;

		double[] par;
		private void button1_Click(object sender, EventArgs e) {
			if(n == 0 || n == 3) {
				if (!double.TryParse(t1.Text.Replace('.', ','), out par[0]) || !double.TryParse(t2.Text.Replace('.', ','), out par[1])) {
					MessageBox.Show("Некоректний ввід");
					b = false;
				}
				else b = true;
			}
			else if(n == 4) {
				if (!double.TryParse(t1.Text.Replace('.', ','), out par[0]) || !double.TryParse(t2.Text.Replace('.', ','), out par[1]) || !double.TryParse(t3.Text.Replace('.', ','), out par[2])) {
					MessageBox.Show("Некоректний ввід");
					b = false;
				}
				else b = true;
			}
			else {
				if (!double.TryParse(t1.Text.Replace('.', ','), out par[0])) {
					MessageBox.Show("Некоректний ввід");
					b = false;
				}
				else b = true;
			}
		}
	}
}
