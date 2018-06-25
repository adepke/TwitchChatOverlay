using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TwitchChatOverlay
{
    class TwitchOverlayBot
    {
        public static void Main(string[] Args)
        {
            Thread WindowThread = new Thread(() =>
            {
                MainWindow Window = new MainWindow();
                Window.Show();
            });

            WindowThread.Start();

            IRC IRCClient = new IRC();
            IRCClient.Boot("irc.twitch.tv", 6667);
        }
    }
}
