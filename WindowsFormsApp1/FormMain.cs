using SANWA.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TransferControl.Engine;
using TransferControl.Management;
using TransferControl.Parser;
using log4net.Config;
using Adam.UI_Update.Monitoring;
using log4net;
using Adam.UI_Update.Manual;
using Adam.UI_Update.OCR;
using Adam.UI_Update.WaferMapping;
using System.Threading;
using Adam.UI_Update.Authority;
using Adam.UI_Update.Layout;
using Adam.UI_Update.Alarm;
using GUI;
using Adam.UI_Update.Running;
using System.Linq;
using System.Collections.Concurrent;
using Adam.Util;


using System.Security.Cryptography;
using System.Text;
using SANWA.Utility.Config;
using TransferControl.Operation;
using Adam.UI_Update.DifferentialMonitor;
using Adam.UI_Update.Barcode;
using Adam.UI_Update.IO;
using Adam.Menu.WaferMapping;
using System.Diagnostics;
using SANWA;

namespace Adam
{
    public partial class FormMain : Form, IUserInterfaceReport, IXfeStateReport
    {
        public static RouteControl RouteCtrl;
        // public static SECSGEM HostControl;
        public static XfeCrossZone xfe;
        public static AlarmMapping AlmMapping;
        private static readonly ILog logger = LogManager.GetLogger(typeof(FormMain));


        FormAlarm alarmFrom = new FormAlarm();
        FormFoupID BarcodeForm = new FormFoupID();
        private Menu.Monitoring.FormMonitoring formMonitoring = new Menu.Monitoring.FormMonitoring();
        private Menu.IO.FormIO formIO = new Menu.IO.FormIO();
        private Menu.WaferMapping.FormWaferMapping formWaferAssign = new Menu.WaferMapping.FormWaferMapping();
        //private Menu.Status.FormStatus formStatus = new Menu.Status.FormStatus();//20190529 取消
        //private Menu.OCR.FormOCR formOCR = new Menu.OCR.FormOCR();
        //private Menu.SystemSetting.FormSECSSet formSecs = new Menu.SystemSetting.FormSECSSet();
        //private Menu.SystemSetting.FormSystemSetting formSystem = new Menu.SystemSetting.FormSystemSetting();//舊設定方式 20190529 取消
        private Menu.SystemSetting.FormSetting formSystemNew = new Menu.SystemSetting.FormSetting();//新設定方式
        private Menu.DifferentialMonitor.FormDifferentialMonitor formTestMode = new Menu.DifferentialMonitor.FormDifferentialMonitor();
        private Menu.RunningScreen.FormRunningScreen formRun = new Menu.RunningScreen.FormRunningScreen();
        private Menu.Wafer.FormWafer WaferForm = new Menu.Wafer.FormWafer();
        public static GUI.FormManual formManual = null;
        public static string CurrentMode = "AUTO";
        public static bool Start = false;
        public static bool Initial = false;
        public static string RunMode = "";

        public FormMain()
        {


            InitializeComponent();
            XmlConfigurator.Configure();
            Initialize();

            //HostControl = new SECSGEM(this);
            //RouteCtrl = new RouteControl(this, HostControl);


            RouteCtrl = new RouteControl(this, null);
            AlmMapping = new AlarmMapping();

            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(-200, 0);

            SanwaUtil.addPartition();
            SanwaUtil.dropPartition();
            ThreadPool.QueueUserWorkItem(new WaitCallback(DBUtil.consumeSqlCmd));

            xfe = new XfeCrossZone(this);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void Initialize()
        {


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var HST = (from prs in Process.GetProcessesByName("VB9BReaderForm").OfType<Process>().ToList()
                       where prs.MainWindowTitle.Equals("Wafer Reader Version 4.3.1.0")
                       select prs);
            if (HST.Count() == 0)
            {
                MessageBox.Show("請先開啟HST OCR程式!");
                //this.Close();
                //return;
            }

            Int32 oldWidth = this.Width;
            Int32 oldHeight = this.Height;

            this.WindowState = FormWindowState.Normal;
            this.Width = 1;
            this.Height = 1;

            //Control[] ctrlForm = new Control[] { formMonitoring, formIO, formWafer, formStatus, formTestMode, WaferForm, formSystem, formSystemNew };
            //20190529 取消 formStatus, formStatus          Control[] ctrlForm = new Control[] { formMonitoring, formWaferAssign, formIO,  formSystemNew, formTestMode }; //WaferForm 和 壓差監控之後再開放  , formTestMode
            Control[] ctrlForm = new Control[] { formMonitoring, formWaferAssign, formRun, formIO, formSystemNew, formTestMode }; //WaferForm 和 壓差監控之後再開放  , formTestMode


            try
            {
                //20190529 移除未開放或使用不到的功能
                tbcMain.TabPages.Remove(tabStatus);//狀態查詢
                tbcMain.TabPages.Remove(tabSetting);//舊設定功能
                tbcMain.TabPages.Remove(tabWafer);//Wafer 刪帳功能

                for (int i = 0; i < ctrlForm.Length; i++)
                {
                    ((Form)ctrlForm[i]).TopLevel = false;
                    tbcMain.TabPages[i].Controls.Add(((Form)ctrlForm[i]));
                    ((Form)ctrlForm[i]).Show();
                    tbcMain.SelectTab(i);
                }

                //20190531 未登入時隱藏功能

                AuthorityUpdate.tabPages.Add("tabRunning", tabRunning);
                AuthorityUpdate.tabPages.Add("tabDIO", tabDIO);
                AuthorityUpdate.tabPages.Add("tabNewSetting", tabNewSetting);
                AuthorityUpdate.tabPages.Add("tbDiffMonitor", tbDiffMonitor);
                tbcMain.TabPages.Remove(tabDIO);//IO點檢
                tbcMain.TabPages.Remove(tabNewSetting);//設定功能
                tbcMain.TabPages.Remove(tbDiffMonitor);//壓差顯示
                tbcMain.TabPages.Remove(tabRunning);//壓差顯示

                tbcMain.SelectTab(0);

                alarmFrom.Show();
                //alarmFrom.SendToBack();
                alarmFrom.Hide();
                BarcodeForm.Show();
                BarcodeForm.Hide();

                CurrentRecipe_lb.Text = SystemConfig.Get().CurrentRecipe;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            //Thread.Sleep(2000);

            if (SplashScreen.Instance != null)
            {
                SplashScreen.Instance.BeginInvoke(new MethodInvoker(SplashScreen.Instance.Dispose));
                SplashScreen.Instance = null;
            }
            this.Width = oldWidth;
            this.Height = oldHeight;
            this.WindowState = FormWindowState.Maximized;

            RouteCtrl.ConnectAll();
            //AuthorityUpdate.UpdateFuncGroupEnable("INIT");//init 權限
            //RouteCtrl.ConnectAll();

            this.Width = oldWidth;
            this.Height = oldHeight;
            this.WindowState = FormWindowState.Maximized;
            ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateCheckBox));
        }

        private void UpdateCheckBox(object input)
        {


            //DIOUpdate.UpdateDIOStatus("RED", "False");
            //DIOUpdate.UpdateDIOStatus("ORANGE", "False");
            //DIOUpdate.UpdateDIOStatus("GREEN", "False");
            //DIOUpdate.UpdateDIOStatus("BLUE", "False");
            //DIOUpdate.UpdateDIOStatus("BUZZER1", "False");
            //DIOUpdate.UpdateDIOStatus("BUZZER2", "False");

            foreach (Node node in NodeManagement.GetList())
            {
                MonitoringUpdate.DisableUpdate(node.Name + "_disable", !node.Enable);
            }


        }

        private void LoadPort01_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ((DataGridView)sender).ClearSelection();
        }

