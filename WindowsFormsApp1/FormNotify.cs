using log4net;
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

namespace Adam
{
    internal partial class FormNotify : Form
    {
        bool running = true;

        delegate void UpdatePort(string Name, string text);
        string FormName = "";
        ILog logger = LogManager.GetLogger("FormNotify");
        public FormNotify()
        {
            InitializeComponent();
        }
        public FormNotify(string title, string PortName, string FoupID)
        {
            try
            {
                InitializeComponent();

                this.Text = title;
                this.LoadportName_lb.Text = PortName;
                this.FoupID_lb.Text = FoupID;
                //switch (PortName.ToUpper())
                //{
                //    case "LOADPORT01":
                //        this.LOADPORT01_lb.Text = "■";
                //        this.LOADPORT01_lb.ForeColor = Color.Red;
                //        break;
                //    case "LOADPORT02":
                //        this.LOADPORT02_lb.Text = "■";
                //        this.LOADPORT02_lb.ForeColor = Color.Red;
                //        break;
                //    case "LOADPORT03":
                //        this.LOADPORT03_lb.Text = "■";
                //        this.LOADPORT03_lb.ForeColor = Color.Red;
                //        break;
                //    case "LOADPORT04":
                //        this.LOADPORT04_lb.Text = "■";
                //        this.LOADPORT04_lb.ForeColor = Color.Red;
                //        break;
                //}
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "\n" + e.StackTrace);
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(PortBlink), PortName);
        }

        private void PortBlink(object PortName)
        {
            string text = "□";
            while (running)
            {
                BlinkUpdate(PortName, text);
                if (text.Equals("□"))
                {
                    text = "■";
                }
                else
                {
                    text = "□";
                }
                SpinWait.SpinUntil(() => false, 700);
            }
        }

        private void BlinkUpdate(object PortName, string Text)
        {


            try
            {
                Form form = Application.OpenForms[FormName];
                Label port;
                if (form == null)
                    return;

                port = form.Controls.Find(PortName + "_lb", true).FirstOrDefault() as Label;
                if (port == null)
                    return;

                if (port.InvokeRequired)
                {
                    UpdatePort ph = new UpdatePort(BlinkUpdate);
                    port.BeginInvoke(ph, PortName, Text);
                }
                else
                {
                    port.ForeColor = Color.Red;
                    port.Text = Text;

                }


            }
            catch
            {
                logger.Error("BlinkUpdate: Update fail.");
            }
        }

        private void Confirm_btn_Click(object sender, EventArgs e)
        {
            running = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FormNotify_Load(object sender, EventArgs e)
        {
            // port = form.Controls.Find(PortName + "_lb", true).FirstOrDefault() as Label;
        }
    }

    //public static class NotifyMessageBox
    //{
    //    public static void Show(string title, string PortName, string FoupID)
    //    {
    //        // using construct ensures the resources are freed when form is closed
    //        using (var form = new FormNotify(title, PortName, FoupID))
    //        {
    //            form.ShowDialog();
    //        }
    //    }
    //}
}
