namespace Adam.Menu.SystemSetting
{
    partial class FormSysConfig
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
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbFakeData = new System.Windows.Forms.CheckBox();
            this.cbChkBypass = new System.Windows.Forms.CheckBox();
            this.tbOcr1Exe = new System.Windows.Forms.TextBox();
            this.tbOcr1ImgBak = new System.Windows.Forms.TextBox();
            this.tbTxfLogPath = new System.Windows.Forms.TextBox();
            this.cbRecipe = new System.Windows.Forms.ComboBox();
            this.tbEqpId = new System.Windows.Forms.TextBox();
            this.tbSysMode = new System.Windows.Forms.TextBox();
            this.tbConnString = new System.Windows.Forms.TextBox();
            this.tbOcr1ImgSrc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Setting_Port_lb = new System.Windows.Forms.Label();
            this.setting_Address_lb = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.DarkGray;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSave.FlatAppearance.BorderSize = 2;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("微軟正黑體", 14.25F);
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(1089, 495);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(201, 46);
            this.btnSave.TabIndex = 95;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbFakeData);
            this.groupBox2.Controls.Add(this.cbChkBypass);
            this.groupBox2.Controls.Add(this.tbOcr1Exe);
            this.groupBox2.Controls.Add(this.tbOcr1ImgBak);
            this.groupBox2.Controls.Add(this.tbTxfLogPath);
            this.groupBox2.Controls.Add(this.cbRecipe);
            this.groupBox2.Controls.Add(this.tbEqpId);
            this.groupBox2.Controls.Add(this.tbSysMode);
            this.groupBox2.Controls.Add(this.tbConnString);
            this.groupBox2.Controls.Add(this.tbOcr1ImgSrc);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.Setting_Port_lb);
            this.groupBox2.Controls.Add(this.setting_Address_lb);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1278, 462);
            this.groupBox2.TabIndex = 96;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controller";
            // 
            // cbFakeData
            // 
            this.cbFakeData.AutoSize = true;
            this.cbFakeData.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.cbFakeData.Checked = true;
            this.cbFakeData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFakeData.Enabled = false;
            this.cbFakeData.Location = new System.Drawing.Point(399, 360);
            this.cbFakeData.Name = "cbFakeData";
            this.cbFakeData.Size = new System.Drawing.Size(143, 28);
            this.cbFakeData.TabIndex = 12;
            this.cbFakeData.Text = "使用虛擬資料";
            this.cbFakeData.UseVisualStyleBackColor = false;
            // 
            // cbChkBypass
            // 
            this.cbChkBypass.AutoSize = true;
            this.cbChkBypass.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.cbChkBypass.Checked = true;
            this.cbChkBypass.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbChkBypass.Enabled = false;
            this.cbChkBypass.Location = new System.Drawing.Point(250, 360);
            this.cbChkBypass.Name = "cbChkBypass";
            this.cbChkBypass.Size = new System.Drawing.Size(143, 28);
            this.cbChkBypass.TabIndex = 12;
            this.cbChkBypass.Text = "忽略安全檢查";
            this.cbChkBypass.UseVisualStyleBackColor = false;
            // 
            // tbOcr1Exe
            // 
            this.tbOcr1Exe.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tbOcr1Exe.Enabled = false;
            this.tbOcr1Exe.Location = new System.Drawing.Point(250, 180);
            this.tbOcr1Exe.Name = "tbOcr1Exe";
            this.tbOcr1Exe.Size = new System.Drawing.Size(894, 33);
            this.tbOcr1Exe.TabIndex = 11;
            // 
            // tbOcr1ImgBak
            // 
            this.tbOcr1ImgBak.BackColor = System.Drawing.SystemColors.Info;
            this.tbOcr1ImgBak.Location = new System.Drawing.Point(250, 270);
            this.tbOcr1ImgBak.Name = "tbOcr1ImgBak";
            this.tbOcr1ImgBak.Size = new System.Drawing.Size(368, 33);
            this.tbOcr1ImgBak.TabIndex = 11;
            // 
            // tbTxfLogPath
            // 
            this.tbTxfLogPath.BackColor = System.Drawing.SystemColors.Info;
            this.tbTxfLogPath.Location = new System.Drawing.Point(250, 315);
            this.tbTxfLogPath.Name = "tbTxfLogPath";
            this.tbTxfLogPath.Size = new System.Drawing.Size(368, 33);
            this.tbTxfLogPath.TabIndex = 11;
            // 
            // cbRecipe
            // 
            this.cbRecipe.BackColor = System.Drawing.SystemColors.Info;
            this.cbRecipe.FormattingEnabled = true;
            this.cbRecipe.Items.AddRange(new object[] {
            "Socket",
            "Comport"});
            this.cbRecipe.Location = new System.Drawing.Point(250, 135);
            this.cbRecipe.Name = "cbRecipe";
            this.cbRecipe.Size = new System.Drawing.Size(121, 32);
            this.cbRecipe.TabIndex = 9;
            // 
            // tbEqpId
            // 
            this.tbEqpId.BackColor = System.Drawing.SystemColors.Info;
            this.tbEqpId.Location = new System.Drawing.Point(250, 90);
            this.tbEqpId.Name = "tbEqpId";
            this.tbEqpId.Size = new System.Drawing.Size(161, 33);
            this.tbEqpId.TabIndex = 11;
            // 
            // tbSysMode
            // 
            this.tbSysMode.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tbSysMode.Enabled = false;
            this.tbSysMode.Location = new System.Drawing.Point(250, 45);
            this.tbSysMode.Name = "tbSysMode";
            this.tbSysMode.Size = new System.Drawing.Size(161, 33);
            this.tbSysMode.TabIndex = 11;
            // 
            // tbConnString
            // 
            this.tbConnString.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tbConnString.Enabled = false;
            this.tbConnString.Location = new System.Drawing.Point(250, 405);
            this.tbConnString.Name = "tbConnString";
            this.tbConnString.Size = new System.Drawing.Size(894, 33);
            this.tbConnString.TabIndex = 10;
            // 
            // tbOcr1ImgSrc
            // 
            this.tbOcr1ImgSrc.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.tbOcr1ImgSrc.Enabled = false;
            this.tbOcr1ImgSrc.Location = new System.Drawing.Point(250, 225);
            this.tbOcr1ImgSrc.Name = "tbOcr1ImgSrc";
            this.tbOcr1ImgSrc.Size = new System.Drawing.Size(368, 33);
            this.tbOcr1ImgSrc.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(99, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 24);
            this.label3.TabIndex = 0;
            this.label3.Text = "SystemMode:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(115, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 24);
            this.label5.TabIndex = 8;
            this.label5.Text = "生產 Recipe:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "機台名稱:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 318);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "生產檔案輸出路徑:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "OCR1 程式路徑:";
            // 
            // Setting_Port_lb
            // 
            this.Setting_Port_lb.AutoSize = true;
            this.Setting_Port_lb.Location = new System.Drawing.Point(48, 273);
            this.Setting_Port_lb.Name = "Setting_Port_lb";
            this.Setting_Port_lb.Size = new System.Drawing.Size(184, 24);
            this.Setting_Port_lb.TabIndex = 8;
            this.Setting_Port_lb.Text = "OCR1 照片備份路徑:";
            // 
            // setting_Address_lb
            // 
            this.setting_Address_lb.AutoSize = true;
            this.setting_Address_lb.Location = new System.Drawing.Point(86, 228);
            this.setting_Address_lb.Name = "setting_Address_lb";
            this.setting_Address_lb.Size = new System.Drawing.Size(146, 24);
            this.setting_Address_lb.TabIndex = 2;
            this.setting_Address_lb.Text = "OCR1 照片來源:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(115, 408);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(117, 24);
            this.label10.TabIndex = 6;
            this.label10.Text = "DB connect:";
            // 
            // FormSysConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1432, 752);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormSysConfig";
            this.Text = "FormSysConfig";
            this.Load += new System.EventHandler(this.FormSysConfig_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbSysMode;
        private System.Windows.Forms.TextBox tbOcr1ImgSrc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbRecipe;
        private System.Windows.Forms.Label Setting_Port_lb;
        private System.Windows.Forms.Label setting_Address_lb;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbOcr1Exe;
        private System.Windows.Forms.TextBox tbOcr1ImgBak;
        private System.Windows.Forms.TextBox tbTxfLogPath;
        private System.Windows.Forms.TextBox tbConnString;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbFakeData;
        private System.Windows.Forms.CheckBox cbChkBypass;
        private System.Windows.Forms.TextBox tbEqpId;
    }
}