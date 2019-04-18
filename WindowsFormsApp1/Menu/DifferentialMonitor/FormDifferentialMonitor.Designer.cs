namespace Adam.Menu.RunningScreen
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartDifferential = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CurrentVal_lb = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartDifferential)).BeginInit();
            this.SuspendLayout();
            // 
            // chartDifferential
            // 
            chartArea3.BorderColor = System.Drawing.Color.DarkGray;
            chartArea3.Name = "ChartArea1";
            this.chartDifferential.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartDifferential.Legends.Add(legend3);
            this.chartDifferential.Location = new System.Drawing.Point(12, 12);
            this.chartDifferential.Name = "chartDifferential";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartDifferential.Series.Add(series3);
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
            // FormDifferentialMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1620, 760);
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
    }
}