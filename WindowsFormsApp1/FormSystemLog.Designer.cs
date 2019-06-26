namespace Adam
{
    partial class FormSystemLog
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
            this.log_rt = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // log_rt
            // 
            this.log_rt.Font = new System.Drawing.Font("新細明體", 12F);
            this.log_rt.Location = new System.Drawing.Point(12, 12);
            this.log_rt.Name = "log_rt";
            this.log_rt.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.log_rt.Size = new System.Drawing.Size(1374, 795);
            this.log_rt.TabIndex = 0;
            this.log_rt.Text = "";
            // 
            // FormSystemLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1396, 812);
            this.Controls.Add(this.log_rt);
            this.Name = "FormSystemLog";
            this.Text = "FormSystemLog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSystemLog_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox log_rt;
    }
}