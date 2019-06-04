using Adam.UI_Update.Layout;
using SANWA;
using SANWA.Utility;
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
    public partial class FormSysConfig : Form
    {
        public TreeNode previousSelectedNode = null;
        public FormSysConfig()
        {
            InitializeComponent();
        }

        private void FormSysConfig_Load(object sender, EventArgs e)
        {
            //init recipe combobox
            cbRecipe.Items.Clear(); DirectoryInfo d = new DirectoryInfo(@".\recipe");
            FileInfo[] Files = d.GetFiles("*.json"); //Getting Json files
            foreach (FileInfo file in Files)
            {
                string recipeId = file.Name.Replace(".json", "");
                cbRecipe.Items.Add(recipeId);
            }
            //set gui data
            SystemConfig config = SystemConfig.Get();
            tbConnString.Text = config.DBConnectionString;
            tbEqpId.Text = config.EquipmentID;
            tbOcr1Exe.Text = config.OCR1ExePath;
            tbOcr1ImgBak.Text = config.OCR1ImgToJpgPath;
            tbOcr1ImgSrc.Text = config.OCR1ImgSourcePath;
            tbSysMode.Text = config.SystemMode;
            tbTxfLogPath.Text = config.FoupTxfLogPath;
            cbChkBypass.Checked = config.SaftyCheckByPass;
            cbFakeData.Checked = config.FakeData;
            cbRecipe.SelectedItem = config.CurrentRecipe;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SystemConfig config = SystemConfig.Get();
            //目前只開放查詢以下資料，其餘
            config.EquipmentID = tbEqpId.Text;
            config.OCR1ImgToJpgPath = tbOcr1ImgBak.Text;
            config.FoupTxfLogPath = tbTxfLogPath.Text;
            config.CurrentRecipe = cbRecipe.SelectedItem.ToString();

            config.Save();
            MessageBox.Show("Update Completed.","Success");
        }
    }
}
