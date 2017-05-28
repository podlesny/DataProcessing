namespace DataProcessing {
	partial class SettingsForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.sigLevel = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.alpha = new System.Windows.Forms.TextBox();
			this.shiftDist = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.logBase = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.ok = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lm_t = new System.Windows.Forms.TextBox();
			this.sig_t = new System.Windows.Forms.TextBox();
			this.m_t = new System.Windows.Forms.TextBox();
			this.bet_t = new System.Windows.Forms.TextBox();
			this.al_t = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.classNum_t = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.sigx_t = new System.Windows.Forms.TextBox();
			this.mx_t = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.sigy_t = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.my_t = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.r_t = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.pgrad_t = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.Mx__t = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.My__t = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.at = new System.Windows.Forms.TextBox();
			this.bt = new System.Windows.Forms.TextBox();
			this.ct = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// sigLevel
			// 
			this.sigLevel.Location = new System.Drawing.Point(159, 17);
			this.sigLevel.Name = "sigLevel";
			this.sigLevel.Size = new System.Drawing.Size(53, 20);
			this.sigLevel.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(19, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(99, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Рівень значущості";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(19, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "α(усічене середнє)";
			// 
			// alpha
			// 
			this.alpha.Location = new System.Drawing.Point(159, 49);
			this.alpha.Name = "alpha";
			this.alpha.Size = new System.Drawing.Size(53, 20);
			this.alpha.TabIndex = 1;
			// 
			// shiftDist
			// 
			this.shiftDist.Location = new System.Drawing.Point(159, 82);
			this.shiftDist.Name = "shiftDist";
			this.shiftDist.Size = new System.Drawing.Size(53, 20);
			this.shiftDist.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(19, 85);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(109, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Відстань для здвигу";
			// 
			// logBase
			// 
			this.logBase.Location = new System.Drawing.Point(159, 117);
			this.logBase.Name = "logBase";
			this.logBase.Size = new System.Drawing.Size(53, 20);
			this.logBase.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(19, 120);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(134, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Основа логарифмування";
			// 
			// ok
			// 
			this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ok.Location = new System.Drawing.Point(43, 408);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(64, 23);
			this.ok.TabIndex = 8;
			this.ok.Text = "OK";
			this.ok.UseVisualStyleBackColor = true;
			this.ok.Click += new System.EventHandler(this.ok_Click);
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(132, 408);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(64, 23);
			this.cancel.TabIndex = 9;
			this.cancel.Text = "Відмінити";
			this.cancel.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(19, 197);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(15, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "m";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(17, 230);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(14, 13);
			this.label6.TabIndex = 11;
			this.label6.Text = "σ";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(19, 265);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(12, 13);
			this.label7.TabIndex = 12;
			this.label7.Text = "λ";
			// 
			// lm_t
			// 
			this.lm_t.Location = new System.Drawing.Point(159, 258);
			this.lm_t.Name = "lm_t";
			this.lm_t.Size = new System.Drawing.Size(53, 20);
			this.lm_t.TabIndex = 7;
			// 
			// sig_t
			// 
			this.sig_t.Location = new System.Drawing.Point(159, 223);
			this.sig_t.Name = "sig_t";
			this.sig_t.Size = new System.Drawing.Size(53, 20);
			this.sig_t.TabIndex = 6;
			// 
			// m_t
			// 
			this.m_t.Location = new System.Drawing.Point(159, 190);
			this.m_t.Name = "m_t";
			this.m_t.Size = new System.Drawing.Size(53, 20);
			this.m_t.TabIndex = 5;
			// 
			// bet_t
			// 
			this.bet_t.Location = new System.Drawing.Point(159, 328);
			this.bet_t.Name = "bet_t";
			this.bet_t.Size = new System.Drawing.Size(53, 20);
			this.bet_t.TabIndex = 9;
			// 
			// al_t
			// 
			this.al_t.Location = new System.Drawing.Point(159, 293);
			this.al_t.Name = "al_t";
			this.al_t.Size = new System.Drawing.Size(53, 20);
			this.al_t.TabIndex = 8;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(19, 339);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(13, 13);
			this.label8.TabIndex = 17;
			this.label8.Text = "β";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(17, 300);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(14, 13);
			this.label9.TabIndex = 16;
			this.label9.Text = "α";
			// 
			// classNum_t
			// 
			this.classNum_t.Location = new System.Drawing.Point(159, 153);
			this.classNum_t.Name = "classNum_t";
			this.classNum_t.Size = new System.Drawing.Size(53, 20);
			this.classNum_t.TabIndex = 4;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(19, 156);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(88, 13);
			this.label10.TabIndex = 20;
			this.label10.Text = "Кількість класів";
			// 
			// sigx_t
			// 
			this.sigx_t.Location = new System.Drawing.Point(160, 67);
			this.sigx_t.Name = "sigx_t";
			this.sigx_t.Size = new System.Drawing.Size(53, 20);
			this.sigx_t.TabIndex = 12;
			// 
			// mx_t
			// 
			this.mx_t.Location = new System.Drawing.Point(160, 6);
			this.mx_t.Name = "mx_t";
			this.mx_t.Size = new System.Drawing.Size(53, 20);
			this.mx_t.TabIndex = 10;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(18, 74);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(19, 13);
			this.label11.TabIndex = 23;
			this.label11.Text = "σx";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(18, 13);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(20, 13);
			this.label12.TabIndex = 22;
			this.label12.Text = "mx";
			// 
			// sigy_t
			// 
			this.sigy_t.Location = new System.Drawing.Point(160, 102);
			this.sigy_t.Name = "sigy_t";
			this.sigy_t.Size = new System.Drawing.Size(53, 20);
			this.sigy_t.TabIndex = 13;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(18, 109);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(19, 13);
			this.label13.TabIndex = 26;
			this.label13.Text = "σy";
			// 
			// my_t
			// 
			this.my_t.Location = new System.Drawing.Point(160, 34);
			this.my_t.Name = "my_t";
			this.my_t.Size = new System.Drawing.Size(53, 20);
			this.my_t.TabIndex = 11;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(18, 45);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(20, 13);
			this.label14.TabIndex = 28;
			this.label14.Text = "my";
			// 
			// r_t
			// 
			this.r_t.Location = new System.Drawing.Point(160, 142);
			this.r_t.Name = "r_t";
			this.r_t.Size = new System.Drawing.Size(53, 20);
			this.r_t.TabIndex = 14;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(20, 145);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(29, 13);
			this.label15.TabIndex = 30;
			this.label15.Text = "r(x,y)";
			// 
			// pgrad_t
			// 
			this.pgrad_t.Location = new System.Drawing.Point(160, 247);
			this.pgrad_t.Name = "pgrad_t";
			this.pgrad_t.Size = new System.Drawing.Size(53, 20);
			this.pgrad_t.TabIndex = 17;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(18, 250);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(100, 13);
			this.label16.TabIndex = 32;
			this.label16.Text = "Класи ймовірності";
			// 
			// Mx__t
			// 
			this.Mx__t.Location = new System.Drawing.Point(160, 179);
			this.Mx__t.Name = "Mx__t";
			this.Mx__t.Size = new System.Drawing.Size(53, 20);
			this.Mx__t.TabIndex = 15;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(18, 182);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(63, 13);
			this.label17.TabIndex = 34;
			this.label17.Text = "Класи по Х";
			// 
			// My__t
			// 
			this.My__t.Location = new System.Drawing.Point(160, 212);
			this.My__t.Name = "My__t";
			this.My__t.Size = new System.Drawing.Size(53, 20);
			this.My__t.TabIndex = 16;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(18, 215);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(63, 13);
			this.label18.TabIndex = 36;
			this.label18.Text = "Класи по Y";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Location = new System.Drawing.Point(4, 3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(237, 399);
			this.tabControl1.TabIndex = 37;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.sigLevel);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.alpha);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.shiftDist);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.logBase);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.label7);
			this.tabPage1.Controls.Add(this.m_t);
			this.tabPage1.Controls.Add(this.sig_t);
			this.tabPage1.Controls.Add(this.lm_t);
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Controls.Add(this.label8);
			this.tabPage1.Controls.Add(this.al_t);
			this.tabPage1.Controls.Add(this.classNum_t);
			this.tabPage1.Controls.Add(this.bet_t);
			this.tabPage1.Controls.Add(this.label10);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(229, 373);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "1-вимірний аналіз";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.mx_t);
			this.tabPage2.Controls.Add(this.My__t);
			this.tabPage2.Controls.Add(this.label12);
			this.tabPage2.Controls.Add(this.label18);
			this.tabPage2.Controls.Add(this.label11);
			this.tabPage2.Controls.Add(this.Mx__t);
			this.tabPage2.Controls.Add(this.sigx_t);
			this.tabPage2.Controls.Add(this.label17);
			this.tabPage2.Controls.Add(this.label13);
			this.tabPage2.Controls.Add(this.pgrad_t);
			this.tabPage2.Controls.Add(this.sigy_t);
			this.tabPage2.Controls.Add(this.label16);
			this.tabPage2.Controls.Add(this.label14);
			this.tabPage2.Controls.Add(this.r_t);
			this.tabPage2.Controls.Add(this.my_t);
			this.tabPage2.Controls.Add(this.label15);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(229, 373);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "2-вимірний аналіз";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.groupBox1);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(229, 373);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Регресії";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.label20);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.ct);
			this.groupBox1.Controls.Add(this.bt);
			this.groupBox1.Controls.Add(this.at);
			this.groupBox1.Location = new System.Drawing.Point(4, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(218, 103);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Параболічна";
			// 
			// at
			// 
			this.at.Location = new System.Drawing.Point(35, 19);
			this.at.Name = "at";
			this.at.Size = new System.Drawing.Size(36, 20);
			this.at.TabIndex = 0;
			// 
			// bt
			// 
			this.bt.Location = new System.Drawing.Point(35, 45);
			this.bt.Name = "bt";
			this.bt.Size = new System.Drawing.Size(36, 20);
			this.bt.TabIndex = 1;
			// 
			// ct
			// 
			this.ct.Location = new System.Drawing.Point(35, 71);
			this.ct.Name = "ct";
			this.ct.Size = new System.Drawing.Size(36, 20);
			this.ct.TabIndex = 2;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(16, 22);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(13, 13);
			this.label19.TabIndex = 3;
			this.label19.Text = "a";
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(16, 48);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(13, 13);
			this.label20.TabIndex = 4;
			this.label20.Text = "b";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(16, 74);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(13, 13);
			this.label21.TabIndex = 5;
			this.label21.Text = "c";
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(242, 440);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.ok);
			this.Name = "SettingsForm";
			this.Text = "Установки";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox sigLevel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox alpha;
		private System.Windows.Forms.TextBox shiftDist;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox logBase;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox lm_t;
		private System.Windows.Forms.TextBox sig_t;
		private System.Windows.Forms.TextBox m_t;
		private System.Windows.Forms.TextBox bet_t;
		private System.Windows.Forms.TextBox al_t;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox classNum_t;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox sigx_t;
		private System.Windows.Forms.TextBox mx_t;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox sigy_t;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox my_t;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox r_t;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox pgrad_t;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.TextBox Mx__t;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.TextBox My__t;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.TextBox ct;
		private System.Windows.Forms.TextBox bt;
		private System.Windows.Forms.TextBox at;
	}
}