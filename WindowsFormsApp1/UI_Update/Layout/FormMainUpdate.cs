using log4net;
using SANWA;
using SANWA.Utility;
using SANWA.Utility.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Management;

namespace Adam.UI_Update.Layout
{
    class FormMainUpdate
    {
        static ILog logger = LogManager.GetLogger(typeof(FormMainUpdate));
        delegate void UpdateValue(string Value);

        public static void UpdateRecipe(string Value)
        {
            try
            {
                Form form = Application.OpenForms["FormMain"];
                if (form == null)
                    return;
                Label lblRecipe = form.Controls.Find("CurrentRecipe_lb", true).FirstOrDefault() as Label;

                if (lblRecipe == null)
                    return;
                if (lblRecipe.InvokeRequired)
                {
                    UpdateValue ph = new UpdateValue(UpdateRecipe);
                    lblRecipe.BeginInvoke(ph, Value);
                }
                else
                {
                    lblRecipe.Text = Value;
                    updateLoadPortConfig(Recipe.Get(Value));//更新DB資料
                }
            }
            catch (Exception e)
            {
                logger.Error("UpdateRecipe: Update fail. err:" + e.StackTrace);
            }
        }
        private void UpdateNode(Recipe rcp)
        {
            
        }
        private static string getPortType(string port_type)
        {
            string result = "";
            switch (port_type)
            {
                case "L":
                    result = "LD";
                    break;
                case "U":
                    result = "ULD";
                    break;
                case "N":
                    result = "";
                    break;
            }
            return result;
        }
        private static int getEnable(string port_type)
        {
            int result = 0;
            switch (port_type)
            {
                case "L":
                case "U":
                    result = 1;
                    break;
                case "N":
                    result = 0;
                    break;
            }
            return result;
        }

        private static void updateLoadPortConfig(Recipe recipe)
        {
            Boolean result = false;
            try
            {
                DBUtil dBUtil = new DBUtil();
                Dictionary<string, object> keyValues = new Dictionary<string, object>();
                string strSql = " UPDATE config_node SET carrier_type = CASE node_id WHEN 'LOADPORT01' THEN @ctype1 " +
                                "                                                    WHEN 'LOADPORT02' THEN @ctype2 " +
                                "                                                    WHEN 'LOADPORT03' THEN @ctype3 " +
                                "                                                    WHEN 'LOADPORT04' THEN @ctype4 " +
                                "                                                    ELSE carrier_type END, " +
                                "                          mode = CASE node_id WHEN 'LOADPORT01' THEN @mode1 " +
                                "                                              WHEN 'LOADPORT02' THEN @mode2 " +
                                "                                              WHEN 'LOADPORT03' THEN @mode3 " +
                                "                                              WHEN 'LOADPORT04' THEN @mode4 " +
                                "                                              ELSE mode END," +
                                "                          enable_flg = CASE node_id WHEN 'LOADPORT01' THEN @enable1 " +
                                "                                                    WHEN 'LOADPORT02' THEN @enable2 " +
                                "                                                    WHEN 'LOADPORT03' THEN @enable3 " +
                                "                                                    WHEN 'LOADPORT04' THEN @enable4 " +
                                "                                                    ELSE enable_flg END," +
                                "                          modify_user = @modify_user, modify_timestamp = NOW() " +
                                " WHERE equipment_model_id = @equipment_model_id " +
                                "   AND node_type = 'LOADPORT' ;";

                keyValues.Add("@equipment_model_id", SystemConfig.Get().SystemMode);
                keyValues.Add("@modify_user", Global.currentUser);
                keyValues.Add("@ctype1", recipe.port1_carrier_type);
                keyValues.Add("@ctype2", recipe.port2_carrier_type);
                keyValues.Add("@ctype3", recipe.port3_carrier_type);
                keyValues.Add("@ctype4", recipe.port4_carrier_type);
                keyValues.Add("@mode1", getPortType(recipe.port1_type));
                keyValues.Add("@mode2", getPortType(recipe.port2_type));
                keyValues.Add("@mode3", getPortType(recipe.port3_type));
                keyValues.Add("@mode4", getPortType(recipe.port4_type));
                keyValues.Add("@enable1", getEnable(recipe.port1_type));
                keyValues.Add("@enable2", getEnable(recipe.port2_type));
                keyValues.Add("@enable3", getEnable(recipe.port3_type));
                keyValues.Add("@enable4", getEnable(recipe.port4_type));
                dBUtil.ExecuteNonQuery(strSql, keyValues);
                foreach(Node node in NodeManagement.GetList())
                {
                    switch (node.Name.ToUpper())
                    {
                        case "LOADPORT01":
                            node.CarrierType = recipe.port1_carrier_type;
                            node.Mode = getPortType(recipe.port1_type);
                            node.Enable = getEnable(recipe.port1_type) == 1 ? true : false;
                            node.OrgSearchComplete = false;
                            break;
                        case "LOADPORT02":
                            node.CarrierType = recipe.port2_carrier_type;
                            node.Mode = getPortType(recipe.port2_type);
                            node.Enable = getEnable(recipe.port2_type) == 1 ? true : false;
                            node.OrgSearchComplete = false;
                            break;
                        case "LOADPORT03":
                            node.CarrierType = recipe.port3_carrier_type;
                            node.Mode = getPortType(recipe.port3_type);
                            node.Enable = getEnable(recipe.port3_type) == 1 ? true : false;
                            node.OrgSearchComplete = false;
                            break;
                        case "LOADPORT04":
                            node.CarrierType = recipe.port4_carrier_type;
                            node.Mode = getPortType(recipe.port4_type);
                            node.Enable = getEnable(recipe.port4_type) == 1 ? true : false;
                            node.OrgSearchComplete = false;
                            break;
                        case "ROBOT01":
                            node.RArmActive = recipe.is_use_r_arm;
                            node.LArmActive = recipe.is_use_l_arm;
                            node.DoubleArmActive = recipe.is_use_double_arm;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update load port 資訊失敗! " + ex.StackTrace);
            }
        }
    }
}
