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
            //gbRecipe.Enabled = true;
            btnCreateRecipe.Enabled = false;
            btnModifyRecipe.Enabled = false;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            tbRecipeName.Text = "";
            tbRecipeID.Text = "";
            tbRecipeName.ReadOnly = false;
            tbRecipeID.ReadOnly = false;
            tbRecipeName.Focus();
            trvRecipe.Enabled = false;
            //gbRecipeHeader.Enabled = true;
            cbAutoFin1.SelectedIndex = 0;
            cbAutoFin2.SelectedIndex = 0;
            cbAutoGetRule.SelectedIndex = 0;
            cbAutoPutRule.SelectedIndex = 0;
            cbInputFin1.SelectedIndex = 0;
            cbInputFin2.SelectedIndex = 0;
            cbInputFin3.SelectedIndex = 0;
            cbManualFin1.SelectedIndex = 0;
            cbManualFin2.SelectedIndex = 0;
            cbManualGetRule.SelectedIndex = 0;
            cbManualPutRule.SelectedIndex = 0;
            cbOutputFin1.SelectedIndex = 0;
            cbOutputFin2.SelectedIndex = 0;
            cbOutputFin3.SelectedIndex = 0;
            cbP1CstType.SelectedIndex = 0;
            cbP1LoadType.SelectedIndex = 0;
            cbP1Seq.SelectedIndex = 0;
            cbP1Seq.SelectedIndex = 0;
            cbP2CstType.SelectedIndex = 0;
            cbP2LoadType.SelectedIndex = 0;
            cbP2Seq.SelectedIndex = 0;
            cbP3CstType.SelectedIndex = 0;
            cbP3LoadType.SelectedIndex = 0;
            cbP3Seq.SelectedIndex = 0;
            cbP4CstType.SelectedIndex = 0;
            cbP4LoadType.SelectedIndex = 0;
            cbP4Seq.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //檢查資料
            if (tbRecipeID.Text.Trim().Equals("") || tbRecipeName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Recipe Name or recipe id should not be empty.");
                return;
            }
            //GUI 處理
            //gbRecipe.Enabled = false;
            btnCreateRecipe.Enabled = true;
            btnModifyRecipe.Enabled = true;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            tbRecipeName.ReadOnly = true;
            tbRecipeID.ReadOnly = true;
            trvRecipe.Enabled = true;
            //Recipe 存檔
            Recipe recipe = new Recipe();
            recipe.aligner1_angle = tbA1_angle.Text.Equals("") ? "0" : Int32.Parse(tbA1_angle.Text).ToString();
            recipe.aligner1_speed = tbA1_speed.Text.Equals("") ? "20" : Int32.Parse(tbA1_speed.Text).ToString();
            recipe.aligner2_angle = tbA2_angle.Text.Equals("") ? "0" : Int32.Parse(tbA2_angle.Text).ToString();
            recipe.aligner2_speed = tbA2_speed.Text.Equals("") ? "20" : Int32.Parse(tbA2_speed.Text).ToString();

            recipe.auto_fin_unclamp = "Y";//固定Y

            recipe.auto_get_constrict = cbAutoGetRule.Text;
            recipe.auto_proc_fin = cbAutoFin1.Text + cbAutoFin2.Text;
            recipe.auto_put_constrict = cbAutoPutRule.Text;
            recipe.equip_id = tbEqpID.Text;
            recipe.input_proc_fin = cbInputFin1.Text + cbInputFin2.Text + cbInputFin3.Text;

            recipe.manual_fin_unclamp = "Y";//固定Y

            recipe.manual_get_constrict = cbManualGetRule.Text;
            recipe.manual_proc_fin = cbManualFin1.Text + cbManualFin2.Text;
            recipe.manual_put_constrict = cbManualPutRule.Text;
            recipe.output_proc_fin = cbOutputFin1.Text + cbOutputFin2.Text + cbOutputFin3.Text;

            recipe.port1_carrier_type = cbP1CstType.Text;
            recipe.port1_priority = Int32.Parse(cbP1Seq.Text);
            recipe.port1_type = cbP1LoadType.Text;

            recipe.port2_carrier_type = cbP2CstType.Text;
            recipe.port2_priority = Int32.Parse(cbP2Seq.Text);
            recipe.port2_type = cbP2LoadType.Text;

            recipe.port3_carrier_type = cbP3CstType.Text;
            recipe.port3_priority = Int32.Parse(cbP3Seq.Text);
            recipe.port3_type = cbP3LoadType.Text;

            recipe.port4_carrier_type = cbP4CstType.Text;
            recipe.port4_priority = Int32.Parse(cbP4Seq.Text);
            recipe.port4_type = cbP4LoadType.Text;

            recipe.recipe_id = tbRecipeID.Text.Trim();
            recipe.recipe_name = tbRecipeName.Text;
            recipe.robot1_speed = tbR1Speed.Text.Equals("") ? "20" : Int32.Parse(tbR1Speed.Text).ToString();
            recipe.robot2_speed = tbR2Speed.Text.Equals("") ? "20" : Int32.Parse(tbR2Speed.Text).ToString();

            recipe.notch_angle = tbNotch_angle.Text.Equals("") ? "0" : Int32.Parse(tbNotch_angle.Text).ToString();

            Recipe.Set(recipe.recipe_id, recipe);

            string CurrentRecipe = SystemConfig.Get().CurrentRecipe;
            if (cbActive.Checked)//設定生效
            {
                SystemConfig config = SystemConfig.Get();
                config.CurrentRecipe = tbRecipeID.Text;
                config.Save();
            }
            else if(CurrentRecipe.Equals(tbRecipeID.Text))//取消生效
            {
                SystemConfig config = SystemConfig.Get();
                config.CurrentRecipe = "default";
                config.Save();
            }
            //紀錄修改Log
            if(tbRecipeID.Enabled)
                Util.SanwaUtil.addActionLog("Recipe", "Create", Global.currentUser, "建立 Recipe:" + recipe.recipe_id);
            if (tbRecipeID.Enabled)
                Util.SanwaUtil.addActionLog("Recipe", "Modify", Global.currentUser, "修改 Recipe:" + recipe.recipe_id);

            refreshList();
            MessageBox.Show("Execute successfully.", "Success");
        }

        private void btnModifyRecipe_Click(object sender, EventArgs e)
        {
            if(trvRecipe.SelectedNode == null)
            {
                MessageBox.Show("Please select a recipe first.", "Notice");
                return;
            }
            updateInfo(trvRecipe.SelectedNode.Text);
            //gbRecipe.Enabled = true;
            //gbRecipeBody.Enabled = true;
            //gbRecipeHeader.Enabled = false;
            btnCreateRecipe.Enabled = false;
            btnModifyRecipe.Enabled = false;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            tbRecipeName.ReadOnly = true;
            tbRecipeID.ReadOnly = true;
            trvRecipe.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //gbRecipe.Enabled = false;
            btnCreateRecipe.Enabled = true;
            btnModifyRecipe.Enabled = true;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            tbRecipeName.ReadOnly = true;
            tbRecipeID.ReadOnly = true;
            trvRecipe.Enabled = true;
        }

        private void FormRecipeSetting_Load(object sender, EventArgs e)
        {
            refreshList();
        }

        private void refreshList()
        {
            trvRecipe.Nodes.Clear();
            DirectoryInfo d = new DirectoryInfo(@".\recipe");
            FileInfo[] Files = d.GetFiles("*.json"); //Getting Json files
            foreach (FileInfo file in Files)
            {
                string recipeId = file.Name.Replace(".json", "");
                TreeNode tmp = new TreeNode(recipeId);
                trvRecipe.Nodes.Add(tmp);
            }
            trvRecipe.ExpandAll();
        }

        private void trvRecipe_AfterSelect(object sender, TreeViewEventArgs e)
        {
            updateInfo(trvRecipe.SelectedNode.Text);
        }

        private void updateInfo(string recipeID)
        {
            try
            {
                //gbRecipe.Enabled = true;
                Recipe recipe = Recipe.Get(recipeID);
                tbA1_angle.Text = recipe.aligner1_angle;
                tbA1_speed.Text = recipe.aligner1_speed;
                tbA2_angle.Text = recipe.aligner2_angle;
                tbA2_speed.Text = recipe.aligner2_speed;

                //recipe.auto_fin_unclamp = "Y";//固定Y 無UI

                cbAutoGetRule.SelectedItem = recipe.auto_get_constrict;
                cbAutoFin1.SelectedItem = recipe.auto_proc_fin.Substring(0, 1);
                cbAutoFin2.SelectedItem = recipe.auto_proc_fin.Substring(1, 1);
                cbAutoPutRule.SelectedItem = recipe.auto_put_constrict;
                tbEqpID.Text = recipe.equip_id;
                cbInputFin1.SelectedItem = recipe.input_proc_fin.Substring(0, 1);
                cbInputFin2.SelectedItem = recipe.input_proc_fin.Substring(1, 1);
                cbInputFin3.SelectedItem = recipe.input_proc_fin.Substring(2, 1);

                //recipe.manual_fin_unclamp = "Y";//固定Y 無UI

                cbManualGetRule.SelectedItem = recipe.manual_get_constrict;
                cbManualFin1.SelectedItem = recipe.manual_proc_fin.Substring(0, 1);
                cbManualFin2.SelectedItem = recipe.manual_proc_fin.Substring(1, 1);

                cbManualPutRule.SelectedItem = recipe.manual_put_constrict;
                cbOutputFin1.SelectedItem = recipe.output_proc_fin.Substring(0, 1);
                cbOutputFin2.SelectedItem = recipe.output_proc_fin.Substring(1, 1);
                cbOutputFin3.SelectedItem = recipe.output_proc_fin.Substring(2, 1);

                cbP1CstType.SelectedItem = recipe.port1_carrier_type;
                cbP1Seq.SelectedItem = recipe.port1_priority.ToString();
                cbP1LoadType.SelectedItem = recipe.port1_type;

                cbP2CstType.SelectedItem = recipe.port2_carrier_type;
                cbP2Seq.SelectedItem = recipe.port2_priority.ToString();
                cbP2LoadType.SelectedItem = recipe.port2_type;

                cbP3CstType.SelectedItem = recipe.port3_carrier_type;
                cbP3Seq.SelectedItem = recipe.port3_priority.ToString();
                cbP3LoadType.SelectedItem = recipe.port3_type;

                cbP4CstType.SelectedItem = recipe.port4_carrier_type;
                cbP4Seq.SelectedItem = recipe.port4_priority.ToString();
                cbP4LoadType.SelectedItem = recipe.port4_type;

                tbRecipeID.Text = recipe.recipe_id;
                tbRecipeName.Text = recipe.recipe_name;
                tbR1Speed.Text = recipe.robot1_speed;
                tbR2Speed.Text = recipe.robot2_speed;

                tbNotch_angle.Text = recipe.notch_angle;

                //gbRecipe.Enabled = false;
                string CurrentRecipe = SystemConfig.Get().CurrentRecipe;
                if (CurrentRecipe.Equals(tbRecipeID.Text))//取消生效
                {
                    cbActive.Checked = true;
                }
                else
                {
                    cbActive.Checked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Recipe 格式錯誤，讀取失敗! " + ex.StackTrace);
            }
        }

        private void digit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
