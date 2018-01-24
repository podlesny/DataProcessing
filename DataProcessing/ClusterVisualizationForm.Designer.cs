namespace DataProcessing {
    partial class ClusterVisualizationForm {
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.x_select = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.y_select = new System.Windows.Forms.ComboBox();
            this.ok = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Location = new System.Drawing.Point(1, 0);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(586, 478);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            // 
            // x_select
            // 
            this.x_select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.x_select.FormattingEnabled = true;
            this.x_select.Location = new System.Drawing.Point(61, 484);
            this.x_select.Name = "x_select";
            this.x_select.Size = new System.Drawing.Size(72, 21);
            this.x_select.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 487);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Вісь Х";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 487);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Вісь Y";
            // 
            // y_select
            // 
            this.y_select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.y_select.FormattingEnabled = true;
            this.y_select.Location = new System.Drawing.Point(198, 484);
            this.y_select.Name = "y_select";
            this.y_select.Size = new System.Drawing.Size(72, 21);
            this.y_select.TabIndex = 3;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(287, 484);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // ClusterVisualizationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 515);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.y_select);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.x_select);
            this.Controls.Add(this.chart);
            this.Name = "ClusterVisualizationForm";
            this.Text = "ClusterVisualizationForm";
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ComboBox x_select;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox y_select;
        private System.Windows.Forms.Button ok;
    }
}