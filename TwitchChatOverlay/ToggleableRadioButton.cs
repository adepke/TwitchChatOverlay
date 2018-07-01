using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TwitchChatOverlay
{
    class ToggleableRadioButton : RadioButton
    {
        protected override void OnClick()
        {
            if (IsChecked == true)
            {
                IsChecked = false;
            }

            else
            {
                base.OnClick();
            }
        }
    }
}
