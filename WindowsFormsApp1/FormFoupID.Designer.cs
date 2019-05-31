namespace Adam
{
    partial class FormFoupID
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
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.Clear_btn = new System.Windows.Forms.Button();
            this.LoadportName_lb = new System.Windows.Forms.Label();
            this.FoupID_Read_tb = new System.Windows.Forms.TextBox();
            this.FoupID_Read_Confirm_btn = new System.Windows.Forms.Button();
            this.FoupID_Status_lb = new System.Windows.Forms.Label();
            this.ManualInput_ck = new System.Windows.Forms.CheckBox();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.ManualInput_ck);
            this.groupBox7.Controls.Add(this.Clear_btn);
            this.groupBox7.Controls.Add(this.LoadportName_lb);
            this.groupBox7.Controls.Add(this.FoupID_Read_tb);
            this.groupBox7.Controls.Add(this.FoupID_Read_Confirm_btn);
            this.groupBox7.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox7.Location = new System.Drawing.Point(173, 126);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(377, 190);
            this.groupBox7.TabIndex = 126;
            this.groupBox7.TabStop = false;
            this.groupBox7.Tag = "1";
            this.groupBox7.Text = "Foup ID";
            // 
            // Clear_btn
            // 
            this.Clear_btn.BackColor = System.Drawing.Color.DarkGray;
            this.Clear_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Clear_btn.FlatAppearance.BorderSize = 2;
            this.Clear_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Clear_btn.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Clear_btn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Clear_btn.Location = new System.Drawing.Point(250, 119);
            this.Clear_btn.Name = "Clear_btn";
            this.Clear_btn.Size = new System.Drawing.Size(86, 35);
            this.Clear_btn.TabIndex = 124;
            this.Clear_btn.Text = "Clear";
            this.Clear_btn.UseVisualStyleBackColor = false;
            this.Clear_btn.Click += new System.EventHandler(this.Clear_btn_Click);
            // 
            // LoadportName_lb
            // 
            this.LoadportName_lb.AutoSize = true;
            this.LoadportName_lb.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LoadportName_lb.Location = new System.Drawing.Point(129, -2);
            this.LoadportName_lb.Name = "LoadportName_lb";
            this.LoadportName_lb.Size = new System.Drawing.Size(148, 31);
            this.LoadportName_lb.TabIndex = 127;
            this.LoadportName_lb.Text = "Loadport02";
            this.LoadportName_lb.TextChanged += new System.EventHandler(this.LoadportName_lb_TextChanged);
            // 
            // FoupID_Read_tb
            // 
            this.FoupID_Read_tb.Location = new System.Drawing.Point(38, 67);
            this.FoupID_Read_tb.Name = "FoupID_Read_tb";
            this.FoupID_Read_tb.Size = new System.Drawing.Size(298, 33);
            this.FoupID_Read_tb.TabIndex = 123;
            this.FoupID_Read_tb.TextChanged += new System.EventHandler(this.FoupID_Read_tb_TextChanged);
            this.FoupID_Read_tb.Leave += new System.EventHandler(this.FoupID_Read_tb_Leave);
            // 
            // FoupID_Read_Confirm_btn
            // 
            this.FoupID_Read_Confirm_btn.BackColor = System.Drawing.Color.DarkGray;
            this.FoupID_Read_Confirm_btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.FoupID_Read_Confirm_btn.FlatAppearance.BorderSize = 2;
            this.FoupID_Read_Confirm_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.FoupID_Read_Confirm_btn.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FoupID_Read_Confirm_btn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.FoupID_Read_Confirm_btn.Location = new System.Drawing.Point(135, 119);
            this.FoupID_Read_Confirm_btn.Name = "FoupID_Read_Confirm_btn";
            this.FoupID_Read_Confirm_btn.Size = new System.Drawing.Size(86, 35);
            this.FoupID_Read_Confirm_btn.TabIndex = 122;
            this.FoupID_Read_Confirm_btn.Text = "Confirm";
            this.FoupID_Read_Confirm_btn.UseVisualStyleBackColor = false;
            this.FoupID_Read_Confirm_btn.Click += new System.EventHandler(this.FoupID_Read_Confirm_btn_Click);
            // 
            // FoupID_Status_lb
            // 
            this.FoupID_Status_lb.AutoSize = true;
            this.FoupID_Status_lb.Font = new System.Drawing.Font("新細明體", 12F);
            this.FoupID_Status_lb.Location = new System.Drawing.Point(257, 409);
            this.FoupID_Status_lb.Name = "FoupID_Status_lb";
            this.FoupID_Status_lb.Size = new System.Drawing.Size(156, 16);
            this.FoupID_Status_lb.TabIndex = 128;
            this.FoupID_Status_lb.Text = "Waiting Barcode reader";
            // 
            // ManualInput_ck
            // 
            this.ManualInput_ck.AutoSize = true;
            this.ManualInput_ck.Location = new System.Drawing.Point(38, 33);
            this.ManualInput_ck.Name = "ManualInput_ck";
            this.ManualInput_ck.Size = new System.Drawing.Size(149, 28);
            this.ManualInput_ck.TabIndex = 128;
            this.ManualInput_ck.Text = "Manual input";
            this.ManualInput_ck.UseVisualStyleBackColor = true;
            // 
            // FormFoupID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 483);
            this.Controls.Add(this.FoupID_Status_lb);
            this.Controls.Add(this.groupBox7);
            this.Name = "FormFoupID";
            this.Text = "FoupID";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFoupID_FormClosing);
            this.Load += new System.EventHandler(this.FormFoupID_Load);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button FoupID_Read_Confirm_btn;
        private System.Windows.Forms.Label LoadportName_lb;
        private System.Windows.Forms.Label FoupID_Status_lb;
        private System.Windows.Forms.TextBox FoupID_Read_tb;
        private System.Windows.Forms.Button Clear_btn;
        private System.Windows.Forms.CheckBox ManualInput_ck;
    }
}