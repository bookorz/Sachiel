using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TransferControl.Comm;
using TransferControl.Config;

namespace GUI
{
    public partial class FormConfirm : Form
    {
        //ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ILog log = LogManager.GetLogger("Database");
        public FormConfirm(string message)
        {
            InitializeComponent();
            lblMessage.Text = message;
            tbUserID.Text = Global.currentUser;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Boolean result = false;
            //set SQL
            StringBuilder sql = new StringBuilder();
            sql.Append("\n SELECT user_id, user_name, user_group_id");
            sql.Append("\n   FROM account ");
            sql.Append("\n  WHERE user_id = @user_id ");
            sql.Append("\n    AND password = MD5(@password)");
            //set parameter
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@user_id", tbUserID.Text);
            param.Add("@password", tbPassword.Text);
            //Query
            DBUtil dBUtil = new DBUtil();
            DataTableReader rs = dBUtil.GetDataReader(sql.ToString(), param);
            if (rs != null)
            {
                //Console.Write("\n ID:" + rs["user_id"] + " Password:" + rs["password"] + " MD5:" + rs["md5"]);

                string user_id = "";
                string user_name = "";
                string user_group_id = "";
                while (rs.Read())
                {
                    user_id = (string)rs["user_id"];
                    user_name = (string)rs["user_name"];
                    user_group_id = (string)rs["user_group_id"];
                    result = true;
                }
                rs.Close();
                if (result)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    //this.DialogResult = DialogResult.Cancel; //不能加這行，會跳出
                    MessageBox.Show("Password error.", "Check Fail");
                    tbPassword.Focus();
                    return;
                }
            }

        }

        private void FormLogin_Activated(object sender, EventArgs e)
        {
            tbPassword.Focus();
        }
        
    }
}
