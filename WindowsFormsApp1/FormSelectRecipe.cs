using Adam.UI_Update.Layout;
using Adam.Util;
using SANWA;
using SANWA.Utility.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormSelectRecipe : Form
    {
        private string recipe = "";
        public FormSelectRecipe()
        {
            InitializeComponent();
        }
        public FormSelectRecipe(string recipe)
        {
            InitializeComponent();
            this.recipe = recipe;
        }
       
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormSelectRecipe_Load(object sender, EventArgs e)
        {
            cbRecipe.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(@".\recipe");
            FileInfo[] Files = d.GetFiles("*.json"); //Getting Json files
            foreach (FileInfo file in Files)
            {
                string recipeId = file.Name.Replace(".json", "");
                cbRecipe.Items.Add(recipeId);
            }
            cbRecipe.SelectedItem = this.recipe;
        }

        private void btnChange_Click_1(object sender, EventArgs e)
        {
            SystemConfig config = SystemConfig.Get();
            config.CurrentRecipe = cbRecipe.SelectedItem.ToString();
            config.Save();
            FormMainUpdate.UpdateRecipe(config.CurrentRecipe);
            MessageBox.Show("Update Completed.", "Success");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cbRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            Recipe recipe = Recipe.Get(cbRecipe.Text);
            lblRecipeName.Text = recipe.recipe_name;
        }
    }
}
