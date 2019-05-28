namespace GUI
{
    partial class FormChgPwd
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
            this.panel40 = new System.Windows.Forms.Panel();
            this.tbConfirmPwd = new System.Windows.Forms.TextBox();
            this.tbNewPwd = new System.Windows.Forms.TextBox();
            this.tbUserID = new System.Windows.Forms.TextBox();
            this.tbOldPwd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.btnChange = new System.Windows.Forms.Button();
            this.panel40.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel40
            // 
            this.panel40.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel40.Controls.Add(this.tbConfirmPwd);
            this.panel40.Controls.Add(this.tbNewPwd);
            this.panel40.Controls.Add(this.tbUserID);
            this.panel40.Controls.Add(this.tbOldPwd);
            this.panel40.Controls.Add(this.label2);
            this.panel40.Controls.Add(this.btnCancel);
            this.panel40.Controls.Add(this.label1);
            this.panel40.Controls.Add(this.label80);
            this.panel40.Controls.Add(this.label84);
            this.panel40.Controls.Add(this.btnChange);
            this.panel40.Location = new System.Drawing.Point(12, 12);
            this.panel40.Name = "panel40";
            this.panel40.Size = new System.Drawing.Size(492, 301);
            this.panel40.TabIndex = 66;
            // 
            // tbConfirmPwd
            // 
            this.tbConfirmPwd.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbConfirmPwd.Location = new System.Drawing.Point(209, 177);
            this.tbConfirmPwd.Name = "tbConfirmPwd";
            this.tbConfirmPwd.PasswordChar = '*';
            this.tbConfirmPwd.Size = new System.Drawing.Size(266, 39);
            this.tbConfirmPwd.TabIndex = 69;
            // 
            // tbNewPwd
            // 
            this.tbNewPwd.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbNewPwd.Location = new System.Drawing.Point(209, 123);
            this.tbNewPwd.Name = "tbNewPwd";
            this.tbNewPwd.PasswordChar = '*';
            this.tbNewPwd.Size = new System.Drawing.Size(266, 39);
            this.tbNewPwd.TabIndex = 69;
            // 
            // tbUserID
            // 
            this.tbUserID.Enabled = false;
            this.tbUserID.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbUserID.Location = new System.Drawing.Point(209, 22);
            this.tbUserID.Name = "tbUserID";
            this.tbUserID.Size = new System.Drawing.Size(266, 39);
            this.tbUserID.TabIndex = 69;
            // 
            // tbOldPwd
            // 
            this.tbOldPwd.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbOldPwd.Location = new System.Drawing.Point(209, 71);
            this.tbOldPwd.Name = "tbOldPwd";
            this.tbOldPwd.PasswordChar = '*';
            this.tbOldPwd.Size = new System.Drawing.Size(266, 39);
            this.tbOldPwd.TabIndex = 69;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(36, 177);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 30);
            this.label2.TabIndex = 3;
            this.label2.Text = "Confirm New";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DarkGray;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.FlatAppearance.BorderSize = 2;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(163, 236);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(148, 46);
            this.btnCancel.TabIndex = 67;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(25, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 30);
            this.label1.TabIndex = 3;
            this.label1.Text = "New Password";
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label80.Location = new System.Drawing.Point(36, 74);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(167, 30);
            this.label80.TabIndex = 3;
            this.label80.Text = "Old Password";
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label84.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label84.Location = new System.Drawing.Point(107, 25);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(96, 30);
            this.label84.TabIndex = 56;
            this.label84.Text = "User ID";
            // 
            // btnChange
            // 
            this.btnChange.BackColor = System.Drawing.Color.DarkGray;
            this.btnChange.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnChange.FlatAppearance.BorderSize = 2;
            this.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnChange.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnChange.Location = new System.Drawing.Point(327, 236);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(148, 46);
            this.btnChange.TabIndex = 53;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // FormChgPwd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(516, 324);
            this.Controls.Add(this.panel40);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChgPwd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Password";
            this.panel40.ResumeLayout(false);
            this.panel40.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel40;
        private System.Windows.Forms.TextBox tbConfirmPwd;
        private System.Windows.Forms.TextBox tbNewPwd;
        private System.Windows.Forms.TextBox tbUserID;
        private System.Windows.Forms.TextBox tbOldPwd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Button btnChange;
    }
}