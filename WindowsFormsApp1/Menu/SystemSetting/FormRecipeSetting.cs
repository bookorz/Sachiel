using Adam.UI_Update.Layout;
using GUI;
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
using TransferControl.Config;

namespace Adam.Menu.SystemSetting
{
    public partial class FormRecipeSetting : Form
    {
        public TreeNode previousSelectedNode = null;
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
            ////gbRecipeHeader.Enabled = true;
            //cbAutoFin1.SelectedIndex = 0;
            //cbAutoFin2.SelectedIndex = 0;
            //cbAutoGetRule.SelectedIndex = 0;
            //cbAutoPutRule.SelectedIndex = 0;
            //cbInputFin1.SelectedIndex = 0;
            //cbInputFin2.SelectedIndex = 0;
            //cbInputFin3.SelectedIndex = 0;
            //cbManualFin1.SelectedIndex = 0;
            //cbManualFin2.SelectedIndex = 0;
            //cbManualGetRule.SelectedIndex = 0;
            //cbManualPutRule.SelectedIndex = 0;
            //cbOutputFin1.SelectedIndex = 0;
            //cbOutputFin2.SelectedIndex = 0;
            //cbOutputFin3.SelectedIndex = 0;
            //cbP1CstType.SelectedIndex = 0;
            //cbP1LoadType.SelectedIndex = 0;
            //cbP1Seq.SelectedIndex = 0;
            //cbP1Seq.SelectedIndex = 0;
            //cbP2CstType.SelectedIndex = 0;
            //cbP2LoadType.SelectedIndex = 0;
            //cbP2Seq.SelectedIndex = 0;
            //cbP3CstType.SelectedIndex = 0;
            //cbP3LoadType.SelectedIndex = 0;
            //cbP3Seq.SelectedIndex = 0;
            //cbP4CstType.SelectedIndex = 0;
            //cbP4LoadType.SelectedIndex = 0;
            //cbP4Seq.SelectedIndex = 0;

            lblMode.Text = "新增模式";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //檢查資料
            if (tbRecipeID.Text.Trim().Equals("") || tbRecipeName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Recipe Name or recipe id should not be empty.", "Data check error");
                return;
            }
            if (!checkOCRConfig(cbUseOcrTTL.Checked, tbOcrTTL.Text))
            {
                MessageBox.Show("OCR config format error.", "Data check error");
                tbOcrTTL.Focus();
                return;
            }
            if (!checkOCRConfig(cbUseOcrT7.Checked, tbOcrT7.Text))
            {
                MessageBox.Show("OCR T7 config format error.", "Error");
                tbOcrT7.Focus();
                return;
            }
            if (!checkOCRConfig(cbUseOcrM12.Checked, tbOcrM12.Text))
            {
                MessageBox.Show("OCR M12 config format error.", "Error");
                tbOcrM12.Focus();
                return;
            }
            //權限檢查
            using (var form = new FormConfirm("確認是否儲存 Recipe:" + tbRecipeID.Text))
            {
                var result = form.ShowDialog();
                if (result != DialogResult.OK)
                {
                    MessageBox.Show("Cancel.", "Notice");
                    return;
                }
            }
            //GUI 處理
            //gbRecipe.Enabled = false;
            btnCreateRecipe.Enabled = true;
            btnModifyRecipe.Enabled = true;
            btnDeleteRecipe.Enabled = true;
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

            recipe.is_use_aligner1 = cbUseA1.Checked;
            recipe.is_use_aligner2 = cbUseA2.Checked;
            recipe.is_use_ocr_ttl = cbUseOcrTTL.Checked;
            recipe.is_use_ocr_t7 = cbUseOcrT7.Checked;
            recipe.is_use_ocr_m12 = cbUseOcrM12.Checked;
            recipe.is_use_exchange = cbUseExchange.Checked;
            recipe.is_use_burnin = cbUseBurnIn.Checked;
            

            recipe.ocr_ttl_config = tbOcrTTL.Text;
            recipe.ocr_t7_config = tbOcrT7.Text;
            recipe.ocr_m12_config = tbOcrM12.Text;

            recipe.auto_fin_unclamp = "Y";//固定Y

            recipe.auto_get_constrict = cbAutoGetRule.Text;
            recipe.auto_proc_fin = cbAutoFin1.Text + cbAutoFin2.Text;
            recipe.auto_put_constrict = cbAutoPutRule.Text;
            //recipe.equip_id = tbEqpID.Text;
            recipe.ffu_rpm_close = tbFFUCloseRpm.Text;
            recipe.ffu_rpm_open = tbFFUOpenRpm.Text;
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
            recipe.robot2_speed = "20";//default tbR2Speed.Text.Equals("") ? "20" : Int32.Parse(tbR2Speed.Text).ToString();

