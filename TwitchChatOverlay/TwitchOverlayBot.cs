using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.IO;

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

        public static void Boot(string Channel)
        {
            StreamReader ConfigReader = new StreamReader(new FileStream(
#if DEBUG
            @"../../Secret.config"
#else
            @"Secret.config"
#endif
            , FileMode.Open, FileAccess.Read));

            XmlDocument SecretXml = new XmlDocument();
            string SecretXmlRaw = ConfigReader.ReadToEnd();
            ConfigReader.Close();

            SecretXml.LoadXml(SecretXmlRaw);

            string Username = "";
            string OAuthToken = "";

            foreach (XmlNode Child in SecretXml.ChildNodes)
            {
                if (Child.Name.Equals("configuration"))
                {
                    foreach (XmlNode Node in Child.ChildNodes)
                    {
                        if (Node.Name.Equals("account"))
                        {
                            Username = Node.Attributes["username"].Value;
                            OAuthToken = Node.Attributes["oauthtoken"].Value;
                        }
                    }
                }
            }

            IRC IRCClient = new IRC();
            IRCClient.Boot("irc.twitch.tv", 6667, Username, OAuthToken);
            IRCClient.JoinRoom(Channel);

            IRCClient.SendMessage("/me Overlay IRC Warmup");
        }
    }
}
