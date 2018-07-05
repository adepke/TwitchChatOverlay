/*
	Copyright (c) 2018 Andrew Depke
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchChatOverlay
{
    internal class TwitchOverlayBot : System.Windows.Application
    {
        [STAThread]
        public static void Main(string[] Args)
        {
            TwitchChatOverlay.App App = new TwitchChatOverlay.App();
            App.InitializeComponent();
            App.Run();
        }
    }
}
