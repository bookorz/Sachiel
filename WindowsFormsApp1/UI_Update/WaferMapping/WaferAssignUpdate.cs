using Adam.UI_Update.Monitoring;
using Adam.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Engine;
using TransferControl.Management;

namespace Adam.UI_Update.WaferMapping
{
    class WaferAssignUpdate
    {
        static ILog logger = LogManager.GetLogger(typeof(WaferAssignUpdate));
        delegate void UpdatePort(string PortName, string Mapping);
        delegate void UpdatePortMapping(string PortName);
        delegate void UpdatePortUsed(string PortName, bool Used);
        delegate void UpdateAssign(string PortName, string Mapping, bool Enable);
        delegate void UpdateForSlot(Node Port, string Slot);

        public static void UpdateFoupID(string PortName, string FoupID)
        {
            try
            {
                Form form = Application.OpenForms["FormWaferMapping"];
                TextBox W;
                if (form == null)
                    return;

                W = form.Controls.Find(PortName.ToUpper() + "_FID", true).FirstOrDefault() as TextBox;
                if (W == null)
                    return;

                if (W.InvokeRequired)
                {
                    UpdatePort ph = new UpdatePort(UpdateFoupID);
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
        public static void UpdateEnabled(string Name, bool Enable)
        {
            try
            {
                Form form = Application.OpenForms["FormWaferMapping"];
                Button btn;
                if (form == null)
                    return;


                if (Name.Equals("FORM"))
                {
                    if (form.InvokeRequired)
                    {
                        UpdatePortUsed ph = new UpdatePortUsed(UpdateEnabled);
                        form.BeginInvoke(ph, Name, Enable);
                    }
                    else
                    {
                        if (Enable)
                        {
                            form.Enabled = true;
                        }
                        else
                        {
                            form.Enabled = false;
                        }

                    }
                }
                else
                {
                    btn = form.Controls.Find(Name, true).FirstOrDefault() as Button;
                    if (btn == null)
                        return;
                    if (btn.InvokeRequired)
                    {
                        UpdatePortUsed ph = new UpdatePortUsed(UpdateEnabled);
                        btn.BeginInvoke(ph, Name, Enable);
                    }
                    else
                    {
                        if (Enable)
                        {
                            btn.Enabled = true;
                        }
                        else
                        {
                            btn.Enabled = false;
                        }

                    }
                }
            }
            catch
            {
                logger.Error("UpdateUseState: Update fail.");
            }
        }



        public static void UpdateLoadPortMode(string PortName, string Mode)
        {
            try
            {
                Form form = Application.OpenForms["FormWaferMapping"];
                Label Port_Mode;
                if (form == null)
                    return;

                Port_Mode = form.Controls.Find(PortName + "State_lb", true).FirstOrDefault() as Label;
                if (Port_Mode == null)
                    return;

                if (Port_Mode.InvokeRequired)
                {
                    UpdatePort ph = new UpdatePort(UpdateLoadPortMode);
                    Port_Mode.BeginInvoke(ph, PortName, Mode);
                }
                else
                {
                    NodeManagement.Get(PortName).Mode = Mode;
                    Port_Mode.Text = PortName + "  [" + Mode + "]";

                }
            }
            catch (Exception e)
            {
                logger.Error("UpdateLoadPortMode: Update fail:" + e.StackTrace);
            }
        }
        private static void UpdateSlot(Node Port, string Slot)
        {
            Form form = Application.OpenForms["FormWaferMapping"];
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
                Form form = Application.OpenForms["FormWaferMapping"];
                TextBox Mode;

                if (form == null)
                    return;



                Node node = NodeManagement.Get(NodeName);

                //Mode.Text = node.Mode;
                //if (node.IsMapping)
                //{
                for (int i = 1; i <= Tools.GetSlotCount(node.Type); i++)
                {
                    UpdateSlot(node, i.ToString());
                }
                //}



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
                Form form = Application.OpenForms["FormWaferMapping"];
                TextBox tb;

                if (form == null)
                    return;

                tb = form.Controls.Find("LoadPort01_FID", true).FirstOrDefault() as TextBox;

                if (tb == null)
                    return;

                if (tb.InvokeRequired)
                {
                    UpdatePortMapping ph = new UpdatePortMapping(UpdateJobMove);
                    tb.BeginInvoke(ph, JobId);
                }
                else
                {
                    Job Job = JobManagement.Get(JobId);
                    if (Job != null)
                    {
                        Node LastNode = NodeManagement.Get(Job.LastNode);
                        Node CurrentNode = NodeManagement.Get(Job.Position);
                        if (LastNode != null)
                        {

                            Label present = form.Controls.Find(Job.LastNode + "_Slot_" + Job.LastSlot, true).FirstOrDefault() as Label;
                            if (present != null)
                            {

                                present.Text = "No wafer";
                                present.BackColor = Color.DimGray;
                                present.ForeColor = Color.White;

                            }

                        }
                        if (CurrentNode != null)
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
