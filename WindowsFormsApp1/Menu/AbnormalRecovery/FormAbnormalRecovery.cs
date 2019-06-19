using System;


namespace Adam.Menu.Communications
{
    public partial class FormAbnormalRecovery : Adam.Menu.FormFrame
    {
        public FormAbnormalRecovery()
        {
            InitializeComponent();
        }

      

        private void FormCommunications_Load(object sender, EventArgs e)
        {
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DetectPresense_panel.SendToBack();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadPortRemapping_panel.SendToBack();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DetectArmExtend_panel.SendToBack();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DetectedAlignerPresense_panel.SendToBack();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DetectedRobotPresense_panel.SendToBack();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ORG_panel.SendToBack();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Finished_panel.SendToBack();
        }
    }
}
