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
            this.WaferID_tb = new System.Windows.Forms.TextBox();
            this.Info_lb = new System.Windows.Forms.Label();
            this.Confirm_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OCR_Img)).BeginInit();
            this.SuspendLayout();
            // 
            // OCR_Img
            // 
            this.OCR_Img.Location = new System.Drawing.Point(41, 12);
            this.OCR_Img.Name = "OCR_Img";
            this.OCR_Img.Size = new System.Drawing.Size(1287, 741);
            this.OCR_Img.TabIndex = 1;
            this.OCR_Img.TabStop = false;
            // 
            // WaferID_tb
            // 
            this.WaferID_tb.Font = new System.Drawing.Font("新細明體", 28F);
            this.WaferID_tb.Location = new System.Drawing.Point(313, 771);
            this.WaferID_tb.Name = "WaferID_tb";
            this.WaferID_tb.Size = new System.Drawing.Size(801, 52);
            this.WaferID_tb.TabIndex = 2;
            // 
            // Info_lb
            // 
            this.Info_lb.AutoSize = true;
            this.Info_lb.Font = new System.Drawing.Font("新細明體", 28F);
            this.Info_lb.Location = new System.Drawing.Point(34, 779);
            this.Info_lb.Name = "Info_lb";
            this.Info_lb.Size = new System.Drawing.Size(235, 38);
            this.Info_lb.TabIndex = 3;
            this.Info_lb.Text = "M12 OCR Fail";
            // 
            // Confirm_btn
            // 
            this.Confirm_btn.Font = new System.Drawing.Font("新細明體", 28F);
            this.Confirm_btn.Location = new System.Drawing.Point(1153, 772);
            this.Confirm_btn.Name = "Confirm_btn";
            this.Confirm_btn.Size = new System.Drawing.Size(175, 51);
            this.Confirm_btn.TabIndex = 4;
            this.Confirm_btn.Text = "Confirm";
            this.Confirm_btn.UseVisualStyleBackColor = true;
            this.Confirm_btn.Click += new System.EventHandler(this.Confirm_btn_Click);
            // 
            // FormOCRKeyIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1371, 856);
            this.Controls.Add(this.Confirm_btn);
            this.Controls.Add(this.Info_lb);
            this.Controls.Add(this.WaferID_tb);
            this.Controls.Add(this.OCR_Img);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormOCRKeyIn";
            this.Text = "Manual Key In";
            this.Load += new System.EventHandler(this.FormOCRKeyIn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OCR_Img)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox OCR_Img;
        private System.Windows.Forms.TextBox WaferID_tb;
        private System.Windows.Forms.Label Info_lb;
        private System.Windows.Forms.Button Confirm_btn;
    }
}