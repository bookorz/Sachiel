using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Comm;
using TransferControl.Config;
using TransferControl.Management;

namespace Adam.UI_Update.Manual
{
    public class OCRStatusUpdate
    {
        static ILog logger = LogManager.GetLogger(typeof(OCRStatusUpdate));
        //delegate void ShowMessage(string str);

        delegate void UpdateRobotStatus_D(string name, string status);

        public static void UpdateOCRConnection(string name, string status)
        {
            Form manual = Application.OpenForms["FormManual"];

            if (manual == null)
                return;

            TextBox tb = manual.Controls.Find("OCRConnection_tb", true).FirstOrDefault() as TextBox;
            if (tb == null)
                return;

            if (tb.InvokeRequired)
            {
                UpdateRobotStatus_D ph = new UpdateRobotStatus_D(UpdateOCRConnection);
                tb.BeginInvoke(ph, name, status);
            }
            else
            {
                ComboBox ocrName = manual.Controls.Find("Cb_OCRSelect", true).FirstOrDefault() as ComboBox;
                if (ocrName.Text.ToUpper().Equals(name.ToUpper()))
                {
                    tb.Text = status;
                    Button connectBtn = manual.Controls.Find("OCRConnect_btn", true).FirstOrDefault() as Button;
                    switch (status)
                    {
                        case "Connected":
                        case "Connecting":
                            connectBtn.Enabled = false;
                            break;
                        default:
                            connectBtn.Enabled = true;
                            break;
                    }
                }
            }

        }

        public static void UpdateOCROnlineMode(string name, string status)
        {
            Form manual = Application.OpenForms["FormManual"];

            if (manual == null)
                return;

            TextBox tb = manual.Controls.Find("OCRMode_tb", true).FirstOrDefault() as TextBox;
            if (tb == null)
                return;

            if (tb.InvokeRequired)
            {
                UpdateRobotStatus_D ph = new UpdateRobotStatus_D(UpdateOCROnlineMode);
                tb.BeginInvoke(ph, name, status);
            }
            else
            {
                tb.Text = status;

            }

        }

