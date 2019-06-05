using Adam.UI_Update.Barcode;
using Adam.UI_Update.Monitoring;
using Adam.UI_Update.WaferMapping;
using Adam.Util;
using log4net;
using SANWA.Utility.Config;
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
using TransferControl.Operation;

namespace Adam.Menu.WaferMapping
{
    public partial class FormWaferMapping : Adam.Menu.FormFrame
    {
        static ILog logger = LogManager.GetLogger(typeof(FormWaferMapping));

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
        private void RefreshMap()
        {
            fromPort = "";
            fromSlot = "";
            toPort = "";
            toSlot = "";
            Form form = this;
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

      
        private Node SearchLoadport()
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
            return result;
        }
        private void Assign_finish_btn_Click(object sender, EventArgs e)
        {

            if (XfeCrossZone.Running)
            {
                MessageBox.Show("請先執行整機初始化");
            }
            else
            {
                Node Loadport = SearchLoadport();
                if (Loadport == null)
                {
                    MessageBox.Show("找不到"+Loadport.Name);
                }
                else
                {
                    
                    if (!FormMain.xfe.Start(Loadport.Name))
                    {
                        MessageBox.Show("xfe.Start fail!");
                    }
                    else
                    {
                        this.Enabled = false;
                        fromPort = "";
                        fromSlot = "";
                        toPort = "";
                        toSlot = "";

                    }
                }
            }


        }
        public static string fromPort = "";
        public static string fromSlot = "";
        public static string toPort = "";
        public static string toSlot = "";
        bool bypass = true;
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
                                Label present = form.Controls.Find(fromPort + "_Slot_" + fromSlot.ToString(), true).FirstOrDefault() as Label;
                                present.Text += "";
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
                                        if (!bypass)
                                        {
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

        private void AutoAssign_btn_Click(object sender, EventArgs e)
        {
            if (Source_cb.Text.Equals(To_cb.Text))
            {
                MessageBox.Show("Not support assign to the same loadport");
                return;
            }
            
            Node Loadport = NodeManagement.Get(Source_cb.Text);
            Node UnLoadport = NodeManagement.Get(To_cb.Text);
            if (Loadport == null)
            {
                MessageBox.Show("Source loadport not found!");
                return;
            }
            if (UnLoadport == null)
            {
                MessageBox.Show("To loadport not found!");
                return;
            }
            var LD_Jobs = from wafer in Loadport.JobList.Values
                          where wafer.MapFlag && !wafer.ErrPosition && wafer.Destination.Equals("")
                          orderby Convert.ToInt16(wafer.Slot)
                          select wafer;
            int assignCnt = LD_Jobs.Count();
           

            bool isAssign = false;
           
          

                var ULD_Jobs = (from Slot in UnLoadport.JobList.Values
                                where !Slot.MapFlag && !Slot.ErrPosition && !Slot.IsAssigned
                                select Slot).OrderByDescending(x => Convert.ToInt16(x.Slot));

                foreach (Job wafer in LD_Jobs)
                {//檢查LD的所有WAFER

                    isAssign = false;
                    foreach (Job Slot in ULD_Jobs)
                    {//搜尋所有FOSB Slot 找到能放的     
                    if (Recipe.Get(SystemConfig.Get().CurrentRecipe).manual_put_constrict.Equals("1") && UnLoadport.CheckPreviousPresence(Slot.Slot))
                    {//下一層有片所以不能放
                        Slot.Locked = true;
                    }
                    else if (Recipe.Get(SystemConfig.Get().CurrentRecipe).manual_put_constrict.Equals("2") && UnLoadport.CheckForwardPresence(Slot.Slot))
                    {//下方所有都不能有片
                        Slot.Locked = true;
                    }
                    else
                        {
                            wafer.NeedProcess = true;
                            wafer.ProcessFlag = false;
                            wafer.AssignPort(UnLoadport.Name, Slot.Slot);
                            isAssign = true;
                          
                            Slot.IsAssigned = true;
                            logger.Debug("Assign booktest2 from " + Loadport.Name + " slot:" + wafer.Slot + " to " + UnLoadport.Name + " slot:" + Slot.Slot);

                            break;
                        }
                    }
                    if (!isAssign)
                    {
                        break;
                    }
                }
            if (assignCnt != 0)
            {
                LD_Jobs = from wafer in Loadport.JobList.Values
                          where wafer.NeedProcess
                          orderby Convert.ToInt16(wafer.Slot)
                          select wafer;

                for (int i = 1; i < LD_Jobs.Count(); i = i + 2)
                {//重新排序目的地for雙Arm
                    Job upper = LD_Jobs.ToList()[i];
                    Job lower = LD_Jobs.ToList()[i - 1];
                    if (upper.Destination.Equals(lower.Destination) && upper.NeedProcess && lower.NeedProcess && Math.Abs(Convert.ToInt16(upper.DestinationSlot)- Convert.ToInt16(lower.DestinationSlot))==1)
                    {//目的地Slot相鄰 才進來
                        string swapDes = upper.Destination;
                        string swapSlot = upper.DestinationSlot;
                        upper.AssignPort(lower.Destination, lower.DestinationSlot);
                        lower.AssignPort(swapDes, swapSlot);
                        logger.Debug("Reverse booktest2 from " + Loadport.Name + " slot:" + upper.Slot + " to " + upper.Destination + " slot:" + upper.DestinationSlot);
                        logger.Debug("Reverse booktest2 from " + Loadport.Name + " slot:" + lower.Slot + " to " + upper.Destination + " slot:" + lower.DestinationSlot);
                        logger.Debug("Reverse booktest2 ---------- ");
                    }
                    //else
                    //{
                    //    i--;
                    //}
                }
            }
            RefreshMap();
            
        }

        private void LOADPORT_FID_Click(object sender, EventArgs e)
        {
            string PortName = ((TextBox)sender).Name.Substring(0, ((TextBox)sender).Name.IndexOf("_"));
            Node port = NodeManagement.Get(PortName);
            if (port.OrgSearchComplete && port.Foup_Placement)
            {
                Barcodeupdate.UpdateLoadport(PortName, true);
            }
            else
            {
                if (port.OrgSearchComplete)
                {
                    MessageBox.Show("Please excute org search");
                }
                if (!port.Foup_Placement)
                {
                    MessageBox.Show("No Foup exist");
                }

            }
        }
    }
}