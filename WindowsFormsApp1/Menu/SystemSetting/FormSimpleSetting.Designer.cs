namespace Adam.Menu.SystemSetting
{
    partial class FormSimpleSetting
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
            this.dgvMysql = new System.Windows.Forms.DataGridView();
            this.btnUpdate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMysql)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMysql
            // 
            this.dgvMysql.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMysql.Location = new System.Drawing.Point(14, 4);
            this.dgvMysql.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.dgvMysql.Name = "dgvMysql";
            this.dgvMysql.RowTemplate.Height = 24;
            this.dgvMysql.Size = new System.Drawing.Size(1392, 607);
            this.dgvMysql.TabIndex = 7;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnUpdate.Location = new System.Drawing.Point(1134, 631);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(272, 75);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update Data";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // FormSimpleSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1420, 720);
            this.Controls.Add(this.dgvMysql);
            this.Controls.Add(this.btnUpdate);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FormSimpleSetting";
            this.Text = "FormSimpleSetting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSimpleSetting_FormClosing);
            this.Load += new System.EventHandler(this.FormSimpleSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMysql)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMysql;
        private System.Windows.Forms.Button btnUpdate;
    }
}