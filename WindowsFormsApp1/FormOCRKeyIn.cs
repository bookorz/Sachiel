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



        private void FormOCRKeyIn_Load(object sender, EventArgs e)
        {
            Info_lb.Text = Type + " OCR Fail";
            string savePath = "";
            switch (Type)
            {
                case "M12":
                    savePath = wafer.OCR_M12_ImgPath;
                    M12_panel.Visible = true;
                    T7_panel.Visible = false;
                    break;
                case "T7":
                    savePath = wafer.OCR_T7_ImgPath;
                    M12_panel.Visible = false;
                    T7_panel.Visible = true;
                    break;
                case "ALL":
                    savePath = wafer.OCR_M12_ImgPath;
                    M12_panel.Visible = true;
                    T7_panel.Visible = true;
                    break;
            }
            try
            {
                Bitmap t = new Bitmap(Image.FromFile(savePath), new Size(1280, 720));
                OCR_Img.Image = t;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
            M12WaferID_tb.ImeMode = ImeMode.Off;
            T7WaferID_tb.ImeMode = ImeMode.Off;
        }

        private void M12Confirm_btn_Click(object sender, EventArgs e)
        {
            if (!M12WaferID_tb.Text.Equals(""))
            {

                wafer.OCR_M12_Result = M12WaferID_tb.Text;

                wafer.Host_Job_Id = M12WaferID_tb.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please key in wafer id.");
            }
        }

        private void T7Confirm_btn_Click(object sender, EventArgs e)
        {
            if (!T7WaferID_tb.Text.Equals(""))
            {

                wafer.OCR_T7_Result = T7WaferID_tb.Text;

                wafer.Host_Job_Id = T7WaferID_tb.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please key in wafer id.");
            }
        }
    }
}