            recipe.notch_angle = tbNotch_angle.Text.Equals("") ? "0" : Int32.Parse(tbNotch_angle.Text).ToString();
            recipe.motion_timeout = tbMotionTimeout.Text;

            recipe.is_use_l_arm = cbUseLArm.Checked;
            recipe.is_use_r_arm = cbUseRArm.Checked;
            recipe.is_use_double_arm = cbUserBothArm.Checked;
            recipe.ocr_check_Rule = cbOcrCheckRule.Text;
            recipe.get_slot_order = cbGetSlotOrder.Text;
            recipe.put_slot_order = cbPutSlotOrder.Text;

            Recipe.Set(recipe.recipe_id, recipe);

            string CurrentRecipe = SystemConfig.Get().CurrentRecipe;
            if (cbActive.Checked)//設定生效
            {
                SystemConfig config = SystemConfig.Get();
                config.CurrentRecipe = tbRecipeID.Text;
                config.Save();
                FormMainUpdate.UpdateRecipe(tbRecipeID.Text);
            }
            else if(CurrentRecipe.Equals(tbRecipeID.Text))//取消生效
            {
                SystemConfig config = SystemConfig.Get();
                config.CurrentRecipe = "default";
                config.Save();
                //update node config
                FormMainUpdate.UpdateRecipe("default");
            }
            //紀錄修改Log
            if (tbRecipeID.Enabled)
                Util.SanwaUtil.addActionLog("Recipe", "Create", Global.currentUser, "建立 Recipe:" + recipe.recipe_id);
            if (tbRecipeID.Enabled)
                Util.SanwaUtil.addActionLog("Recipe", "Modify", Global.currentUser, "修改 Recipe:" + recipe.recipe_id);

            refreshList();
            lblMode.Text = "瀏覽模式";
            MessageBox.Show("Execute successfully.", "Success");
        }

        private Boolean checkOCRConfig(bool isUse, string content)
        {
            Boolean result = false;
            if (!isUse)
            {
                result = true;
            }else if (content.Trim().Equals("")|| content.Contains("."))
            {
                result = false;
            }
            else
            {
                try
                {
                    string[] configs = content.Split(',');
                    foreach (string id in configs)
                    {
                        if (Int32.Parse(id) > 50)
                        {
                            result = false;
                            return result;
                        }
                    }
                    result = true;
                }
                catch (Exception e)
                {
                    result = false;
                }
            }
            return result;
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
            btnDeleteRecipe.Enabled = false;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            tbRecipeName.ReadOnly = true;
            tbRecipeID.ReadOnly = true;
            trvRecipe.Enabled = false;

            lblMode.Text = "編輯模式";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //gbRecipe.Enabled = false;
            btnCreateRecipe.Enabled = true;
            btnModifyRecipe.Enabled = true;
            btnDeleteRecipe.Enabled = true;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            tbRecipeName.ReadOnly = true;
            tbRecipeID.ReadOnly = true;
            trvRecipe.Enabled = true;
            lblMode.Text = "瀏覽模式";
        }

        private void FormRecipeSetting_Load(object sender, EventArgs e)
        {
            refreshList();
            lblMode.Text = "瀏覽模式";
        }

        private void refreshList()
        {
            trvRecipe.Nodes.Clear();
            DirectoryInfo d = new DirectoryInfo(@".\recipe");
            FileInfo[] Files = d.GetFiles("*.json"); //Getting Json files
            TreeNode firstNode = null;
            foreach (FileInfo file in Files)
            {
                string recipeId = file.Name.Replace(".json", "");
                TreeNode tmp = new TreeNode(recipeId);
                trvRecipe.Nodes.Add(tmp);
                if (firstNode == null)
                    firstNode = tmp;
            }
            trvRecipe.ExpandAll();
            if (firstNode != null)
                trvRecipe.SelectedNode = firstNode;
            //if(trvRecipe.Nodes.Count>0)
            //    trvRecipe.Nodes.
        }

