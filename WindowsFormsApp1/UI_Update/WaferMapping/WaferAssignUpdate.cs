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
        delegate void UpdateAssign(string PortName, string Mapping,bool Enable);

        public static void UpdateUseState(string PortName, bool used)
        {
            try
            {
                Form form = Application.OpenForms["FormWaferMapping"];
                Label Used;
                if (form == null)
                    return;

                Used = form.Controls.Find(PortName + "_Use", true).FirstOrDefault() as Label;
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
                        Used.Text = "Used";
                        Used.BackColor = Color.Green;
                        Used.ForeColor = Color.White;
                    }
                    else
                    {
                        Used.Text = "Not Used";
                        Used.BackColor = Color.DimGray;
                        Used.ForeColor = Color.White;
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

        public static void UpdateNodesJob(string NodeName)
        {
            try
            {
                Form form = Application.OpenForms["FormWaferMapping"];
                TextBox Mode;

                if (form == null)
                    return;

                Mode = form.Controls.Find(NodeName + "_FID", true).FirstOrDefault() as TextBox;

                if (Mode == null)
                    return;

                if (Mode.InvokeRequired)
                {
                    UpdatePortMapping ph = new UpdatePortMapping(UpdateNodesJob);
                    Mode.BeginInvoke(ph, NodeName);
                }
                else
                {
                    Node node = NodeManagement.Get(NodeName);

                    Mode.Text = node.Mode;
                    //if (node.IsMapping)
                    //{
                    for (int i = 1; i <= Tools.GetSlotCount(node.Type); i++)
                    {
                        Label present = form.Controls.Find(node.Name + "_Slot_" + i.ToString(), true).FirstOrDefault() as Label;
                        if (present != null)
                        {

                            Job tmp;
                            if (node.JobList.TryGetValue(i.ToString(), out tmp))
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
                    //}
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
                Form form = Application.OpenForms["FormWaferMapping"];
                Label tb;

                if (form == null)
                    return;

                tb = form.Controls.Find("LoadPort01_FID", true).FirstOrDefault() as Label;

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
