using System;
using System.Windows.Forms;
using TransferControl.Config;

namespace Adam.Menu.SystemSetting
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        private void btnDeviceManager_Click(object sender, EventArgs e)
        {
            FormDeviceManager form = new FormDeviceManager();
            AddForm(form);
        }

        private void AddForm(Form form)
        {
            foreach (Control foo in pnlSetting.Controls)
            {
                if(foo is Form)
                {
                    ((Form)foo).Close();
                }
                pnlSetting.Controls.Remove(foo);
                foo.Dispose();
            }
            if (form != null)
            {
                form.TopLevel = false;
                form.AutoScroll = true;
                pnlSetting.Controls.Add(form);
                form.Show();
            }
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            AddForm(null);
        }

        private void btnAccountSetting_Click(object sender, EventArgs e)
        {
            FormAccountSetting form = new FormAccountSetting();
            AddForm(form);
        }

        private void btnCommandScript_Click(object sender, EventArgs e)
        {
            FormCommandScript form = new FormCommandScript();
            AddForm(form);
        }

        private void btnOnlineSettings_Click(object sender, EventArgs e)
        {
            FormOnlineSettings form = new FormOnlineSettings();
            AddForm(form);
        }

        private void btnSignalTtower_Click(object sender, EventArgs e)
        {
            FormSignalTower form = new FormSignalTower();
            AddForm(form);
        }

        private void btnSECSSetting_Click(object sender, EventArgs e)
        {
            FormSECSSet form = new FormSECSSet();
            AddForm(form);
        }

        private void btnAlarmEventSet_Click(object sender, EventArgs e)
        {
            FormAlarmEventSet form = new FormAlarmEventSet();
            AddForm(form);
        }

        private void btnCodeSetting_Click(object sender, EventArgs e)
        {
            
        }

        private void tbpDIOSetting_Click(object sender, EventArgs e)
        {
            FormDIOSetting form = new FormDIOSetting();
            AddForm(form);
        }

        private void btnRecipeSetting_Click(object sender, EventArgs e)
        {
            FormRecipeSetting form = new FormRecipeSetting();
            AddForm(form);
        }

        private void btnDIOSetting_Click(object sender, EventArgs e)
        {
            FormSimpleSetting form = new FormSimpleSetting(Config_Type.DIO);
            AddForm(form);
        }

        private void btnNodeSetting_Click(object sender, EventArgs e)
        {
            FormSimpleSetting form = new FormSimpleSetting(Config_Type.NODE);
            AddForm(form);
            Recipe recipe = new Recipe();
            Recipe.Set("default", recipe);
        }

        private void btnSysConfig_Click(object sender, EventArgs e)
        {
            FormSysConfig form = new FormSysConfig();
            AddForm(form);
        }
    }
}
