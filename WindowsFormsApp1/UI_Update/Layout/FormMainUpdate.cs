using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adam.UI_Update.Layout
{
    class FormMainUpdate
    {
        static ILog logger = LogManager.GetLogger(typeof(FormMainUpdate));
        delegate void UpdateValue(string Value);

        public static void UpdateRecipe(string Value)
        {
            try
            {
                Form form = Application.OpenForms["FormMain"];
                if (form == null)
                    return;
                Label Recipe = form.Controls.Find("CurrentRecipe_lb", true).FirstOrDefault() as Label;

                if (Recipe == null)
                    return;
                if (Recipe.InvokeRequired)
                {
                    UpdateValue ph = new UpdateValue(UpdateRecipe);
                    Recipe.BeginInvoke(ph, Value);
                }
                else
                {
                    Recipe.Text = Value;
                }
            }
            catch (Exception e)
            {
                logger.Error("UpdateRecipe: Update fail. err:" + e.StackTrace);
            }
        }
    }
}
