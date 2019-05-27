using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adam.UI_Update.Barcode
{
    class Barcodeupdate
    {
        static ILog logger = LogManager.GetLogger(typeof(Barcodeupdate));
        delegate void UpdateName(string Msg);
        public static void UpdateLoadport(string Name)
        {
            try
            {

                Form form = Application.OpenForms["FormFoupID"];
                Label name;



                if (form == null)
                    return;


                name = form.Controls.Find("LoadportName_lb", true).FirstOrDefault() as Label ;
                if (name == null)
                    return;

                if (name.InvokeRequired)
                {
                    UpdateName ph = new UpdateName(UpdateLoadport);

                    name.BeginInvoke(ph, Name);

                }
                else
                {
                    name.Text = Name;

                    form.Visible = true;

                    TextBox FoupID = form.Controls.Find("FoupID_Read_tb", true).FirstOrDefault() as TextBox;
                    FoupID.Text = "";
                    FoupID.Focus();
                }


            }
            catch (Exception e)
            {
                logger.Error("UpdateAlarmList: Update fail." + e.Message + "\n" + e.StackTrace);
            }

        }
    }
}
