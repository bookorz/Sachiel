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
    public partial class OCRResult : Form
    {
        Job _Job;

        public OCRResult(Job Job)
        {
            _Job = Job;
            InitializeComponent();
        }

        private void OCRResult_Load(object sender, EventArgs e)
        {
            if (_Job != null)
            {
                string filePath = "";
                string Socre = "";
                string Result = "";
                if (!_Job.OCRImgPath.Equals(""))
                {
                    OCR_Config_cb.SelectedIndex=0;
                    filePath = _Job.OCRImgPath;
                    Socre = _Job.OCRScore;
                    Result = _Job.OCRResult;
                }
                else if (!_Job.OCR_M12_ImgPath.Equals(""))
                {
                    OCR_Config_cb.SelectedIndex = 1;
                    filePath = _Job.OCR_M12_ImgPath;
                    Socre = _Job.OCR_M12_Score;
                    Result = _Job.OCR_M12_Result;
                }
                else if (!_Job.OCR_T7_ImgPath.Equals(""))
                {
                    OCR_Config_cb.SelectedIndex = 2;
                    filePath = _Job.OCR_T7_ImgPath;
                    Socre = _Job.OCR_T7_Score;
                    Result = _Job.OCR_T7_Result;
                }
                OCR_Img.Image = Image.FromFile(filePath);
                WaferID.Text = "Wafer ID:" + Result;
                OCR_Score.Text = "Score:" + Socre;
            }
            else
            {
                MessageBox.Show("Wafer 紀錄不存在");
            }
        }

        private void OCR_Config_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filePath = "";
            string Socre = "";
            string Result = "";
            switch (OCR_Config_cb.Text)
            {
                case "TTL":
                    filePath = _Job.OCRImgPath;
                    Socre = _Job.OCRScore;
                    Result = _Job.OCRResult;
                    break;
                case "M12":
                    filePath = _Job.OCR_M12_ImgPath;
                    Socre = _Job.OCR_M12_Score;
                    Result = _Job.OCR_M12_Result;
                    break;
                case "T7":
                    filePath = _Job.OCR_T7_ImgPath;
                    Socre = _Job.OCR_T7_Score;
                    Result = _Job.OCR_T7_Result;
                    break;
            }
            if (filePath.Equals(""))
            {
                OCR_Img.Image = null;
            }
            else
            {
                OCR_Img.Image = Image.FromFile(filePath);
            }
           

            WaferID.Text = "Wafer ID:" + Result;
            OCR_Score.Text = "Score:" + Socre;
        }
    }
}