        public static void UpdateOCRRead(string name, string status)
        {
            Form manual = Application.OpenForms["FormManual"];

            if (manual == null)
                return;

            TextBox tb = manual.Controls.Find("OCRRead_tb", true).FirstOrDefault() as TextBox;
            if (tb == null)
                return;

            if (tb.InvokeRequired)
            {
                UpdateRobotStatus_D ph = new UpdateRobotStatus_D(UpdateOCRRead);
                tb.BeginInvoke(ph, name, status);
            }
            else
            {
              
                string src = "";
                switch (name)
                {
                    case "OCR01":

                        src = SystemConfig.Get().OCR1ImgSourcePath;
                        break;
                    case "OCR02":

                        src = SystemConfig.Get().OCR2ImgSourcePath;
                        break;
                }
                Thread.Sleep(500);
                Node OCR = NodeManagement.Get(name);
                if (OCR != null)
                {

                    string OCRResult = status;

                        switch (OCR.Brand)
                        {
                            case "COGNEX":
                                string[] ocrResult = OCRResult.Replace("[", "").Replace("]", "").Split(',');
                                string info = ocrResult[0];
                                if (ocrResult.Length >= 2)
                                {
                                    info += " Score:" + ocrResult[1];
                                }
                                else
                                {
                                    info += " Score:0";
                                }
                                tb.Text = info;
                                FTP ftp = new FTP(OCR.GetController().GetIPAdress(), "21", "", "admin", "");
                               
                                PictureBox Pic_OCR = manual.Controls.Find("OCR_Pic", true).FirstOrDefault() as PictureBox;
                                if (Pic_OCR == null)
                                    return;
                                Bitmap t = new Bitmap(Image.FromFile(src), new Size(320, 240));
                                Pic_OCR.Image = t;

                                break;
                            case "HST":
                                ocrResult = OCRResult.Replace("[", "").Replace("]", "").Split(',');
                                string WaferID = ocrResult[0];
                                if (ocrResult.Length >= 2)
                                {
                                    WaferID += " Score:" + ocrResult[1];
                                }
                                else
                                {
                                    WaferID += " Score:0";
                                }
                                tb.Text = WaferID;

                                string[] files = Directory.GetFiles(src);
                                List<string> fileList = files.ToList();
                                if (fileList.Count != 0)
                                {
                                    fileList.Sort((x, y) => { return -File.GetLastWriteTime(x).CompareTo(File.GetLastWriteTime(y)); });

                                   
                                    Pic_OCR = manual.Controls.Find("OCR_Pic", true).FirstOrDefault() as PictureBox;
                                    
                                    if (Pic_OCR == null)
                                        return;
                                    t = new Bitmap(Image.FromFile(fileList[0]), new Size(320, 240));
                                    Pic_OCR.Image = t;
                                    
                                }
                                break;
                        }

                    
                }
            }

        }
        public static void UpdateOCRM12Read(string name, string status)
        {
            Form manual = Application.OpenForms["FormManual"];

            if (manual == null)
                return;

            TextBox tb = manual.Controls.Find("OCRRead_tb", true).FirstOrDefault() as TextBox;
            if (tb == null)
                return;

            if (tb.InvokeRequired)
            {
                UpdateRobotStatus_D ph = new UpdateRobotStatus_D(UpdateOCRM12Read);
                tb.BeginInvoke(ph, name, status);
            }
            else
            {

                string src = "";
                switch (name)
                {
                    case "OCR01":

                        src = SystemConfig.Get().OCR1ImgSourcePath;
                        break;
                    case "OCR02":

                        src = SystemConfig.Get().OCR2ImgSourcePath;
                        break;
                }
                Thread.Sleep(500);
                Node OCR = NodeManagement.Get(name);
                if (OCR != null)
                {

                    string OCRResult = status;

                    switch (OCR.Brand)
                    {
                        case "COGNEX":
                            string[] ocrResult = OCRResult.Replace("[", "").Replace("]", "").Split(',');
                            string info = ocrResult[0];
                            if (ocrResult.Length >= 2)
                            {
                                info += " Score:" + ocrResult[1];
                            }
                            else
                            {
                                info += " Score:0";
                            }
                            tb.Text = info;
                            FTP ftp = new FTP(OCR.GetController().GetIPAdress(), "21", "", "admin", "");

                            PictureBox Pic_OCR = manual.Controls.Find("OCR_Pic", true).FirstOrDefault() as PictureBox;
                            if (Pic_OCR == null)
                                return;
                            Bitmap t = new Bitmap(Image.FromFile(src), new Size(320, 240));
                            Pic_OCR.Image = t;

                            break;
                        case "HST":
                            ocrResult = OCRResult.Replace("[", "").Replace("]", "").Split(',');
                            string WaferID = ocrResult[0];
                            if (ocrResult.Length >= 2)
                            {
                                WaferID += " Score:" + ocrResult[1];
                            }
                            else
                            {
                                WaferID += " Score:0";
                            }
                            tb.Text = WaferID;

                            string[] files = Directory.GetFiles(src);
                            List<string> fileList = files.ToList();
                            if (fileList.Count != 0)
                            {
                                fileList.Sort((x, y) => { return -File.GetLastWriteTime(x).CompareTo(File.GetLastWriteTime(y)); });


                                Pic_OCR = manual.Controls.Find("OCR_Pic", true).FirstOrDefault() as PictureBox;

                                if (Pic_OCR == null)
                                    return;
                                t = new Bitmap(Image.FromFile(fileList[0]), new Size(320, 240));
                                Pic_OCR.Image = t;

                            }
                            break;
                    }


                }
            }

        }
        public static void UpdateOCRT7Read(string name, string status)
        {
            Form manual = Application.OpenForms["FormManual"];

            if (manual == null)
                return;

            TextBox tb = manual.Controls.Find("OCR01ReadT7_Tb", true).FirstOrDefault() as TextBox;
            if (tb == null)
                return;

            if (tb.InvokeRequired)
            {
                UpdateRobotStatus_D ph = new UpdateRobotStatus_D(UpdateOCRT7Read);
                tb.BeginInvoke(ph, name, status);
            }
            else
            {

                string src = "";
                switch (name)
                {
                    case "OCR01":

                        src = SystemConfig.Get().OCR1ImgSourcePath;
                        break;
                    case "OCR02":

                        src = SystemConfig.Get().OCR2ImgSourcePath;
                        break;
                }
                Thread.Sleep(500);
                Node OCR = NodeManagement.Get(name);
                if (OCR != null)
                {

                    string OCRResult = status;

                    switch (OCR.Brand)
                    {
                        case "COGNEX":
                            string[] ocrResult = OCRResult.Replace("[", "").Replace("]", "").Split(',');
                            string info = ocrResult[0];
                            if (ocrResult.Length >= 2)
                            {
                                info += " Score:" + ocrResult[1];
                            }
                            else
                            {
                                info += " Score:0";
                            }
                            tb.Text = info;
                            FTP ftp = new FTP(OCR.GetController().GetIPAdress(), "21", "", "admin", "");

                            PictureBox Pic_OCR = manual.Controls.Find("OCR_Pic", true).FirstOrDefault() as PictureBox;
                            if (Pic_OCR == null)
                                return;
                            Bitmap t = new Bitmap(Image.FromFile(src), new Size(320, 240));
                            Pic_OCR.Image = t;

                            break;
                        case "HST":
                            ocrResult = OCRResult.Replace("[", "").Replace("]", "").Split(',');
                            string WaferID = ocrResult[0];
                            if (ocrResult.Length >= 2)
                            {
                                WaferID += " Score:" + ocrResult[1];
                            }
                            else
                            {
                                WaferID += " Score:0";
                            }
                            tb.Text = WaferID;

                            string[] files = Directory.GetFiles(src);
                            List<string> fileList = files.ToList();
                            if (fileList.Count != 0)
                            {
                                fileList.Sort((x, y) => { return -File.GetLastWriteTime(x).CompareTo(File.GetLastWriteTime(y)); });


                                Pic_OCR = manual.Controls.Find("OCR_Pic", true).FirstOrDefault() as PictureBox;

                                if (Pic_OCR == null)
                                    return;
                                t = new Bitmap(Image.FromFile(fileList[0]), new Size(320, 240));
                                Pic_OCR.Image = t;

                            }
                            break;
                    }


                }
            }

        }
    }
}
