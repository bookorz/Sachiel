using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TransferControl.Engine;
using TransferControl.Management;
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
using Adam.Util;
using TransferControl.Operation;
using Adam.UI_Update.DifferentialMonitor;
using Adam.UI_Update.Barcode;
using Adam.UI_Update.IO;
using Adam.Menu.WaferMapping;
using System.Diagnostics;
using TransferControl.CommandConvert;
using TransferControl.Comm;
using TransferControl.Config;
using System.IO;

namespace Adam
{
    public partial class FormMain : Form, IUserInterfaceReport, IXfeStateReport
    {
        public static RouteControl RouteCtrl;
        // public static SECSGEM HostControl;
        public static XfeCrossZone xfe;
        public static AlarmMapping AlmMapping;
        private static readonly ILog logger = LogManager.GetLogger(typeof(FormMain));

        FormSystemLog sysLog = new FormSystemLog();
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
        bool Initializing = false;
        public static string RunMode = "";
        bool DemoMode = false;
        string DemoLD = "";


        public FormMain()
        {


            InitializeComponent();
            XmlConfigurator.Configure();
            Initialize();

            //HostControl = new SECSGEM(this);
            //RouteCtrl = new RouteControl(this, HostControl);


            RouteCtrl = new RouteControl(this);
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

            var hstOcr = from node in NodeManagement.GetList()
                         where node.Type.ToUpper().Equals("OCR") && node.Brand.ToUpper().Equals("HST")
                         select node;
            if (hstOcr.Count() != 0)
            {
                var HST = (from prs in Process.GetProcessesByName("VB9BReaderForm").OfType<Process>().ToList()
                           where prs.MainWindowTitle.Contains("Wafer Reader Version")
                           select prs);
                if (HST.Count() == 0)
                {
                    MessageBox.Show("請先開啟HST OCR程式!");
                    this.Close();
                    return;
                }
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
                sysLog.Show();
                sysLog.Hide();
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
                    Mode_btn.Text = "Auto-Mode";
                    Mode_btn.BackColor = Color.Green;
                    Mode_btn.Enabled = false;
                    btnManual.Enabled = false;
                    btnManual.BackColor = Color.Gray;


                    DIOUpdate.UpdateControlButton("Start_btn", Initial);
                    Global.currentUser = "";
                    DIOUpdate.UpdateControlButton("Stop_btn", false);
                    DIOUpdate.UpdateControlButton("ALL_INIT_btn", false);
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



        public void On_Command_Excuted(Node Node, Transaction Txn, CommandReturnMessage Msg)
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
                case "FFU":
                    switch (Txn.Method)
                    {
                        case Transaction.Command.FFUType.GetStatus:
                            DifferentialMonitorUpdate.UpdateFFU(Msg.Value);
                            break;
                    }
                    break;
            }

            switch (Txn.TaskId)
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
                default:
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

               
            }
        }

        public void On_Command_Error(Node Node, Transaction Txn, CommandReturnMessage Msg)
        {

            logger.Debug("On_Command_Error");
            ShowAlarm(Node.Name, Msg.Value);
        }

