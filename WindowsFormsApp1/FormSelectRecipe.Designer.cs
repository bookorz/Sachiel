namespace GUI
{
    partial class FormSelectRecipe
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRecipeName = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.btnChange = new System.Windows.Forms.Button();
            this.cbRecipe = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel40.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel40
            // 
            this.panel40.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel40.Controls.Add(this.cbRecipe);
            this.panel40.Controls.Add(this.btnCancel);
            this.panel40.Controls.Add(this.lblRecipeName);
            this.panel40.Controls.Add(this.label3);
            this.panel40.Controls.Add(this.label84);
            this.panel40.Controls.Add(this.btnChange);
            this.panel40.Location = new System.Drawing.Point(12, 12);
            this.panel40.Name = "panel40";
            this.panel40.Size = new System.Drawing.Size(492, 218);
            this.panel40.TabIndex = 66;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.DarkGray;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCancel.FlatAppearance.BorderSize = 2;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnCancel.Location = new System.Drawing.Point(158, 139);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(148, 46);
            this.btnCancel.TabIndex = 67;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblRecipeName
            // 
            this.lblRecipeName.AutoSize = true;
            this.lblRecipeName.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblRecipeName.Location = new System.Drawing.Point(204, 73);
            this.lblRecipeName.Name = "lblRecipeName";
            this.lblRecipeName.Size = new System.Drawing.Size(0, 30);
            this.lblRecipeName.TabIndex = 3;
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label84.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label84.Location = new System.Drawing.Point(25, 25);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(170, 30);
            this.label84.TabIndex = 56;
            this.label84.Text = "Select Recipe:";
            // 
            // btnChange
            // 
            this.btnChange.BackColor = System.Drawing.Color.DarkGray;
            this.btnChange.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnChange.FlatAppearance.BorderSize = 2;
            this.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnChange.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnChange.Location = new System.Drawing.Point(327, 139);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(148, 46);
            this.btnChange.TabIndex = 53;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click_1);
            // 
            // cbRecipe
            // 
            this.cbRecipe.BackColor = System.Drawing.SystemColors.Info;
            this.cbRecipe.Font = new System.Drawing.Font("微軟正黑體", 18F);
            this.cbRecipe.FormattingEnabled = true;
            this.cbRecipe.Items.AddRange(new object[] {
            "Socket",
            "Comport"});
            this.cbRecipe.Location = new System.Drawing.Point(204, 22);
            this.cbRecipe.Name = "cbRecipe";
            this.cbRecipe.Size = new System.Drawing.Size(271, 38);
            this.cbRecipe.TabIndex = 71;
            this.cbRecipe.SelectedIndexChanged += new System.EventHandler(this.cbRecipe_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label3.Location = new System.Drawing.Point(25, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 30);
            this.label3.TabIndex = 56;
            this.label3.Text = "Recipe Name:";
            // 
            // FormSelectRecipe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(527, 242);
            this.Controls.Add(this.panel40);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectRecipe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Recipe";
            this.Load += new System.EventHandler(this.FormSelectRecipe_Load);
            this.panel40.ResumeLayout(false);
            this.panel40.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel40;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRecipeName;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.ComboBox cbRecipe;
        private System.Windows.Forms.Label label3;
    }
}