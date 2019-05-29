using Adam.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Management;

namespace Adam.UI_Update.Monitoring
{
    public class MonitoringUpdate
    {
        static ILog logger = LogManager.GetLogger(typeof(MonitoringUpdate));

        delegate void UpdatePortUsed(string PortName, bool Used);
        delegate void UpdateID(string PortName, string FoupID);
        delegate void UpdateNode(string JobId);
        delegate void UpdateForSlot(Node Port, string Slot);

        public static void DisableUpdate(string Name, bool Checked)
        {
            try
            {
                Form form = Application.OpenForms["FormMonitoring"];
                CheckBox W;
                if (form == null)
                    return;

                W = form.Controls.Find(Name + "_ck", true).FirstOrDefault() as CheckBox;
                if (W == null)
                    return;

                if (W.InvokeRequired)
                {
                    UpdatePortUsed ph = new UpdatePortUsed(DisableUpdate);
                    W.BeginInvoke(ph, Name, Checked);
                }
                else
                {
                    W.Checked = Checked;

                }
            }
            catch
            {

            }
        }

        public static void UpdateFoupID(string PortName, string FoupID)
        {
            try
            {
                Form form = Application.OpenForms["FormMonitoring"];
                TextBox W;
                if (form == null)
                    return;

                W = form.Controls.Find(PortName.ToUpper() + "_FID", true).FirstOrDefault() as TextBox;
                if (W == null)
                    return;

                if (W.InvokeRequired)
                {
                    UpdateID ph = new UpdateID(UpdateFoupID);
                    W.BeginInvoke(ph, PortName, FoupID);
                }
                else
                {
                    W.Text = FoupID;

                }
            }
            catch
            {
                logger.Error("UpdateFoupID: Update fail.");
            }
        }

        public static void UpdateWPH(string WPH)
        {
            try
            {
                Form form = Application.OpenForms["FormMonitoring"];
                Label W;
                if (form == null)
                    return;

                W = form.Controls.Find("WPH", true).FirstOrDefault() as Label;
                if (W == null)
                    return;

                if (W.InvokeRequired)
                {
                    UpdateNode ph = new UpdateNode(UpdateWPH);
                    W.BeginInvoke(ph, WPH);
                }
                else
                {
                    W.Text = WPH;

                }
            }
            catch
            {
                logger.Error("UpdateWPH: Update fail.");
            }
        }

        public static void UpdateUseState(string PortName, bool used)
        {
            try
            {
                Form form = Application.OpenForms["FormMonitoring"];
                TextBox Used;
                if (form == null)
                    return;

                Used = form.Controls.Find(PortName + "_FID", true).FirstOrDefault() as TextBox;
                if (Used == null)
                    return;

                if (Used.InvokeRequired)
                {
                    UpdatePortUsed ph = new UpdatePortUsed(UpdateUseState);
                    Used.BeginInvoke(ph, PortName, used);
                }
                else
                {
                    if (used)
                    {
                        //Used.Text = "Used";
                        Used.BackColor = Color.Green;
                        Used.ForeColor = Color.White;
                    }
                    else
                    {
                        //Used.Text = "Not Used";
                        Used.BackColor = Color.White;
                        Used.ForeColor = Color.Black;
                    }

                }
            }
            catch
            {
                logger.Error("UpdateUseState: Update fail.");
            }
        }
        private static void UpdateSlot(Node Port, string Slot)
        {
            Form form = Application.OpenForms["FormMonitoring"];
            if (form == null)
                return;

            Label present = form.Controls.Find(Port.Name + "_Slot_" + Slot, true).FirstOrDefault() as Label;
            if (present == null)
            {
                return;
            }
            if (present.InvokeRequired)
            {
                UpdateForSlot ph = new UpdateForSlot(UpdateSlot);
                present.BeginInvoke(ph, Port, Slot);
            }
            else
            {
                Job tmp;
                if (Port.JobList.TryGetValue(Slot, out tmp))
                {
                    present.Text = tmp.Host_Job_Id;
                    switch (present.Text)
                    {
                        case "No wafer":
                            present.BackColor = Color.DimGray;
                            present.ForeColor = Color.White;
                            break;
                        case "Crossed":
                        case "Undefined":
                        case "Double":
                            present.BackColor = Color.Red;
                            present.ForeColor = Color.White;
                            break;
                        default:
                            present.BackColor = Color.Green;
                            present.ForeColor = Color.White;
                            break;
                    }

                }
                else
                {
                    present.Text = "";
                    present.BackColor = Color.White;
                }
            }
        }
        public static void UpdateNodesJob(string NodeName)
        {
            try
            {
                Form form = Application.OpenForms["FormMonitoring"];
          

                if (form == null)
                    return;

                
                    Node node = NodeManagement.Get(NodeName);


                for (int i = 1; i <= Tools.GetSlotCount(node.Type); i++)
                {
                    UpdateSlot(node, i.ToString());
                }

            }
            catch
            {
                logger.Error("UpdateNodesJob: Update fail.");
            }
        }

        public static void UpdateJobMove(string JobId)
        {
            try
            {
                Form form = Application.OpenForms["FormMonitoring"];
                Label tb;

                if (form == null)
                    return;

                tb = form.Controls.Find("LoadPort01_Mode", true).FirstOrDefault() as Label;

                if (tb == null)
                    return;

                if (tb.InvokeRequired)
                {
                    UpdateNode ph = new UpdateNode(UpdateJobMove);
                    tb.BeginInvoke(ph, JobId);
                }
                else
                {
                    Job Job = JobManagement.Get(JobId);
                    if (Job != null)
                    {
                        Node LastNode = NodeManagement.Get(Job.LastNode);
                        Node CurrentNode = NodeManagement.Get(Job.Position);
                        if (LastNode != null )
                        {

                            Label present = form.Controls.Find(Job.LastNode + "_Slot_" + Job.LastSlot, true).FirstOrDefault() as Label;
                            if (present != null)
                            {

                                present.Text = "No wafer";
                                present.BackColor = Color.DimGray;
                                present.ForeColor = Color.White;

                            }

                        }
                        if ( CurrentNode != null)
                        {
                            Label present = form.Controls.Find(Job.Position + "_Slot_" + Job.Slot, true).FirstOrDefault() as Label;
                            if (present != null)
                            {


                                present.Text = Job.Host_Job_Id;

                                present.BackColor = Color.Green;
                                present.ForeColor = Color.White;

                            }

                        }
                    }
                }


            }
            catch
            {
                logger.Error("UpdateJobMove: Update fail.");
            }
        }

       
    }
}
