using LiteDB;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TransferControl.Comm;
using TransferControl.Engine;
using TransferControl.Management;

namespace Adam.UI_Update.Monitoring
{
    class NodeStatusUpdate
    {
        static ILog logger = LogManager.GetLogger(typeof(NodeStatusUpdate));
        delegate void UpdateNode(string Device_ID, string State);
        delegate void UpdateState(string State);
        static List<Setting> SignalSetting;
        class Setting
        {
            [BsonField("eqp_status")]
            public string Eqp_Status { get; set; }
            [BsonField("is_alarm")]
            public bool Is_Alarm { get; set; }
            [BsonField("red")]
            public string Red { get; set; }
            [BsonField("orange")]
            public string Orange { get; set; }
            [BsonField("blue")]
            public string Blue { get; set; }
            [BsonField("green")]
            public string Green { get; set; }
            [BsonField("buzzer1")]
            public string Buzzer1 { get; set; }
            [BsonField("buzzer2")]
            public string Buzzer2 { get; set; }
        }
      
        public static void InitialSetting()
        {
            //DBUtil dBUtil = new DBUtil();

            //string Sql = @"select * from config_signal_tower";
            //DataTable dt = dBUtil.GetDataTable(Sql, null);

            //string str_json = JsonConvert.SerializeObject(dt, Formatting.Indented);
            //SignalSetting = JsonConvert.DeserializeObject<List<Setting>>(str_json);
            using (var db = new LiteDatabase(@"Filename=config\MyData.db;Connection=shared;"))
            {
                // Get customer collection
                var col = db.GetCollection<Setting>("config_signal_tower");
                var rs = col.Query();
                SignalSetting = rs.ToList();
            }
        }

        public static void UpdateNodeState(string Device_ID, string State)
        {
            try
            {
                Form form = Application.OpenForms["FormMain"];
                TextBox State_tb;
                if (form == null)
                    return;

                State_tb = form.Controls.Find(Device_ID + "_State", true).FirstOrDefault() as TextBox;
                if (State_tb == null)
                    return;

                if (State_tb.InvokeRequired)
                {
                    UpdateNode ph = new UpdateNode(UpdateNodeState);
                    State_tb.BeginInvoke(ph, Device_ID, State);
                }
                else
                {
                    State_tb.Text = State;
                    switch (State)
                    {
                        case "Run":
                            State_tb.BackColor = Color.Lime;
                            //UpdateCurrentState();
                            break;
                        case "Idle":
                            State_tb.BackColor = Color.Orange;
                            //UpdateCurrentState();
                            break;
                        case "Ready To Load":
                            State_tb.BackColor = Color.DarkGray;
                            break;
                        case "Load Complete":
                            State_tb.BackColor = Color.Green;
                            break;
                        case "Ready To UnLoad":
                            State_tb.BackColor = Color.DarkOrange;
                            break;
                        case "UnLoad Complete":
                            State_tb.BackColor = Color.Blue;
                            break;
                        case "Alarm":
                            State_tb.BackColor = Color.Red;
                            //UpdateCurrentState();
                            break;
                    }

                }


            }
            catch
            {
                logger.Error("UpdateControllerStatus: Update fail.");
            }
        }

        public static void UpdateCurrentState(string State)
        {
            try
            {
                Form form = Application.OpenForms["FormMain"];

                if (form == null)
                    return;

                Button state_btn = form.Controls.Find("EQP_State", true).FirstOrDefault() as Button;

                if (state_btn == null)
                    return;

                if (state_btn.InvokeRequired)
                {
                    UpdateState ph = new UpdateState(UpdateCurrentState);
                    state_btn.BeginInvoke(ph, State);
                }
                else
                {
                    if (SignalSetting == null)
                    {
                        InitialSetting();
                    }
                    state_btn.Text = State;
                    Dictionary<string, string> Params = new Dictionary<string, string>();

                    var findSetting = from Setting in SignalSetting
                                      where Setting.Eqp_Status.Equals(State.ToUpper()) && Setting.Is_Alarm == (AlarmManagement.GetAll().Count!=0)
                                      select Setting;

                    if (findSetting.Count() != 0)
                    {
                        Setting each = findSetting.First();
                        if (!each.Blue.Equals("BLINK"))
                        {
                            Params.Add("BLUE", each.Blue);
                        }
                        else
                        {
                            RouteControl.Instance.DIO.SetBlink("BLUE", "True");
                        }
                        if (!each.Green.Equals("BLINK"))
                        {
                            Params.Add("GREEN", each.Green);
                        }
                        else
                        {
                            RouteControl.Instance.DIO.SetBlink("GREEN", "True");
                        }
                        if (!each.Red.Equals("BLINK"))
                        {
                            Params.Add("RED", each.Red);
                        }
                        else
                        {
                            RouteControl.Instance.DIO.SetBlink("RED", "True");
                        }
                        if (!each.Orange.Equals("BLINK"))
                        {
                            Params.Add("ORANGE", each.Orange);
                        }
                        else
                        {
                            RouteControl.Instance.DIO.SetBlink("ORANGE", "True");
                        }

                        RouteControl.Instance.DIO.SetIO(Params);
                    }

                }


            }
            catch (Exception e)
            {
                logger.Error("UpdateCurrentState: Update fail.:" + e.StackTrace);
            }
        }

    }
}
