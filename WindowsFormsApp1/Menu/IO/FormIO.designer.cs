namespace Adam.Menu.IO
{
    partial class FormIO
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
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tabIOControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Digital_I_List = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Digital_O_List = new System.Windows.Forms.Panel();
            this.groupBox8.SuspendLayout();
            this.tabIOControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.tabIOControl1);
            this.groupBox8.Location = new System.Drawing.Point(45, 40);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox8.Size = new System.Drawing.Size(858, 678);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Digital I/O";
            // 
            // tabIOControl1
            // 
            this.tabIOControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabIOControl1.Controls.Add(this.tabPage1);
            this.tabIOControl1.Controls.Add(this.tabPage2);
            this.tabIOControl1.Location = new System.Drawing.Point(12, 47);
            this.tabIOControl1.Margin = new System.Windows.Forms.Padding(5);
            this.tabIOControl1.Name = "tabIOControl1";
            this.tabIOControl1.SelectedIndex = 0;
            this.tabIOControl1.Size = new System.Drawing.Size(836, 626);
            this.tabIOControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Digital_I_List);
            this.tabPage1.Location = new System.Drawing.Point(4, 32);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage1.Size = new System.Drawing.Size(828, 590);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IN";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Digital_I_List
            // 
            this.Digital_I_List.AutoScroll = true;
            this.Digital_I_List.Location = new System.Drawing.Point(5, 8);
            this.Digital_I_List.Margin = new System.Windows.Forms.Padding(5);
            this.Digital_I_List.Name = "Digital_I_List";
            this.Digital_I_List.Size = new System.Drawing.Size(801, 572);
            this.Digital_I_List.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Digital_O_List);
            this.tabPage2.Location = new System.Drawing.Point(4, 32);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(5);
            this.tabPage2.Size = new System.Drawing.Size(828, 628);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "OUT";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Digital_O_List
            // 
            this.Digital_O_List.AutoScroll = true;
            this.Digital_O_List.Location = new System.Drawing.Point(5, 7);
            this.Digital_O_List.Margin = new System.Windows.Forms.Padding(5);
            this.Digital_O_List.Name = "Digital_O_List";
            this.Digital_O_List.Size = new System.Drawing.Size(743, 1150);
            this.Digital_O_List.TabIndex = 1;
            // 
            // FormIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1940, 1358);
            this.Controls.Add(this.groupBox8);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormIO";
            this.Text = "IO";
            this.Load += new System.EventHandler(this.FormIO_Load);
            this.groupBox8.ResumeLayout(false);
            this.tabIOControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TabControl tabIOControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel Digital_I_List;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel Digital_O_List;
    }
}