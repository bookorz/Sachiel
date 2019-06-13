namespace Adam.Menu.DifferentialMonitor
{
    partial class FormDifferentialMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartDifferential = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CurrentVal_lb = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.FFU_rpm_lb = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartDifferential)).BeginInit();
            this.SuspendLayout();
            // 
            // chartDifferential
            // 
            chartArea1.BorderColor = System.Drawing.Color.DarkGray;
            chartArea1.Name = "ChartArea1";
            this.chartDifferential.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartDifferential.Legends.Add(legend1);
            this.chartDifferential.Location = new System.Drawing.Point(12, 12);
            this.chartDifferential.Name = "chartDifferential";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartDifferential.Series.Add(series1);
            this.chartDifferential.Size = new System.Drawing.Size(1596, 736);
            this.chartDifferential.TabIndex = 0;
            this.chartDifferential.Text = "chart1";
            // 
            // CurrentVal_lb
            // 
            this.CurrentVal_lb.AutoSize = true;
            this.CurrentVal_lb.Location = new System.Drawing.Point(1536, 79);
            this.CurrentVal_lb.Name = "CurrentVal_lb";
            this.CurrentVal_lb.Size = new System.Drawing.Size(54, 20);
            this.CurrentVal_lb.TabIndex = 1;
            this.CurrentVal_lb.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1424, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "當前數值(Pa):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1424, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "FFU轉速(rpm):";
            // 
            // FFU_rpm_lb
            // 
            this.FFU_rpm_lb.AutoSize = true;
            this.FFU_rpm_lb.Location = new System.Drawing.Point(1536, 142);
            this.FFU_rpm_lb.Name = "FFU_rpm_lb";
            this.FFU_rpm_lb.Size = new System.Drawing.Size(54, 20);
            this.FFU_rpm_lb.TabIndex = 3;
            this.FFU_rpm_lb.Text = "label1";
            // 
            // FormDifferentialMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1620, 760);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FFU_rpm_lb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CurrentVal_lb);
            this.Controls.Add(this.chartDifferential);
            this.Name = "FormDifferentialMonitor";
            this.Text = "FormTestMode";
            ((System.ComponentModel.ISupportInitialize)(this.chartDifferential)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartDifferential;
        private System.Windows.Forms.Label CurrentVal_lb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label FFU_rpm_lb;
    }
}