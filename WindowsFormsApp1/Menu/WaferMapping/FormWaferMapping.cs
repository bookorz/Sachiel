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
            {//選擇來源slot
                if (s.MapFlag && !s.ErrPosition)
                {
                    if (Slot_Label.BackColor == Color.SkyBlue)
                    {// 取消選擇來源slot                     
                        if (s.Destination.Equals("") && s.DestinationSlot.Equals(""))
                        {//還沒選
                            Slot_Label.BackColor = Color.Green;
                            Slot_Label.ForeColor = Color.White;
                        }
                        else
                        {//已選
                            Slot_Label.BackColor = Color.Brown;
                            Slot_Label.ForeColor = Color.White;
                        }
                        fromPort = "";
                        fromSlot = "";

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
                                }
                            }
                        }
                    }
                    else
                    {// 選擇來源
                        Form form = Application.OpenForms["FormWaferMapping"];
                        Job lastSlot;
                        if (!bypass)
                        {
                            foreach (Node p in NodeManagement.GetLoadPortList()) //標記不能選的位置
                            {
                                if (p.Enable && p.IsMapping)
                                {
                                    foreach (Job eachSlot in p.JobList.Values)
                                    {
                                        if (eachSlot.MapFlag && !eachSlot.ErrPosition)
                                        {
                                            //只能由下往上取，標記不能取的Wafer
                                            if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out lastSlot))
                                            {
                                                if ((lastSlot.MapFlag && lastSlot.Destination.Equals("")) || (!lastSlot.MapFlag && !lastSlot.ReservePort.Equals("")))
                                                {
                                                    Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                                    if (present != null)
                                                    {//把不能取的slot標記
                                                        present.ForeColor = Color.Red;
                                                    }
                                                }
                                            }
                                        }
                                        
                                    }
                                }
                            }


                            //判斷能不能選，不能選就跳脫
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
                        //標記選定來源
                        Slot_Label.BackColor = Color.SkyBlue;
                        Slot_Label.ForeColor = Color.White;
                        fromPort = PortName;
                        fromSlot = SlotNo;

                        foreach (Node p in NodeManagement.GetLoadPortList()) //標記能放的位置
                        {
                            if (p.Enable && p.IsMapping)
                            {
                                foreach (Job eachSlot in p.JobList.Values)
                                {
                                    if (!eachSlot.MapFlag && !eachSlot.ErrPosition)
                                    {//找到空slot

                                        Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                        if (present != null)
                                        {

                                            if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                            {//該空slot沒被預約
                                                if (!bypass)
                                                {
                                                    Job nextSlot = null;
                                                    if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out nextSlot))
                                                    {
                                                        if (!nextSlot.ReservePort.Equals(""))
                                                        {
                                                            continue;//判斷是否能從上往下放的SLOT，不能放的話就不標記
                                                        }
                                                    }
                                                }

                                                present.BackColor = Color.White;
                                                present.ForeColor = Color.Black;
                                            }
                                            else
                                            {//該slot已被選定
                                                if (eachSlot.ReservePort.ToUpper().Equals(fromPort.ToUpper()) && eachSlot.ReserveSlot.ToUpper().Equals(fromSlot.ToUpper()))
                                                {//被目前選擇的來源綁定
                                                    present.BackColor = Color.Yellow;
                                                    present.ForeColor = Color.Black;
                                                    toPort = eachSlot.Position;
                                                    toSlot = eachSlot.Slot;
                                                }
                                                else
                                                {//被其他綁定
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
             //判斷可不可以選
                Job lastSlot;

                Form form = Application.OpenForms["FormWaferMapping"];
                if (!bypass)
                {
                    Node pt = NodeManagement.Get(PortName);
                    //只能由上往下放
                    if (pt.JobList.TryGetValue((Convert.ToInt16(SlotNo) - 1).ToString(), out lastSlot))
                    {
                        if (!lastSlot.ReservePort.Equals(""))
                        {
                            Label present = form.Controls.Find(pt.Name + "_Slot_" + SlotNo, true).FirstOrDefault() as Label;
                            if (present != null)
                            {
                                return;//如果下面的slot有東西，跳脫不給選
                            }
                        }
                    }
                }
                if (!s.MapFlag && !s.ErrPosition)
                {
                    if (Slot_Label.BackColor == Color.Yellow)
                    {// 取消選擇目的地
                        Slot_Label.BackColor = Color.White;
                        Slot_Label.ForeColor = Color.Black;

                        Node fPort = NodeManagement.Get(fromPort);
                        if (fPort != null)
                        {
                            Job fSlot;
                            if (fPort.JobList.TryGetValue(fromSlot, out fSlot))
                            {
                                fSlot.UnAssignPort();//取消綁定
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
                                    Label present = form.Controls.Find(toPort + "_Slot_" + (Convert.ToInt16(toSlot) + 1).ToString(), true).FirstOrDefault() as Label;
                                    if (!tSlot.MapFlag && !tSlot.ErrPosition && tSlot.ReservePort.Equals(""))
                                    {//如果上一層還沒被指派，標記為可被選
                                        present.BackColor = Color.White;
                                        present.ForeColor = Color.Black;

                                    }
                                    else if (tSlot.MapFlag && !tSlot.ErrPosition && tSlot.Destination.Equals(""))
                                    {//如果上一層還沒被指派，標記為可被選
                                        if (!tSlot.Slot.Equals(fromSlot) || !port.Name.ToUpper().Equals(fromPort))
                                        {
                                            present.BackColor = Color.Green;
                                            present.ForeColor = Color.White;
                                        }
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
                                        //只能由下往上取，標記能取的Wafer
                                        if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out lastSlot))
                                        {
                                            Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                            if (present != null)
                                            {
                                                if (((lastSlot.MapFlag && lastSlot.Destination.Equals("")) || (!lastSlot.MapFlag && !lastSlot.ReservePort.Equals(""))) && !bypass)
                                                {
                                                    //下面一層Slot未被選，則標記為不可選
                                                    present.ForeColor = Color.Red;
                                                }
                                                else
                                                {  //標記可選
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
                            Node tPort = NodeManagement.Get(toPort);//如果上一層還沒被指派，標記為不可被選
                            if (tPort != null)
                            {
                                Job tSlot;
                                if (tPort.JobList.TryGetValue((Convert.ToInt16(toSlot) + 1).ToString(), out tSlot))
                                {
                                    Label present = form.Controls.Find(toPort + "_Slot_" + (Convert.ToInt16(toSlot) + 1).ToString(), true).FirstOrDefault() as Label;
                                    if (!tSlot.MapFlag && !tSlot.ErrPosition && tSlot.ReservePort.Equals(""))
                                    {                                   
                                        present.BackColor = Color.DimGray;
                                        present.ForeColor = Color.White;

                                    }
                                    else if (tSlot.MapFlag && !tSlot.ErrPosition && tSlot.Destination.Equals(""))
                                    {
                                        present.BackColor = Color.Green;
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
                                        if(p.Name.ToUpper().Equals(fromPort.ToUpper())&& eachSlot.Slot.Equals(fromSlot))
                                        {
                                            continue;
                                        }
                                        if (eachSlot.MapFlag && !eachSlot.ErrPosition)
                                        {
                                            //只能由下往上取，標記能取的Wafer
                                            if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out lastSlot))
                                            {
                                                Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                                if (present != null)
                                                {
                                                    if (((lastSlot.MapFlag && lastSlot.Destination.Equals("")) || (!lastSlot.MapFlag && !lastSlot.ReservePort.Equals(""))))
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
            {//已經選完來源根目的後，點另一個來源
             //reset all from slot 
                Job lastSlot;
                Form form = Application.OpenForms["FormWaferMapping"];
                if (!bypass)
                {
                    //判斷能不能選
                    Node pt = NodeManagement.Get(PortName);
                    //只能由下往上取，標記能取的Wafer
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
                                        //只能由下往上取，標記能取的Wafer
                                        if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out lastSlot))
                                        {

                                            if (present != null)
                                            {
                                                if (((lastSlot.MapFlag && lastSlot.Destination.Equals("")) || (!lastSlot.MapFlag && !lastSlot.ReservePort.Equals(""))) && !bypass)
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
                if (!s.Destination.Equals("") && !s.DestinationSlot.Equals(""))//選到的是已綁定的slot
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
                                {//找到空slot

                                    Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                    if (present != null)
                                    {

                                        if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                        {//這個空slot還沒被綁定
                                            Job nextSlot;
                                            if (p.JobList.TryGetValue((Convert.ToInt16(eachSlot.Slot) - 1).ToString(), out nextSlot))
                                            {
                                                Form tform = Application.OpenForms["FormWaferMapping"];
                                                //Label tpresent = form.Controls.Find(p.Name + "_Slot_" + nextSlot.Slot, true).FirstOrDefault() as Label;
                                                if (!nextSlot.ReservePort.Equals("") && !bypass)
                                                {//下層slot已被綁定

                                                    if (nextSlot.ReservePort.Equals(""))
                                                    {

                                                        present.BackColor = Color.White;
                                                        present.ForeColor = Color.Black;

                                                    }
                                                    else
                                                    {//下層已被綁定，限制放片

                                                        present.BackColor = Color.DimGray;
                                                        present.ForeColor = Color.White;
                                                    }

                                                }
                                                else
                                                {//bypass
                                                    present.BackColor = Color.White;
                                                    present.ForeColor = Color.Black;
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


                    //在已經選擇來源與目的後，點選新的來源slot
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
                                {//找到空的slot

                                    Label present = form.Controls.Find(p.Name + "_Slot_" + eachSlot.Slot, true).FirstOrDefault() as Label;
                                    if (present != null)
                                    {
                                        if (eachSlot.ReservePort.Equals("") && eachSlot.ReserveSlot.Equals(""))
                                        {//該slot還沒被綁定
                                            Form tform = Application.OpenForms["FormWaferMapping"];
                                            Label tpresent = form.Controls.Find(p.Name + "_Slot_" + (Convert.ToInt16(eachSlot.Slot) + 1).ToString(), true).FirstOrDefault() as Label;
                                            Job tSlot;
                                            if (p.JobList.TryGetValue(toSlot, out tSlot)&& !bypass)
                                            {

                                                if (tSlot.ReservePort.Equals("") )//檢查下層slot是否已被綁定，改變目前slot限制狀態
                                                {
                                                    //未被綁定
                                                    tpresent.BackColor = Color.White;
                                                    tpresent.ForeColor = Color.Black;

                                                }
                                                else
                                                {
                                                    //已被綁定
                                                    tpresent.BackColor = Color.DimGray;
                                                    tpresent.ForeColor = Color.White;
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
                }
            }

        }
    }
}