        public void On_Command_Finished(Node Node, Transaction Txn, CommandReturnMessage Msg)
        {
            logger.Debug("On_Command_Finished");
            //Transaction txn = new Transaction();
            switch (Txn.TaskId)
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
                default:
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
         
               
                    switch (Node.Type)
                    {
                        case "ALIGNER":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.AlignerType.AlignOffset:
                                    Job wafer = Node.JobList["1"];
                                    if ((!wafer.OCR_M12_Pass && !wafer.OCR_T7_Pass) && (Recipe.Get(SystemConfig.Get().CurrentRecipe).ocr_check_Rule.Equals("EITHER")))
                                    {
                                        RouteControl.Instance.DIO.SetIO("BUZZER1", "True");
                                        using (var form = new FormOCRKeyIn("ALL", NodeManagement.Get(Node.Associated_Node).JobList["1"]))
                                        {
                                            var result = form.ShowDialog();
                                            if (result == DialogResult.OK)
                                            {

                                            }
                                        }
                                        RouteControl.Instance.DIO.SetIO("BUZZER1", "False");
                                    }
                                    break;
                                case Transaction.Command.AlignerType.WaferHold:
                                    OCRUpdate.ClearOCRInfo(Node.Associated_Node,"M12", "FormMonitoring");
                                    OCRUpdate.ClearOCRInfo(Node.Associated_Node, "T7", "FormMonitoring");
                                    OCRUpdate.ClearOCRInfo(Node.Associated_Node, "M12", "FormWaferMapping");
                                    OCRUpdate.ClearOCRInfo(Node.Associated_Node, "T7", "FormWaferMapping");
                                    break;
                            }
                            break;
                        case "LOADPORT":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.LoadPortType.Unload:
                                case Transaction.Command.LoadPortType.MappingUnload:
                                case Transaction.Command.LoadPortType.DoorUp:
                                case Transaction.Command.LoadPortType.InitialPos:
                                case Transaction.Command.LoadPortType.ForceInitialPos:
                                    MonitoringUpdate.UpdateFoupID(Node.Name, "");
                                    WaferAssignUpdate.UpdateFoupID(Node.Name, "");
                                    break;
                            }
                            break;
                        case "OCR":
                            switch (Txn.Method)
                            {
                                case Transaction.Command.OCRType.Read:
                                    OCR_ImageHandle(Node, NodeManagement.Get(Node.Associated_Node).JobList["1"], "ALL", Msg.Value);
                                    OCRUpdate.UpdateOCRRead(Node.Name, Msg.Value, NodeManagement.Get(Node.Associated_Node).JobList["1"], "ALL", "FormMonitoring");
                                    OCRUpdate.UpdateOCRRead(Node.Name, Msg.Value, NodeManagement.Get(Node.Associated_Node).JobList["1"], "ALL", "FormWaferMapping");
                                    //OCRStatusUpdate.UpdateOCRRead(Node.Name, Msg.Value);
                                    break;
                                case Transaction.Command.OCRType.ReadM12:
                                    OCR_ImageHandle(Node, NodeManagement.Get(Node.Associated_Node).JobList["1"], "M12", Msg.Value);
                                    OCRUpdate.UpdateOCRRead(Node.Name, Msg.Value, NodeManagement.Get(Node.Associated_Node).JobList["1"], "M12", "FormMonitoring");
                                    OCRUpdate.UpdateOCRRead(Node.Name, Msg.Value, NodeManagement.Get(Node.Associated_Node).JobList["1"], "M12", "FormWaferMapping");
                                    if (!NodeManagement.Get(Node.Associated_Node).JobList["1"].OCR_M12_Pass && (Recipe.Get(SystemConfig.Get().CurrentRecipe).ocr_check_Rule.Equals("BOTH") || Recipe.Get(SystemConfig.Get().CurrentRecipe).ocr_check_Rule.Equals("M12")))
                                    {
                                        RouteControl.Instance.DIO.SetIO("BUZZER1", "True");
                                        using (var form = new FormOCRKeyIn("M12", NodeManagement.Get(Node.Associated_Node).JobList["1"]))
                                        {
                                            var result = form.ShowDialog();
                                            if (result == DialogResult.OK)
                                            {

                                            }
                                        }
                                        RouteControl.Instance.DIO.SetIO("BUZZER1", "False");
                                    }
                                    break;
                                case Transaction.Command.OCRType.ReadT7:
                                    OCR_ImageHandle(Node, NodeManagement.Get(Node.Associated_Node).JobList["1"], "T7", Msg.Value);
                                    OCRUpdate.UpdateOCRRead(Node.Name, Msg.Value, NodeManagement.Get(Node.Associated_Node).JobList["1"], "T7", "FormMonitoring");
                                    OCRUpdate.UpdateOCRRead(Node.Name, Msg.Value, NodeManagement.Get(Node.Associated_Node).JobList["1"], "T7", "FormWaferMapping");
                                    if (!NodeManagement.Get(Node.Associated_Node).JobList["1"].OCR_T7_Pass && (Recipe.Get(SystemConfig.Get().CurrentRecipe).ocr_check_Rule.Equals("BOTH") || Recipe.Get(SystemConfig.Get().CurrentRecipe).ocr_check_Rule.Equals("T7")))
                                    {
                                        RouteControl.Instance.DIO.SetIO("BUZZER1", "True");
                                        using (var form = new FormOCRKeyIn("T7", NodeManagement.Get(Node.Associated_Node).JobList["1"]))
                                        {
                                            var result = form.ShowDialog();
                                            if (result == DialogResult.OK)
                                            {

                                            }
                                        }
                                        RouteControl.Instance.DIO.SetIO("BUZZER1", "False");
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
            }

        }
        private void OCR_ImageHandle(Node OCR, Job Job, string OCRType, string WaferID)
        {
            string save = "";
            string src = "";


            switch (OCR.Name)
            {
                case "OCR01":
                    save = SystemConfig.Get().OCR1ImgToJpgPath;
                    src = SystemConfig.Get().OCR1ImgSourcePath;
                    break;
                case "OCR02":
                    save = SystemConfig.Get().OCR2ImgToJpgPath;
                    src = SystemConfig.Get().OCR2ImgSourcePath;
                    break;
            }
            save += "/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Job.FromFoupID;


            if (OCR != null)
            {

                if (!Directory.Exists(save))
                {
                    Directory.CreateDirectory(save);
                }


                string FileName = "";

                switch (OCRType)
                {
                    case "M12":

                        if (!Job.OCR_M12_Pass)
                        {
                            WaferID = "Failed";
                        }
                        FileName = WaferID + "_" + DateTime.Now.ToString("yyyyMMdd_HHMMss");
                        FileName += "_M12.jpg";
                        break;
                    case "T7":

                        if (!Job.OCR_T7_Pass)
                        {
                            WaferID = "Failed";
                        }
                        FileName = WaferID + "_" + DateTime.Now.ToString("yyyyMMdd_HHMMss");
                        FileName += "_T7.jpg";
                        break;
                    default:

                        if (!Job.OCRPass)
                        {
                            WaferID = "Failed";
                        }
                        FileName = WaferID + "_" + DateTime.Now.ToString("yyyyMMdd_HHMMss");
                        FileName += ".jpg";
                        break;
                }

                string savePath = save + "/" + FileName;
                string saveTmpPath = save + "/" + WaferID + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bmp";
                if (savePath != "")
                {

                    switch (OCR.Brand)
                    {
                        case "COGNEX":

                            FTP ftp = new FTP(OCR.GetController().GetIPAdress(), "21", "", "admin", "");
                            string imgPath = ftp.Get("image.jpg", FileName, save);

                            switch (OCRType)
                            {
                                case "M12":
                                    Job.OCR_M12_ImgPath = savePath;
                                    break;
                                case "T7":
                                    Job.OCR_T7_ImgPath = savePath;
                                    break;
                                default:
                                    Job.OCRImgPath = savePath;
                                    break;
                            }
                            //Job.OCRScore = ocrResult[1];
                            //ProcessRecord.updateSubstrateOCR(NodeManagement.Get(Job.FromPort).PrID, Job);
                            break;
                        case "HST":

                            int retryCnt = 20;
                            while (retryCnt > 0)
                            {
                                string[] files = Directory.GetFiles(src);
                                List<string> fileList = files.ToList();
                                if (fileList.Count != 0)
                                {
                                    fileList.Sort((x, y) => { return -File.GetLastWriteTime(x).CompareTo(File.GetLastWriteTime(y)); });

                                    try
                                    {
                                        File.Copy(fileList[0], saveTmpPath);

                                    }
                                    catch (Exception ee)
                                    {

                                        retryCnt--;
                                        if (retryCnt == 0)
                                        {
                                            logger.Error("OCR Image copy fail!");
                                            ShowAlarm("SYSTEM", "S0300181");
                                            return;
                                        }
                                        logger.Error("OCR Image copy retry " + retryCnt.ToString());
                                        logger.Error(ee.Message);
                                        Thread.Sleep(100);
                                        continue;
                                    }


                                    Image bmp = Image.FromFile(saveTmpPath);

                                    bmp.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    bmp.Dispose();
                                    File.Delete(saveTmpPath);

                                    switch (OCRType)
                                    {
                                        case "M12":
                                            Job.OCR_M12_ImgPath = savePath;
                                            break;
                                        case "T7":
                                            Job.OCR_T7_ImgPath = savePath;
                                            break;
                                        default:
                                            Job.OCRImgPath = savePath;
                                            break;
                                    }

                                    //ProcessRecord.updateSubstrateOCR(NodeManagement.Get(Job.FromPort).PrID, Job);
                                    break;
                                }
                                else
                                {
                                    retryCnt--;
                                    Thread.Sleep(100);
                                }
                            }
                            break;
                    }

                }
            }
        }
        public void On_Command_TimeOut(Node Node, Transaction Txn)
        {
            logger.Debug("On_Command_TimeOut");
            ShowAlarm("SYSTEM", "00200002", Node.Name);
           
        }

