namespace DataProcessing {
	partial class ReduceDimForm {
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
			this.table = new System.Windows.Forms.DataGridView();
			this.del = new System.Windows.Forms.Button();
			this.ok = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
			this.SuspendLayout();
			// 
			// table
			// 
			this.table.AllowUserToAddRows = false;
			this.table.AllowUserToDeleteRows = false;
			this.table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Name = "table";
			this.table.ReadOnly = true;
			this.table.Size = new System.Drawing.Size(797, 208);
			this.table.TabIndex = 0;
			// 
			// del
			// 
			this.del.Location = new System.Drawing.Point(188, 214);
			this.del.Name = "del";
			this.del.Size = new System.Drawing.Size(106, 23);
			this.del.TabIndex = 1;
			this.del.Text = "Видалити вектор";
			this.del.UseVisualStyleBackColor = true;
			this.del.Click += new System.EventHandler(this.del_Click);
			// 
			// ok
			// 
			this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ok.Location = new System.Drawing.Point(467, 214);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(106, 23);
			this.ok.TabIndex = 2;
			this.ok.Text = "ОК";
			this.ok.UseVisualStyleBackColor = true;
			// 
			// ReduceDimForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(798, 244);
			this.Controls.Add(this.ok);
			this.Controls.Add(this.del);
			this.Controls.Add(this.table);
			this.Name = "ReduceDimForm";
			this.Text = "ReduceDimForm";
			((System.ComponentModel.ISupportInitialize)(this.table)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView table;
		private System.Windows.Forms.Button del;
		private System.Windows.Forms.Button ok;
	}
}