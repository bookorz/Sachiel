using LiteDB;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Comm;
using TransferControl.Config;
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
        private static bool getEnable(string port_type)
        {
            bool result = false;
            switch (port_type)
            {
                case "L":
                case "U":
                    result = true;
                    break;
                case "N":
                    result = false;
                    break;
            }
            return result;
        }
        public class config_node
        {
            public ObjectId _id { get; set; }
            public string equipment_model_id { get; set; }
            public string node_id { get; set; }
            public string associated_node { get; set; }
            public string node_type { get; set; }
            public string carrier_type { get; set; }
            public string mode { get; set; }
            public int ack_timeout { get; set; }
            public int motion_timeout { get; set; }
            public bool Double_Arm { get; set; }
            public bool r_arm { get; set; }
            public bool l_arm { get; set; }
            public int sn_no { get; set; }
            public string vendor { get; set; }
            public string wafer_size { get; set; }
            public int Notch_Angle { get; set; }
            public string R_Flip_Degree { get; set; }
            public string conn_address { get; set; }
            public string controller_id { get; set; }
            public bool bypass { get; set; }
            public bool enable_flg { get; set; }
        }
        private static void updateLoadPortConfig(Recipe recipe)
        {
            Boolean result = false;
            try
            {
                //DBUtil dBUtil = new DBUtil();
                //Dictionary<string, object> keyValues = new Dictionary<string, object>();
                //string strSql = " UPDATE config_node SET carrier_type = CASE node_id WHEN 'LOADPORT01' THEN @ctype1 " +
                //                "                                                    WHEN 'LOADPORT02' THEN @ctype2 " +
                //                "                                                    WHEN 'LOADPORT03' THEN @ctype3 " +
                //                "                                                    WHEN 'LOADPORT04' THEN @ctype4 " +
                //                "                                                    ELSE carrier_type END, " +
                //                "                          mode = CASE node_id WHEN 'LOADPORT01' THEN @mode1 " +
                //                "                                              WHEN 'LOADPORT02' THEN @mode2 " +
                //                "                                              WHEN 'LOADPORT03' THEN @mode3 " +
                //                "                                              WHEN 'LOADPORT04' THEN @mode4 " +
                //                "                                              ELSE mode END," +
                //                "                          enable_flg = CASE node_id WHEN 'LOADPORT01' THEN @enable1 " +
                //                "                                                    WHEN 'LOADPORT02' THEN @enable2 " +
                //                "                                                    WHEN 'LOADPORT03' THEN @enable3 " +
                //                "                                                    WHEN 'LOADPORT04' THEN @enable4 " +
                //                "                                                    ELSE enable_flg END," +
                //                "                          bypass = CASE node_id     WHEN 'ALIGNER01' THEN @bypassA1 " +
                //                "                                                    ELSE bypass END," +
                //                "                          double_arm = CASE node_id WHEN 'ROBOT01' THEN @double_arm_r1 " +
                //                "                                                    ELSE double_arm END, " +
                //                "                          r_arm = CASE node_id WHEN 'ROBOT01' THEN @r_arm_r1 " +
                //                "                                                    ELSE r_arm END, " +
                //                "                          l_arm = CASE node_id WHEN 'ROBOT01' THEN @l_arm_r1 " +
                //                "                                                    ELSE l_arm END, " +
                //                "                          modify_user = @modify_user, modify_timestamp = NOW() " +
                //                " WHERE equipment_model_id = @equipment_model_id " +
                //                "   AND node_type IN ('LOADPORT','ROBOT','Aligner') ;";

                //keyValues.Add("@equipment_model_id", SystemConfig.Get().SystemMode);
                //keyValues.Add("@modify_user", Global.currentUser);
                //keyValues.Add("@ctype1", recipe.port1_carrier_type);
                //keyValues.Add("@ctype2", recipe.port2_carrier_type);
                //keyValues.Add("@ctype3", recipe.port3_carrier_type);
                //keyValues.Add("@ctype4", recipe.port4_carrier_type);
                //keyValues.Add("@mode1", getPortType(recipe.port1_type));
                //keyValues.Add("@mode2", getPortType(recipe.port2_type));
                //keyValues.Add("@mode3", getPortType(recipe.port3_type));
                //keyValues.Add("@mode4", getPortType(recipe.port4_type));
                //keyValues.Add("@enable1", getEnable(recipe.port1_type));
                //keyValues.Add("@enable2", getEnable(recipe.port2_type));
                //keyValues.Add("@enable3", getEnable(recipe.port3_type));
                //keyValues.Add("@enable4", getEnable(recipe.port4_type));
                //keyValues.Add("@bypassA1", recipe.is_use_aligner1 ? 0 : 1);//要用ALIGNER 時 bypass = 0, ELSE bypass = 1
                //keyValues.Add("@double_arm_r1", recipe.is_use_double_arm ? 1 : 0);
                //keyValues.Add("@r_arm_r1", recipe.is_use_r_arm ? 1 : 0);
                //keyValues.Add("@l_arm_r1", recipe.is_use_l_arm ? 1 : 0);
                //dBUtil.ExecuteNonQuery(strSql, keyValues);
                using (var db = new LiteDatabase(@"Filename=config\MyData.db;Connection=shared;"))
                {
                    List<string> ty = new List<string>() { "LOADPORT","ROBOT","Aligner"};
                    // Get customer collection
                    var col = db.GetCollection<config_node>("config_node");
                    var rs = col.Query().Where(x => x.equipment_model_id.Equals(SystemConfig.Get().SystemMode) && ty.Any(y=>y.Equals(x.node_type)));
                    List<config_node> tmp = rs.ToList();
                    foreach (config_node each in tmp)
                    {
                        switch (each.node_id)
                        {
                            case "LOADPORT01":
                                each.carrier_type = recipe.port1_carrier_type;
                                each.mode= getPortType(recipe.port1_type);
                                each.enable_flg = getEnable(recipe.port1_type);
                                break;
                            case "LOADPORT02":
                                each.carrier_type = recipe.port2_carrier_type;
                                each.mode = getPortType(recipe.port2_type);
                                each.enable_flg = getEnable(recipe.port2_type);
                                break;
                            case "LOADPORT03":
                                each.carrier_type = recipe.port3_carrier_type;
                                each.mode = getPortType(recipe.port3_type);
                                each.enable_flg = getEnable(recipe.port3_type);
                                break;
                            case "LOADPORT04":
                                each.carrier_type = recipe.port4_carrier_type;
                                each.mode = getPortType(recipe.port4_type);
                                each.enable_flg = getEnable(recipe.port4_type);
                                break;
                            case "ALIGNER01":
                                each.bypass = !recipe.is_use_aligner1;
                                break;
                            case "ROBOT01":
                                each.Double_Arm = recipe.is_use_double_arm;
                                each.r_arm = recipe.is_use_r_arm;
                                each.l_arm = recipe.is_use_l_arm;
                                break;
                        }
                    }
                    col.Update(tmp);
                }
                foreach (Node node in NodeManagement.GetList())
                {
                    switch (node.Name.ToUpper())
                    {
                        case "LOADPORT01":
                            node.CarrierType = recipe.port1_carrier_type;
                            node.Mode = getPortType(recipe.port1_type);
                            node.Enable = getEnable(recipe.port1_type);
                            node.OrgSearchComplete = false;
                            break;
                        case "LOADPORT02":
                            node.CarrierType = recipe.port2_carrier_type;
                            node.Mode = getPortType(recipe.port2_type);
                            node.Enable = getEnable(recipe.port2_type);
                            node.OrgSearchComplete = false;
                            break;
                        case "LOADPORT03":
                            node.CarrierType = recipe.port3_carrier_type;
                            node.Mode = getPortType(recipe.port3_type);
                            node.Enable = getEnable(recipe.port3_type) ;
                            node.OrgSearchComplete = false;
                            break;
                        case "LOADPORT04":
                            node.CarrierType = recipe.port4_carrier_type;
                            node.Mode = getPortType(recipe.port4_type);
                            node.Enable = getEnable(recipe.port4_type);
                            node.OrgSearchComplete = false;
                            break;
                        case "ROBOT01":
                            node.RArmActive = recipe.is_use_r_arm;
                            node.LArmActive = recipe.is_use_l_arm;
                            node.DoubleArmActive = recipe.is_use_double_arm;
                            break;
                        case "ALIGNER01":
                            node.ByPass = !recipe.is_use_aligner1;
                            break;
                        case "ALIGNER02":
                            node.ByPass = !recipe.is_use_aligner2;
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
