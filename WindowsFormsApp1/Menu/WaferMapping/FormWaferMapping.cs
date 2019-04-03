using Adam.UI_Update.Monitoring;
using Adam.UI_Update.WaferMapping;
using Adam.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TransferControl.Management;

namespace Adam.Menu.WaferMapping
{
    public partial class FormWaferMapping : Adam.Menu.FormFrame
    {


        public FormWaferMapping()
        {
            InitializeComponent();
        }

        private void FormWaferMapping_Load(object sender, EventArgs e)
        {


        }

        private void Assign_finish_btn_Click(object sender, EventArgs e)
        {

        }
        string fromPort = "";
        string fromSlot = "";
        string toPort = "";
        string toSlot = "";
        private void Slot_Click(object sender, EventArgs e)
        {
            Label Slot_Label = (Label)sender;
            string PortName = Slot_Label.Name.Substring(0, Slot_Label.Name.IndexOf("_Slot")).ToUpper();
            string SlotNo = Slot_Label.Name.Substring(Slot_Label.Name.IndexOf("_Slot") + 6);
            Node port = NodeManagement.Get(PortName);
            Job s;
            if (!port.JobList.TryGetValue(SlotNo, out s))
            {
                return;
            }

            if ((PortName.Equals(fromPort) && SlotNo.Equals(fromSlot) && toPort.Equals("") && toSlot.Equals("")) || (fromPort.Equals("") && fromSlot.Equals("")))
            {//select from
                if (s.MapFlag && !s.ErrPosition)
                {
                    if (Slot_Label.BackColor == Color.SkyBlue)
                    {// cancel select                       
                        if (s.Destination.Equals("") && s.DestinationSlot.Equals(""))
                        {
                            Slot_Label.BackColor = Color.Green;
                            Slot_Label.ForeColor = Color.White;
                        }
                        else
                        {
                            Slot_Label.BackColor = Color.Brown;
                            Slot_Label.ForeColor = Color.White;
                        }
                        fromPort = "";
                        fromSlot = "";

                        Form form = Application.OpenForms["FormWaferMapping"];
                        foreach (Node p in NodeManagement.GetLoadPortList())
                        {
                            if (p.Enable && p.IsMapping)
                            {
                                foreach (Job eachSlot in p.JobList.Values)
                                {
                                    if (!eachSlot.MapFlag && !eachSlot.ErrPosition)
                                    {

                                        Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                        if (present != null)
                                        {
                                            if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                            {
                                                present.BackColor = Color.DimGray;
                                                present.ForeColor = Color.White;
                                            }
                                            else
                                            {
                                                present.BackColor = Color.Brown;
                                                present.ForeColor = Color.White;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {// not select
                        Slot_Label.BackColor = Color.SkyBlue;
                        Slot_Label.ForeColor = Color.White;
                        fromPort = PortName;
                        fromSlot = SlotNo;
                        Form form = Application.OpenForms["FormWaferMapping"];
                        foreach (Node p in NodeManagement.GetLoadPortList())
                        {
                            if (p.Enable && p.IsMapping)
                            {
                                foreach (Job eachSlot in p.JobList.Values)
                                {
                                    if (!eachSlot.MapFlag && !eachSlot.ErrPosition)
                                    {

                                        Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                        if (present != null)
                                        {
                                            
                                            if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                            {
                                                present.BackColor = Color.White;
                                                present.ForeColor = Color.Black;
                                            }
                                            else
                                            {
                                                if (eachSlot.ReservePort.ToUpper().Equals(fromPort.ToUpper()) && eachSlot.ReserveSlot.ToUpper().Equals(fromSlot.ToUpper()))
                                                {
                                                    present.BackColor = Color.Yellow;
                                                    present.ForeColor = Color.Black;
                                                    toPort = eachSlot.Position;
                                                    toSlot = eachSlot.Slot;
                                                }
                                                else
                                                {
                                                    present.BackColor = Color.Brown;
                                                    present.ForeColor = Color.White;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
            else if ((PortName.Equals(toPort) && SlotNo.Equals(toSlot)) || (toPort.Equals("") && toSlot.Equals("")))
            {//select to


                if (!s.MapFlag && !s.ErrPosition)
                {
                    if (Slot_Label.BackColor == Color.Yellow)
                    {// cancel select
                        Slot_Label.BackColor = Color.White;
                        Slot_Label.ForeColor = Color.Black;
                        toPort = "";
                        toSlot = "";
                        Node fPort = NodeManagement.Get(fromPort);
                        if (fPort != null)
                        {
                            Job fSlot;
                            if (fPort.JobList.TryGetValue(fromSlot, out fSlot))
                            {
                                fSlot.UnAssignPort();
                            }
                        }
                    }
                    else if(s.ReservePort.Equals("") && s.ReserveSlot.Equals(""))
                    {// not select
                        Slot_Label.BackColor = Color.Yellow;
                        Slot_Label.ForeColor = Color.Black;
                        toPort = PortName;
                        toSlot = SlotNo;
                        Node fPort = NodeManagement.Get(fromPort);
                        if (fPort != null)
                        {
                            Job fSlot;
                            if (fPort.JobList.TryGetValue(fromSlot, out fSlot))
                            {
                                fSlot.AssignPort(toPort, toSlot);
                            }
                        }
                    }
                }

            }
            else if (s.MapFlag && !s.ErrPosition && !(PortName.Equals(fromPort) && SlotNo.Equals(fromSlot)))
            {//select other from
             //reset all from slot
                Form form = Application.OpenForms["FormWaferMapping"];
                foreach (Node p in NodeManagement.GetLoadPortList())
                {
                    if (p.Enable && p.IsMapping)
                    {
                        foreach (Job eachSlot in p.JobList.Values)
                        {
                            if (eachSlot.MapFlag && !eachSlot.ErrPosition)
                            {

                                Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                if (present != null)
                                {
                                    if (eachSlot.Destination.Equals("") && eachSlot.DestinationSlot.Equals(""))
                                    {
                                        present.BackColor = Color.Green;
                                        present.ForeColor = Color.White;
                                    }
                                    else
                                    {
                                        present.BackColor = Color.Brown;
                                        present.ForeColor = Color.White;
                                    }
                                }
                            }
                        }
                    }
                }
                if (!s.Destination.Equals("") && !s.DestinationSlot.Equals(""))
                {
                    Slot_Label.BackColor = Color.SkyBlue;
                    Slot_Label.ForeColor = Color.White;
                    fromPort = PortName;
                    fromSlot = SlotNo;
                    //Form form = Application.OpenForms["FormWaferMapping"];
                    foreach (Node p in NodeManagement.GetLoadPortList())
                    {
                        if (p.Enable && p.IsMapping)
                        {
                            foreach (Job eachSlot in p.JobList.Values)
                            {
                                if (!eachSlot.MapFlag && !eachSlot.ErrPosition)
                                {

                                    Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                    if (present != null)
                                    {

                                        if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                        {
                                            present.BackColor = Color.White;
                                            present.ForeColor = Color.Black;
                                        }
                                        else
                                        {
                                            if (eachSlot.ReservePort.ToUpper().Equals(fromPort.ToUpper()) && eachSlot.ReserveSlot.ToUpper().Equals(fromSlot.ToUpper()))
                                            {
                                                present.BackColor = Color.Yellow;
                                                present.ForeColor = Color.Black;
                                                toPort = eachSlot.Position;
                                                toSlot = eachSlot.Slot;
                                            }
                                            else
                                            {
                                                present.BackColor = Color.Brown;
                                                present.ForeColor = Color.White;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    fromPort = PortName;
                    fromSlot = SlotNo;
                    toPort = "";
                    toSlot = "";
                    

                    //select new from slot
                    Slot_Label.BackColor = Color.SkyBlue;
                    Slot_Label.ForeColor = Color.White;
                    fromPort = PortName;
                    fromSlot = SlotNo;
                    // Form form = Application.OpenForms["FormWaferMapping"];
                    foreach (Node p in NodeManagement.GetLoadPortList())
                    {
                        if (p.Enable && p.IsMapping)
                        {
                            foreach (Job eachSlot in p.JobList.Values)
                            {
                                if (!eachSlot.MapFlag && !eachSlot.ErrPosition)
                                {

                                    Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                    if (present != null)
                                    {
                                        if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                        {
                                            present.BackColor = Color.White;
                                            present.ForeColor = Color.Black;
                                        }
                                        else
                                        {
                                            present.BackColor = Color.Brown;
                                            present.ForeColor = Color.White;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}