using Adam.UI_Update.Running;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Engine;
using TransferControl.Management;

namespace Adam.Menu.RunningScreen
{
    public partial class FormRunningScreen : Adam.Menu.FormFrame
    {

        bool[] ProcessSlotList = new bool[25];
        bool Run = false;
        bool CycleStop = false;
        bool LotEnd = false;
        bool ThreadEnd = false;
        string LL = "";
        int TransCount = 0;
        string SpeedSet = "";

        List<Node> SelectLoadports = new List<Node>();

        public FormRunningScreen()
        {
            InitializeComponent();

        }
        private void FormRunningScreen_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < ProcessSlotList.Length; i++)
            {
                ProcessSlotList[i] = true;
            }
            SelectLoadports.Clear();
        }
        private void Start_btn_Click(object sender, EventArgs e)
        {
            if (Start_btn.Text.Equals("Start Running"))
            {
                if (SelectLoadports.Count != 2)
                {
                    MessageBox.Show("請選擇兩個Loadport!");
                    return;
                }
                if(LL_cb.Text.ToUpper().Equals("ALIGNER01"))
                {
                    LL = LL_cb.Text;
                }
                else
                {
                    LL = "";
                }
                TransCount = Convert.ToInt32(TransCount_tb.Text);
                SpeedSet = RunningSpeed_cb.Text.Replace("%","");
                SpeedSet = SpeedSet.Equals("100") ? "0" : SpeedSet;

                Form form = Application.OpenForms["FormMain"];
                
                Button btn = form.Controls.Find("Mode_btn", true).FirstOrDefault() as Button;
                btn.Enabled = false;
                Button btn2 = form.Controls.Find("btnManual", true).FirstOrDefault() as Button;
                btn2.Enabled = false;

                Start_btn.Text = "End Running";
                Run = true;
                CycleStop = false;
                LotEnd = false;
                ThreadEnd = false;
                ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateLapsedTime));
                ThreadPool.QueueUserWorkItem(new WaitCallback(Transfer));
            }
            else
            {
                using (var form = new FormEndOption())
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        switch (form.Option)
                        {
                            case FormEndOption.EndOption.CycleStop:
                                CycleStop = true;
                                break;
                            case FormEndOption.EndOption.LotEnd:
                                LotEnd = true;
                                break;
                        }
                        SpinWait.SpinUntil(() => ThreadEnd, 99999999);
                        Start_btn.Text = "Start Running";
                        Form formA = Application.OpenForms["FormMain"];
                        
                        Button btn = formA.Controls.Find("Mode_btn", true).FirstOrDefault() as Button;
                        btn.Enabled = true;
                    }

                }

            }

        }
        private void UpdateLapsedTime(object obj)
        {
            DateTime StartTime = DateTime.Now;
            while (!ThreadEnd)
            {
                SpinWait.SpinUntil(() => false, 1000);
                TimeSpan timeDiff = DateTime.Now - StartTime;
                RunningUpdate.UpdateRunningInfo("LapsedTime", timeDiff.ToString(@"hh\:mm\:ss"));
            }
        }
        private void Transfer(object obj)
        {
            int LapsedWfCount = 0;
            int LapsedLotCount = 0;
            
            
            
            Node LD = SelectLoadports[1];
            Node ULD = SelectLoadports[0];
           
            //org
            
            if (!NextAction("ROBOT_ORGSH", "ROBOT01", "", "", "", ""))
            {
                return;
            }
            if (!NextAction("ALIGNER_ORGSH", "ALIGNER01", "", "", "", ""))
            {
                return;
            }
            if (!NextAction("LOADPORT_ORGSH", LD.Name, "", "", "", ""))
            {
                return;
            }
            if (!NextAction("LOADPORT_ORGSH", ULD.Name, "", "", "", ""))
            {
                return;
            }
            //set speed

            if (!NextAction("SPEED", "ROBOT01", "", "", "", SpeedSet))
            {
                return;
            }

            if (!NextAction("SPEED", "ALIGNER01", "", "", "", SpeedSet))
            {
                return;
            }

            while (!LotEnd)
            {
                Node swap = LD;
                LD = ULD;
                ULD = swap;
                //loadport open
                
                if (!NextAction("LOADPORT_OPEN", LD.Name, "", "", "", ""))
                {
                    return;
                }
                if (!NextAction("LOADPORT_OPEN", ULD.Name, "", "", "", ""))
                {
                    return;
                }
                string useArm = "2";
                for (int i = 0; i < ProcessSlotList.Length; i++)
                {

                    if (useArm.Equals("2"))
                    {
                        useArm = "1";
                    }
                    else
                    {
                        useArm = "2";
                    }
                    if (TransCount == 0)
                    {
                        ThreadEnd = true;
                        RunningUpdate.UpdateModeStatus("Start Running");
                        return;
                    }
                    int slotNo = i + 1;
                    bool needProcess = ProcessSlotList[i];
                    if (needProcess && ULD.JobList.ContainsKey(slotNo.ToString()) && LD.JobList.ContainsKey(slotNo.ToString()))
                    {
                        Job FromSlot = LD.JobList[slotNo.ToString()];
                        Job ToSlot = ULD.JobList[slotNo.ToString()];
                        if (FromSlot.MapFlag && !FromSlot.ErrPosition && !ToSlot.MapFlag && !ToSlot.ErrPosition)//check slot status by from and to 
                        {
                            if (!LL.Equals(""))
                            {//ALIGNER use
                                
                                if (!NextAction("LOAD", "ROBOT01", LD.Name, slotNo.ToString(), useArm, ""))
                                {
                                    return;
                                }
                                if (!NextAction("UNLOAD", "ROBOT01", LL, "1", useArm, ""))
                                {
                                    return;
                                }

                                if (!NextAction("ALIGNER_WAFER_HOLD", LL, "", "", "", ""))
                                {
                                    return;
                                }
                                if (!NextAction("ALIGNER_ALIGN", LL, "", "", "", "336"))
                                {
                                    return;
                                }
                                if (!NextAction("ALIGNER_WAFER_RELEASE", LL, "", "", "", ""))
                                {
                                    return;
                                }

                                if (!NextAction("LOAD", "ROBOT01", LL, "1", useArm, ""))
                                {
                                    return;
                                }
                                if (!NextAction("ALIGNER_HOME", LL, "", "", "", ""))
                                {
                                    return;
                                }
                                if (!NextAction("UNLOAD", "ROBOT01", ULD.Name, slotNo.ToString(), useArm, ""))
                                {
                                    return;
                                }
                                TransCount--;
                                RunningUpdate.UpdateRunningInfo("TransCount",TransCount.ToString());
                                LapsedWfCount++;
                                RunningUpdate.UpdateRunningInfo("LapsedWfCount", LapsedWfCount.ToString());
                                if (CycleStop)
                                {
                                    break;
                                }
                            }
                            else
                            {//ALIGNER not use
                                

                                if (!NextAction("LOAD", "ROBOT01", LD.Name, slotNo.ToString(), useArm, ""))
                                {
                                    return;
                                }
                                if (!NextAction("UNLOAD", "ROBOT01", ULD.Name, slotNo.ToString(), useArm, ""))
                                {
                                    return;
                                }
                                
                                TransCount--;
                                RunningUpdate.UpdateRunningInfo("TransCount", TransCount.ToString());
                                LapsedWfCount++;
                                RunningUpdate.UpdateRunningInfo("LapsedWfCount", LapsedWfCount.ToString());
                                if (CycleStop)
                                {
                                    break;
                                }
                            }
                            

                        }
                    }
                }
                if (CycleStop)
                {
                    break;
                }
                LapsedLotCount++;
                RunningUpdate.UpdateRunningInfo("LapsedLotCount", LapsedLotCount.ToString());

                if (!NextAction("LOADPORT_CLOSE_NOMAP", LD.Name, "", "", "", ""))
                {
                    return;
                }
               
               if(!NextAction("LOADPORT_CLOSE_NOMAP", ULD.Name, "", "", "", ""))
                {
                    return;
                }
                

                if (LotEnd)
                {
                    break;
                }
            }
            ThreadEnd = true;
        }

        private bool NextAction(string TaskName,string Target,string Position,string Slot, string Arm,string Value)
        {
            bool result = true;
            string Message = "";

            TaskJobManagment.CurrentProceedTask CurrTask;
            Dictionary<string, string> param = new Dictionary<string, string>();
           
           
            if (!Target.Trim().Equals(""))
            {
                param.Add("@Target", Target);
            }
            if (!Position.Trim().Equals(""))
            {
                param.Add("@Position", Position);
            }
            if (!Slot.Trim().Equals(""))
            {
                param.Add("@Slot", Slot);
            }
            if (!Arm.Trim().Equals(""))
            {
                param.Add("@Arm", Arm);
            }
            if (!Value.Trim().Equals(""))
            {
                param.Add("@Value", Value);
            }
            RouteControl.Instance.TaskJob.Excute("RunningScreen", out Message, out CurrTask, TaskName, param);
            SpinWait.SpinUntil(() => CurrTask.Finished, 99999999);
            if (CurrTask.HasError)
            {
                ThreadEnd = true;
                RunningUpdate.UpdateModeStatus("Start Running");
                result = false;
            }
            return result;
        }

        private void RunningSpeed_cb_TextChanged(object sender, EventArgs e)
        {
            //string strMsg = "確定要修改整機速度?";
            //if (MessageBox.Show(strMsg, "ChangeSpeed", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            //{
            //    ChangeSpeed();
            //}
        }

        //private void ChangeSpeed()
        //{
        //    string sp = RunningSpeed_cb.Text.Replace("%", "");
        //    if (sp.Equals("100"))
        //    {
        //        sp = "0";
        //    }
        //    foreach (Node node in NodeManagement.GetList())
        //    {
        //        string Message = "";
        //        if (node.Type.Equals("ROBOT"))
        //        {
        //            Transaction txn = new Transaction();
        //            txn.Method = Transaction.Command.RobotType.Speed;
        //            txn.Value = sp;
        //            txn.FormName = "Running";
        //            node.SendCommand(txn, out Message);
        //        }
        //        else
        //        if (node.Type.Equals("ALIGNER"))
        //        {
        //            Transaction txn = new Transaction();
        //            txn.Method = Transaction.Command.AlignerType.Speed;
        //            txn.Value = sp;
        //            txn.FormName = "Running";
        //            node.SendCommand(txn, out Message);
        //        }
        //    }
        //}



        private void option_btn_Click(object sender, EventArgs e)
        {
            using (var form = new FormOption(ProcessSlotList))
            {
                var result = form.ShowDialog();
                //if (result == DialogResult.OK)
                //{

                //}
            }
        }


        private void use_loadport_ck_CheckedChanged(object sender, EventArgs e)
        {
            string LoadportName = ((CheckBox)sender).Name.Replace("use_", "").Replace("_ck", "").ToUpper();
            Node port = NodeManagement.Get(LoadportName);
            if (((CheckBox)sender).Checked)
            {
                SelectLoadports.Add(port);
                if (SelectLoadports.Count >= 2)
                {
                    if (!use_loadport01_ck.Checked) { use_loadport01_ck.Enabled = false; }
                    if (!use_loadport02_ck.Checked) { use_loadport02_ck.Enabled = false; }
                    if (!use_loadport03_ck.Checked) { use_loadport03_ck.Enabled = false; }
                    if (!use_loadport04_ck.Checked) { use_loadport04_ck.Enabled = false; }
                }
            }
            else
            {
                SelectLoadports.Remove(port);
                if (SelectLoadports.Count < 2)
                {
                    if (!use_loadport01_ck.Checked) { use_loadport01_ck.Enabled = true; }
                    if (!use_loadport02_ck.Checked) { use_loadport02_ck.Enabled = true; }
                    if (!use_loadport03_ck.Checked) { use_loadport03_ck.Enabled = true; }
                    if (!use_loadport04_ck.Checked) { use_loadport04_ck.Enabled = true; }
                }
            }

        }

        private void FormRunningScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            Run = false;
        }
    }
}
