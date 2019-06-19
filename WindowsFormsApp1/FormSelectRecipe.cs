using Adam.UI_Update.Layout;
using Adam.Util;
using System;
using System.IO;
using System.Windows.Forms;
using TransferControl.Config;

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
            string oldRecipeId = SystemConfig.Get().CurrentRecipe;
            string newRecipeId = cbRecipe.SelectedItem.ToString();

            using (var form = new FormConfirm("是否變更生產 Recipe:" + oldRecipeId + "=>" + newRecipeId))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    SystemConfig config = SystemConfig.Get();
                    config.CurrentRecipe = cbRecipe.SelectedItem.ToString();
                    config.Save();
                    FormMainUpdate.UpdateRecipe(config.CurrentRecipe);
                    SanwaUtil.addActionLog("Recipe", "Change", Global.currentUser, "變更生產 Recipe:" + oldRecipeId + "=>" + newRecipeId);
                    MessageBox.Show("Change recipe completed.", "Success");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cancel.", "Notice");
                }
            }
        }

        private void cbRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            Recipe recipe = Recipe.Get(cbRecipe.Text);
            lblRecipeName.Text = recipe.recipe_name;
        }
    }
}