        public void On_Event_Trigger(Node Node, CommandReturnMessage Msg)
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
                                if (Node.OrgSearchComplete && Node.ManaulControl && !Node.CurrentStatus.Equals("UnloadComplete") && !Node.IsLoad)
                                {
                                    Node.CurrentStatus = "UnloadComplete";
                                    TaskName = "LOADPORT_UNLOADCOMPLETE";
                                    Message = "";
                                    Dictionary<string, string> param1 = new Dictionary<string, string>();
                                    param1.Add("@Target", Node.Name);

                                    RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out Task, TaskName, param1);
                                }
                                if (Node.IsLoad)
                                {
                                    ShowAlarm("SYSTEM", "S0300182", Node.Name);
                                }
                                break;
                            case "PODON":
                                //Foup Arrived
                                if (Node.OrgSearchComplete && Node.ManaulControl && !Node.CurrentStatus.Equals("ReadyToLoad") && !Node.IsLoad)
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
            switch (Parameter.ToUpper())
            {
                //case "SAFETYRELAY":
                //    if (Value.ToUpper().Equals("FALSE"))
                //    {
                //        FormReconnect.Show(true);
                //    }
                //    else
                //    {
                //        FormReconnect.Show(false);
                //    }
                //    break;

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
                    //if (Parameter.ToUpper().Equals("SAFETYRELAY") && Value.ToUpper().Equals("FALSE"))
                    //{
                    //    FormReconnect.Show(true);
                    //}
                    //else if (Parameter.ToUpper().Equals("SAFETYRELAY") && Value.ToUpper().Equals("TRUE"))
                    //{
                    //    FormReconnect.Show(false);
                    //}
                    switch (Parameter.ToUpper())
                    {
                        case "SAFETYRELAY":
                            if (Value.ToUpper().Equals("TRUE"))
                            {
                                FormReconnect.Show(false);
                            }
                            else if (Value.ToUpper().Equals("FALSE"))
                            {
                                FormReconnect.Show(true);
                            }
                            break;
                        case "DOORSWITCH":
                            string TaskName = "FFU_SET_SPEED";
                            //RouteControl.Instance.TaskJob.ForceFinishTask(TaskName);

                            Node ffu = NodeManagement.Get("FFU01");
                            if (ffu != null)
                            {
                                string Message = "";
                                Dictionary<string, string> param1 = new Dictionary<string, string>();
                                param1.Add("@Target", ffu.Name);
                                if (Value.ToUpper().Equals("TRUE"))
                                {
                                    param1.Add("@Value", Recipe.Get(SystemConfig.Get().CurrentRecipe).ffu_rpm_close);
                                    DifferentialMonitorUpdate.UpdateFFU(Recipe.Get(SystemConfig.Get().CurrentRecipe).ffu_rpm_close);
                                }
                                else
                                {
                                    param1.Add("@Value", Recipe.Get(SystemConfig.Get().CurrentRecipe).ffu_rpm_open);
                                    DifferentialMonitorUpdate.UpdateFFU(Recipe.Get(SystemConfig.Get().CurrentRecipe).ffu_rpm_open);
                                }
                                TaskJobManagment.CurrentProceedTask tmpTask;
                                RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out tmpTask, TaskName, param1);
                            }
                            break;
                    }
                    break;
            }


        }

        public void On_Alarm_Happen(string DIOName, string ErrorCode)
        {
            ShowAlarm(DIOName, ErrorCode);

        }

        public void On_Connection_Error(string DIOName, string ErrorMsg)
        {
            //斷線 發ALARM
            logger.Debug("On_Error_Occurred");
            ShowAlarm(DIOName, "00200001");

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
                Initial = false;
                DIOUpdate.UpdateControlButton("Start_btn", false);
                DIOUpdate.UpdateControlButton("Stop_btn", false);
                if (tbcMain.SelectedTab.Text.Equals("Monitoring") || tbcMain.SelectedTab.Text.Equals("Wafer Assign"))
                {
                    DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
                }
                else
                {
                    DIOUpdate.UpdateControlButton("ALL_INIT_btn", false);
                }

                Mode_btn.Text = "Auto-Mode";
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


            if (RunMode.ToUpper().Equals("FULLAUTO") && !tbcMain.SelectedTab.Name.Equals("tabMonitor"))
            {


                if (Start)
                {
                    tbcMain.SelectTab("tabMonitor");
                    MessageBox.Show("Please switch to STOP");
                }
                else if (XfeCrossZone.Running)
                {
                    tbcMain.SelectTab("tabMonitor");
                    MessageBox.Show("Please wait for STOP");
                }
                else
                {
                    foreach (Job j in JobManagement.GetJobList())
                    {
                        j.IsAssigned = false;
                        j.UnAssignPort();
                    }
                }

            }
            else if (RunMode.ToUpper().Equals("SEMIAUTO") && !tbcMain.SelectedTab.Name.Equals("tabWaferAssign"))
            {

                if (Start)
                {
                    tbcMain.SelectTab("tabWaferAssign");
                    MessageBox.Show("Please switch to STOP");
                }
                else if (XfeCrossZone.Running)
                {
                    tbcMain.SelectTab("tabWaferAssign");
                    MessageBox.Show("Please wait for STOP");
                }
                else
                {
                    foreach (Job j in JobManagement.GetJobList())
                    {
                        j.IsAssigned = false;
                        j.UnAssignPort();
                    }
                }

            }



            if (tbcMain.SelectedTab.Text.Equals("Monitoring"))
            {
                if (Mode_btn.Text.Equals("Auto-Mode") && !Global.currentUser.Equals(""))
                {
                    DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
                    DIOUpdate.UpdateControlButton("Start_btn", Initial);
                }
                Form form = Application.OpenForms["FormMonitoring"];
                foreach (Node port in NodeManagement.GetLoadPortList())
                {
                    Label present = form.Controls.Find(port.Name + "_Mode", true).FirstOrDefault() as Label;
                    if (present != null)
                    {
                        present.Text = port.Mode;
                    }
                }


                foreach (Node port in NodeManagement.GetLoadPortList())
                {

                    for (int i = 1; i <= 25; i++)
                    {
                        Label present = form.Controls.Find(port.Name + "_Slot_" + i.ToString(), true).FirstOrDefault() as Label;
                        if (present != null)
                        {
                            switch (port.CarrierType.ToUpper())
                            {
                                case "FOSB":
                                case "FOUP":
                                    present.Visible = true;
                                    break;
                                case "OPEN":
                                    if (i > 13)
                                    {
                                        present.Visible = false;
                                    }
                                    else
                                    {
                                        present.Visible = true;
                                    }
                                    break;
                            }

                        }
                    }


                }
            }
            else if (tbcMain.SelectedTab.Text.Equals("Wafer Assign"))
            {
                if (Mode_btn.Text.Equals("Auto-Mode") && !Global.currentUser.Equals(""))
                {
                    DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
                    DIOUpdate.UpdateControlButton("Start_btn", Initial);
                }
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
                                    if (!eachSlot.Destination.Equals("") && !eachSlot.DestinationSlot.Equals("") /*&& (!eachSlot.Destination.Equals(eachSlot.Position) && !eachSlot.DestinationSlot.Equals(eachSlot.Slot))*/)
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
                foreach (Node port in NodeManagement.GetLoadPortList())
                {

                    for (int i = 1; i <= 25; i++)
                    {
                        Label present = form.Controls.Find(port.Name + "_Slot_" + i.ToString(), true).FirstOrDefault() as Label;
                        if (present != null)
                        {
                            switch (port.CarrierType.ToUpper())
                            {
                                case "FOSB":
                                case "FOUP":
                                    present.Visible = true;
                                    break;
                                case "OPEN":
                                    if (i > 13)
                                    {
                                        present.Visible = false;
                                    }
                                    else
                                    {
                                        present.Visible = true;
                                    }
                                    break;
                            }

                        }
                    }


                }
            }
            else
            {
                
                DIOUpdate.UpdateControlButton("ALL_INIT_btn", false);
                DIOUpdate.UpdateControlButton("Start_btn", false);
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

        private void ShowAlarm(string NodeName, string AlarmCode,string DisplayName="")
        {
            AlarmInfo CurrentAlarm = new AlarmInfo();
            if (DisplayName == null)
            {
                DisplayName = "";
            }
            CurrentAlarm.NodeName = DisplayName.Equals("")? NodeName: DisplayName;
            CurrentAlarm.AlarmCode = AlarmCode;
            CurrentAlarm.NeedReset = false;
            try
            {

                AlarmMessage Detail = AlmMapping.Get(NodeName, CurrentAlarm.AlarmCode);
                //if (!Detail.Code_Group.Equals("UNDEFINITION"))
                //{
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
                // }
            }
            catch (Exception e)
            {
                CurrentAlarm.Desc = "未定義";
                logger.Error("(GetAlarmMessage)" + e.Message + "\n" + e.StackTrace);
            }
        }

        public void On_TaskJob_Aborted(TaskJobManagment.CurrentProceedTask Task, string NodeName, string ReportType, string Message)
        {
            switch (Task.ProceedTask.TaskName)
            {
                case "SORTER_INIT":

                    Initializing = false;
                    break;
            }
            DIOUpdate.UpdateControlButton("Start_btn", false);
            DIOUpdate.UpdateControlButton("Stop_btn", false);
            if (Mode_btn.Text.Equals("Auto-Mode"))
            {
                DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
            }
            else
            {
                DIOUpdate.UpdateControlButton("ALL_INIT_btn", false);
            }
            DIOUpdate.UpdateControlButton("Mode_btn", true);
            Start = false;
            RunMode = "";
            WaferAssignUpdate.UpdateEnabled("FORM", true);
            //XfeCrossZone.Stop();
            //if (Task.Id.IndexOf("FormManual") != -1)
            //{
                ManualPortStatusUpdate.LockUI(false);
            //}
            if (!ReportType.Equals("On_Command_Error"))
            {
                ShowAlarm("SYSTEM", Message, NodeName);
            }
        }

        public void On_TaskJob_Finished(TaskJobManagment.CurrentProceedTask Task)
        {
            string TaskName = "";
            string Message = "";

            TaskJobManagment.CurrentProceedTask tmpTask;
            //if (Task.Id.IndexOf("FormManual") != -1)
            //{
                ManualPortStatusUpdate.LockUI(false);
            //}
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

                    //foreach (Node port in NodeManagement.GetLoadPortList())
                    //{
                    //    if (port.Enable && port.Foup_Placement)
                    //    {
                    //        MonitoringUpdate.UpdateFoupID(port.Name, "");
                    //        WaferAssignUpdate.UpdateFoupID(port.Name, "");
                    //        TaskName = "LOADPORT_READYTOLOAD";
                    //        Message = "";
                    //        Dictionary<string, string> param = new Dictionary<string, string>();
                    //        param.Add("@Target", port.Name);

                    //        RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out tmpTask, TaskName, param);
                    //        CarrierManagement.Add().SetLocation(port.Name);
                    //    }
                    //}
                    foreach (Job eachJob in JobManagement.GetJobList())
                    {
                        eachJob.InProcess = false;
                        if (eachJob.Position.ToUpper().Equals("ROBOT01") || eachJob.Position.ToUpper().Equals("ALIGNER01"))
                        {
                            JobManagement.Remove(eachJob.Job_Id);
                            Node pos = NodeManagement.Get(eachJob.Position);
                            if (pos != null)
                            {
                                MonitoringUpdate.UpdateNodesJob(pos.Name);
                                WaferAssignUpdate.UpdateNodesJob(pos.Name);
                                RunningUpdate.UpdateNodesJob(pos.Name);
                            }
                        }
                    }


                    Initial = true;
                    Initializing = false;
                    RouteControl.Instance.DIO.SetIO("BUZZER1", "True");
                    MessageBox.Show("Initial finished!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    RouteControl.Instance.DIO.SetIO("BUZZER1", "False");
                    break;
                case "LOADPORT_OPEN":
                case "LOADPORT_REOPEN":
                    Node currentPort = NodeManagement.Get(Task.Params["@Target"]);
                    if (Start)
                    {
                        if (Recipe.Get(SystemConfig.Get().CurrentRecipe).is_use_burnin)
                        {
                            var Slots = from slot in currentPort.JobList.Values
                                                 where slot.MapFlag && !slot.ErrPosition
                                                 select slot;
                            if (Slots.Count() != 0)
                            {
                                Node ld = SearchLoadport();
                                if (ld != null)
                                {
                                    AssignWafer(ld);
                                }
                            }
                        }
                        else
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
                    }
                    //if (Task.Id.Contains("Unload_btn"))
                    //{
                    MonitoringUpdate.ButtonEnabled(Task.Params["@Target"].ToUpper() + "_Unload_btn", true);
                    WaferAssignUpdate.ButtonEnabled(Task.Params["@Target"].ToUpper() + "_Unload_btn", true);
                    //}
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
                var AvailableOPENs = (from OPEN in NodeManagement.GetLoadPortList()
                                     where OPEN.Mode.Equals("LD") && OPEN.IsMapping &&                                   
                                     (from wafer in OPEN.JobList.Values
                                     where wafer.MapFlag && !wafer.ErrPosition && !wafer.AbortProcess select wafer).Count()!=0
                                     select OPEN).OrderBy(x => x.LoadTime);
                if (Recipe.Get(SystemConfig.Get().CurrentRecipe).is_use_burnin)
                {
                    AvailableOPENs = (from OPEN in NodeManagement.GetLoadPortList()
                                     where OPEN.IsMapping &&
                                     (from wafer in OPEN.JobList.Values
                                     where wafer.MapFlag && !wafer.ErrPosition && !wafer.AbortProcess select wafer).Count()!=0
                                     select OPEN).OrderBy(x => x.LoadTime);
                }
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
                                  where job.NeedProcess && !job.AbortProcess
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
                // MessageBox.Show("Transfer finished");
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

                if (!Start)
                {
                    return;
                }


                if (RunMode.Equals("FULLAUTO"))
                {

                    var LD_Jobs = (from wafer in Loadport.JobList.Values
                                   where wafer.MapFlag && !wafer.ErrPosition && wafer.Destination.Equals("")
                                   select wafer).OrderByDescending(x => Convert.ToInt16(x.Slot));
                    if (Recipe.Get(SystemConfig.Get().CurrentRecipe).get_slot_order.Equals("BOTTOM_UP"))
                    {
                        LD_Jobs = (from wafer in Loadport.JobList.Values
                                   where wafer.MapFlag && !wafer.ErrPosition && wafer.Destination.Equals("")
                                   select wafer).OrderBy(x => Convert.ToInt16(x.Slot));
                    }

                    var AvailableFOSBs = from FOSB in NodeManagement.GetLoadPortList()
                                         where FOSB.Mode.Equals("ULD") && FOSB.IsMapping
                                         orderby FOSB.LoadTime
                                         select FOSB;
                    if (Recipe.Get(SystemConfig.Get().CurrentRecipe).is_use_burnin)
                    {
                        AvailableFOSBs = from FOSB in NodeManagement.GetLoadPortList()
                                         where FOSB.IsMapping
                                         orderby FOSB.LoadTime
                                         select FOSB;
                    }
                    bool isAssign = false;
                    bool isAssign2 = false;
                    foreach (Node fosb in AvailableFOSBs)
                    {//找到能放的FOSB
                        if (Recipe.Get(SystemConfig.Get().CurrentRecipe).is_use_burnin)
                        {
                            var Jobs = from Slot in fosb.JobList.Values
                                       where Slot.MapFlag && !Slot.ErrPosition
                                       select Slot;
                            if (Jobs.Count() != 0)
                            {//有片就不選他當作uld
                                continue;
                            }
                        }
                        var ULD_Jobs = (from Slot in fosb.JobList.Values
                                        where !Slot.MapFlag && !Slot.ErrPosition && !Slot.IsAssigned
                                        select Slot).OrderByDescending(x => Convert.ToInt16(x.Slot));
                        if (Recipe.Get(SystemConfig.Get().CurrentRecipe).put_slot_order.Equals("BOTTOM_UP"))
                        {
                            ULD_Jobs = (from Slot in fosb.JobList.Values
                                        where !Slot.MapFlag && !Slot.ErrPosition && !Slot.IsAssigned
                                        select Slot).OrderBy(x => Convert.ToInt16(x.Slot));
                        }
                        foreach (Job wafer in LD_Jobs)
                        {//檢查LD的所有WAFER

                            isAssign = false;
                            foreach (Job Slot in ULD_Jobs)
                            {//搜尋所有FOSB Slot 找到能放的     
                                if (Recipe.Get(SystemConfig.Get().CurrentRecipe).auto_put_constrict.Equals("1") && fosb.CheckPreviousPresence(Slot.Slot) && Recipe.Get(SystemConfig.Get().CurrentRecipe).put_slot_order.Equals("TOP_DOWN"))
                                {//下一層有片所以不能放
                                    Slot.Locked = true;
                                }
                                else if (Recipe.Get(SystemConfig.Get().CurrentRecipe).auto_put_constrict.Equals("2") && fosb.CheckForwardPresence(Slot.Slot) && Recipe.Get(SystemConfig.Get().CurrentRecipe).put_slot_order.Equals("TOP_DOWN"))
                                {//下方所有都不能有片
                                    Slot.Locked = true;
                                }
                                else
                                {
                                    wafer.NeedProcess = true;
                                    wafer.ProcessFlag = false;
                                    wafer.AlignerFlag = false;
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
                    LD_Jobs = (from wafer in Loadport.JobList.Values
                               where wafer.NeedProcess && !wafer.IsReversed
                               select wafer).OrderByDescending(x => Convert.ToInt16(x.Slot));
                    if (Recipe.Get(SystemConfig.Get().CurrentRecipe).get_slot_order.Equals("BOTTOM_UP"))
                    {
                        LD_Jobs = (from wafer in Loadport.JobList.Values
                                   where wafer.NeedProcess && !wafer.IsReversed
                                   select wafer).OrderBy(x => Convert.ToInt16(x.Slot));
                    }
                    Node Rbt = NodeManagement.Get("ROBOT01");
                    if (!Recipe.Get(SystemConfig.Get().CurrentRecipe).put_slot_order.Equals(Recipe.Get(SystemConfig.Get().CurrentRecipe).get_slot_order))
                    {
                        if (Rbt.RArmActive && Rbt.LArmActive && Rbt.DoubleArmActive)
                        {
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
                            }

                            foreach (Job each in Loadport.JobList.Values)
                            {
                                if (!each.Destination.Equals(""))
                                {
                                    each.IsReversed = true;
                                }
                            }
                        }
                    }
                    var NeedProcessSlot = from wafer in Loadport.JobList.Values
                                          where wafer.NeedProcess
                                          select wafer;

                    if (!XfeCrossZone.Running && NeedProcessSlot.Count() != 0)
                    {
                        //in port log
                        FoupInfo foup = new FoupInfo(SystemConfig.Get().CurrentRecipe, Global.currentUser, Loadport.FoupID);
                        foreach (Job j in Loadport.JobList.Values)
                        {
                            if (j.MapFlag && !j.ErrPosition)
                            {
                                int slot = Convert.ToInt16(j.Slot);
                                Node ULD = NodeManagement.Get(j.Destination);
                                string ULD_Foup = "";
                                if (ULD != null)
                                {
                                    ULD_Foup = ULD.FoupID;
                                }
                                foup.record[slot - 1] = new Adam.waferInfo(Loadport.Name, Loadport.FoupID, j.Slot, j.FromPort, Loadport.FoupID, j.FromPortSlot, j.Destination, ULD_Foup, j.DestinationSlot);
                                //foup.record[slot - 1].SetStartTime(j.StartTime);
                                //foup.record[slot - 1].setM12(j.OCR_M12_Result);
                                //foup.record[slot - 1].setT7(j.OCR_T7_Result);
                                //foup.record[slot - 1].SetEndTime(j.EndTime);
                                foup.record[slot - 1].SetLoadTime(Loadport.LoadTime);
                                //foup.record[slot - 1].SetUnloadTime(DateTime.Now);
                                //foup.record[slot - 1].setM12Score(j.OCR_M12_Score);
                                //foup.record[slot - 1].setT7Score(j.OCR_T7_Score);
                            }
                        }
                        foup.SaveTmp(Loadport.Name);
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
                        //in port log
                        FoupInfo foup = new FoupInfo(SystemConfig.Get().CurrentRecipe, Global.currentUser, Loadport.FoupID);
                        foreach (Job j in Loadport.JobList.Values)
                        {
                            if (j.MapFlag && !j.ErrPosition)
                            {
                                int slot = Convert.ToInt16(j.Slot);
                                Node ULD = NodeManagement.Get(j.Destination);
                                string ULD_Foup = "";
                                if (ULD != null)
                                {
                                    ULD_Foup = ULD.FoupID;
                                }

                                foup.record[slot - 1] = new Adam.waferInfo(Loadport.Name, Loadport.FoupID, j.Slot, j.FromPort, Loadport.FoupID, j.FromPortSlot, j.Destination, ULD_Foup, j.DestinationSlot);
                                //foup.record[slot - 1].SetStartTime(j.StartTime);
                                //foup.record[slot - 1].setM12(j.OCR_M12_Result);
                                //foup.record[slot - 1].setT7(j.OCR_T7_Result);
                                //foup.record[slot - 1].SetEndTime(j.EndTime);
                                foup.record[slot - 1].SetLoadTime(Loadport.LoadTime);
                                //foup.record[slot - 1].SetUnloadTime(DateTime.Now);
                                //foup.record[slot - 1].setM12Score(j.OCR_M12_Score);
                                //foup.record[slot - 1].setT7Score(j.OCR_T7_Score);
                            }
                        }
                        foup.SaveTmp(Loadport.Name);


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


            MonitoringUpdate.UpdateWPH((xfe.ProcessCount / (xfe.ProcessTime / 1000.0 / 60.0 / 60.0)).ToString("f1"));
            logger.Debug("ProcessCount:" + xfe.ProcessCount.ToString() + " ProcessTime:" + xfe.ProcessTime.ToString() + " WPH:" + (xfe.ProcessCount / (xfe.ProcessTime / 1000.0 / 60.0 / 60.0)).ToString("f1"));
            if (Recipe.Get(SystemConfig.Get().CurrentRecipe).is_use_burnin)
            {
                return;
            }
            Node ld = SearchLoadport();
            if (ld != null)
            {
                AssignWafer(ld);
            }
            else
            {
                NodeStatusUpdate.UpdateCurrentState("IDLE");
                RouteControl.Instance.DIO.SetIO("BUZZER1", "True");
                MessageBox.Show("All job finished!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                RouteControl.Instance.DIO.SetIO("BUZZER1", "False");
            }

            WaferAssignUpdate.UpdateEnabled("FORM", true);
        }

        public void On_LoadPort_Selected(Node Port)
        {
            NodeStatusUpdate.UpdateCurrentState("RUN");
            MonitoringUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", false);
            WaferAssignUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", false);
        }

        public void On_UnLoadPort_Selected(Node Port)
        {
            MonitoringUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", false);
            WaferAssignUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", false);
        }

        public void On_LoadPort_Complete(Node Port)
        {
            try
            {
                if (Recipe.Get(SystemConfig.Get().CurrentRecipe).is_use_burnin)
                {
                    string TaskName = "LOADPORT_REOPEN";
                    string Message = "";
                    Dictionary<string, string> param1 = new Dictionary<string, string>();
                    param1.Add("@Target", Port.Name);
                    TaskJobManagment.CurrentProceedTask tmpTask;
                    RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out tmpTask, TaskName, param1);
                    return;
                }
                var AvailableSlots = from eachSlot in Port.JobList.Values.ToList()
                                     where eachSlot.MapFlag && !eachSlot.ErrPosition && !eachSlot.Locked
                                     select eachSlot;
                if (AvailableSlots.Count() != 0)
                {
                    MonitoringUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", true);
                    WaferAssignUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", true);                   
                }
                else
                {//滿了才退
                    
                    FoupInfo tmp = FoupInfo.Get(Port.Name);
                    tmp.SetAllUnloadTime(DateTime.Now);
                    tmp.Save();
                    string constrict = Recipe.Get(SystemConfig.Get().CurrentRecipe).input_proc_fin;
                    string Light = "";
                    string Buzzer = "";
                    switch (constrict[1])
                    {
                        case '0':
                            Light = "RED";
                            break;
                        case '1':
                            Light = "ORANGE";
                            break;
                        case '2':
                            Light = "GREEN";
                            break;
                        case '3':
                            Light = "BLUE";
                            break;
                    }
                    switch (constrict[2])
                    {
                        case '4':
                            Buzzer = "BUZZER1";
                            break;
                        case '5':
                            Buzzer = "BUZZER2";
                            break;

                    }
                    if (!constrict[0].Equals('N'))
                    {//N:無動作
                        if (!Light.Equals(""))
                        {
                            RouteControl.Instance.DIO.SetBlink(Light, "True");
                        }
                        if (!Buzzer.Equals(""))
                        {
                            RouteControl.Instance.DIO.SetIO(Buzzer, "True");
                        }


                        using (var form = new FormNotify("Loadport Finished", Port.Name, Port.FoupID))
                        {

                            switch (constrict.Substring(0, 1))
                            {
                                case "M"://跳出不暫停
                                         // new Thread(() =>
                                         // {

                                    try
                                    {

                                        //Thread.CurrentThread.IsBackground = true;

                                        var result = form.ShowDialog();

                                        if (result == DialogResult.OK)
                                        {

                                            if (!Light.Equals(""))
                                            {

                                                RouteControl.Instance.DIO.SetIO(Light, "False");
                                            }
                                            if (!Buzzer.Equals(""))
                                            {

                                                RouteControl.Instance.DIO.SetIO(Buzzer, "False");
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {

                                        logger.Error(e.Message + "\n" + e.StackTrace);
                                    }
                                    // }).Start();


                                    break;
                                case "P"://跳出暫停

                                    // new Thread(() =>
                                    //{
                                    //Thread.CurrentThread.IsBackground = true;
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
                                    var result2 = form.ShowDialog();
                                    if (result2 == DialogResult.OK)
                                    {
                                        if (!Light.Equals(""))
                                        {
                                            RouteControl.Instance.DIO.SetIO(Light, "False");
                                        }
                                        if (!Buzzer.Equals(""))
                                        {
                                            RouteControl.Instance.DIO.SetIO(Buzzer, "False");
                                        }
                                        foreach (Job j in JobManagement.GetJobList())
                                        {
                                            j.AbortProcess = false;

                                        }
                                        Node ld = SearchLoadport();
                                        if (ld != null)
                                        {
                                            AssignWafer(ld);
                                        }
                                        else
                                        {
                                            NodeStatusUpdate.UpdateCurrentState("IDLE");
                                        }
                                    }
                                    //}).Start();
                                    break;
                            }




                        }
                    }
                    MonitoringUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", false);
                    WaferAssignUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", false);
                    string TaskName = "LOADPORT_CLOSE_NOMAP";
                    string Message = "";
                    Dictionary<string, string> param1 = new Dictionary<string, string>();
                    param1.Add("@Target", Port.Name);
                    TaskJobManagment.CurrentProceedTask tmpTask;
                    RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out tmpTask, TaskName, param1);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
            //MonitoringUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", true);
        }

        public void On_UnLoadPort_Complete(Node Port)
        {
            try
            {
                if (Recipe.Get(SystemConfig.Get().CurrentRecipe).is_use_burnin)
                {
                    string TaskName = "LOADPORT_REOPEN";
                    string Message = "";
                    Dictionary<string, string> param1 = new Dictionary<string, string>();
                    param1.Add("@Target", Port.Name);
                    TaskJobManagment.CurrentProceedTask tmpTask;
                    RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out tmpTask, TaskName, param1);
                    return;
                }
                var Available = from each in Port.JobList.Values
                                where !each.MapFlag && !each.ErrPosition && !each.Locked
                                select each;
                if (Available.Count() == 0)
                {//Unloadport 滿了 自動退
                    
                    FoupInfo foup = new FoupInfo(SystemConfig.Get().CurrentRecipe, Global.currentUser, Port.FoupID);
                    foreach (Job j in Port.JobList.Values)
                    {
                        if (j.MapFlag && !j.ErrPosition)
                        {
                            int slot = Convert.ToInt16(j.Slot);
                            foup.record[slot - 1] = new Adam.waferInfo(Port.Name, Port.FoupID, j.Slot, j.FromPort, j.FromFoupID, j.FromPortSlot, j.Position, Port.FoupID, j.Slot);
                            foup.record[slot - 1].SetStartTime(j.StartTime);
                            foup.record[slot - 1].setM12(j.OCR_M12_Result);
                            foup.record[slot - 1].setT7(j.OCR_T7_Result);
                            foup.record[slot - 1].SetEndTime(j.EndTime);
                            foup.record[slot - 1].SetLoadTime(Port.LoadTime);
                            foup.record[slot - 1].SetUnloadTime(DateTime.Now);
                            foup.record[slot - 1].setM12Score(j.OCR_M12_Score);
                            foup.record[slot - 1].setT7Score(j.OCR_T7_Score);
                        }
                    }
                    foup.Save();
                    string constrict = Recipe.Get(SystemConfig.Get().CurrentRecipe).output_proc_fin;
                    string Light = "";
                    string Buzzer = "";
                    switch (constrict[1])
                    {
                        case '0':
                            Light = "RED";
                            break;
                        case '1':
                            Light = "ORANGE";
                            break;
                        case '2':
                            Light = "GREEN";
                            break;
                        case '3':
                            Light = "BLUE";
                            break;
                    }
                    switch (constrict[2])
                    {
                        case '4':
                            Buzzer = "BUZZER1";
                            break;
                        case '5':
                            Buzzer = "BUZZER2";
                            break;

                    }
                    if (!constrict[0].Equals('N'))
                    {//N:無動作
                        if (!Light.Equals(""))
                        {
                            RouteControl.Instance.DIO.SetBlink(Light, "True");
                        }
                        if (!Buzzer.Equals(""))
                        {
                            RouteControl.Instance.DIO.SetIO(Buzzer, "True");
                        }
                        using (var form = new FormNotify("UnLoadport Finished", Port.Name, Port.FoupID))
                        {
                            switch (constrict[0])
                            {
                                case 'M'://跳出不暫停
                                         //new Thread(() =>
                                         //{
                                         // Thread.CurrentThread.IsBackground = true;
                                    var result = form.ShowDialog();
                                    if (result == DialogResult.OK)
                                    {
                                        if (!Light.Equals(""))
                                        {
                                            RouteControl.Instance.DIO.SetIO(Light, "False");
                                        }
                                        if (!Buzzer.Equals(""))
                                        {
                                            RouteControl.Instance.DIO.SetIO(Buzzer, "False");
                                        }
                                    }
                                    //}).Start();

                                    break;
                                case 'P'://跳出暫停
                                         //new Thread(() =>
                                         //{
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
                                    //Thread.CurrentThread.IsBackground = true;
                                    var result2 = form.ShowDialog();
                                    if (result2 == DialogResult.OK)
                                    {
                                        if (!Light.Equals(""))
                                        {
                                            RouteControl.Instance.DIO.SetIO(Light, "False");
                                        }
                                        if (!Buzzer.Equals(""))
                                        {
                                            RouteControl.Instance.DIO.SetIO(Buzzer, "False");
                                        }
                                        foreach (Job j in JobManagement.GetJobList())
                                        {
                                            j.AbortProcess = false;

                                        }
                                        Node ld = SearchLoadport();
                                        if (ld != null)
                                        {
                                            AssignWafer(ld);
                                        }
                                        else
                                        {
                                            NodeStatusUpdate.UpdateCurrentState("IDLE");
                                        }
                                    }
                                    // }).Start();
                                    break;
                            }



                        }
                    }
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
                    WaferAssignUpdate.ButtonEnabled(Port.Name.ToUpper() + "_Unload_btn", true);
                    
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fakeData("LOADPORT01", "1111111111111000000000000");
            //fakeData("LOADPORT02");
            //fakeData("LOADPORT03");
            fakeData("LOADPORT04", "0000000000000000000000000");
            WaferAssignUpdate.UpdateNodesJob("LOADPORT01");
            WaferAssignUpdate.UpdateNodesJob("LOADPORT02");
            WaferAssignUpdate.UpdateNodesJob("LOADPORT03");
            WaferAssignUpdate.UpdateNodesJob("LOADPORT04");
        }

        private void fakeData(string name, string Mapping)
        {
            //string Mapping = Msg.Value;

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
            if (XfeCrossZone.Running)
            {
                MessageBox.Show("請先停止搬運");
                return;
            }
            DIOUpdate.UpdateControlButton("Start_btn", false);
            Recipe recipe = Recipe.Get(SystemConfig.Get().CurrentRecipe);
            string TaskName = "SORTER_INIT";
            string Message = "";
            TaskJobManagment.CurrentProceedTask Task;
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("@AlingerSpeed", recipe.aligner1_speed);
            param.Add("@RobotSpeed", recipe.robot1_speed);
            RouteControl.Instance.TaskJob.Excute(Guid.NewGuid().ToString(), out Message, out Task, TaskName, param);
            if (Task == null)
            {
                MessageBox.Show("上一個動作執行中!");
            }
            else
            {
                DIOUpdate.UpdateControlButton("ALL_INIT_btn", false);
                DIOUpdate.UpdateControlButton("Mode_btn", false);
                ALL_INIT_btn.BackColor = Color.Yellow;
                Initializing = true;
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

            }
            foreach (Job j in JobManagement.GetJobList())
            {
                j.AbortProcess = false;

            }
            if (DemoMode)
            {
                var AvailableFoups = from Foup in NodeManagement.GetLoadPortList()
                                     orderby Foup.Name
                                     from wafer in Foup.JobList.Values
                                     where wafer.MapFlag && !wafer.ErrPosition && !wafer.AbortProcess
                                     select Foup;
                if (AvailableFoups.Count() != 0)
                {
                    foreach (Node port in NodeManagement.GetLoadPortList())
                    {
                        if (port.Enable)
                        {
                            if (port.Name.ToUpper().Equals(AvailableFoups.First().Name.ToUpper()))
                            {
                                port.Mode = "LD";
                                DemoLD = port.Name;
                            }
                            else
                            {
                                port.Mode = "ULD";
                            }
                        }
                    }
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
            DIOUpdate.UpdateControlButton("Start_btn", true);
            DIOUpdate.UpdateControlButton("Stop_btn", false);
            DIOUpdate.UpdateControlButton("ALL_INIT_btn", true);
            DIOUpdate.UpdateControlButton("Mode_btn", true);
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

        private void tbcMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (Initializing)
            {
                MessageBox.Show("Please wait for initialize.");
                e.Cancel = true;
            }
            if (!tbcMain.SelectedTab.Text.Equals("Wafer Assign") && !tbcMain.SelectedTab.Text.Equals("Monitoring") && !tbcMain.SelectedTab.Text.Equals("Differential Monitor"))
            {
                bool isFound = false;
                foreach(Node port in NodeManagement.GetLoadPortList())
                {
                    if(port.IsMapping && port.Enable)
                    {
                        MonitoringUpdate.ButtonEnabled(port.Name.ToUpper() + "_Unload_btn", true);
                        WaferAssignUpdate.ButtonEnabled(port.Name.ToUpper() + "_Unload_btn", true);
                        
                        isFound = true;
                        
                    }
                }
                if (isFound)
                {
                    MessageBox.Show("Please unload all loadports!", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.Cancel = true;
                }
            }
        }



        public void On_TaskJob_Ack(TaskJobManagment.CurrentProceedTask Task)
        {

        }

        public void On_Message_Log(string Type, string Message)
        {

        }

        public void On_Status_Changed(string Type, string Message)
        {

        }

        private void SystemLog_btn_Click(object sender, EventArgs e)
        {
            sysLog.Show();
        }
    }
}
