namespace DataProcessing {
	partial class CheckSelParamForm {
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
			this.addList = new System.Windows.Forms.ComboBox();
			this.methList = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.ok = new System.Windows.Forms.Button();
			this.add = new System.Windows.Forms.Button();
			this.delete = new System.Windows.Forms.Button();
			this.delList = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// addList
			// 
			this.addList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.addList.FormattingEnabled = true;
			this.addList.Location = new System.Drawing.Point(104, 12);
			this.addList.Name = "addList";
			this.addList.Size = new System.Drawing.Size(210, 21);
			this.addList.TabIndex = 0;
			// 
			// methList
			// 
			this.methList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.methList.FormattingEnabled = true;
			this.methList.Location = new System.Drawing.Point(104, 91);
			this.methList.Name = "methList";
			this.methList.Size = new System.Drawing.Size(210, 21);
			this.methList.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(33, 94);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(24, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Дія";
			// 
			// ok
			// 
			this.ok.Location = new System.Drawing.Point(134, 128);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(75, 23);
			this.ok.TabIndex = 6;
			this.ok.Text = "ОК";
			this.ok.UseVisualStyleBackColor = true;
			this.ok.Click += new System.EventHandler(this.ok_Click);
			// 
			// add
			// 
			this.add.Location = new System.Drawing.Point(12, 12);
			this.add.Name = "add";
			this.add.Size = new System.Drawing.Size(75, 23);
			this.add.TabIndex = 7;
			this.add.Text = "Добавити";
			this.add.UseVisualStyleBackColor = true;
			this.add.Click += new System.EventHandler(this.add_Click);
			// 
			// delete
			// 
			this.delete.Location = new System.Drawing.Point(12, 50);
			this.delete.Name = "delete";
			this.delete.Size = new System.Drawing.Size(75, 23);
			this.delete.TabIndex = 9;
			this.delete.Text = "Видалити";
			this.delete.UseVisualStyleBackColor = true;
			this.delete.Click += new System.EventHandler(this.delete_Click);
			// 
			// delList
			// 
			this.delList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.delList.FormattingEnabled = true;
			this.delList.Location = new System.Drawing.Point(104, 50);
			this.delList.Name = "delList";
			this.delList.Size = new System.Drawing.Size(210, 21);
			this.delList.TabIndex = 8;
			// 
			// CheckSelParamForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(326, 157);
			this.Controls.Add(this.delete);
			this.Controls.Add(this.delList);
			this.Controls.Add(this.add);
			this.Controls.Add(this.ok);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.methList);
			this.Controls.Add(this.addList);
			this.Name = "CheckSelParamForm";
			this.Text = "Аналіз n вибірок";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox addList;
		private System.Windows.Forms.ComboBox methList;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button add;
		private System.Windows.Forms.Button delete;
		private System.Windows.Forms.ComboBox delList;
	}
}