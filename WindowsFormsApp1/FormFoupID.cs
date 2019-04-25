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
            if(result == DialogResult.Yes)
            {
                //Unclamp foup            
            }
            else
            {
                FoupID_Read_tb.Focus();
            }
        }

        private void FoupID_Read_Confirm_btn_Click(object sender, EventArgs e)
        {
            if (!FoupID_Read_tb.Text.Equals(""))
            {
                //open & mapping foup

                TaskJobManagment.CurrentProceedTask Task;
                Node port = NodeManagement.Get(LoadportName_lb.Text);
                if (port != null)
                {
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
    }
}
