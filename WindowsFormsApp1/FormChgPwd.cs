using Adam.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TransferControl.Comm;
using TransferControl.Config;

namespace GUI
{
    public partial class FormChgPwd : Form
    {
        public FormChgPwd()
        {
            InitializeComponent();
        }
        public FormChgPwd(string userid)
        {
            InitializeComponent();
            tbUserID.Text = userid;
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            if (tbNewPwd.Text.Trim().Equals(""))
            {
                MessageBox.Show("New Password is empty!","Error");
            }else if (!tbNewPwd.Text.Equals(tbConfirmPwd.Text))
            {
                MessageBox.Show("New password and confirm password does not match!", "Error");
            }
            else if(!checkOldPwd(tbUserID.Text,tbOldPwd.Text))
            {
                MessageBox.Show("Old password do not match.", "Check Password Fail");
            }
            else
            {
                DBUtil dBUtil = new DBUtil();
                Dictionary<string, object> keyValues = new Dictionary<string, object>();
                string strSql = "UPDATE account SET PASSWORD = MD5(@password), " +
                                                "modify_user = @modify_user, " +
                                                "modify_timestamp = NOW() " +
                                                "WHERE user_id = @user_id ";

                keyValues.Add("@user_id", tbUserID.Text.Trim());
                keyValues.Add("@password", tbNewPwd.Text.Trim());
                keyValues.Add("@modify_user", Global.currentUser);
                dBUtil.ExecuteNonQuery(strSql, keyValues);
                SanwaUtil.addActionLog("GUI.FormChgPwd", "Change password", Global.currentUser, "變更本人密碼");
                MessageBox.Show("變更密碼成功!!", "Success");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
                
        }

        private bool checkOldPwd(string userid, string password)
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
            param.Add("@user_id", userid);
            param.Add("@password", password);
            //Query
            DBUtil dBUtil = new DBUtil();
            DataTableReader rs = dBUtil.GetDataReader(sql.ToString(), param);
            if (rs != null)
            {
                while (rs.Read())
                {
                    result = true;
                }
            }
            return result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
