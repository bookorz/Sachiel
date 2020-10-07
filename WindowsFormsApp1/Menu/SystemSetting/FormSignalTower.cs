using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using Adam.UI_Update.Monitoring;
using GUI;
using TransferControl.Config;
using LiteDB;

namespace Adam.Menu.SystemSetting
{
    public partial class FormSignalTower : Adam.Menu.SystemSetting.FormSettingFram
    {
        public FormSignalTower()
        {
            InitializeComponent();
        }

        private DataTable dtSignalTower = new DataTable();
  

        private void FormSignalTtower_Load(object sender, EventArgs e)
        {

            DataTable dtTemp = new DataTable();
            string strSql = string.Empty;

            try
            {
                UpdateList();
                
                lsbCondition.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void lsbCondition_Click(object sender, EventArgs e)
        {
            txbEqpStatus.Text = string.Empty;
            txbIsAlarm.Text = string.Empty;
            cmbBlue.SelectedIndex = -1;
            cmbGreen.SelectedIndex = -1;
            cmbRad.SelectedIndex = -1;
            cmbYellow.SelectedIndex = -1;
            cmbBuzzer1.SelectedIndex = -1;
            cmbBuzzer2.SelectedIndex = -1;

            try
            {
                var query = (from a in dtSignalTower.AsEnumerable()
                             where a.Field<string>("eqp_status") == lsbCondition.Text.Split('-')[0].ToString()
                             && a.Field<UInt64>("is_alarm") == Convert.ToUInt64(lsbCondition.SelectedValue.ToString())
                             select a).ToList();

                if (query.Count > 0)
                {
                    txbEqpStatus.Text = query[0]["eqp_status"].ToString();
                    txbIsAlarm.Text = query[0]["is_alarm"].ToString();
                    cmbBlue.SelectedItem = query[0]["blue"].ToString();
                    cmbGreen.SelectedItem = query[0]["green"].ToString();
                    cmbRad.SelectedItem = query[0]["red"].ToString();
                    cmbYellow.SelectedItem = query[0]["orange"].ToString();
                    cmbBuzzer1.SelectedItem = query[0]["buzzer1"].ToString();
                    cmbBuzzer2.SelectedItem = query[0]["buzzer2"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if ((DataTable)lsbCondition.DataSource == null || ((DataTable)lsbCondition.DataSource).Rows.Count == 0)
            {
                MessageBox.Show("The grid data does not exist.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }

            if (lsbCondition.SelectedIndex < 0)
            {
                MessageBox.Show("Choose the condition.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }

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

            string strSql = string.Empty;
            Dictionary<string, object> keyValues = new Dictionary<string, object>();

            try
            {
                //strSql = "UPDATE config_signal_tower " +
                //    "SET red = @red, " +
                //    "orange = @orange, " +
                //    "green = @green, " +
                //    "blue = @blue, " +
                //    "buzzer1 = @buzzer1, " +
                //    "buzzer2 = @buzzer2, " +
                //    "update_user = @update_user, " +
                //    "update_time = NOW() " +
                //    "WHERE eqp_status  =  @eqp_status " +
                //    "AND is_alarm = @is_alarm ";

                //Form form = Application.OpenForms["FormMain"];
                //Label Signal = form.Controls.Find("lbl_login_id", true).FirstOrDefault() as Label;

                //keyValues.Add("@red", cmbRad.Text.ToString());
                //keyValues.Add("@orange", cmbYellow.Text.ToString());
                //keyValues.Add("@green", cmbGreen.Text.ToString());
                //keyValues.Add("@blue", cmbBlue.Text.ToString());
                //keyValues.Add("@buzzer1", cmbBuzzer1.Text.ToString());
                //keyValues.Add("@buzzer2", cmbBuzzer2.Text.ToString());
                //keyValues.Add("@update_user", Signal.Text);
                //keyValues.Add("@eqp_status", lsbCondition.Text.Split('-')[0].ToString());
                //keyValues.Add("@is_alarm", Convert.ToUInt64(lsbCondition.SelectedValue.ToString()));

                //dBUtil.ExecuteNonQuery(strSql, keyValues);
                using (var db = new LiteDatabase(@"Filename=config\MyData.db;Connection=shared;"))
                {
                    // Get customer collection
                    var col = db.GetCollection<config_signal_tower>("config_signal_tower");
                    var result = col.Query().Where(x=>x.eqp_status.Equals(lsbCondition.Text.Split('-')[0].ToString()) && x.is_alarm== Convert.ToBoolean(lsbCondition.SelectedValue.ToString()));

                    config_signal_tower target = result.First();
                    target.red = cmbRad.Text.ToString();
                    target.orange = cmbYellow.Text.ToString();
                    target.green = cmbGreen.Text.ToString();
                    target.blue = cmbBlue.Text.ToString();
                    target.buzzer1 = cmbBuzzer1.Text.ToString();
                    target.buzzer2 = cmbBuzzer2.Text.ToString();
                    col.Update(target);
                }

                MessageBox.Show("Done it.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                //Adam.Util.SanwaUtil.addActionLog("Adam.Menu.SystemSetting", "FormSignalTower", Signal.Text);

                UpdateList();

                txbEqpStatus.Text = string.Empty;
                txbIsAlarm.Text = string.Empty;
                cmbBlue.SelectedIndex = -1;
                cmbGreen.SelectedIndex = -1;
                cmbRad.SelectedIndex = -1;
                cmbYellow.SelectedIndex = -1;
                cmbBuzzer1.SelectedIndex = -1;
                cmbBuzzer2.SelectedIndex = -1;
                lsbCondition.SelectedIndex = -1;
                //改設定後套用
                NodeStatusUpdate.InitialSetting();
                NodeStatusUpdate.UpdateCurrentState(FormMain.RouteCtrl.EqpState);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public class config_signal_tower
        {
            public string item { get; set; }
            public string eqp_status { get; set; }
            public bool is_alarm { get; set; }
            public string red { get; set; }
            public string orange { get; set; }
            public string green { get; set; }
            public string blue { get; set; }
            public string buzzer1 { get; set; }
            public string buzzer2 { get; set; }

        }
        private void UpdateList()
        {
            string strSql = string.Empty;

            try
            {
                //strSql = "select concat(eqp_status, '-', (case when is_alarm = 0 then 'Normal' else 'Alarm' end)) item, " +
                //            "eqp_status, is_alarm, red, orange, green, blue, buzzer1, buzzer2 " +
                //            "from config_signal_tower order by is_alarm, eqp_status asc ";

                //dtSignalTower = dBUtil.GetDataTable(strSql, null);

                //if (dtSignalTower.Rows.Count > 0)
                //{
                //    lsbCondition.DataSource = dtSignalTower;
                //    lsbCondition.DisplayMember = "item";
                //    lsbCondition.ValueMember = "is_alarm";
                //}
                //else
                //{
                //    lsbCondition.DataSource = null;
                //}
                using (var db = new LiteDatabase(@"Filename=config\MyData.db;Connection=shared;"))
                {
                    // Get customer collection
                    var col = db.GetCollection<config_signal_tower>("config_signal_tower");
                    var result = col.Query().OrderBy(x=> new { x.eqp_status ,x.is_alarm});

                    foreach (config_signal_tower each in result.ToList())
                    {
                        each.item = each.eqp_status + "-" + (each.is_alarm? "Alarm": "Normal");
                    }
                    lsbCondition.DataSource = result.ToList().ToDataTable();
                    lsbCondition.DisplayMember = "item";
                    lsbCondition.ValueMember = "is_alarm";

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
