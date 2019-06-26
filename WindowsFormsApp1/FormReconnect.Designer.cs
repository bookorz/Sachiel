namespace Adam
{
    partial class FormReconnect
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
            this.EMO_lb = new System.Windows.Forms.Label();
            this.Countdown_lb = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // EMO_lb
            // 
            this.EMO_lb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EMO_lb.AutoSize = true;
            this.EMO_lb.Font = new System.Drawing.Font("新細明體", 24F);
            this.EMO_lb.Location = new System.Drawing.Point(624, 557);
            this.EMO_lb.Name = "EMO_lb";
            this.EMO_lb.Size = new System.Drawing.Size(698, 96);
            this.EMO_lb.TabIndex = 0;
            this.EMO_lb.Text = "EMO button has been activated!\r\n\r\nplease release the EMO button and push the rese" +
    "t button.";
            this.EMO_lb.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Countdown_lb
            // 
            this.Countdown_lb.AutoSize = true;
            this.Countdown_lb.Font = new System.Drawing.Font("新細明體", 24F);
            this.Countdown_lb.Location = new System.Drawing.Point(624, 673);
            this.Countdown_lb.Name = "Countdown_lb";
            this.Countdown_lb.Size = new System.Drawing.Size(674, 32);
            this.Countdown_lb.TabIndex = 2;
            this.Countdown_lb.Text = "Reconnecting to all equipment, please wait for seconds";
            this.Countdown_lb.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Adam.Properties.Resources._21736064411375_205;
            this.pictureBox1.Location = new System.Drawing.Point(746, 333);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(442, 198);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // FormReconnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1655, 861);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Countdown_lb);
            this.Controls.Add(this.EMO_lb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormReconnect";
            this.Text = "FormReconnect";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormReconnect_FormClosing);
            this.Load += new System.EventHandler(this.FormReconnect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EMO_lb;
        private System.Windows.Forms.Label Countdown_lb;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}