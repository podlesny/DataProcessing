namespace DataProcessing {
	partial class GenerateForm {
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
			this.label1 = new System.Windows.Forms.Label();
			this.M = new System.Windows.Forms.TextBox();
			this.N = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.low = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.up = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.chs = new System.Windows.Forms.ComboBox();
			this.input = new System.Windows.Forms.TextBox();
			this.generate = new System.Windows.Forms.Button();
			this.ok = new System.Windows.Forms.Button();
			this.cancel_btn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(71, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Згенерувати";
			// 
			// M
			// 
			this.M.Location = new System.Drawing.Point(88, 12);
			this.M.Name = "M";
			this.M.Size = new System.Drawing.Size(32, 20);
			this.M.TabIndex = 1;
			// 
			// N
			// 
			this.N.Location = new System.Drawing.Point(182, 12);
			this.N.Name = "N";
			this.N.Size = new System.Drawing.Size(32, 20);
			this.N.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(128, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "рядків по";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(218, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "чисел від";
			// 
			// low
			// 
			this.low.Location = new System.Drawing.Point(272, 12);
			this.low.Name = "low";
			this.low.Size = new System.Drawing.Size(32, 20);
			this.low.TabIndex = 5;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(309, 15);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(19, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "до";
			// 
			// up
			// 
			this.up.Location = new System.Drawing.Point(332, 12);
			this.up.Name = "up";
			this.up.Size = new System.Drawing.Size(32, 20);
			this.up.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(365, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(26, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Тип";
			// 
			// chs
			// 
			this.chs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.chs.FormattingEnabled = true;
			this.chs.Items.AddRange(new object[] {
			"Нормальний",
			"Експоненціальний",
			"Вейбулла",
			"Нормальний 2-вимірний",
			"Рівномірний 2-вимірний(параб.регр)",
			"Рівномірний 2-вимірний(квазіл.регр)",
			"—"});
			this.chs.Location = new System.Drawing.Point(391, 12);
			this.chs.Name = "chs";
			this.chs.Size = new System.Drawing.Size(195, 21);
			this.chs.TabIndex = 9;
			// 
			// input
			// 
			this.input.Location = new System.Drawing.Point(16, 38);
			this.input.Multiline = true;
			this.input.Name = "input";
			this.input.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.input.Size = new System.Drawing.Size(570, 160);
			this.input.TabIndex = 10;
			// 
			// generate
			// 
			this.generate.Location = new System.Drawing.Point(23, 204);
			this.generate.Name = "generate";
			this.generate.Size = new System.Drawing.Size(97, 23);
			this.generate.TabIndex = 11;
			this.generate.Text = "Згенерувати";
			this.generate.UseVisualStyleBackColor = true;
			this.generate.Click += new System.EventHandler(this.generate_Click);
			// 
			// ok
			// 
			this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ok.Location = new System.Drawing.Point(261, 204);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(97, 23);
			this.ok.TabIndex = 12;
			this.ok.Text = "OK";
			this.ok.UseVisualStyleBackColor = true;
			this.ok.Click += new System.EventHandler(this.ok_Click);
			// 
			// cancel_btn
			// 
			this.cancel_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel_btn.Location = new System.Drawing.Point(489, 204);
			this.cancel_btn.Name = "cancel_btn";
			this.cancel_btn.Size = new System.Drawing.Size(97, 23);
			this.cancel_btn.TabIndex = 13;
			this.cancel_btn.Text = "Відмінити";
			this.cancel_btn.UseVisualStyleBackColor = true;
			this.cancel_btn.Click += new System.EventHandler(this.cancel_btn_Click);
			// 
			// AddManuallyForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(598, 237);
			this.Controls.Add(this.cancel_btn);
			this.Controls.Add(this.ok);
			this.Controls.Add(this.generate);
			this.Controls.Add(this.input);
			this.Controls.Add(this.chs);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.up);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.low);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.N);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.M);
			this.Controls.Add(this.label1);
			this.Name = "AddManuallyForm";
			this.Text = "Згенерувати дані";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox M;
		private System.Windows.Forms.TextBox N;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox low;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox up;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox chs;
		private System.Windows.Forms.TextBox input;
		private System.Windows.Forms.Button generate;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button cancel_btn;
	}
}