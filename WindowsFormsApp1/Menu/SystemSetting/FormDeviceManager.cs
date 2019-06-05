using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SANWA.Utility;
using TransferControl.Management;
using Adam.UI_Update.OCR;
using log4net;
using TransferControl.Controller;
using GUI;

namespace Adam.Menu.SystemSetting
{
    public partial class FormDeviceManager : Form
    {
        public FormDeviceManager()
        {
            InitializeComponent();
        }

        private SANWA.Utility.config_equipment_model equipment_Model = new SANWA.Utility.config_equipment_model();
        private DataTable dtConfigNode = new DataTable();
        private DataTable dtControllerTable = new DataTable();
        private static readonly ILog logger = LogManager.GetLogger(typeof(FormDeviceManager));

        private void FormDeviceManager_Load(object sender, EventArgs e)
        {
            string strSql = string.Empty;
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            DBUtil dBUtil = new DBUtil();
            DataTable dtTemp = new DataTable();

            try
            {
                UpdateNodeList();



            }
            catch (Exception ex)
            {
                //throw new Exception(ex.ToString());
                logger.Error(ex.StackTrace);
                MessageBox.Show(ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateNodeList()
        {
            string strSql = string.Empty;
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            DBUtil dBUtil = new DBUtil();

            try
            {
                strSql = @"SELECT * 
                            FROM config_node
                            WHERE equipment_model_id = @equipment_model_id
                            ORDER BY node_id";
                keyValues.Add("@equipment_model_id", SANWA.Utility.Config.SystemConfig.Get().SystemMode);
                dtConfigNode = dBUtil.GetDataTable(strSql, keyValues);

                if (dtConfigNode.Rows.Count > 0)
                {
                    lstNodeList.DataSource = dtConfigNode;
                    lstNodeList.DisplayMember = "node_id";
                    lstNodeList.ValueMember = "node_id";
                    lstNodeList.SelectedIndex = -1;
                }
                else
                {
                    lstNodeList.DataSource = null;
                }

                strSql = @"SELECT * 
                            FROM config_controller_setting
                            WHERE equipment_model_id = @equipment_model_id";

                dtControllerTable = dBUtil.GetDataTable(strSql, keyValues);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void lstNodeList_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = new DataTable();
            string ControllerName = "";
            try
            {
                var query = (from a in dtConfigNode.AsEnumerable()
                             where a.Field<string>("node_id") == lstNodeList.SelectedValue.ToString()
                             select a).ToList();

                if (query.Count == 0)
                {

                    throw new RowNotInTableException();
                }
                else
                {
                    dtTemp = query.CopyToDataTable();

                    Setting_NodeName_lb.Text = dtTemp.Rows[0]["node_id"].ToString();

                    Setting_NodeType_lb.Text = dtTemp.Rows[0]["node_type"].ToString();

                    ControllerName = dtTemp.Rows[0]["controller_id"].ToString();

                    if (dtTemp.Rows[0]["enable_flg"].ToString() == "1" ? true : false)
                    {
                        Setting_NodeEnable_rb.Checked = true;
                    }
                    else
                    {
                        Setting_NodeDisable_rb.Checked = true;
                    }

                    Setting_CarrierType_cb.Text = dtTemp.Rows[0]["carrier_type"].ToString();
                    Setting_Mode_cb.Text = dtTemp.Rows[0]["mode"].ToString();
                    if (Setting_NodeType_lb.Text.ToUpper().Equals("LOADPORT"))
                    {
                        Setting_CarrierType_cb.Visible = true;
                        Setting_CarrierType_lb.Visible = true;
                        Setting_Mode_cb.Visible = true;
                        Setting_Mode_lb.Visible = true;
                    }
                    else
                    {
                        Setting_CarrierType_cb.Visible = false;
                        Setting_CarrierType_lb.Visible = false;
                        Setting_Mode_cb.Visible = false;
                        Setting_Mode_lb.Visible = false;
                    }
                }
                query = (from a in dtControllerTable.AsEnumerable()
                         where a.Field<string>("device_name") == ControllerName
                         select a).ToList();

                if (query.Count == 0)
                {
                    throw new RowNotInTableException();
                }
                else
                {
                    dtTemp = query.CopyToDataTable();
                    Setting_ControllerName_lb.Text = dtTemp.Rows[0]["device_name"].ToString();
                    Setting_connectType_cb.Text = dtTemp.Rows[0]["conn_type"].ToString();
                    Setting_Address_tb.Text = dtTemp.Rows[0]["conn_address"].ToString();
                    Setting_Port_tb.Text = dtTemp.Rows[0]["conn_port"].ToString();
                    if (Setting_connectType_cb.Text.ToUpper().Equals("SOCKET"))
                    {
                        setting_Address_lb.Text = "Address:";
                        Setting_Port_lb.Text = "Port:";
                    }
                    else
                    {
                        setting_Address_lb.Text = "Comport:";
                        Setting_Port_lb.Text = "Baud Rate:";
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            

            string strSql = string.Empty;
            StringBuilder sbErrorMessage = new StringBuilder();
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            DBUtil dBUtil = new DBUtil();
            DataTable dtTemp = new DataTable();
      

            try
            {
                Node currentNode = NodeManagement.Get(Setting_NodeName_lb.Text);

                if (currentNode == null)
                {
                    MessageBox.Show("Node "+Setting_NodeName_lb.Text + " is not exist!");
                    return;
                }
                DeviceController currentController = ControllerManagement.Get(currentNode.Controller);
                if (currentController == null)
                {
                    MessageBox.Show("Controller "+currentNode.Controller + " is not exist!");
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
                currentNode.Enable = Setting_NodeEnable_rb.Checked;
                currentNode.CarrierType = Setting_CarrierType_cb.Text;
                currentNode.Mode = Setting_Mode_cb.Text;
                strSql = @"UPDATE config_node SET enable_flg = @enable_flg ,carrier_type = @carrier_type, mode = @mode WHERE equipment_model_id = @equipment_model_id AND node_id = @node_id";

                keyValues.Add("@equipment_model_id", equipment_Model.EquipmentModel.equipment_model_id);
                keyValues.Add("@node_id", currentNode.Name);
                keyValues.Add("@enable_flg", currentNode.Enable ? 1 : 0);
                keyValues.Add("@carrier_type", currentNode.CarrierType);
                keyValues.Add("@mode", currentNode.Mode);

                dBUtil.ExecuteNonQuery(strSql, keyValues);

                keyValues.Clear();
                currentController.ConnectionType = Setting_connectType_cb.Text;
                currentController.IPAdress = Setting_Address_tb.Text;
                currentController.Port = Convert.ToInt32(Setting_Port_tb.Text);
                strSql = "UPDATE config_controller_setting SET conn_type = @conn_type, conn_address = @conn_address , conn_port = @conn_port WHERE equipment_model_id = @equipment_model_id AND device_name = @device_name";

                keyValues.Add("@equipment_model_id", equipment_Model.EquipmentModel.equipment_model_id);
                keyValues.Add("@device_name", currentNode.Controller);
                keyValues.Add("@conn_type", currentController.ConnectionType);
                keyValues.Add("@conn_address", Setting_Address_tb.Text);
                keyValues.Add("@conn_port", currentController.Port.ToString());
                dBUtil.ExecuteNonQuery(strSql, keyValues);



                MessageBox.Show("連線相關設定值，將於重啟程式後生效.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);




            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void Setting_connectType_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Setting_connectType_cb.Text.ToUpper())
            {
                case "COMPORT":
                    setting_Address_lb.Text = "Comport:";
                    Setting_Port_lb.Text = "Baud Rate:";
                    break;
                case "SOCKET":
                    setting_Address_lb.Text = "Address:";
                    Setting_Port_lb.Text = "Port:";
                    break;
            }
        }
    }
}