        private void btnLogInOut_Click(object sender, EventArgs e)
        {
            switch (btnLogInOut.Text)
            {
                case "Login":
                    //GUI.FormLogin formLogin = new GUI.FormLogin();
                    //formLogin.ShowDialog();

                    using (var form = new GUI.FormLogin())
                    {
                        var result = form.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            tbcMain.Enabled = true;
                            Mode_btn.Enabled = true;
                            DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
                        }
                    }
                    break;
                case "Logout":
                    btnChgPWD.Visible = false;
                    AuthorityUpdate.UpdateLogoutInfo();
                    //disable authroity function
                    AuthorityUpdate.UpdateFuncGroupEnable("INIT");
                    //((TabControl)formSystem.Controls["tbcSystemSetting"]).SelectTab(0);//20190529 取消
                    tbcMain.SelectTab(0);
                    tbcMain.Enabled = false;
                    Mode_btn.Text = "Online-Mode";
                    Mode_btn.BackColor = Color.Green;
                    Mode_btn.Enabled = false;
                    btnManual.Enabled = false;
                    btnManual.BackColor = Color.Gray;


                    DIOUpdate.UpdateControlButton("Start_btn", Initial);
                    Global.currentUser = "";
                    DIOUpdate.UpdateControlButton("Stop_btn", false);
                    DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
                    if (formManual != null)
                    {
                        formManual.Close();
                    }

                    break;
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FormAlarmHis form4 = new FormAlarmHis();
            form4.Text = "Message History";
            form4.label21.Text = "Message History";
            form4.Show();
        }

        private void bBBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAlarmHis form4 = new FormAlarmHis();
            form4.Show();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            string strMsg = "This equipment performs the initialization and origin search OK?\r\n" + "This equipment will be initalized, each axis will return to home position.\r\n" + "Check the condition of the wafer.";
            if (MessageBox.Show(strMsg, "Initialize", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                foreach (Node each in NodeManagement.GetList())
                {
                    //string Message = "";
                    switch (each.Type)
                    {
                        case "ALIGNER":
                            each.ErrorMsg = "";
                            //each.ExcuteScript("AlignerStateGet", "GetStatsBeforeInit", out Message);
                            break;
                        case "ROBOT":
                            each.ErrorMsg = "";
                            //each.ExcuteScript("RobotStateGet", "GetStatsBeforeInit", out Message);
                            break;
                    }
                }
            }
        }

        private void ProceedInitial()
        {

            foreach (Node each in NodeManagement.GetList())
            {
                each.InitialComplete = false;
                each.CheckStatus = false;
                //string Message = "";
                switch (each.Type.ToUpper())
                {
                    case "ROBOT":
                        //each.ExcuteScript("RobotInit", "Initialize", out Message);
                        break;
                        //先做ROBOT
                        //case "ALIGNER":
                        //    each.ExcuteScript("AlignerInit", "Initialize");
                        //    break;
                        //case "LOADPORT":
                        //    each.ExcuteScript("LoadPortInit", "Initialize");
                        //    break;
                }
            }
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            string strMsg = "Move to Home position. OK?";
            if (MessageBox.Show(strMsg, "Org.Back", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                string Message = "";
                Transaction txn = new Transaction();
                txn.Method = Transaction.Command.RobotType.Home;
                NodeManagement.Get("Robot01").SendCommand(txn, out Message);
                txn = new Transaction();
                txn.Method = Transaction.Command.RobotType.Home;
                NodeManagement.Get("Robot02").SendCommand(txn, out Message);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            string strMsg = "Switching to manual mode.\r\n" + "In this mode, your operation may damage the equipment.\r\n" + "Suffcient cautions are required for your operation.";
            //if (MessageBox.Show(strMsg, "Manual", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) == DialogResult.OK)
            //{
            GUI.FormManual formManual = new GUI.FormManual();
            formManual.Show();
            //}
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            FormUnitCtrlData form2 = new FormUnitCtrlData();
            form2.ShowDialog();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            FormTransTest form3 = new FormTransTest();
            form3.ShowDialog();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            UI_TEST.LLSetting lLSetting = new UI_TEST.LLSetting();
            lLSetting.ShowDialog();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI_TEST.Setting setting = new UI_TEST.Setting();
            setting.ShowDialog();
        }

        private void terminalToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FormTerminal formTerminal = new FormTerminal();
            formTerminal.ShowDialog();
        }

        private void btnTeach_Click(object sender, EventArgs e)
        {
            UI_TEST.Teaching teaching = new UI_TEST.Teaching();
            teaching.ShowDialog();
        }

        private void btnVersion_Click(object sender, EventArgs e)
        {
            GUI.FormVersion formVersion = new GUI.FormVersion();
            formVersion.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            GUI.FormLogSave formLogSave = new GUI.FormLogSave();
            formLogSave.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FormAlarm alarmFrom = new FormAlarm();
            alarmFrom.Text = "MessageFrom";
            alarmFrom.BackColor = Color.Blue;
            alarmFrom.ResetAll_bt.Enabled = false;
            alarmFrom.ShowDialog();
        }

        private void aAAToolStripMenuItem_Click(object sender, EventArgs e)
        {

            alarmFrom.Visible = true;
        }



        public void On_Command_Excuted(Node Node, Transaction Txn, ReturnMessage Msg)
        {
            logger.Debug("On_Command_Excuted");
            //string Message = "";

            Transaction SendTxn = new Transaction();

            if (Txn.Method == Transaction.Command.LoadPortType.Reset)
            {
                AlarmUpdate.UpdateAlarmList(AlarmManagement.GetAll());
            }



            switch (Node.Type)
            {
                case "LOADPORT":
                    switch (Txn.Method)
                    {
                        case Transaction.Command.LoadPortType.GetMapping:

                        case Transaction.Command.LoadPortType.Unload:
                        case Transaction.Command.LoadPortType.MappingUnload:
                        case Transaction.Command.LoadPortType.DoorUp:
                        case Transaction.Command.LoadPortType.InitialPos:
                        case Transaction.Command.LoadPortType.ForceInitialPos:
                            //WaferAssignUpdate.RefreshMapping(Node.Name);
                            MonitoringUpdate.UpdateNodesJob(Node.Name);
                            WaferAssignUpdate.UpdateNodesJob(Node.Name);
                            RunningUpdate.UpdateNodesJob(Node.Name);
                            break;
                    }
                    break;
                case "ROBOT":
                    switch (Txn.Method)
                    {
                        case Transaction.Command.RobotType.GetMapping:
                            // WaferAssignUpdate.RefreshMapping(Node.CurrentPosition);
                            MonitoringUpdate.UpdateNodesJob(Node.CurrentPosition);
                            break;
                    }
                    break;
            }

            switch (Txn.FormName)
            {
                case "GetStatsBeforeInit":
                    switch (Txn.Method)
                    {
                        //case Transaction.Command.AlignerType.GetStatus:

                        //    break;
                        //case Transaction.Command.RobotType.GetCombineStatus:

                        //    break;
                        //case Transaction.Command.AlignerType.GetSpeed:

                        //    break;
                        case Transaction.Command.AlignerType.GetRIO:
                            if (Msg.Value == null || Msg.Value.IndexOf(",") < 0)
                            {
                                break;
                            }
                            string[] result = Msg.Value.Split(',');
                            switch (result[0])
                            {
                                case "004":

                                    if (result[1].Equals("1"))
                                    {
                                        Node.ErrorMsg += "Present_R 在席存在 ";
                                    }

                                    break;
                                case "005":
                                    if (result[1].Equals("1"))
                                    {
                                        Node.ErrorMsg += "Present_L 在席存在 ";
                                    }
                                    break;
                            }
                            break;
                            //case Transaction.Command.AlignerType.GetError:

                            //    break;
                            //case Transaction.Command.AlignerType.GetMode:

                            //    break;
                            //case Transaction.Command.AlignerType.GetSV:

                            //    break;
                    }
                    break;
                case "FormStatus":
                    Util.StateUtil.UpdateSTS(Node.Name, Msg.Value);
                    break;
                case "PauseProcedure":

                    break;
                case "FormManual":
                case "FormManual-1":
                    switch (Node.Type)
                    {
                        case "SMARTTAG":
                            if (!Txn.Method.Equals(Transaction.Command.SmartTagType.GetLCDData))
                            {
                                //ManualPortStatusUpdate.LockUI(false);
                            }
                            break;
                        case "LOADPORT":
                            if (!Txn.CommandType.Equals("MOV") && !Txn.CommandType.Equals("HCS"))
                            {
                                //ManualPortStatusUpdate.LockUI(false);
                            }
                            else
                            {
                                if (Txn.Method.Equals(Transaction.Command.LoadPortType.Reset))
                                {
                                    // ManualPortStatusUpdate.LockUI(false);
                                }
                            }
                            ManualPortStatusUpdate.UpdateLog(Node.Name, Msg.Command + " Excuted");
                            switch (Txn.Method)
                            {
                                case Transaction.Command.LoadPortType.ReadVersion:
                                    ManualPortStatusUpdate.UpdateVersion(Node.Name, Msg.Value);
                                    break;
                                case Transaction.Command.LoadPortType.GetLED:
                                    ManualPortStatusUpdate.UpdateLED(Node.Name, Msg.Value);
                                    break;
                                case Transaction.Command.LoadPortType.ReadStatus:
                                    ManualPortStatusUpdate.UpdateStatus(Node.Name, Msg.Value);
                                    break;
                                case Transaction.Command.LoadPortType.GetCount:

                                    break;
                                case Transaction.Command.LoadPortType.GetMapping:
                                    ManualPortStatusUpdate.UpdateMapping(Node.Name, Msg.Value);
                                    break;
                            }
                            break;
                        case "OCR":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.OCRType.GetOnline:
                                    OCRStatusUpdate.UpdateOCROnlineMode(Node.Name, Msg.Value);
                                    break;

                            }
                            break;
                        case "ROBOT":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.RobotType.Speed:
                                case Transaction.Command.RobotType.Mode:
                                case Transaction.Command.RobotType.Reset:
                                case Transaction.Command.RobotType.Servo:

                                    ManualRobotStatusUpdate.UpdateGUI(Txn, Node.Name, Msg.Value);//update 手動功能畫面 
                                    break;
                                case Transaction.Command.RobotType.GetSpeed:
                                case Transaction.Command.RobotType.GetRIO:
                                case Transaction.Command.RobotType.GetError:
                                case Transaction.Command.RobotType.GetMode:
                                case Transaction.Command.RobotType.GetStatus:
                                case Transaction.Command.RobotType.GetSV:
                                    ManualRobotStatusUpdate.UpdateGUI(Txn, Node.Name, Msg.Value);//update 手動功能畫面
                                    break;
                                case Transaction.Command.RobotType.GetCombineStatus:
                                    ManualRobotStatusUpdate.UpdateGUI(Txn, Node.Name, Msg.Command);//update 手動功能畫面
                                    break;
                            }
                            break;
                        case "ALIGNER":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.AlignerType.Speed:
                                case Transaction.Command.AlignerType.Mode:
                                case Transaction.Command.AlignerType.Reset:
                                case Transaction.Command.AlignerType.Servo:
                                    Thread.Sleep(500);
                                    //向Aligner 詢問狀態
                                    Node aligner = NodeManagement.Get(Node.Name);
                                    String script_name = aligner.Brand.ToUpper().Equals("SANWA") ? "AlignerStateGet" : "AlignerStateGet(Kawasaki)";
                                    //aligner.ExcuteScript(script_name, "FormManual", out Message);
                                    ManualAlignerStatusUpdate.UpdateGUI(Txn, Node.Name, Msg.Value);//update 
                                    break;
                                case Transaction.Command.AlignerType.GetMode:
                                case Transaction.Command.AlignerType.GetSV:
                                case Transaction.Command.AlignerType.GetStatus:
                                case Transaction.Command.AlignerType.GetSpeed:
                                case Transaction.Command.AlignerType.GetRIO:
                                case Transaction.Command.AlignerType.GetError:
                                    ManualAlignerStatusUpdate.UpdateGUI(Txn, Node.Name, Msg.Value);//update 手動功能畫面
                                    break;
                                case Transaction.Command.RobotType.GetCombineStatus:
                                    ManualAlignerStatusUpdate.UpdateGUI(Txn, Node.Name, Msg.Command);//update 手動功能畫面
                                    break;
                            }
                            break;

                    }
                    break;

