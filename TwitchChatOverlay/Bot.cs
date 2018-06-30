using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.IO;
using System.Collections.Concurrent;

namespace TwitchChatOverlay
{
    class Bot
    {
        public void Boot(ref ConcurrentQueue<string> Queue, ref List<string> IgnoredUserList, string Channel, ref EventWaitHandle FinishedLock)
        {
            StreamReader SecretConfigReader = new StreamReader(new FileStream(
#if DEBUG
            @"../../Secret.config"
#else
            @"Secret.config"
#endif
            , FileMode.Open, FileAccess.Read));

            XmlDocument SecretXml = new XmlDocument();
            string SecretXmlRaw = SecretConfigReader.ReadToEnd();
            SecretConfigReader.Close();

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

            StreamReader IgnoredUserListConfigReader = new StreamReader(new FileStream(
#if DEBUG
            @"../../IgnoredUserList.config"
#else
            @"IgnoredUserList.config"
#endif
            , FileMode.Open, FileAccess.Read));

            XmlDocument IgnoredUserListXml = new XmlDocument();
            string IgnoredUserListXmlRaw = IgnoredUserListConfigReader.ReadToEnd();
            IgnoredUserListConfigReader.Close();

            IgnoredUserListXml.LoadXml(IgnoredUserListXmlRaw);

            foreach (XmlNode Child in IgnoredUserListXml.ChildNodes)
            {
                if (Child.Name.Equals("configuration"))
                {
                    foreach (XmlNode Node in Child.ChildNodes)
                    {
                        if (Node.Name.Equals("user"))
                        {
                            IgnoredUserList.Add(Node.Attributes["name"].Value);
                        }
                    }
                }
            }

            FinishedLock.Set();

            IRC IRCClient = new IRC();
            IRCClient.Boot("irc.twitch.tv", 6667, Username, OAuthToken);
            IRCClient.JoinRoom(Channel);

            while (true)
            {
                string Message = IRCClient.ReadMessage();

                if (Message.Equals("PING :tmi.twitch.tv"))
                {
                    IRCClient.SendToIRC("PONG :tmi.twitch.tv");
                }

                else
                {
                    Queue.Enqueue(Message);
                }
            }
        }
    }
}
