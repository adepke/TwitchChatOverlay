using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace TwitchChatOverlay
{
    class IRC
    {
        private TcpClient TCPHandle;
        private StreamReader InStream;
        private StreamWriter OutStream;

        private string Username;
        private string CurrentChannel;

        public void Boot(string HostName, int Port, string InUsername, string OAuthToken)
        {
            Username = InUsername;

            TCPHandle = new TcpClient(HostName, Port);
            InStream = new StreamReader(TCPHandle.GetStream());
            OutStream = new StreamWriter(TCPHandle.GetStream());

            SendToIRC("PASS " + OAuthToken);
            SendToIRC("NICK " + Username);
            SendToIRC("USER " + Username);
        }

        public void SendToIRC(string Message)
        {
            OutStream.WriteLine(Message);
            OutStream.Flush();
        }

        public void JoinRoom(string Channel)
        {
            SendToIRC("JOIN #" + Channel);

            CurrentChannel = Channel;
        }

        public void SendMessage(string Message)
        {
            SendToIRC(":" + Username + "!" + Username + "@" + Username + ".tmi.twitch.tv PRIVMSG #" + CurrentChannel + " :" + Message);
        }

        public string ReadMessage()
        {
            return InStream.ReadLine();
        }
    }
}
