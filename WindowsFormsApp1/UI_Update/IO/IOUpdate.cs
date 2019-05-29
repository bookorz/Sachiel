using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Engine;

namespace Adam.UI_Update.IO
{
    class IOUpdate
    {
        static ILog logger = LogManager.GetLogger(typeof(IOUpdate));
        delegate void UpdateIO(string Parameter, string Value, string Type);
        delegate Label UpdatePanel(string Parameter, string Type);
        static int currentY_I = 15;
        static int currentY_O = 15;

        public static void UpdateDIO(string Parameter, string Value,string Type)
        {
            try
            {
                Form form = Application.OpenForms["FormIO"];

                if (form == null)
                    return;

                Label lb = form.Controls.Find(Parameter+"_"+ Type, true).FirstOrDefault() as Label;

                if (lb == null)
                {
                    lb = InsertDIO(Parameter, Type);
                    UpdateDIO(Parameter, Value, Type);
                    return;
                }

                if (lb.InvokeRequired)
                {
                    UpdateIO ph = new UpdateIO(UpdateDIO);
                    lb.BeginInvoke(ph, Parameter, Value, Type);
                }
                else
                {
                    if (Value.ToUpper().Equals("TRUE"))
                    {
                        lb.ForeColor = Color.GreenYellow;
                    }
                    else
                    {
                        lb.ForeColor = Color.Red;
                    }
                   
                }


            }
            catch (Exception e)
            {
                logger.Error("UpdateChart: Update fail. err:" + e.StackTrace);
            }
        }

        private static Label InsertDIO(string Name, string Type)
        {
            Form form = Application.OpenForms["FormIO"];
            Panel P;
            Label value = new Label();
            int currentY = 0;
            if (Type.ToUpper().Equals("DIN"))
            {
                currentY = currentY_I;
                currentY_I += 30;
                P = form.Controls.Find("Digital_I_List", true).FirstOrDefault() as Panel;
            }
            else
            {
                currentY = currentY_O;
                currentY_O += 30;
                P = form.Controls.Find("Digital_O_List", true).FirstOrDefault() as Panel;
            }
            if (P.InvokeRequired)
            {
                UpdatePanel ph = new UpdatePanel(InsertDIO);
                P.Invoke(ph, Name, Type);
            }
            else
            {


                

                value.Name = Name + "_" + Type;
                value.Text = "■";//"●"
                                 //value.ForeColor = Color.Red;
                value.ForeColor = Color.DimGray;//預設為未知
                value.Location = new System.Drawing.Point(0, currentY);
                value.Font = new Font(new FontFamily(value.Font.Name), 16, value.Font.Style);
                //value.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                value.Size = new System.Drawing.Size(20, 20);
                P.Controls.Add(value);

                Label id = new Label();
                id.Text = Name;
                id.Location = new System.Drawing.Point(20, currentY);
                if (Type.ToUpper().Equals("DIN"))
                {
                    id.Size = new System.Drawing.Size(330, 20);
                }
                else
                {
                    id.Size = new System.Drawing.Size(180, 20);
                }
                //id.Font = new Font(new FontFamily(id.Font.Name), 12, id.Font.Style);
                id.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //hint.SetToolTip(id, desc);
                P.Controls.Add(id);

                if (Type.ToUpper().Equals("DOUT"))
                {
                    Button On = new Button();
                    On.Text = "On";

                    On.Name = Name + "_" + Type + "_ON";
                    //On.Name = AddressNo + "_" + ID + "_" + Type + "_ON";
                    On.Click += On_IO_Click;
                    On.Location = new System.Drawing.Point(200, currentY);
                    //On.Font = new Font(new FontFamily(On.Font.Name), 9, On.Font.Style);
                    On.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    On.Size = new System.Drawing.Size(38, 25);
                    P.Controls.Add(On);

                    Button Off = new Button();
                    Off.Text = "Off";

                    Off.Name = Name + "_" + Type + "_OFF";
                    //Off.Name = AddressNo + "_" + ID + "_" + Type + "_OFF";
                    Off.Click += On_IO_Click;
                    Off.Location = new System.Drawing.Point(240, currentY);
                    //Off.Font = new Font(new FontFamily(On.Font.Name), 9, On.Font.Style);
                    Off.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    Off.Size = new System.Drawing.Size(38, 25);
                    P.Controls.Add(Off);
                }
            }
            return value;
        }
        private static void On_IO_Click(object sender, EventArgs e)
        {
            string key = ((Button)sender).Name;
            string value = key.Substring(key.LastIndexOf("_") + 1);
            key = key.Substring(0, key.LastIndexOf("_"));
            key = key.Substring(0, key.LastIndexOf("_"));
            switch (value.ToUpper())
            {
                case "ON":
                    RouteControl.Instance.DIO.SetIO(key, "TRUE");
                    break;
                case "OFF":
                    RouteControl.Instance.DIO.SetIO(key, "FALSE");
                    break;
            }
        }
    }
}
