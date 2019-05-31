using Adam.UI_Update.Monitoring;
using Adam.UI_Update.WaferMapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Engine;
using TransferControl.Management;

namespace Adam
{
    public partial class FormFoupID : Form
    {

        public FormFoupID()
        {
            InitializeComponent();
        }

        private void FoupID_Read_tb_Leave(object sender, EventArgs e)
        {
            FoupID_Read_tb.Focus();
        }

        private void FormFoupID_Load(object sender, EventArgs e)
        {
            FoupID_Read_tb.Focus();
        }

        private void FormFoupID_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Abort load procedure?", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //Unclamp foup     
                this.Hide();
            }
            else
            {
                FoupID_Read_tb.Focus();
            }
            e.Cancel = true;
        }

        private void FoupID_Read_Confirm_btn_Click(object sender, EventArgs e)
        {       
            if (!FoupID_Read_tb.Text.Equals(""))
            {
                if (!ManualInput_ck.Checked)
                {
                    //open & mapping foup
                    string end = "";
                    switch (LoadportName_lb.Text)
                    {
                        case "LOADPORT01":
                            end = "!";
                            break;
                        case "LOADPORT02":
                            end = "\"";
                            break;
                        case "LOADPORT03":
                            end = "#";
                            break;
                        case "LOADPORT04":
                            end = "$";
                            break;
                    }
                    if (!end.Equals(endCode))
                    {
                        MessageBox.Show("請使用正確的條碼槍");
                        return;
                    }
                }
                else
                {
                    foupID = FoupID_Read_tb.Text;
                }

                TaskJobManagment.CurrentProceedTask Task;
                Node port = NodeManagement.Get(LoadportName_lb.Text);
                if (port != null)
                {
                    port.FoupID = foupID;
                    MonitoringUpdate.UpdateFoupID(port.Name.ToUpper(), foupID);
                    WaferAssignUpdate.UpdateFoupID(port.Name.ToUpper(), foupID);
                    port.OPACCESS = false;
                    string TaskName = "LOADPORT_OPEN";
                    string Message = "";
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("@Target", port.Name);

                    RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out Task, TaskName, param);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("找不到該Laodport資料");
                }
            }
            else
            {
                MessageBox.Show("請輸入Foup ID");
            }
        }
        public static string startCode = "";
        public static string endCode = "";
        public static string foupID = "";
        private void FoupID_Read_tb_TextChanged(object sender, EventArgs e)
        {
            if (!ManualInput_ck.Checked)
            {
                if (!startCode.Equals("") && !endCode.Equals(""))
                {
                    FoupID_Read_tb.Text = foupID;
                    return;
                }

                if (startCode.Equals(""))
                {
                    if (FoupID_Read_tb.Text != "")
                    {
                        if (FoupID_Read_tb.Text[0].Equals('@'))
                        {
                            startCode = "@";
                        }
                        else
                        {
                            MessageBox.Show("請使用條碼槍");
                        }
                    }
                }
                else
                {
                    if (!FoupID_Read_tb.Text.Equals(""))
                    {
                        switch (FoupID_Read_tb.Text[FoupID_Read_tb.Text.Length - 1].ToString())
                        {
                            case "!":
                            case "\"":
                            case "#":
                            case "$":
                                endCode = FoupID_Read_tb.Text[FoupID_Read_tb.Text.Length - 1].ToString();
                                foupID = FoupID_Read_tb.Text.Replace(startCode, "").Replace(endCode, "");
                                FoupID_Read_tb.Text = foupID;
                                break;
                        }
                    }
                }
            }
        }

        private void Clear_btn_Click(object sender, EventArgs e)
        {
            endCode = "";
            FoupID_Read_tb.Text = "";
            startCode = "";

        }

        private void LoadportName_lb_TextChanged(object sender, EventArgs e)
        {
            endCode = "";
            FoupID_Read_tb.Text = "";
            startCode = "";
        }
    }
}
