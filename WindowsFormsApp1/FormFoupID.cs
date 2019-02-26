using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adam
{
    public partial class FormFoupID : Form
    {
        public FormFoupID()
        {
            InitializeComponent();
        }

        private void FoupID_Read_tb_Leave(object sender, EventArgs e)
        {
            FoupID_Read_tb.Focus();
        }

        private void FormFoupID_Load(object sender, EventArgs e)
        {
            FoupID_Read_tb.Focus();
        }

        private void FormFoupID_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Abort load procedure?", "Warning", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {
                //Unclamp foup            
            }
            else
            {
                FoupID_Read_tb.Focus();
            }
        }

        private void FoupID_Read_Confirm_btn_Click(object sender, EventArgs e)
        {
            //open & mapping foup
        }
    }
}
