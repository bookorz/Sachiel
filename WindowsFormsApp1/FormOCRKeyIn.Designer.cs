namespace Adam
{
    partial class FormOCRKeyIn
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
            this.OCR_Img = new System.Windows.Forms.PictureBox();
            this.M12WaferID_tb = new System.Windows.Forms.TextBox();
            this.Info_lb = new System.Windows.Forms.Label();
            this.M12Confirm_btn = new System.Windows.Forms.Button();
            this.T7Confirm_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.T7WaferID_tb = new System.Windows.Forms.TextBox();
            this.M12_panel = new System.Windows.Forms.Panel();
            this.T7_panel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.OCR_Img)).BeginInit();
            this.M12_panel.SuspendLayout();
            this.T7_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OCR_Img
            // 
            this.OCR_Img.Location = new System.Drawing.Point(41, 12);
            this.OCR_Img.Name = "OCR_Img";
            this.OCR_Img.Size = new System.Drawing.Size(1149, 656);
            this.OCR_Img.TabIndex = 1;
            this.OCR_Img.TabStop = false;
            // 
            // M12WaferID_tb
            // 
            this.M12WaferID_tb.Font = new System.Drawing.Font("新細明體", 28F);
            this.M12WaferID_tb.Location = new System.Drawing.Point(282, 5);
            this.M12WaferID_tb.Name = "M12WaferID_tb";
            this.M12WaferID_tb.Size = new System.Drawing.Size(642, 52);
            this.M12WaferID_tb.TabIndex = 2;
            // 
            // Info_lb
            // 
            this.Info_lb.AutoSize = true;
            this.Info_lb.Font = new System.Drawing.Font("新細明體", 28F);
            this.Info_lb.Location = new System.Drawing.Point(3, 13);
            this.Info_lb.Name = "Info_lb";
            this.Info_lb.Size = new System.Drawing.Size(235, 38);
            this.Info_lb.TabIndex = 3;
            this.Info_lb.Text = "M12 OCR Fail";
            // 
            // M12Confirm_btn
            // 
            this.M12Confirm_btn.Font = new System.Drawing.Font("新細明體", 28F);
            this.M12Confirm_btn.Location = new System.Drawing.Point(984, 6);
            this.M12Confirm_btn.Name = "M12Confirm_btn";
            this.M12Confirm_btn.Size = new System.Drawing.Size(175, 51);
            this.M12Confirm_btn.TabIndex = 4;
            this.M12Confirm_btn.Text = "Confirm";
            this.M12Confirm_btn.UseVisualStyleBackColor = true;
            this.M12Confirm_btn.Click += new System.EventHandler(this.M12Confirm_btn_Click);
            // 
            // T7Confirm_btn
            // 
            this.T7Confirm_btn.Font = new System.Drawing.Font("新細明體", 28F);
            this.T7Confirm_btn.Location = new System.Drawing.Point(984, 7);
            this.T7Confirm_btn.Name = "T7Confirm_btn";
            this.T7Confirm_btn.Size = new System.Drawing.Size(175, 51);
            this.T7Confirm_btn.TabIndex = 7;
            this.T7Confirm_btn.Text = "Confirm";
            this.T7Confirm_btn.UseVisualStyleBackColor = true;
            this.T7Confirm_btn.Click += new System.EventHandler(this.T7Confirm_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 28F);
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 38);
            this.label1.TabIndex = 6;
            this.label1.Text = "T7 OCR Fail";
            // 
            // T7WaferID_tb
            // 
            this.T7WaferID_tb.Font = new System.Drawing.Font("新細明體", 28F);
            this.T7WaferID_tb.Location = new System.Drawing.Point(282, 6);
            this.T7WaferID_tb.Name = "T7WaferID_tb";
            this.T7WaferID_tb.Size = new System.Drawing.Size(642, 52);
            this.T7WaferID_tb.TabIndex = 5;
            // 
            // M12_panel
            // 
            this.M12_panel.Controls.Add(this.Info_lb);
            this.M12_panel.Controls.Add(this.M12WaferID_tb);
            this.M12_panel.Controls.Add(this.M12Confirm_btn);
            this.M12_panel.Location = new System.Drawing.Point(27, 684);
            this.M12_panel.Name = "M12_panel";
            this.M12_panel.Size = new System.Drawing.Size(1163, 63);
            this.M12_panel.TabIndex = 8;
            // 
            // T7_panel
            // 
            this.T7_panel.Controls.Add(this.label1);
            this.T7_panel.Controls.Add(this.T7WaferID_tb);
            this.T7_panel.Controls.Add(this.T7Confirm_btn);
            this.T7_panel.Location = new System.Drawing.Point(27, 766);
            this.T7_panel.Name = "T7_panel";
            this.T7_panel.Size = new System.Drawing.Size(1163, 63);
            this.T7_panel.TabIndex = 9;
            // 
            // FormOCRKeyIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 856);
            this.Controls.Add(this.T7_panel);
            this.Controls.Add(this.M12_panel);
            this.Controls.Add(this.OCR_Img);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormOCRKeyIn";
            this.Text = "Manual Key In";
            this.Load += new System.EventHandler(this.FormOCRKeyIn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OCR_Img)).EndInit();
            this.M12_panel.ResumeLayout(false);
            this.M12_panel.PerformLayout();
            this.T7_panel.ResumeLayout(false);
            this.T7_panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox OCR_Img;
        private System.Windows.Forms.TextBox M12WaferID_tb;
        private System.Windows.Forms.Label Info_lb;
        private System.Windows.Forms.Button M12Confirm_btn;
        private System.Windows.Forms.Button T7Confirm_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox T7WaferID_tb;
        private System.Windows.Forms.Panel M12_panel;
        private System.Windows.Forms.Panel T7_panel;
    }
}