                default:

                    break;
            }
        }

        public void On_Command_Error(Node Node, Transaction Txn, ReturnMessage Msg)
        {

            logger.Debug("On_Command_Error");
            AlarmInfo CurrentAlarm = new AlarmInfo();
            CurrentAlarm.NodeName = Node.Name;
            CurrentAlarm.AlarmCode = Msg.Value;
            CurrentAlarm.NeedReset = true;
            try
            {

                AlarmMessage Detail = AlmMapping.Get(Node.Name, CurrentAlarm.AlarmCode);

                CurrentAlarm.SystemAlarmCode = Detail.CodeID;
                CurrentAlarm.Desc = Detail.Code_Cause;
                CurrentAlarm.EngDesc = Detail.Code_Cause_English;
                CurrentAlarm.Type = Detail.Code_Type;
                CurrentAlarm.IsStop = Detail.IsStop;
                if (CurrentAlarm.IsStop)
                {
                    Start = false;
                    Initial = false;
                    XfeCrossZone.Stop();
                }
            }
            catch (Exception e)
            {
                CurrentAlarm.Desc = "未定義";
                logger.Error(Node.Controller + "-" + Node.AdrNo + "(GetAlarmMessage)" + e.Message + "\n" + e.StackTrace);
            }
            CurrentAlarm.TimeStamp = DateTime.Now;

            AlarmManagement.Add(CurrentAlarm);

            AlarmUpdate.UpdateAlarmList(AlarmManagement.GetAll());
            AlarmUpdate.UpdateAlarmHistory(AlarmManagement.GetHistory());
            DIOUpdate.UpdateControlButton("Start_btn", false);
            DIOUpdate.UpdateControlButton("Stop_btn", false);
            DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
            RunMode = "";
        }

        public void On_Command_Finished(Node Node, Transaction Txn, ReturnMessage Msg)
        {
            logger.Debug("On_Command_Finished");
            //Transaction txn = new Transaction();
            switch (Txn.FormName)
            {
                case "ChangeAlignWaferSize":
                    switch (Node.Type)
                    {
                        case "ROBOT":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.RobotType.GetWait:
                                    Node.WaitForFinish = false;
                                    break;
                            }
                            break;
                    }
                    break;
                case "FormManual":
                case "FormManual-1":
                    switch (Node.Type)
                    {
                        case "SMARTTAG":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.SmartTagType.GetLCDData:
                                    ManualPortStatusUpdate.UpdateID(Msg.Value);
                                    break;
                            }
                            //ManualPortStatusUpdate.LockUI(false);
                            break;
                        case "LOADPORT":

                            ManualPortStatusUpdate.UpdateLog(Node.Name, Msg.Command + " Finished");
                            //ManualPortStatusUpdate.LockUI(false);

                            break;

                        case "ROBOT":
                            ManualRobotStatusUpdate.UpdateGUI(Txn, Node.Name, Msg.Value);//update 手動功能畫面
                            break;
                        case "ALIGNER":
                            ManualAlignerStatusUpdate.UpdateGUI(Txn, Node.Name, Msg.Value);//update 手動功能畫面
                            break;
                    }
                    break;
                default:
                    switch (Node.Type)
                    {
                        case "ROBOT":
                        //switch (Txn.Method)
                        //{
                        //    case Transaction.Command.RobotType.Mapping: //when 200mm port mapped by robot's fork, then port's busy switch to false.
                        //        NodeManagement.Get(Txn.Position).Busy = false;
                        //        break;
                        //}
                        //break;
                        case "LOADPORT":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.LoadPortType.Unload:
                                case Transaction.Command.LoadPortType.MappingUnload:
                                case Transaction.Command.LoadPortType.DoorUp:
                                case Transaction.Command.LoadPortType.InitialPos:
                                case Transaction.Command.LoadPortType.ForceInitialPos:
                                    MonitoringUpdate.UpdateFoupID(Node.Name,"");
                                    WaferAssignUpdate.UpdateFoupID(Node.Name,"");
                                    break;
                            }
                            break;
                        case "OCR":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.OCRType.Read:
                                    OCRUpdate.UpdateOCRRead(Node.Name, Msg.Value, NodeManagement.Get(Node.Associated_Node).JobList["1"]);
                                    OCRStatusUpdate.UpdateOCRRead(Node.Name, Msg.Value);
                                    break;
                            }
                            break;
                    }
                    break;
            }

        }

        public void On_Command_TimeOut(Node Node, Transaction Txn)
        {
            logger.Debug("On_Command_TimeOut");
            AlarmInfo CurrentAlarm = new AlarmInfo();
            CurrentAlarm.NodeName = Node.Name;
            CurrentAlarm.AlarmCode = "00200002";
            CurrentAlarm.NeedReset = false;
            try
            {

                AlarmMessage Detail = AlmMapping.Get(Node.Name, CurrentAlarm.AlarmCode);

                CurrentAlarm.SystemAlarmCode = Detail.CodeID;
                CurrentAlarm.Desc = Detail.Code_Cause;
                CurrentAlarm.EngDesc = Detail.Code_Cause_English;
                CurrentAlarm.Type = Detail.Code_Type;
                CurrentAlarm.IsStop = Detail.IsStop;
                if (CurrentAlarm.IsStop)
                {
                    XfeCrossZone.Stop();
                }
            }
            catch (Exception e)
            {
                CurrentAlarm.Desc = "未定義";
                logger.Error(Node.Controller + "-" + Node.AdrNo + "(GetAlarmMessage)" + e.Message + "\n" + e.StackTrace);
            }
            CurrentAlarm.TimeStamp = DateTime.Now;
            AlarmManagement.Add(CurrentAlarm);
            AlarmUpdate.UpdateAlarmList(AlarmManagement.GetAll());
            AlarmUpdate.UpdateAlarmHistory(AlarmManagement.GetHistory());
        }

        public void On_Event_Trigger(Node Node, ReturnMessage Msg)
        {
            logger.Debug("On_Event_Trigger");
            string TaskName = "";
            string Message = "";

            TaskJobManagment.CurrentProceedTask Task;
            try
            {
                Transaction txn = new Transaction();
                switch (Node.Type)
                {
                    case "LOADPORT":
                        switch (Msg.Command)
                        {
                            case "MANSW":
                                if (Node.OPACCESS)
                                {
                                    Barcodeupdate.UpdateLoadport(Node.Name, false);
                                    //Node.OPACCESS = false;
                                    //TaskName = "LOADPORT_OPEN";
                                    //Message = "";
                                    //Dictionary<string, string> param = new Dictionary<string, string>();
                                    //param.Add("@Target", Node.Name);

                                    //RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out Task, TaskName, param);
                                }
                                break;
                            case "MANOF":

                                break;
                            case "SMTON":

                                break;
                            case "PODOF":
                                if (Node.OrgSearchComplete && !Node.CurrentStatus.Equals("UnloadComplete"))
                                {
                                    Node.CurrentStatus = "UnloadComplete";
                                    TaskName = "LOADPORT_UNLOADCOMPLETE";
                                    Message = "";
                                    Dictionary<string, string> param1 = new Dictionary<string, string>();
                                    param1.Add("@Target", Node.Name);

                                    RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out Task, TaskName, param1);
                                }
                                break;
                            case "PODON":
                                //Foup Arrived
                                if (Node.OrgSearchComplete && !Node.CurrentStatus.Equals("ReadyToLoad"))
                                {
                                    Node.CurrentStatus = "ReadyToLoad";
                                    TaskName = "LOADPORT_READYTOLOAD";
                                    Message = "";
                                    Dictionary<string, string> param2 = new Dictionary<string, string>();
                                    param2.Add("@Target", Node.Name);

                                    RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out Task, TaskName, param2);
                                }
                                break;
                            case "ABNST":

                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Error(e.StackTrace, e);
            }

        }

        public void On_Node_State_Changed(Node Node, string Status)
        {
            logger.Debug("On_Node_State_Changed");
            NodeStatusUpdate.UpdateNodeState(Node.Name, Status);
            switch (Node.Type.ToUpper())
            {
                case "ROBOT":

                    ManualRobotStatusUpdate.UpdateRobotStatus(Node.Name, Status);//update 手動功能畫面
                    break;
                case "ALIGNER":

                    ManualAlignerStatusUpdate.UpdateAlignerStatus(Node.Name, Status);//update 手動功能畫面
                    break;

            }
        }

        public void On_Eqp_State_Changed(string OldStatus, string NewStatus)
        {
            NodeStatusUpdate.UpdateCurrentState(NewStatus);
            //StateRecord.EqpStateUpdate("Sorter", OldStatus, NewStatus);
        }

        public void On_Node_Connection_Changed(string NodeName, string Status)
        {

            ConnectionStatusUpdate.UpdateControllerStatus(NodeName, Status);
            Node node = NodeManagement.Get(NodeName);

            switch (node.Type.ToUpper())
            {
                case "ROBOT":

                    ManualRobotStatusUpdate.UpdateRobotConnection(NodeName, Status);//update 手動功能畫面
                    break;
                case "ALIGNER":

                    ManualAlignerStatusUpdate.UpdateAlignerConnection(NodeName, Status);//update 手動功能畫面
                    break;
                case "OCR":
                    OCRStatusUpdate.UpdateOCRConnection(NodeName, Status);
                    break;
            }


            logger.Debug("On_Node_Connection_Changed");
        }

        public void On_Port_Begin(string PortName, string FormName)
        {
            logger.Debug("On_Port_Begin");



        }

        public void On_Port_Finished(string PortName, string FormName)
        {
            logger.Debug("On_Port_Finished");
            try
            {

            }
            catch (Exception e)
            {
                logger.Error(e.StackTrace);
            }
        }



        public void On_Mode_Changed(string Mode)
        {
            logger.Debug("On_Mode_Changed");

            ConnectionStatusUpdate.UpdateModeStatus(Mode);
            RunningUpdate.UpdateModeStatus(Mode);
            //MonitoringUpdate.UpdateStatus(Mode);
            foreach (Node port in NodeManagement.GetLoadPortList())
            {
                // WaferAssignUpdate.RefreshMapping(port.Name);
                if (Mode.Equals("Stop"))
                {
                    // WaferAssignUpdate.ResetAssignCM(port.Name, true);
                }
            }

        }

        public void On_Job_Location_Changed(Job Job)
        {
            logger.Debug("On_Job_Location_Changed");
            MonitoringUpdate.UpdateJobMove(Job.Job_Id);
            WaferAssignUpdate.UpdateJobMove(Job.Job_Id);
            RunningUpdate.UpdateJobMove(Job.Job_Id);
        }








        public void On_Connection_Status_Report(string DIOName, string Status)
        {
            ConnectionStatusUpdate.UpdateControllerStatus(DIOName, Status);
        }


        public void On_Data_Chnaged(string Parameter, string Value, string Type)
        {
            switch (Parameter)
            {
                case "DIFFERENTIAL":
                    DifferentialMonitorUpdate.UpdateChart(Parameter, Value);
                    break;
                case "BF1_DOOR_OPEN":
                case "BF1_ARM_EXTEND_ENABLE":
                case "BF2_DOOR_OPEN":
                case "BF2_ARM_EXTEND_ENABLE":
                case "ARM_NOT_EXTEND_BF1":
                case "ARM_NOT_EXTEND_BF2":
                    DIOUpdate.UpdateInterLock(Parameter, Value);
                    break;
                default:
                    DIOUpdate.UpdateDIOStatus(Parameter, Value);
                    IOUpdate.UpdateDIO(Parameter, Value, Type);
                    //if (Parameter.ToUpper().Equals("SAFETYRELAY")&&Value.ToUpper().Equals("TRUE"))
                    //{
                    //    SpinWait.SpinUntil(() => false, 3000);
                    //    foreach(Node port in NodeManagement.GetLoadPortList())
                    //    {
                    //        if (port.Enable)
                    //        {
                    //            ThreadPool.QueueUserWorkItem(new WaitCallback(port.GetController().Start));
                    //        }
                    //    }
                    //}
                    break;
            }


        }

        public void On_Alarm_Happen(string DIOName, string ErrorCode)
        {

            AlarmInfo CurrentAlarm = new AlarmInfo();
            CurrentAlarm.NodeName = DIOName;
            CurrentAlarm.AlarmCode = ErrorCode;
            CurrentAlarm.NeedReset = false;
            try
            {

                AlarmMessage Detail;
                if (DIOName.Equals("DIO"))
                {
                    Detail = AlmMapping.Get("SYSTEM", CurrentAlarm.AlarmCode);
                }
                else
                {
                    Detail = AlmMapping.Get("DIO", CurrentAlarm.AlarmCode);
                }

                CurrentAlarm.SystemAlarmCode = Detail.CodeID;
                CurrentAlarm.Desc = Detail.Code_Cause;
                CurrentAlarm.EngDesc = Detail.Code_Cause_English;
                CurrentAlarm.Type = Detail.Code_Type;
                CurrentAlarm.IsStop = Detail.IsStop;


                if (CurrentAlarm.IsStop)
                {
                    //XfeCrossZone.Stop();
                }
            }
            catch (Exception e)
            {
                CurrentAlarm.Desc = "未定義";
                logger.Error(DIOName + "(GetAlarmMessage)" + e.Message + "\n" + e.StackTrace);
            }
            CurrentAlarm.TimeStamp = DateTime.Now;
            AlarmManagement.Add(CurrentAlarm);
            AlarmUpdate.UpdateAlarmList(AlarmManagement.GetAll());
            AlarmUpdate.UpdateAlarmHistory(AlarmManagement.GetHistory());
        }

        public void On_Connection_Error(string DIOName, string ErrorMsg)
        {
            //斷線 發ALARM
            logger.Debug("On_Error_Occurred");
            AlarmInfo CurrentAlarm = new AlarmInfo();
            CurrentAlarm.NodeName = DIOName;
            CurrentAlarm.AlarmCode = "00200001";
            CurrentAlarm.NeedReset = false;
            try
            {

                AlarmMessage Detail = AlmMapping.Get("DIO", CurrentAlarm.AlarmCode);

                CurrentAlarm.SystemAlarmCode = Detail.CodeID;
                CurrentAlarm.Desc = Detail.Code_Cause;
                CurrentAlarm.EngDesc = Detail.Code_Cause_English;
                CurrentAlarm.Type = Detail.Code_Type;
                CurrentAlarm.IsStop = Detail.IsStop;
                if (CurrentAlarm.IsStop)
                {
                    // RouteCtrl.Stop();
                }
            }
            catch (Exception e)
            {
                CurrentAlarm.Desc = "未定義";
                logger.Error(DIOName + "(GetAlarmMessage)" + e.Message + "\n" + e.StackTrace);
            }
            CurrentAlarm.TimeStamp = DateTime.Now;
            AlarmManagement.Add(CurrentAlarm);
            AlarmUpdate.UpdateAlarmList(AlarmManagement.GetAll());
            AlarmUpdate.UpdateAlarmHistory(AlarmManagement.GetHistory());
        }





        private void Signal_MouseClick(object sender, MouseEventArgs e)
        {
            switch ((sender as Button).Name)
            {
                case "RED_Signal":
                    if (RouteControl.Instance.DIO.GetIO("DOUT", "RED").ToUpper().Equals("TRUE"))
                    {
                        RouteControl.Instance.DIO.SetIO("RED", "False");
                    }
                    else
                    {
                        RouteControl.Instance.DIO.SetIO("RED", "True");
                    }
                    break;
                case "ORANGE_Signal":
                    if (RouteControl.Instance.DIO.GetIO("DOUT", "ORANGE").ToUpper().Equals("TRUE"))
                    {
                        RouteControl.Instance.DIO.SetIO("ORANGE", "False");
                    }
                    else
                    {
                        RouteControl.Instance.DIO.SetIO("ORANGE", "True");
                    }
                    break;
                case "GREEN_Signal":
                    if (RouteControl.Instance.DIO.GetIO("DOUT", "GREEN").ToUpper().Equals("TRUE"))
                    {
                        RouteControl.Instance.DIO.SetIO("GREEN", "False");
                    }
                    else
                    {
                        RouteControl.Instance.DIO.SetIO("GREEN", "True");
                    }
                    break;
                case "BLUE_Signal":
                    if (RouteControl.Instance.DIO.GetIO("DOUT", "BLUE").ToUpper().Equals("TRUE"))
                    {
                        RouteControl.Instance.DIO.SetIO("BLUE", "False");
                    }
                    else
                    {
                        RouteControl.Instance.DIO.SetIO("BLUE", "True");
                    }
                    break;
                case "BUZZER1_Signal":
                    if (RouteControl.Instance.DIO.GetIO("DOUT", "BUZZER1").ToUpper().Equals("TRUE"))
                    {
                        RouteControl.Instance.DIO.SetIO("BUZZER1", "False");
                    }
                    else
                    {
                        RouteControl.Instance.DIO.SetIO("BUZZER1", "True");
                    }
                    break;
                case "BUZZER2_Signal":
                    if (RouteControl.Instance.DIO.GetIO("DOUT", "BUZZER2").ToUpper().Equals("TRUE"))
                    {
                        RouteControl.Instance.DIO.SetIO("BUZZER2", "False");
                    }
                    else
                    {
                        RouteControl.Instance.DIO.SetIO("BUZZER2", "True");
                    }
                    break;
            }
        }

        private void Conn_gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 1:
                    switch (e.Value)
                    {
                        case "Connecting":
                            e.CellStyle.BackColor = Color.Yellow;
                            e.CellStyle.ForeColor = Color.Black;
                            break;
                        case "Connected":
                            e.CellStyle.BackColor = Color.Green;
                            e.CellStyle.ForeColor = Color.White;
                            break;
                        default:
                            e.CellStyle.BackColor = Color.Red;
                            e.CellStyle.ForeColor = Color.White;
                            break;

                    }
                    break;

            }
        }

        private void Connection_btn_Click(object sender, EventArgs e)
        {

            if (Connection_btn.Tag.ToString() == "Offline")
            {
                // HostControl.OnlieReq();

                ConnectionStatusUpdate.UpdateOnlineStatus("Connecting");
            }
            else
            {
                //HostControl.OffLine();
                ConnectionStatusUpdate.UpdateOnlineStatus("Offline");
            }

        }

        private void Mode_btn_Click(object sender, EventArgs e)
        {
            if (XfeCrossZone.Running)
            {
                MessageBox.Show("請先停止搬運");
                return;
            }
            if (Mode_btn.Text.Equals("Manual-Mode"))
            {
                DIOUpdate.UpdateControlButton("Start_btn", false);
                DIOUpdate.UpdateControlButton("Stop_btn", false);
                DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
                Mode_btn.Text = "Online-Mode";
                Mode_btn.BackColor = Color.Green;
                btnManual.Enabled = false;
                btnManual.BackColor = Color.Gray;
                //tbcMian.Enabled = false;
                //tbcMian.SelectTab(0);
                CurrentMode = "AUTO";
                if (formManual != null)
                {
                    formManual.Close();
                }
            }
            else
            {
                if (Start)
                {
                    MessageBox.Show("請先切換至STOP");
                    return;
                }
                ////check 密碼
                //MD5 md5 = MD5.Create();
                //string[] use_info = ShowLoginDialog();
                //string user_id = use_info[0];
                //string password = use_info[1];
                //byte[] source = Encoding.Default.GetBytes(password);//將字串轉為Byte[]
                //byte[] crypto = md5.ComputeHash(source);//進行MD5加密
                //string md5_result = BitConverter.ToString(crypto).Replace("-", String.Empty).ToUpper();//取得 MD5
                //string config_password = SystemConfig.Get().AdminPassword;
                //if (md5_result.Equals(config_password))
                //{
                DIOUpdate.UpdateControlButton("Start_btn", false);
                DIOUpdate.UpdateControlButton("Stop_btn", false);
                DIOUpdate.UpdateControlButton("ALL_INIT_btn", false);
                Mode_btn.Text = "Manual-Mode";
                Mode_btn.BackColor = Color.Orange;
                btnManual.Enabled = true;
                btnManual.BackColor = Color.Orange;
                CurrentMode = "MANUAL";
                //tbcMian.Enabled = true;
                AuthorityUpdate.UpdateFuncGroupEnable(lbl_login_group.Text);//20190529 add 依照權限開放權限
                //}
                //else
                //{
                //    MessageBox.Show("Password incorrect !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                //    btnManual.Enabled = false;
                //    btnManual.BackColor = Color.Gray;
                //}

            }
        }


        public static string[] ShowLoginDialog()
        {
            string[] result = new string[] { "", "" };
            Form prompt = new Form()
            {
                Width = 450,
                Height = 280,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Authority check",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label lblUser = new Label() { Left = 30, Top = 20, Text = "User", Width = 200 };
            TextBox tbUser = new TextBox() { Left = 30, Top = 50, Width = 350, Text = "Administrator" };
            Label lblPassword = new Label() { Left = 30, Top = 90, Text = "Password", Width = 200 };
            TextBox tbPassword = new TextBox() { Left = 30, Top = 120, Width = 350 };
            tbPassword.PasswordChar = '*';
            Button confirmation = new Button() { Text = "Ok", Left = 280, Width = 100, Top = 170, DialogResult = DialogResult.OK, Height = 35 };
            lblUser.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblPassword.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tbUser.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tbPassword.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            confirmation.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(lblUser);
            prompt.Controls.Add(tbUser);
            prompt.Controls.Add(tbPassword);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(lblPassword);
            prompt.AcceptButton = confirmation;
            tbPassword.Focus();
            tbUser.Enabled = false;
            tbPassword.Text = "admin123";
            if (prompt.ShowDialog() == DialogResult.OK)
            {
                result[0] = tbUser.Text;
                result[1] = tbPassword.Text;
            }
            return result;
        }

        public void On_InterLock_Report(Node Node, bool InterLock)
        {
            //throw new NotImplementedException();
        }

        //private void Pause_btn_Click(object sender, EventArgs e)
        //{
        //    if (RouteCtrl.GetMode().Equals("Start"))
        //    {

        //        RouteCtrl.Pause();
        //        NodeStatusUpdate.UpdateCurrentState("Run");
        //        Pause_btn.Text = "Continue";

        //    }
        //    else if (RouteCtrl.GetMode().Equals("Pause"))
        //    {
        //        RouteCtrl.Continue();
        //        NodeStatusUpdate.UpdateCurrentState("Idle");
        //        Pause_btn.Text = "Pause";
        //    }
        //}

        private void btnHelp_Click(object sender, EventArgs e)
        {
            FormQuery form = new FormQuery();
            form.Show();
        }


        private void tbcMian_SelectedIndexChanged(object sender, EventArgs e)
        {
            //20190529 取消
            //if (tbcMian.SelectedTab.Text.Equals("Status"))
            //{
            //    formStatus.Focus();
            //}
            if (Start)
            {
                
                if (RunMode.ToUpper().Equals("FULLAUTO") && !tbcMain.SelectedTab.Name.Equals("tabMonitor"))
                {
                    tbcMain.SelectTab("tabMonitor");
                    MessageBox.Show("請先切換至STOP");
                }
                else if(RunMode.ToUpper().Equals("SEMIAUTO") && !tbcMain.SelectedTab.Name.Equals("tabWaferAssign"))
                {
                    tbcMain.SelectTab("tabWaferAssign");
                    MessageBox.Show("請先切換至STOP");
                }
               
            }
            if (tbcMain.SelectedTab.Text.Equals("Monitoring"))
            {
                Form form = Application.OpenForms["FormMonitoring"];
                foreach (Node port in NodeManagement.GetLoadPortList())
                {
                    Label present = form.Controls.Find(port.Name + "_Mode", true).FirstOrDefault() as Label;
                    if (present != null)
                    {
                        present.Text = port.Mode;
                    }
                }
            }
            if (tbcMain.SelectedTab.Text.Equals("Wafer Assign"))
            {
                

                FormWaferMapping.fromPort = "";
                FormWaferMapping.fromSlot = "";
                FormWaferMapping.toPort = "";
                FormWaferMapping.toSlot = "";
                Form form = Application.OpenForms["FormWaferMapping"];
                foreach (Node p in NodeManagement.GetLoadPortList())//更新所有目的地slot被選的狀態
                {
                    if (p.Enable && p.IsMapping)
                    {
                        foreach (Job eachSlot in p.JobList.Values)
                        {
                            if (!eachSlot.MapFlag && !eachSlot.ErrPosition)//找到空slot
                            {

                                Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                if (present != null)
                                {
                                    if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                    {//沒被選
                                        present.BackColor = Color.DimGray;
                                        present.ForeColor = Color.White;
                                    }
                                    else
                                    {//已被選
                                        present.BackColor = Color.Brown;
                                        present.ForeColor = Color.White;
                                    }
                                }
                            }
                            if (eachSlot.MapFlag && !eachSlot.ErrPosition)//找到wafer
                            {
                                Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                if (present != null)
                                {
                                    if (!eachSlot.Destination.Equals("") && !eachSlot.DestinationSlot.Equals("") && (!eachSlot.Destination.Equals(eachSlot.Position) && !eachSlot.DestinationSlot.Equals(eachSlot.Slot)))
                                    {//已被選
                                        present.BackColor = Color.Brown;
                                        present.ForeColor = Color.White;
                                    }
                                    else
                                    {//沒被選
                                        present.BackColor = Color.Green;
                                        present.ForeColor = Color.White;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            else
            {
                CurrentMode = "AUTO";
            }


        }

        private void menuMaintenace_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (ToolStripMenuItem item in menuMaintenace.Items)
            {
                string user_group = lbl_login_group.Text;
                string fun_form = "FormMain";
                string fun_ref = item.Name;
                Boolean enable = AuthorityUpdate.getFuncEnable(user_group, fun_form, fun_ref);
                item.Enabled = enable;
            }
        }

        private void Conn_gv_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }



        public void On_TaskJob_Aborted(TaskJobManagment.CurrentProceedTask Task, string NodeName, string ReportType, string Message)
        {
            DIOUpdate.UpdateControlButton("Start_btn", false);
            DIOUpdate.UpdateControlButton("Stop_btn", false);
            DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
            RunMode = "";
            WaferAssignUpdate.UpdateEnabled("FORM", true);

            if (Task.Id.IndexOf("FormManual") != -1)
            {
                ManualPortStatusUpdate.LockUI(false);
            }
            AlarmInfo CurrentAlarm = new AlarmInfo();
            CurrentAlarm.NodeName = "SYSTEM";
            CurrentAlarm.AlarmCode = Message;
            CurrentAlarm.NeedReset = false;
            try
            {

                AlarmMessage Detail = AlmMapping.Get("SYSTEM", CurrentAlarm.AlarmCode);
                if (!Detail.Code_Group.Equals("UNDEFINITION"))
                {
                    CurrentAlarm.SystemAlarmCode = Detail.CodeID;
                    CurrentAlarm.Desc = Detail.Code_Cause;
                    CurrentAlarm.EngDesc = Detail.Code_Cause_English;
                    CurrentAlarm.Type = Detail.Code_Type;
                    CurrentAlarm.IsStop = Detail.IsStop;
                    if (CurrentAlarm.IsStop)
                    {
                        Start = false;
                        Initial = false;
                        XfeCrossZone.Stop();
                    }
                    CurrentAlarm.TimeStamp = DateTime.Now;

                    AlarmManagement.Add(CurrentAlarm);

                    AlarmUpdate.UpdateAlarmList(AlarmManagement.GetAll());
                    AlarmUpdate.UpdateAlarmHistory(AlarmManagement.GetHistory());
                }
            }
            catch (Exception e)
            {
                CurrentAlarm.Desc = "未定義";
                logger.Error("(GetAlarmMessage)" + e.Message + "\n" + e.StackTrace);
            }


        }

        public void On_TaskJob_Finished(TaskJobManagment.CurrentProceedTask Task)
        {
            string TaskName = "";
            string Message = "";

            TaskJobManagment.CurrentProceedTask tmpTask;
            if (Task.Id.IndexOf("FormManual") != -1)
            {
                ManualPortStatusUpdate.LockUI(false);
            }
            switch (Task.ProceedTask.TaskName)
            {
                case "SORTER_INIT":
                    if (CurrentMode.Equals("AUTO"))
                    {
                        //啟用Start按鈕
                        DIOUpdate.UpdateControlButton("Start_btn", true);
                    }
                    //讓INIT按鈕由黃變綠色
                    DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
                    DIOUpdate.UpdateControlButton("Mode_btn", true);
                    xfe.Initial();

                    foreach (Node port in NodeManagement.GetLoadPortList())
                    {
                        if (port.Enable && port.Foup_Placement)
                        {

                            TaskName = "LOADPORT_READYTOLOAD";
                            Message = "";
                            Dictionary<string, string> param = new Dictionary<string, string>();
                            param.Add("@Target", port.Name);

                            RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out tmpTask, TaskName, param);
                            CarrierManagement.Add().SetLocation(port.Name);
                        }
                    }
                    Initial = true;
                    break;
                case "LOADPORT_OPEN":
                    Node currentPort = NodeManagement.Get(Task.Params["@Target"]);
                    if (Start)
                    {
                        if (currentPort.Mode.Equals("LD"))
                        {
                            AssignWafer(currentPort);

                        }
                        else if (currentPort.Mode.Equals("ULD"))
                        {

                            Node ld = SearchLoadport();
                            if (ld != null)
                            {
                                AssignWafer(ld);
                            }
                        }
                    }
                    if (Task.Id.Contains("Unload_btn"))
                    {
                        MonitoringUpdate.ButtonEnabled(Task.Params["@Target"].ToUpper() + "_Unload_btn", true);
                    }
                    break;
                case "LOADPORT_CLOSE_NOMAP":

                    ////test mode
                    //Node p = NodeManagement.Get(Task.Params["@Target"]);
                    //TaskName = "LOADPORT_OPEN";
                    //Message = "";
                    //Dictionary<string, string> param1 = new Dictionary<string, string>();
                    //param1.Add("@Target", p.Name);

                    //RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out tmpTask, TaskName, param1);

                    break;
            }

        }
        private Node SearchLoadport()
        {
            Node result = null;
            if (RunMode.Equals("FULLAUTO"))
            {
                var AvailableOPENs = from OPEN in NodeManagement.GetLoadPortList()
                                     where OPEN.Mode.Equals("LD") && OPEN.IsMapping
                                     orderby OPEN.LoadTime
                                     from wafer in OPEN.JobList.Values
                                     where wafer.MapFlag && !wafer.ErrPosition
                                     select OPEN;
                if (AvailableOPENs.Count() != 0)
                {
                    //List<Node> OPENS = AvailableOPENs.ToList();
                    //OPENS.Sort((x, y) => { return x.LoadTime.CompareTo(y.LoadTime); });
                    result = AvailableOPENs.First();
                }
            }
            else if (RunMode.Equals("SEMIAUTO"))
            {
                result = SearchAvailableWafer();
            }
            return result;
        }
        private Node SearchAvailableWafer()
        {
            Node result = null;
            var AvailableWafers = from job in JobManagement.GetJobList()
                                  where job.NeedProcess
                                  orderby job.AssignTime
                                  select job;
            if (AvailableWafers.Count() != 0)
            {
                result = NodeManagement.Get(AvailableWafers.First().Position);
            }
            if (result == null)
            {
                DIOUpdate.UpdateControlButton("Start_btn", true);
                DIOUpdate.UpdateControlButton("Stop_btn", false);
                DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
                DIOUpdate.UpdateControlButton("Mode_btn", true);
                MessageBox.Show("Transfer finished");
                Start = false;
            }
            return result;
        }
        static object AssignLock = new object();
        private void AssignWafer(Node Loadport)
        {
            lock (AssignLock)
            {
                if (XfeCrossZone.Running)
                {
                    return;
                }
                if (CurrentMode.Equals("MANUAL"))
                {
                    return;
                }
                if (CurrentMode.Equals("AUTO") && !Start)
                {
                    return;
                }


                if (CurrentMode.Equals("AUTO") && RunMode.Equals("FULLAUTO"))
                {

                    var LD_Jobs = from wafer in Loadport.JobList.Values
                                  where wafer.MapFlag && !wafer.ErrPosition && wafer.Destination.Equals("")
                                  orderby Convert.ToInt16(wafer.Slot)
                                  select wafer;

                    var AvailableFOSBs = from FOSB in NodeManagement.GetLoadPortList()
                                         where FOSB.Mode.Equals("ULD") && FOSB.IsMapping
                                         orderby FOSB.LoadTime
                                         select FOSB;

                    bool isAssign = false;
                    bool isAssign2 = false;
                    foreach (Node fosb in AvailableFOSBs)
                    {//找到能放的FOSB

                        var ULD_Jobs = (from Slot in fosb.JobList.Values
                                        where !Slot.MapFlag && !Slot.ErrPosition && !Slot.IsAssigned
                                        select Slot).OrderByDescending(x => Convert.ToInt16(x.Slot));

                        foreach (Job wafer in LD_Jobs)
                        {//檢查LD的所有WAFER

                            isAssign = false;
                            foreach (Job Slot in ULD_Jobs)
                            {//搜尋所有FOSB Slot 找到能放的     
                                if (Slot.PreviousSlotNotEmpty)
                                {//下一層有片所以不能放
                                    Slot.Locked = true;
                                }
                                else
                                {
                                    wafer.NeedProcess = true;
                                    wafer.ProcessFlag = false;
                                    wafer.AssignPort(fosb.Name, Slot.Slot);
                                    isAssign = true;
                                    isAssign2 = true;
                                    Slot.IsAssigned = true;
                                    logger.Debug("Assign booktest from " + Loadport.Name + " slot:" + wafer.Slot + " to " + fosb.Name + " slot:" + Slot.Slot);

                                    break;
                                }
                            }
                            if (!isAssign)
                            {
                                break;
                            }
                        }
                        if (isAssign2)
                        {//已經指派過的話就跳脫
                            break;
                        }
                    }
                    LD_Jobs = from wafer in Loadport.JobList.Values
                              where wafer.NeedProcess
                              orderby Convert.ToInt16(wafer.Slot)
                              select wafer;
                    for (int i = 1; i < LD_Jobs.Count(); i = i + 2)
                    {//重新排序目的地for雙Arm
                        Job upper = LD_Jobs.ToList()[i];
                        Job lower = LD_Jobs.ToList()[i - 1];
                        if (upper.Destination.Equals(lower.Destination) && upper.NeedProcess && lower.NeedProcess)
                        {
                            string swapDes = upper.Destination;
                            string swapSlot = upper.DestinationSlot;
                            upper.AssignPort(lower.Destination, lower.DestinationSlot);
                            lower.AssignPort(swapDes, swapSlot);
                            logger.Debug("Reverse booktest from " + Loadport.Name + " slot:" + upper.Slot + " to " + upper.Destination + " slot:" + upper.DestinationSlot);
                            logger.Debug("Reverse booktest from " + Loadport.Name + " slot:" + lower.Slot + " to " + upper.Destination + " slot:" + lower.DestinationSlot);
                            logger.Debug("Reverse booktest ---------- ");
                        }
                        //else
                        //{
                        //    i--;
                        //}
                    }
                    var NeedProcessSlot = from wafer in LD_Jobs
                                          where wafer.NeedProcess
                                          select wafer;

                    if (!XfeCrossZone.Running && NeedProcessSlot.Count() != 0)
                    {
                        if (!xfe.Start(Loadport.Name))
                        {
                            MessageBox.Show("xfe.Start fail!");
                        }
                    }

                }
                else
                { 

                    var NeedProcessSlot = from wafer in Loadport.JobList.Values
                                          where wafer.NeedProcess
                                          select wafer;

                    if (!XfeCrossZone.Running && NeedProcessSlot.Count() != 0)
                    {
                        if (!xfe.Start(Loadport.Name))
                        {
                            MessageBox.Show("xfe.Start fail!");
                        }
                    }
                }

            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (formManual == null)
            {
                formManual = new GUI.FormManual();
                formManual.Show();
            }
            else
            {
                formManual.Focus();
            }
        }

        public void On_SECS_Message(string msg)
        {

        }

        public void On_SECS_StatusChange(string type, string id, string content)
        {

        }


        private bool checkTask(TaskJobManagment.CurrentProceedTask Task1, TaskJobManagment.CurrentProceedTask Task2)
        {
            return Task1.Finished && Task2.Finished;
        }
        public static bool cycleRun = false;
        public void On_Transfer_Complete(XfeCrossZone xfe)
        {
            NodeStatusUpdate.UpdateCurrentState("IDLE");

            MonitoringUpdate.UpdateWPH((xfe.ProcessCount / (xfe.ProcessTime / 1000.0 / 60.0 / 60.0)).ToString("f1"));

            
                Node ld = SearchLoadport();
                if (ld != null)
                {
                    AssignWafer(ld);
                }
            
            WaferAssignUpdate.UpdateEnabled("FORM", true);
        }

        public void On_LoadPort_Selected(Node Port)
        {
            MonitoringUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", false);
        }

        public void On_UnLoadPort_Selected(Node Port)
        {
            MonitoringUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", false);
        }

        public void On_LoadPort_Complete(Node Port)
        {
            Adam.FoupInfo foup = new Adam.FoupInfo(SystemConfig.Get().CurrentRecipe, Global.currentUser, Port.FoupID);
            foreach (Job j in Port.JobList.Values)
            {
                int slot = Convert.ToInt16(j.Slot);
                foup.record[slot - 1] = new Adam.waferInfo("LOADPORT01", "KUMAFOUPF", slot, "LOADPORT02", "KUMAFOUPT", slot);
                foup.record[slot - 1].SetStartTime(j.StartTime);
                foup.record[slot - 1].setM12("");
                foup.record[slot - 1].setT7("");
                foup.record[slot - 1].SetEndTime(j.EndTime);
            }
            foup.Save();

            string TaskName = "LOADPORT_CLOSE_NOMAP";
            string Message = "";
            Dictionary<string, string> param1 = new Dictionary<string, string>();
            param1.Add("@Target", Port.Name);
            TaskJobManagment.CurrentProceedTask tmpTask;
            RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out tmpTask, TaskName, param1);


            //MonitoringUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", true);
        }

        public void On_UnLoadPort_Complete(Node Port)
        {

            var Available = from each in Port.JobList.Values
                            where !each.MapFlag && !each.ErrPosition && !each.Locked
                            select each;
            if (Available.Count() == 0)
            {//Unloadport 滿了 自動退
                string TaskName = "LOADPORT_CLOSE_NOMAP";
                string Message = "";
                Dictionary<string, string> param1 = new Dictionary<string, string>();
                param1.Add("@Target", Port.Name);
                TaskJobManagment.CurrentProceedTask tmpTask;
                RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out tmpTask, TaskName, param1);
            }
            else
            {
                MonitoringUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", true);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            fakeData("LOADPORT01");
            fakeData("LOADPORT02");
            fakeData("LOADPORT03");
            fakeData("LOADPORT04");
            WaferAssignUpdate.UpdateNodesJob("LOADPORT01");
            WaferAssignUpdate.UpdateNodesJob("LOADPORT02");
            WaferAssignUpdate.UpdateNodesJob("LOADPORT03");
            WaferAssignUpdate.UpdateNodesJob("LOADPORT04");
        }

        private void fakeData(string name)
        {
            //string Mapping = Msg.Value;
            string Mapping = "1111100111011000000000000";
            //if (!Mapping.Equals("0000000000000000000000000"))
            //{
            //    Mapping = "0000000110000000000000000";
            //}
            //WaferAssignUpdate.UpdateLoadPortMapping(Node.Name, Msg.Value);
            //if (Node.Name.Equals("LOADPORT01"))
            //{
            //    //Mapping = "1111111111111111111111111";
            //    Mapping = SystemConfig.Get().MappingData;
            //}
            Node Node = NodeManagement.Get(name);
            Node.MappingResult = Mapping;

            Node.IsMapping = true;

            int currentIdx = 1;
            for (int i = 0; i < Mapping.Length; i++)
            {
                Job wafer = RouteControl.CreateJob();
                wafer.Slot = (i + 1).ToString();
                wafer.FromPort = Node.Name;
                wafer.FromPortSlot = wafer.Slot;
                wafer.Position = Node.Name;
                wafer.AlignerFlag = false;
                string Slot = (i + 1).ToString("00");
                switch (Mapping[i])
                {
                    case '0':
                        wafer.Job_Id = "No wafer";
                        wafer.Host_Job_Id = wafer.Job_Id;
                        wafer.MapFlag = false;
                        wafer.ErrPosition = false;
                        //MappingData.Add(wafer);
                        break;
                    case '1':
                        while (true)
                        {
                            wafer.Job_Id = "Wafer" + currentIdx.ToString("000");
                            wafer.Host_Job_Id = wafer.Job_Id;
                            wafer.MapFlag = true;
                            wafer.ErrPosition = false;
                            if (JobManagement.Add(wafer.Job_Id, wafer))
                            {

                                //MappingData.Add(wafer);
                                break;
                            }
                            currentIdx++;
                        }

                        break;
                    case '2':
                    case 'E':
                        wafer.Job_Id = "Crossed";
                        wafer.Host_Job_Id = wafer.Job_Id;
                        wafer.MapFlag = true;
                        wafer.ErrPosition = true;
                        //MappingData.Add(wafer);
                        Node.IsMapping = false;
                        break;
                    default:
                    case '?':
                        wafer.Job_Id = "Undefined";
                        wafer.Host_Job_Id = wafer.Job_Id;
                        wafer.MapFlag = true;
                        wafer.ErrPosition = true;
                        //MappingData.Add(wafer);
                        Node.IsMapping = false;
                        break;
                    case 'W':
                        wafer.Job_Id = "Double";
                        wafer.Host_Job_Id = wafer.Job_Id;
                        wafer.MapFlag = true;
                        wafer.ErrPosition = true;
                        //MappingData.Add(wafer);
                        Node.IsMapping = false;
                        break;
                }
                if (!Node.AddJob(wafer.Slot, wafer))
                {
                    Job org = Node.GetJob(wafer.Slot);
                    JobManagement.Remove(org.Job_Id);
                    Node.RemoveJob(wafer.Slot);
                    Node.AddJob(wafer.Slot, wafer);
                }

            }
        }

        private void ALL_INIT_btn_Click(object sender, EventArgs e)
        {
            Recipe recipe = Recipe.Get(SystemConfig.Get().CurrentRecipe);
            string TaskName = "SORTER_INIT";
            string Message = "";
            TaskJobManagment.CurrentProceedTask Task;
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@AlingerSpeed", recipe.aligner1_speed);
            param.Add("@RobotSpeed", recipe.robot1_speed);
            RouteControl.Instance.TaskJob.Excute("FormManual", out Message, out Task, TaskName, param);
            if (Task == null)
            {
                MessageBox.Show("上一個動作執行中!");
            }
            else
            {
                DIOUpdate.UpdateControlButton("ALL_INIT_btn", false);
                DIOUpdate.UpdateControlButton("Mode_btn", false);
                ALL_INIT_btn.BackColor = Color.Yellow;
            }
        }

        private void Start_btn_Click(object sender, EventArgs e)
        {
            DIOUpdate.UpdateControlButton("Start_btn", false);
            DIOUpdate.UpdateControlButton("Stop_btn", true);
            DIOUpdate.UpdateControlButton("ALL_INIT_btn", false);
            DIOUpdate.UpdateControlButton("Mode_btn", false);
            Start = true;
            if (tbcMain.SelectedTab.Text.ToUpper().Equals("MONITORING"))
            {

                RunMode = "FULLAUTO";
            }
            else
            {
                RunMode = "SEMIAUTO";
                foreach (Job j in JobManagement.GetJobList())
                {
                    j.AbortProcess = false;

                }
            }
            Node ld = SearchLoadport();
            if (ld != null)
            {
                AssignWafer(ld);
            }
        }

        private void Stop_btn_Click(object sender, EventArgs e)
        {
            DIOUpdate.UpdateControlButton("Start_btn", true);
            DIOUpdate.UpdateControlButton("Stop_btn", false);
            DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
            DIOUpdate.UpdateControlButton("Mode_btn", true);
            Start = false;

            foreach (Job j in JobManagement.GetJobList())
            {
                j.AbortProcess = true;

            }

            foreach (Node port in NodeManagement.GetLoadPortList())
            {
                if (port.IsMapping)
                {
                    foreach (Job j in port.JobList.Values)
                    {
                        if (!j.MapFlag && !j.ErrPosition)
                        {
                            j.IsAssigned = false;
                        }
                    }
                }
            }
        }

        private void btnChgPWD_Click(object sender, EventArgs e)
        {
            using (var form = new GUI.FormChgPwd(lbl_login_id.Text))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {

                }
            }
        }

        private void btnChangeRecipe_Click(object sender, EventArgs e)
        {
            using (var form = new FormSelectRecipe(CurrentRecipe_lb.Text))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {

                }
            }
        }
    }
}
