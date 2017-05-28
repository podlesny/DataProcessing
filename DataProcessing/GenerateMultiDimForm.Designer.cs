namespace DataProcessing {
	partial class GenerateMultiDimForm {
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
			this.label2 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label4 = new System.Windows.Forms.Label();
			this.sig = new System.Windows.Forms.DataGridView();
			this.label3 = new System.Windows.Forms.Label();
			this.Num_ = new System.Windows.Forms.NumericUpDown();
			this.ok = new System.Windows.Forms.Button();
			this.build = new System.Windows.Forms.Button();
			this.sdf = new System.Windows.Forms.Label();
			this.n_ = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.r = new System.Windows.Forms.DataGridView();
			this.E = new System.Windows.Forms.DataGridView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.a0_ = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.erbere = new System.Windows.Forms.Label();
			this.max_ = new System.Windows.Forms.DataGridView();
			this.dghj = new System.Windows.Forms.Label();
			this.min_ = new System.Windows.Forms.DataGridView();
			this.ok_regr = new System.Windows.Forms.Button();
			this.sig_regr = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.Num__ = new System.Windows.Forms.NumericUpDown();
			this.build1 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.n__ = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.avec = new System.Windows.Forms.DataGridView();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sig)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Num_)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.n_)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.r)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.E)).BeginInit();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.max_)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.min_)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Num__)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.n__)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.avec)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 94);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(10, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "r";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(6, 5);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(382, 352);
			this.tabControl1.TabIndex = 10;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.sig);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.Num_);
			this.tabPage1.Controls.Add(this.ok);
			this.tabPage1.Controls.Add(this.build);
			this.tabPage1.Controls.Add(this.sdf);
			this.tabPage1.Controls.Add(this.n_);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.r);
			this.tabPage1.Controls.Add(this.E);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(374, 326);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Нормальний розподіл";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 59);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(14, 13);
			this.label4.TabIndex = 20;
			this.label4.Text = "σ";
			// 
			// sig
			// 
			this.sig.AllowUserToAddRows = false;
			this.sig.AllowUserToDeleteRows = false;
			this.sig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.sig.ColumnHeadersVisible = false;
			this.sig.Location = new System.Drawing.Point(41, 61);
			this.sig.Name = "sig";
			this.sig.RowHeadersVisible = false;
			this.sig.Size = new System.Drawing.Size(313, 22);
			this.sig.TabIndex = 15;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(128, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 13);
			this.label3.TabIndex = 19;
			this.label3.Text = "Обсяг вибірки";
			// 
			// Num_
			// 
			this.Num_.Location = new System.Drawing.Point(210, 7);
			this.Num_.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.Num_.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.Num_.Name = "Num_";
			this.Num_.Size = new System.Drawing.Size(47, 20);
			this.Num_.TabIndex = 11;
			this.Num_.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
			// 
			// ok
			// 
			this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ok.Location = new System.Drawing.Point(143, 288);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(95, 23);
			this.ok.TabIndex = 18;
			this.ok.Text = "ОК";
			this.ok.UseVisualStyleBackColor = true;
			this.ok.Click += new System.EventHandler(this.ok_Click);
			// 
			// build
			// 
			this.build.Location = new System.Drawing.Point(270, 4);
			this.build.Name = "build";
			this.build.Size = new System.Drawing.Size(84, 23);
			this.build.TabIndex = 12;
			this.build.Text = "Побудувати";
			this.build.UseVisualStyleBackColor = true;
			this.build.Click += new System.EventHandler(this.build_Click);
			// 
			// sdf
			// 
			this.sdf.AutoSize = true;
			this.sdf.Location = new System.Drawing.Point(6, 9);
			this.sdf.Name = "sdf";
			this.sdf.Size = new System.Drawing.Size(67, 13);
			this.sdf.TabIndex = 16;
			this.sdf.Text = "Розмірність";
			// 
			// n_
			// 
			this.n_.Location = new System.Drawing.Point(72, 7);
			this.n_.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.n_.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.n_.Name = "n_";
			this.n_.Size = new System.Drawing.Size(47, 20);
			this.n_.TabIndex = 10;
			this.n_.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 37);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(27, 13);
			this.label1.TabIndex = 13;
			this.label1.Text = "E{ξ}";
			// 
			// r
			// 
			this.r.AllowUserToAddRows = false;
			this.r.AllowUserToDeleteRows = false;
			this.r.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.r.ColumnHeadersVisible = false;
			this.r.Location = new System.Drawing.Point(41, 92);
			this.r.Name = "r";
			this.r.RowHeadersVisible = false;
			this.r.Size = new System.Drawing.Size(313, 181);
			this.r.TabIndex = 17;
			// 
			// E
			// 
			this.E.AllowUserToAddRows = false;
			this.E.AllowUserToDeleteRows = false;
			this.E.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.E.ColumnHeadersVisible = false;
			this.E.Location = new System.Drawing.Point(41, 33);
			this.E.Name = "E";
			this.E.RowHeadersVisible = false;
			this.E.Size = new System.Drawing.Size(313, 22);
			this.E.TabIndex = 14;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.a0_);
			this.tabPage2.Controls.Add(this.label9);
			this.tabPage2.Controls.Add(this.erbere);
			this.tabPage2.Controls.Add(this.max_);
			this.tabPage2.Controls.Add(this.dghj);
			this.tabPage2.Controls.Add(this.min_);
			this.tabPage2.Controls.Add(this.ok_regr);
			this.tabPage2.Controls.Add(this.sig_regr);
			this.tabPage2.Controls.Add(this.label8);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Controls.Add(this.Num__);
			this.tabPage2.Controls.Add(this.build1);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.n__);
			this.tabPage2.Controls.Add(this.label7);
			this.tabPage2.Controls.Add(this.avec);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(374, 326);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Рівномірний з регресією";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// a0_
			// 
			this.a0_.Location = new System.Drawing.Point(162, 133);
			this.a0_.Name = "a0_";
			this.a0_.Size = new System.Drawing.Size(50, 20);
			this.a0_.TabIndex = 7;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(127, 136);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(19, 13);
			this.label9.TabIndex = 34;
			this.label9.Text = "a0";
			// 
			// erbere
			// 
			this.erbere.AutoSize = true;
			this.erbere.Location = new System.Drawing.Point(11, 96);
			this.erbere.Name = "erbere";
			this.erbere.Size = new System.Drawing.Size(26, 13);
			this.erbere.TabIndex = 32;
			this.erbere.Text = "max";
			// 
			// max_
			// 
			this.max_.AllowUserToAddRows = false;
			this.max_.AllowUserToDeleteRows = false;
			this.max_.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.max_.ColumnHeadersVisible = false;
			this.max_.Location = new System.Drawing.Point(46, 92);
			this.max_.Name = "max_";
			this.max_.RowHeadersVisible = false;
			this.max_.Size = new System.Drawing.Size(313, 22);
			this.max_.TabIndex = 5;
			// 
			// dghj
			// 
			this.dghj.AutoSize = true;
			this.dghj.Location = new System.Drawing.Point(11, 68);
			this.dghj.Name = "dghj";
			this.dghj.Size = new System.Drawing.Size(23, 13);
			this.dghj.TabIndex = 30;
			this.dghj.Text = "min";
			// 
			// min_
			// 
			this.min_.AllowUserToAddRows = false;
			this.min_.AllowUserToDeleteRows = false;
			this.min_.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.min_.ColumnHeadersVisible = false;
			this.min_.Location = new System.Drawing.Point(46, 64);
			this.min_.Name = "min_";
			this.min_.RowHeadersVisible = false;
			this.min_.Size = new System.Drawing.Size(313, 22);
			this.min_.TabIndex = 4;
			// 
			// ok_regr
			// 
			this.ok_regr.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ok_regr.Location = new System.Drawing.Point(136, 284);
			this.ok_regr.Name = "ok_regr";
			this.ok_regr.Size = new System.Drawing.Size(95, 23);
			this.ok_regr.TabIndex = 8;
			this.ok_regr.Text = "ОК";
			this.ok_regr.UseVisualStyleBackColor = true;
			this.ok_regr.Click += new System.EventHandler(this.ok_regr_Click);
			// 
			// sig_regr
			// 
			this.sig_regr.Location = new System.Drawing.Point(46, 133);
			this.sig_regr.Name = "sig_regr";
			this.sig_regr.Size = new System.Drawing.Size(50, 20);
			this.sig_regr.TabIndex = 6;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(11, 136);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(14, 13);
			this.label8.TabIndex = 27;
			this.label8.Text = "σ";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(133, 13);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(79, 13);
			this.label5.TabIndex = 26;
			this.label5.Text = "Обсяг вибірки";
			// 
			// Num__
			// 
			this.Num__.Location = new System.Drawing.Point(215, 10);
			this.Num__.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.Num__.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.Num__.Name = "Num__";
			this.Num__.Size = new System.Drawing.Size(47, 20);
			this.Num__.TabIndex = 1;
			this.Num__.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
			// 
			// build1
			// 
			this.build1.Location = new System.Drawing.Point(275, 7);
			this.build1.Name = "build1";
			this.build1.Size = new System.Drawing.Size(84, 23);
			this.build1.TabIndex = 2;
			this.build1.Text = "Побудувати";
			this.build1.UseVisualStyleBackColor = true;
			this.build1.Click += new System.EventHandler(this.build1_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(11, 12);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(67, 13);
			this.label6.TabIndex = 25;
			this.label6.Text = "Розмірність";
			// 
			// n__
			// 
			this.n__.Location = new System.Drawing.Point(77, 10);
			this.n__.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.n__.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.n__.Name = "n__";
			this.n__.Size = new System.Drawing.Size(47, 20);
			this.n__.TabIndex = 0;
			this.n__.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(11, 40);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(13, 13);
			this.label7.TabIndex = 23;
			this.label7.Text = "a";
			// 
			// avec
			// 
			this.avec.AllowUserToAddRows = false;
			this.avec.AllowUserToDeleteRows = false;
			this.avec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.avec.ColumnHeadersVisible = false;
			this.avec.Location = new System.Drawing.Point(46, 36);
			this.avec.Name = "avec";
			this.avec.RowHeadersVisible = false;
			this.avec.Size = new System.Drawing.Size(313, 22);
			this.avec.TabIndex = 3;
			// 
			// GenerateMultiDimForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(391, 360);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.label2);
			this.Name = "GenerateMultiDimForm";
			this.Text = "GenerateMultiDimForm";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.sig)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Num_)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.n_)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.r)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.E)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.max_)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.min_)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Num__)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.n__)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.avec)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.DataGridView sig;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown Num_;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button build;
		private System.Windows.Forms.Label sdf;
		private System.Windows.Forms.NumericUpDown n_;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGridView r;
		private System.Windows.Forms.DataGridView E;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button ok_regr;
		private System.Windows.Forms.TextBox sig_regr;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown Num__;
		private System.Windows.Forms.Button build1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.NumericUpDown n__;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.DataGridView avec;
		private System.Windows.Forms.Label erbere;
		private System.Windows.Forms.DataGridView max_;
		private System.Windows.Forms.Label dghj;
		private System.Windows.Forms.DataGridView min_;
		private System.Windows.Forms.TextBox a0_;
		private System.Windows.Forms.Label label9;
	}
}