using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adam.Menu.SystemSetting
{
    public partial class FormRecipeSetting : Form
    {
        public FormRecipeSetting()
        {
            InitializeComponent();
        }

        private void btnCreateRecipe_Click(object sender, EventArgs e)
        {
            var result = new DataTable().Compute("'a'= 'a'", null);
            MessageBox.Show(result.ToString());
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void comboBox25_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
