using Adam.Util;
using LiteDB;
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
        public class account
        {
            public string user_id { get; set; }
            public string user_name { get; set; }
            public string password { get; set; }
            public string user_group_id { get; set; }
            public string active { get; set; }
            public string create_user { get; set; }

        }
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
                //DBUtil dBUtil = new DBUtil();
                //Dictionary<string, object> keyValues = new Dictionary<string, object>();
                //string strSql = "UPDATE account SET PASSWORD = MD5(@password), " +
                //                                "modify_user = @modify_user, " +
                //                                "modify_timestamp = NOW() " +
                //                                "WHERE user_id = @user_id ";

                //keyValues.Add("@user_id", tbUserID.Text.Trim());
                //keyValues.Add("@password", tbNewPwd.Text.Trim());
                //keyValues.Add("@modify_user", Global.currentUser);
                //dBUtil.ExecuteNonQuery(strSql, keyValues);
                //SanwaUtil.addActionLog("GUI.FormChgPwd", "Change password", Global.currentUser, "變更本人密碼");
                using (var db = new LiteDatabase(@"Filename=config\MyData.db;Connection=shared;"))
                {
                    // Get customer collection
                    var col = db.GetCollection<account>("account");
                    var rs = col.Query().Where(x => x.user_id.Equals(tbUserID.Text));
                    account target = rs.First();
                    target.password = tbNewPwd.Text.Trim().ToMD5();
                    col.Update(target);
                }
                    MessageBox.Show("變更密碼成功!!", "Success");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
                
        }

        private bool checkOldPwd(string userid, string password)
        {
            Boolean result = false;
            //set SQL
            //StringBuilder sql = new StringBuilder();
            //sql.Append("\n SELECT user_id, user_name, user_group_id");
            //sql.Append("\n   FROM account ");
            //sql.Append("\n  WHERE user_id = @user_id ");
            //sql.Append("\n    AND password = MD5(@password)");
            ////set parameter
            //Dictionary<string, object> param = new Dictionary<string, object>();
            //param.Add("@user_id", userid);
            //param.Add("@password", password);
            ////Query
            //DBUtil dBUtil = new DBUtil();
            //DataTableReader rs = dBUtil.GetDataReader(sql.ToString(), param);
            using (var db = new LiteDatabase(@"Filename=config\MyData.db;Connection=shared;"))
            {
                // Get customer collection
                var col = db.GetCollection<account>("account");
                var rs = col.Query().Where(x => x.user_id.Equals(tbUserID.Text) && x.password.Equals(password.ToMD5()));
                if (rs.Count()!=0)
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
