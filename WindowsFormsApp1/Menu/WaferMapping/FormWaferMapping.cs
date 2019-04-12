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
            Form form = this;
            foreach (Node port in NodeManagement.GetLoadPortList())
            {
                for (int i = 1; i <= 25; i++)
                {
                    Label present = form.Controls.Find(port.Name + "_Slot_" + i.ToString(), true).FirstOrDefault() as Label;
                    if (present != null)
                    {
                        switch (port.CarrierType.ToUpper())
                        {
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

        private void Assign_finish_btn_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Form form = Application.OpenForms["FormMain"];


        }
        string fromPort = "";
        string fromSlot = "";
        string toPort = "";
        string toSlot = "";
        bool bypass = false;
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
            {//��ܨӷ�slot
                if (s.MapFlag && !s.ErrPosition)
                {
                    if (Slot_Label.BackColor == Color.SkyBlue)
                    {// ������ܨӷ�slot                     
                        if (s.Destination.Equals("") && s.DestinationSlot.Equals(""))
                        {//�٨S��
                            Slot_Label.BackColor = Color.Green;
                            Slot_Label.ForeColor = Color.White;
                        }
                        else
                        {//�w��
                            Slot_Label.BackColor = Color.Brown;
                            Slot_Label.ForeColor = Color.White;
                        }
                        fromPort = "";
                        fromSlot = "";

                        Form form = Application.OpenForms["FormWaferMapping"];
                        foreach (Node p in NodeManagement.GetLoadPortList())//��s�Ҧ��ت��aslot�Q�諸���A
                        {
                            if (p.Enable && p.IsMapping)
                            {
                                foreach (Job eachSlot in p.JobList.Values)
                                {
                                    if (!eachSlot.MapFlag && !eachSlot.ErrPosition)//����slot
                                    {

                                        Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                        if (present != null)
                                        {
                                            if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                            {//�S�Q��
                                                present.BackColor = Color.DimGray;
                                                present.ForeColor = Color.White;
                                            }
                                            else
                                            {//�w�Q��
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
                    {// ��ܨӷ�
                        Form form = Application.OpenForms["FormWaferMapping"];
                        Job lastSlot;
                        if (!bypass)
                        {
                            foreach (Node p in NodeManagement.GetLoadPortList()) //�аO����諸��m
                            {
                                if (p.Enable && p.IsMapping)
                                {
                                    foreach (Job eachSlot in p.JobList.Values)
                                    {
                                        if (eachSlot.MapFlag && !eachSlot.ErrPosition)
                                        {
                                            //�u��ѤU���W���A�аO�������Wafer
                                            if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out lastSlot))
                                            {
                                                if ((lastSlot.MapFlag && lastSlot.Destination.Equals("")) || (!lastSlot.MapFlag && !lastSlot.ReservePort.Equals("")))
                                                {
                                                    Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                                    if (present != null)
                                                    {//�⤣�����slot�аO
                                                        present.ForeColor = Color.Red;
                                                    }
                                                }
                                            }
                                        }
                                        
                                    }
                                }
                            }


                            //�P�_�ण���A�����N����
                            Node pt = NodeManagement.Get(PortName);

                            if (pt.JobList.TryGetValue((Convert.ToInt16(SlotNo) - 1).ToString(), out lastSlot))
                            {
                                if ((lastSlot.MapFlag && lastSlot.Destination.Equals("")) || (!lastSlot.MapFlag && !lastSlot.ReservePort.Equals("")))
                                {
                                    Label present = form.Controls.Find(pt.Name + "_Slot_" + SlotNo, true).FirstOrDefault() as Label;
                                    if (present != null)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                        //�аO��w�ӷ�
                        Slot_Label.BackColor = Color.SkyBlue;
                        Slot_Label.ForeColor = Color.White;
                        fromPort = PortName;
                        fromSlot = SlotNo;

                        foreach (Node p in NodeManagement.GetLoadPortList()) //�аO��񪺦�m
                        {
                            if (p.Enable && p.IsMapping)
                            {
                                foreach (Job eachSlot in p.JobList.Values)
                                {
                                    if (!eachSlot.MapFlag && !eachSlot.ErrPosition)
                                    {//����slot

                                        Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                        if (present != null)
                                        {

                                            if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                            {//�Ӫ�slot�S�Q�w��
                                                if (!bypass)
                                                {
                                                    Job nextSlot = null;
                                                    if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out nextSlot))
                                                    {
                                                        if (!nextSlot.ReservePort.Equals(""))
                                                        {
                                                            continue;//�P�_�O�_��q�W���U��SLOT�A����񪺸ܴN���аO
                                                        }
                                                    }
                                                }

                                                present.BackColor = Color.White;
                                                present.ForeColor = Color.Black;
                                            }
                                            else
                                            {//��slot�w�Q��w
                                                if (eachSlot.ReservePort.ToUpper().Equals(fromPort.ToUpper()) && eachSlot.ReserveSlot.ToUpper().Equals(fromSlot.ToUpper()))
                                                {//�Q�ثe��ܪ��ӷ��j�w
                                                    present.BackColor = Color.Yellow;
                                                    present.ForeColor = Color.Black;
                                                    toPort = eachSlot.Position;
                                                    toSlot = eachSlot.Slot;
                                                }
                                                else
                                                {//�Q��L�j�w
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
             //�P�_�i���i�H��
                Job lastSlot;

                Form form = Application.OpenForms["FormWaferMapping"];
                if (!bypass)
                {
                    Node pt = NodeManagement.Get(PortName);
                    //�u��ѤW���U��
                    if (pt.JobList.TryGetValue((Convert.ToInt16(SlotNo) - 1).ToString(), out lastSlot))
                    {
                        if (!lastSlot.ReservePort.Equals(""))
                        {
                            Label present = form.Controls.Find(pt.Name + "_Slot_" + SlotNo, true).FirstOrDefault() as Label;
                            if (present != null)
                            {
                                return;//�p�G�U����slot���F��A���椣����
                            }
                        }
                    }
                }
                if (!s.MapFlag && !s.ErrPosition)
                {
                    if (Slot_Label.BackColor == Color.Yellow)
                    {// ������ܥت��a
                        Slot_Label.BackColor = Color.White;
                        Slot_Label.ForeColor = Color.Black;

                        Node fPort = NodeManagement.Get(fromPort);
                        if (fPort != null)
                        {
                            Job fSlot;
                            if (fPort.JobList.TryGetValue(fromSlot, out fSlot))
                            {
                                fSlot.UnAssignPort();//�����j�w
                            }
                        }
                        if (!bypass)
                        {
                            Node tPort = NodeManagement.Get(toPort);
                            if (tPort != null)
                            {
                                Job tSlot;
                                if (tPort.JobList.TryGetValue((Convert.ToInt16(toSlot) + 1).ToString(), out tSlot))
                                {
                                    if (tSlot.ReservePort.Equals(""))
                                    {//�p�G�W�@�h�٨S�Q�����A�аO���i�Q��

                                        Label present = form.Controls.Find(toPort + "_Slot_" + (Convert.ToInt16(toSlot) + 1).ToString(), true).FirstOrDefault() as Label;
                                        present.BackColor = Color.White;
                                        present.ForeColor = Color.Black;

                                    }
                                }
                            }
                        }
                        foreach (Node p in NodeManagement.GetLoadPortList())
                        {
                            if (p.Enable && p.IsMapping)
                            {
                                foreach (Job eachSlot in p.JobList.Values)
                                {
                                    if (eachSlot.MapFlag && !eachSlot.ErrPosition)
                                    {
                                        //�u��ѤU���W���A�аO�����Wafer
                                        if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out lastSlot))
                                        {
                                            Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                            if (present != null)
                                            {
                                                if (lastSlot.Destination.Equals("") && !bypass)
                                                {
                                                    //�U���@�hSlot���Q��A�h�аO�����i��
                                                    present.ForeColor = Color.Red;
                                                }
                                                else
                                                {  //�аO�i��
                                                    present.ForeColor = Color.White;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        toPort = "";
                        toSlot = "";
                    }
                    else if (s.ReservePort.Equals("") && s.ReserveSlot.Equals(""))
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
                        if (!bypass)
                        {
                            Node tPort = NodeManagement.Get(toPort);//�p�G�W�@�h�٨S�Q�����A�аO�����i�Q��
                            if (tPort != null)
                            {
                                Job tSlot;
                                if (tPort.JobList.TryGetValue((Convert.ToInt16(toSlot) + 1).ToString(), out tSlot))
                                {
                                    if (tSlot.ReservePort.Equals(""))
                                    {

                                        Label present = form.Controls.Find(toPort + "_Slot_" + (Convert.ToInt16(toSlot) + 1).ToString(), true).FirstOrDefault() as Label;
                                        present.BackColor = Color.DimGray;
                                        present.ForeColor = Color.White;

                                    }
                                }
                            }

                            foreach (Node p in NodeManagement.GetLoadPortList())
                            {
                                if (p.Enable && p.IsMapping)
                                {
                                    foreach (Job eachSlot in p.JobList.Values)
                                    {
                                        if (eachSlot.MapFlag && !eachSlot.ErrPosition)
                                        {
                                            //�u��ѤU���W���A�аO�����Wafer
                                            if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out lastSlot))
                                            {
                                                Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                                if (present != null)
                                                {
                                                    if (lastSlot.Destination.Equals(""))
                                                    {

                                                        present.ForeColor = Color.Red;
                                                    }
                                                    else
                                                    {
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
            else if (s.MapFlag && !s.ErrPosition && !(PortName.Equals(fromPort) && SlotNo.Equals(fromSlot)))
            {//�w�g�粒�ӷ��ڥت���A�I�t�@�Өӷ�
             //reset all from slot 
                Job lastSlot;
                Form form = Application.OpenForms["FormWaferMapping"];
                if (!bypass)
                {
                    //�P�_�ण���
                    Node pt = NodeManagement.Get(PortName);
                    //�u��ѤU���W���A�аO�����Wafer
                    if (pt.JobList.TryGetValue((Convert.ToInt16(SlotNo) - 1).ToString(), out lastSlot))
                    {
                        if (lastSlot.Destination.Equals(""))
                        {
                            Label present = form.Controls.Find(pt.Name + "_Slot_" + SlotNo, true).FirstOrDefault() as Label;
                            if (present != null)
                            {
                                return;
                            }
                        }
                    }
                }

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
                                        //�u��ѤU���W���A�аO�����Wafer
                                        if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out lastSlot))
                                        {

                                            if (present != null)
                                            {
                                                if (lastSlot.Destination.Equals("") && !bypass)
                                                {

                                                    present.ForeColor = Color.Red;
                                                }
                                                else
                                                {
                                                    present.ForeColor = Color.White;
                                                }
                                            }
                                        }
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
                if (!s.Destination.Equals("") && !s.DestinationSlot.Equals(""))//��쪺�O�w�j�w��slot
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
                                {//����slot

                                    Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                    if (present != null)
                                    {

                                        if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                        {//�o�Ӫ�slot�٨S�Q�j�w
                                            Job nextSlot;
                                            if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out nextSlot))
                                            {
                                                Form tform = Application.OpenForms["FormWaferMapping"];
                                                Label tpresent = form.Controls.Find(p.Name + "_Slot_" + nextSlot.Slot, true).FirstOrDefault() as Label;
                                                if (!nextSlot.ReservePort.Equals("") && !bypass)
                                                {//�U�hslot�w�Q�j�w

                                                    if (nextSlot.ReservePort.Equals(""))
                                                    {

                                                        tpresent.BackColor = Color.White;
                                                        tpresent.ForeColor = Color.Black;

                                                    }
                                                    else
                                                    {//�U�h�w�Q�j�w�A������

                                                        tpresent.BackColor = Color.DimGray;
                                                        tpresent.ForeColor = Color.White;
                                                    }

                                                }
                                                else
                                                {//bypass
                                                    tpresent.BackColor = Color.White;
                                                    tpresent.ForeColor = Color.Black;
                                                }
                                            }
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


                    //�b�w�g��ܨӷ��P�ت���A�I��s���ӷ�slot
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
                                {//���Ū�slot

                                    Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                    if (present != null)
                                    {
                                        if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                        {//��slot�٨S�Q�j�w
                                            Form tform = Application.OpenForms["FormWaferMapping"];
                                            Label tpresent = form.Controls.Find(toPort + "_Slot_" + (Convert.ToInt16(toSlot) + 1).ToString(), true).FirstOrDefault() as Label;
                                            Job tSlot;
                                            if (p.JobList.TryGetValue(toSlot, out tSlot)&& !bypass)
                                            {

                                                if (tSlot.ReservePort.Equals("") )//�ˬd�ثeslot�O�_�w�Q�j�w�A���ܤW�hslot����A
                                                {
                                                    //���Q�j�w
                                                    tpresent.BackColor = Color.White;
                                                    tpresent.ForeColor = Color.Black;

                                                }
                                                else
                                                {
                                                    //�w�Q�j�w
                                                    tpresent.BackColor = Color.DimGray;
                                                    tpresent.ForeColor = Color.White;
                                                }
                                            }
                                            else
                                            {
                                                present.BackColor = Color.White;
                                                present.ForeColor = Color.Black;
                                            }
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