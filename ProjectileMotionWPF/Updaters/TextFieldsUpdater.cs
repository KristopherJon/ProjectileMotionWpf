using System.Collections.Generic;
using System.Windows.Controls;

namespace ProjectileMotionWPF.Updaters
{
    public static class TextBoxUpdater
    {
        public static void UpdateTextBoxes(List<TextBox> textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Text = "1";
            }
        }
    }
}