        private void trvRecipe_AfterSelect(object sender, TreeViewEventArgs e)
        {
            updateInfo(trvRecipe.SelectedNode.Text);
            if (previousSelectedNode != null)
            {
                previousSelectedNode.BackColor = trvRecipe.BackColor;
                previousSelectedNode.ForeColor = trvRecipe.ForeColor;
            }
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
                //tbEqpID.Text = recipe.equip_id;

                tbMotionTimeout.Text = recipe.motion_timeout;
                tbFFUCloseRpm.Text = recipe.ffu_rpm_close;
                tbFFUOpenRpm.Text = recipe.ffu_rpm_open;
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

                cbOcrCheckRule.SelectedItem = recipe.ocr_check_Rule;
                cbGetSlotOrder.SelectedItem = recipe.get_slot_order;
                cbPutSlotOrder.SelectedItem = recipe.put_slot_order;

                tbRecipeID.Text = recipe.recipe_id;
                tbRecipeName.Text = recipe.recipe_name;
                tbR1Speed.Text = recipe.robot1_speed;
                //tbR2Speed.Text = recipe.robot2_speed;

                tbNotch_angle.Text = recipe.notch_angle;

                cbUseA1.Checked = recipe.is_use_aligner1;
                cbUseA2.Checked = recipe.is_use_aligner2;
                cbUseOcrTTL.Checked = recipe.is_use_ocr_ttl;
                cbUseOcrT7.Checked = recipe.is_use_ocr_t7;
                cbUseOcrM12.Checked = recipe.is_use_ocr_m12;

                cbUseLArm.Checked = recipe.is_use_l_arm;
                cbUseRArm.Checked = recipe.is_use_r_arm;
                cbUserBothArm.Checked = recipe.is_use_double_arm;

                tbOcrTTL.Text = recipe.ocr_ttl_config;
                tbOcrT7.Text = recipe.ocr_t7_config;
                tbOcrM12.Text = recipe.ocr_m12_config;
                cbUseExchange.Checked = recipe.is_use_exchange;
                cbUseBurnIn.Checked = recipe.is_use_burnin;

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
            if (Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar) || e.KeyChar.Equals(',') || e.KeyChar.Equals('.'))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void modeCheck(object sender, EventArgs e)
        {

            //if (!btnSave.Enabled)
            if (lblMode.Text.Equals("瀏覽模式"))
            {
                //DialogResult myResult = MessageBox.Show
                //("目前為瀏覽模式，是否要編輯?", "Change Mode?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //if (myResult == DialogResult.Yes)
                //{
                //    btnModifyRecipe_Click(null, null);
                //}
                btnModifyRecipe_Click(null, null);
            }
            else
            {
                return;
            }

        }

        private void trvRecipe_Validating(object sender, CancelEventArgs e)
        {
            trvRecipe.SelectedNode.BackColor = SystemColors.Highlight;
            trvRecipe.SelectedNode.ForeColor = Color.White;
            previousSelectedNode = trvRecipe.SelectedNode;
        }

        private void btnDeleteRecipe_Click(object sender, EventArgs e)
        {
            if (trvRecipe.SelectedNode == null)
            {
                MessageBox.Show("Please select a recipe first.", "Notice");
                return;
            }
            Recipe recipe = Recipe.Get(trvRecipe.SelectedNode.Text);
            if (recipe.recipe_id.Equals("default"))
            {
                MessageBox.Show("default recipe 不得刪除.", "Notice");
                return;
            }
            else if (SystemConfig.Get().CurrentRecipe.Equals(recipe.recipe_id))
            {
                MessageBox.Show("Recipe 使用中，請先切換 recipe 後再刪除.", "Notice");
                return;
            }
            using (var form = new FormConfirm("確認是否刪除 Recipe:" + recipe.recipe_id))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (Recipe.Delete(recipe.recipe_id))
                    {
                        refreshList();
                        Util.SanwaUtil.addActionLog("Recipe", "Delete", Global.currentUser, "刪除 Recipe:" + recipe.recipe_id);
                        MessageBox.Show("Delete completed.", "Success");
                    }
                    else
                    {
                        MessageBox.Show("Delete fail.", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("Cancel.", "Notice");
                }
            }
        }

        private void cbUseRArm_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Name.Equals("cbUseRArm"))
            {
                if (!cbUseRArm.Checked || !cbUseLArm.Checked)
                {
                    cbUserBothArm.Checked = false;
                }
            }
        }

        private void cbUseLArm_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Name.Equals("cbUseLArm"))
            {
                if (!cbUseRArm.Checked || !cbUseLArm.Checked)
                {
                    cbUserBothArm.Checked = false;
                }
            }
        }

        private void cbUserBothArm_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Name.Equals("cbUserBothArm"))
            {
                if (cbUserBothArm.Checked)
                {
                    cbUseRArm.Checked = true;
                    cbUseLArm.Checked = true;
                    cbUserBothArm.Checked = true;
                }
            }
        }

        private void cbUseExchange_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUseExchange.Checked)
            {
                cbUseRArm.Checked = true;
                cbUseLArm.Checked = true;
                cbUserBothArm.Checked = false;
                cbUseRArm.Enabled = false;
                cbUseLArm.Enabled = false;
                cbUserBothArm.Enabled = false;
            }
            else
            {
                cbUseRArm.Enabled = true;
                cbUseLArm.Enabled = true;
                cbUserBothArm.Enabled = true;
            }
        }
    }
}
