using GUI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Config;

namespace Adam.Menu.SystemSetting
{
    public partial class FormSimpleSetting : Form
    {
        public string sql;
        MySqlDataAdapter sda;
        DataSet ds;
        MySqlCommand scom;
        MySqlCommandBuilder SCB;
        MySqlConnection conn = new MySqlConnection(SystemConfig.Get().DBConnectionString);

        private void UpdateConfigView(string sql)
        {
            if (sql.Trim().Equals(""))
                return;
            dgvMysql.DataSource = null;
            dgvMysql.Rows.Clear();
            //conn.Close();
            //conn.Open();

            ds = new DataSet();
            scom = new MySqlCommand(sql, conn);
            sda = new MySqlDataAdapter();
            sda.SelectCommand = scom;
            ds.Clear();
            sda.Fill(ds);
            dgvMysql.DataSource = ds.Tables[0];

            foreach (DataGridViewColumn column in dgvMysql.Columns)
            {
                if (column.HeaderText.EndsWith("_r"))
                {
                    column.ReadOnly = true;
                    column.DefaultCellStyle.BackColor = Color.LightGray;
                }
                else
                {
                    column.ReadOnly = false;
                    column.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void FormSimpleSetting_Load(object sender, EventArgs e)
        {
            conn.Open();
            UpdateConfigView(sql);
        }

        private void FormSimpleSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SCB != null)
                SCB.Dispose();
            if (sda != null)
                sda.Dispose();
            if (scom != null)
                scom.Dispose();
            if (ds != null)
                ds.Clear();
            //ds.AcceptChanges();
        }
        
        public FormSimpleSetting(Config_Type config_Type)
        {
            string model_id = SystemConfig.Get().SystemMode;
            InitializeComponent();
            sql = Global.Config_sql(config_Type);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //權限檢查
                using (var form = new FormConfirm("是否儲存變更?"))
                {
                    var result = form.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        MessageBox.Show("Cancel.", "Notice");
                        return;
                    }
                }

                SCB = new MySqlCommandBuilder(sda);
                sda.Update(ds);
                SCB.RefreshSchema();
                SCB.Dispose();
                MessageBox.Show("Update 成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }
    }
}
