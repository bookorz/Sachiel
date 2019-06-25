using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferControl.Management;

namespace Adam
{
    public partial class FormOCRKeyIn : Form
    {
        Job wafer;
        string Type = "";
        public FormOCRKeyIn(string OcrType, Job Wafer)
        {
            InitializeComponent();
            wafer = Wafer;
            Type = OcrType;
        }

        private void Confirm_btn_Click(object sender, EventArgs e)
        {
            if (!WaferID_tb.Text.Equals(""))
            {
                switch (Type)
                {
                    case "M12":
                        wafer.OCR_M12_Result = WaferID_tb.Text;
                        break;
                    case "T7":
                        wafer.OCR_T7_Result = WaferID_tb.Text;
                        break;
                    case "EITHER":
                        wafer.OCR_M12_Result = WaferID_tb.Text;
                        wafer.OCR_T7_Result = WaferID_tb.Text;
                        break;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please key in wafer id.");
            }
        }

        private void FormOCRKeyIn_Load(object sender, EventArgs e)
        {
            Info_lb.Text = Type + " OCR Fail";
            string savePath = "";
                switch (Type)
            {
                case "M12":
                    savePath = wafer.OCR_M12_ImgPath;
                    break;
                case "T7":
                    savePath = wafer.OCR_T7_ImgPath;
                    break;
                case "EITHER":
                    savePath = wafer.OCR_M12_ImgPath;
                    break;
            }
            try
            {
                Bitmap t = new Bitmap(Image.FromFile(savePath), new Size(1280, 720));
                OCR_Img.Image = t;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            WaferID_tb.ImeMode = ImeMode.Off;
        }
    }
}
