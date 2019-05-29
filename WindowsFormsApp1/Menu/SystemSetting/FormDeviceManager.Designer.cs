namespace Adam.Menu.SystemSetting
{
    partial class FormDeviceManager
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbNodeManager = new System.Windows.Forms.GroupBox();
            this.splitContainerNode = new System.Windows.Forms.SplitContainer();
            this.gbCondition = new System.Windows.Forms.GroupBox();
            this.lstNodeList = new System.Windows.Forms.ListBox();
            this.splitContainerSetting = new System.Windows.Forms.SplitContainer();
            this.lbDeviceNodeManager = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Setting_Port_tb = new System.Windows.Forms.TextBox();
            this.Setting_Address_tb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Setting_connectType_cb = new System.Windows.Forms.ComboBox();
            this.Setting_ControllerName_lb = new System.Windows.Forms.Label();
            this.Setting_Port_lb = new System.Windows.Forms.Label();
            this.setting_Address_lb = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Setting_CarrierType_cb = new System.Windows.Forms.ComboBox();
            this.Setting_NodeName_lb = new System.Windows.Forms.Label();
            this.Setting_CarrierType_lb = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Setting_NodeType_lb = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Setting_NodeDisable_rb = new System.Windows.Forms.RadioButton();
            this.Setting_NodeEnable_rb = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.Setting_Mode_cb = new System.Windows.Forms.ComboBox();
            this.Setting_Mode_lb = new System.Windows.Forms.Label();
            this.gbNodeManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNode)).BeginInit();
            this.splitContainerNode.Panel1.SuspendLayout();
            this.splitContainerNode.Panel2.SuspendLayout();
            this.splitContainerNode.SuspendLayout();
            this.gbCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSetting)).BeginInit();
            this.splitContainerSetting.Panel1.SuspendLayout();
            this.splitContainerSetting.Panel2.SuspendLayout();
            this.splitContainerSetting.SuspendLayout();
            this.lbDeviceNodeManager.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbNodeManager
            // 
            this.gbNodeManager.Controls.Add(this.splitContainerNode);
            this.gbNodeManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbNodeManager.Location = new System.Drawing.Point(0, 0);
            this.gbNodeManager.Name = "gbNodeManager";
            this.gbNodeManager.Size = new System.Drawing.Size(1420, 740);
            this.gbNodeManager.TabIndex = 67;
            this.gbNodeManager.TabStop = false;
            this.gbNodeManager.Text = "Device node manager";
            // 
            // splitContainerNode
            // 
            this.splitContainerNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerNode.Location = new System.Drawing.Point(3, 25);
            this.splitContainerNode.Name = "splitContainerNode";
            // 
            // splitContainerNode.Panel1
            // 
            this.splitContainerNode.Panel1.Controls.Add(this.gbCondition);
            // 
            // splitContainerNode.Panel2
            // 
            this.splitContainerNode.Panel2.Controls.Add(this.splitContainerSetting);
            this.splitContainerNode.Size = new System.Drawing.Size(1414, 712);
            this.splitContainerNode.SplitterDistance = 331;
            this.splitContainerNode.TabIndex = 0;
            // 
            // gbCondition
            // 
            this.gbCondition.Controls.Add(this.lstNodeList);
            this.gbCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCondition.Location = new System.Drawing.Point(0, 0);
            this.gbCondition.Name = "gbCondition";
            this.gbCondition.Size = new System.Drawing.Size(331, 712);
            this.gbCondition.TabIndex = 0;
            this.gbCondition.TabStop = false;
            this.gbCondition.Text = "Device node list";
            // 
            // lstNodeList
            // 
            this.lstNodeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNodeList.FormattingEnabled = true;
            this.lstNodeList.ItemHeight = 20;
            this.lstNodeList.Location = new System.Drawing.Point(3, 25);
            this.lstNodeList.Name = "lstNodeList";
            this.lstNodeList.Size = new System.Drawing.Size(325, 684);
            this.lstNodeList.TabIndex = 0;
            this.lstNodeList.Click += new System.EventHandler(this.lstNodeList_Click);
            // 
            // splitContainerSetting
            // 
            this.splitContainerSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSetting.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSetting.Name = "splitContainerSetting";
            this.splitContainerSetting.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerSetting.Panel1
            // 
            this.splitContainerSetting.Panel1.Controls.Add(this.lbDeviceNodeManager);
            // 
            // splitContainerSetting.Panel2
            // 
            this.splitContainerSetting.Panel2.Controls.Add(this.tlpButton);
            this.splitContainerSetting.Size = new System.Drawing.Size(1079, 712);
            this.splitContainerSetting.SplitterDistance = 644;
            this.splitContainerSetting.TabIndex = 0;
            // 
            // lbDeviceNodeManager
            // 
            this.lbDeviceNodeManager.Controls.Add(this.groupBox2);
            this.lbDeviceNodeManager.Controls.Add(this.groupBox1);
            this.lbDeviceNodeManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDeviceNodeManager.Location = new System.Drawing.Point(0, 0);
            this.lbDeviceNodeManager.Name = "lbDeviceNodeManager";
            this.lbDeviceNodeManager.Size = new System.Drawing.Size(1079, 644);
            this.lbDeviceNodeManager.TabIndex = 0;
            this.lbDeviceNodeManager.TabStop = false;
            this.lbDeviceNodeManager.Text = "Device node setting";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Setting_Port_tb);
            this.groupBox2.Controls.Add(this.Setting_Address_tb);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Setting_connectType_cb);
            this.groupBox2.Controls.Add(this.Setting_ControllerName_lb);
            this.groupBox2.Controls.Add(this.Setting_Port_lb);
            this.groupBox2.Controls.Add(this.setting_Address_lb);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(459, 42);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(403, 351);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controller";
            // 
            // Setting_Port_tb
            // 
            this.Setting_Port_tb.Location = new System.Drawing.Point(193, 157);
            this.Setting_Port_tb.Name = "Setting_Port_tb";
            this.Setting_Port_tb.Size = new System.Drawing.Size(161, 29);
            this.Setting_Port_tb.TabIndex = 11;
            // 
            // Setting_Address_tb
            // 
            this.Setting_Address_tb.Location = new System.Drawing.Point(193, 119);
            this.Setting_Address_tb.Name = "Setting_Address_tb";
            this.Setting_Address_tb.Size = new System.Drawing.Size(161, 29);
            this.Setting_Address_tb.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Controller Name:";
            // 
            // Setting_connectType_cb
            // 
            this.Setting_connectType_cb.FormattingEnabled = true;
            this.Setting_connectType_cb.Items.AddRange(new object[] {
            "Socket",
            "Comport"});
            this.Setting_connectType_cb.Location = new System.Drawing.Point(193, 83);
            this.Setting_connectType_cb.Name = "Setting_connectType_cb";
            this.Setting_connectType_cb.Size = new System.Drawing.Size(121, 28);
            this.Setting_connectType_cb.TabIndex = 9;
            this.Setting_connectType_cb.SelectedIndexChanged += new System.EventHandler(this.Setting_connectType_cb_SelectedIndexChanged);
            // 
            // Setting_ControllerName_lb
            // 
            this.Setting_ControllerName_lb.AutoSize = true;
            this.Setting_ControllerName_lb.Location = new System.Drawing.Point(189, 49);
            this.Setting_ControllerName_lb.Name = "Setting_ControllerName_lb";
            this.Setting_ControllerName_lb.Size = new System.Drawing.Size(199, 20);
            this.Setting_ControllerName_lb.TabIndex = 1;
            this.Setting_ControllerName_lb.Text = "Controller_Name_Display";
            // 
            // Setting_Port_lb
            // 
            this.Setting_Port_lb.AutoSize = true;
            this.Setting_Port_lb.Location = new System.Drawing.Point(35, 160);
            this.Setting_Port_lb.Name = "Setting_Port_lb";
            this.Setting_Port_lb.Size = new System.Drawing.Size(45, 20);
            this.Setting_Port_lb.TabIndex = 8;
            this.Setting_Port_lb.Text = "Port:";
            // 
            // setting_Address_lb
            // 
            this.setting_Address_lb.AutoSize = true;
            this.setting_Address_lb.Location = new System.Drawing.Point(35, 125);
            this.setting_Address_lb.Name = "setting_Address_lb";
            this.setting_Address_lb.Size = new System.Drawing.Size(73, 20);
            this.setting_Address_lb.TabIndex = 2;
            this.setting_Address_lb.Text = "Address:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(35, 86);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(117, 20);
            this.label10.TabIndex = 6;
            this.label10.Text = "Connect Type:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Setting_Mode_cb);
            this.groupBox1.Controls.Add(this.Setting_Mode_lb);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Setting_CarrierType_cb);
            this.groupBox1.Controls.Add(this.Setting_NodeName_lb);
            this.groupBox1.Controls.Add(this.Setting_CarrierType_lb);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Setting_NodeType_lb);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(35, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(402, 586);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Node";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Node Name:";
            // 
            // Setting_CarrierType_cb
            // 
            this.Setting_CarrierType_cb.FormattingEnabled = true;
            this.Setting_CarrierType_cb.Items.AddRange(new object[] {
            "FOSB",
            "OPEN",
            "FOUP"});
            this.Setting_CarrierType_cb.Location = new System.Drawing.Point(171, 163);
            this.Setting_CarrierType_cb.Name = "Setting_CarrierType_cb";
            this.Setting_CarrierType_cb.Size = new System.Drawing.Size(121, 28);
            this.Setting_CarrierType_cb.TabIndex = 9;
            this.Setting_CarrierType_cb.Visible = false;
            // 
            // Setting_NodeName_lb
            // 
            this.Setting_NodeName_lb.AutoSize = true;
            this.Setting_NodeName_lb.Location = new System.Drawing.Point(169, 49);
            this.Setting_NodeName_lb.Name = "Setting_NodeName_lb";
            this.Setting_NodeName_lb.Size = new System.Drawing.Size(165, 20);
            this.Setting_NodeName_lb.TabIndex = 1;
            this.Setting_NodeName_lb.Text = "Node_Name_Display";
            // 
            // Setting_CarrierType_lb
            // 
            this.Setting_CarrierType_lb.AutoSize = true;
            this.Setting_CarrierType_lb.Location = new System.Drawing.Point(35, 166);
            this.Setting_CarrierType_lb.Name = "Setting_CarrierType_lb";
            this.Setting_CarrierType_lb.Size = new System.Drawing.Size(104, 20);
            this.Setting_CarrierType_lb.TabIndex = 8;
            this.Setting_CarrierType_lb.Text = "Carrier Type:";
            this.Setting_CarrierType_lb.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Node Enabled:";
            // 
            // Setting_NodeType_lb
            // 
            this.Setting_NodeType_lb.AutoSize = true;
            this.Setting_NodeType_lb.Location = new System.Drawing.Point(169, 86);
            this.Setting_NodeType_lb.Name = "Setting_NodeType_lb";
            this.Setting_NodeType_lb.Size = new System.Drawing.Size(155, 20);
            this.Setting_NodeType_lb.TabIndex = 7;
            this.Setting_NodeType_lb.Text = "Node_Type_Display";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Setting_NodeDisable_rb);
            this.panel1.Controls.Add(this.Setting_NodeEnable_rb);
            this.panel1.Location = new System.Drawing.Point(171, 121);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(174, 32);
            this.panel1.TabIndex = 5;
            // 
            // Setting_NodeDisable_rb
            // 
            this.Setting_NodeDisable_rb.AutoSize = true;
            this.Setting_NodeDisable_rb.Location = new System.Drawing.Point(87, 3);
            this.Setting_NodeDisable_rb.Name = "Setting_NodeDisable_rb";
            this.Setting_NodeDisable_rb.Size = new System.Drawing.Size(82, 24);
            this.Setting_NodeDisable_rb.TabIndex = 4;
            this.Setting_NodeDisable_rb.TabStop = true;
            this.Setting_NodeDisable_rb.Text = "Disable";
            this.Setting_NodeDisable_rb.UseVisualStyleBackColor = true;
            // 
            // Setting_NodeEnable_rb
            // 
            this.Setting_NodeEnable_rb.AutoSize = true;
            this.Setting_NodeEnable_rb.Location = new System.Drawing.Point(3, 3);
            this.Setting_NodeEnable_rb.Name = "Setting_NodeEnable_rb";
            this.Setting_NodeEnable_rb.Size = new System.Drawing.Size(78, 24);
            this.Setting_NodeEnable_rb.TabIndex = 3;
            this.Setting_NodeEnable_rb.TabStop = true;
            this.Setting_NodeEnable_rb.Text = "Enable";
            this.Setting_NodeEnable_rb.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Node Type:";
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpButton.Controls.Add(this.btnSave, 4, 0);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 1;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.Size = new System.Drawing.Size(1079, 64);
            this.tlpButton.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(863, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(213, 58);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Setting_Mode_cb
            // 
            this.Setting_Mode_cb.FormattingEnabled = true;
            this.Setting_Mode_cb.Items.AddRange(new object[] {
            "LD",
            "ULD"});
            this.Setting_Mode_cb.Location = new System.Drawing.Point(171, 204);
            this.Setting_Mode_cb.Name = "Setting_Mode_cb";
            this.Setting_Mode_cb.Size = new System.Drawing.Size(121, 28);
            this.Setting_Mode_cb.TabIndex = 11;
            this.Setting_Mode_cb.Visible = false;
            // 
            // Setting_Mode_lb
            // 
            this.Setting_Mode_lb.AutoSize = true;
            this.Setting_Mode_lb.Location = new System.Drawing.Point(35, 207);
            this.Setting_Mode_lb.Name = "Setting_Mode_lb";
            this.Setting_Mode_lb.Size = new System.Drawing.Size(57, 20);
            this.Setting_Mode_lb.TabIndex = 10;
            this.Setting_Mode_lb.Text = "Mode:";
            this.Setting_Mode_lb.Visible = false;
            // 
            // FormDeviceManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1420, 740);
            this.Controls.Add(this.gbNodeManager);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormDeviceManager";
            this.Text = "Device Manager";
            this.Load += new System.EventHandler(this.FormDeviceManager_Load);
            this.gbNodeManager.ResumeLayout(false);
            this.splitContainerNode.Panel1.ResumeLayout(false);
            this.splitContainerNode.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerNode)).EndInit();
            this.splitContainerNode.ResumeLayout(false);
            this.gbCondition.ResumeLayout(false);
            this.splitContainerSetting.Panel1.ResumeLayout(false);
            this.splitContainerSetting.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSetting)).EndInit();
            this.splitContainerSetting.ResumeLayout(false);
            this.lbDeviceNodeManager.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbNodeManager;
        private System.Windows.Forms.SplitContainer splitContainerNode;
        private System.Windows.Forms.GroupBox gbCondition;
        private System.Windows.Forms.ListBox lstNodeList;
        private System.Windows.Forms.SplitContainer splitContainerSetting;
        private System.Windows.Forms.GroupBox lbDeviceNodeManager;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label Setting_NodeName_lb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Setting_CarrierType_cb;
        private System.Windows.Forms.Label Setting_CarrierType_lb;
        private System.Windows.Forms.Label Setting_NodeType_lb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton Setting_NodeDisable_rb;
        private System.Windows.Forms.RadioButton Setting_NodeEnable_rb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox Setting_Address_tb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Setting_connectType_cb;
        private System.Windows.Forms.Label Setting_ControllerName_lb;
        private System.Windows.Forms.Label Setting_Port_lb;
        private System.Windows.Forms.Label setting_Address_lb;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox Setting_Port_tb;
        private System.Windows.Forms.ComboBox Setting_Mode_cb;
        private System.Windows.Forms.Label Setting_Mode_lb;
    }
}
