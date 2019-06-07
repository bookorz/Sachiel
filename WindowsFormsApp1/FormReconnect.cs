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
using TransferControl.Controller;
using TransferControl.Management;

namespace Adam
{
    public partial class FormReconnect : Form
    {
        static Form instance = null;
        static bool _EMO = false;
        static int countDownSecs = -1;
        delegate void UpdateSecs(string Secs);
        delegate void UpdateNotify(bool EMO);
        static bool isInitial = false;

        public FormReconnect()
        {
            InitializeComponent();
            if (!isInitial)
            {
                isInitial = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(CountDown));
            }
        }

        public static void Show(bool EMO)
        {
            _EMO = EMO;
            if (!EMO)
            {
                countDownSecs = 15;
                NotifyUpdate(false);
            }
            else
            {
                countDownSecs = -1;
                FormShow(true);
                NotifyUpdate(true);
            }
            
            



        }

        private void CountDown(object obj)
        {

            while (true)
            {
                while (countDownSecs <= -1)
                {
                    SpinWait.SpinUntil(() => countDownSecs > -1, 99999999);
                }

                CountDownUpdate(countDownSecs.ToString());
                countDownSecs--;
                if (countDownSecs == -1)
                {//Reconnect all eqp
                    foreach(Node each in NodeManagement.GetList())
                    {
                        if (each.Enable)
                        {
                            each.GetController().Reconnect();
                        }
                    }
                    FormShow(false);
                }
                if (countDownSecs != -1)
                {
                    SpinWait.SpinUntil(() => false, 1000);
                }
            }
        }

        private void CountDownUpdate(string Secs)
        {
            Form form = Application.OpenForms["FormReconnect"];
            Label secs;
            if (form == null)
                return;

            secs = form.Controls.Find("Countdown_lb", true).FirstOrDefault() as Label;
            if (secs == null)
                return;

            if (secs.InvokeRequired)
            {
                UpdateSecs ph = new UpdateSecs(CountDownUpdate);
                secs.Invoke(ph, Secs);
            }
            else
            {
                secs.Text = "Reconnecting to all equipment, please wait for seconds(" + Secs + ")";

            }
        }
        private static void NotifyUpdate(bool EMO)
        {
            Form form = Application.OpenForms["FormReconnect"];
            Label emo;
            if (form == null)
                return;

            emo = form.Controls.Find("EMO_lb", true).FirstOrDefault() as Label;

            if (emo == null)
                return;

            if (emo.InvokeRequired)
            {
                UpdateNotify ph = new UpdateNotify(NotifyUpdate);
                emo.Invoke(ph, EMO);
            }
            else
            {
                Label cntdw = form.Controls.Find("Countdown_lb", true).FirstOrDefault() as Label;
                if (EMO)
                {
                    emo.Visible = true;
                    cntdw.Visible = false;
                }
                else
                {
                    emo.Visible = false;
                    cntdw.Visible = true;
                }
            }
        }
        static object lockObj = new object();
        private static void FormShow(bool Show)
        {
            Form form = null;
            lock (lockObj)
            {
                form = Application.OpenForms["FormReconnect"];

                if (form == null)
                {
                    instance = new FormReconnect();
                    form = instance;
                }
            }
            if (form.InvokeRequired)
            {
                UpdateNotify ph = new UpdateNotify(FormShow);
                form.Invoke(ph, Show);
            }
            else
            {
                if (Show)
                {
                    if (!form.Created)
                    {
                        form.TopMost = true;
                        form.WindowState = FormWindowState.Maximized;
                        form.ShowDialog();
                    }
                    
                }
                else
                {
                    if (form.Created)
                    {
                        form.Close();
                    }
                }
                
            }
        }

        private void FormReconnect_Load(object sender, EventArgs e)
        {

        }

        private void FormReconnect_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((countDownSecs == -1 && _EMO) || (countDownSecs != -1 && !_EMO))
            {
                e.Cancel = true;
            }
        }
    }
}

