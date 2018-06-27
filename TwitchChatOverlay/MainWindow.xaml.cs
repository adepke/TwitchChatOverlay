using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Collections.Concurrent;
using System.Timers;
using System.Windows.Threading;

namespace TwitchChatOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Overlay OverlayHandle = new Overlay();

        public ConcurrentQueue<string> MessageQueue;
        public List<string> IgnoredUserList;

        public SolidColorBrush Brush;
        public SolidColorBrush OutlineBrush;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ProcessMessages(object sender, EventArgs e)
        {
            if (MessageQueue.Count > 0)
            {
                string NewMessage;

                if (MessageQueue.TryDequeue(out NewMessage))
                {
                    if (NewMessage.Contains("PRIVMSG"))
                    {
                        string User = NewMessage.Substring(1, NewMessage.IndexOf('!') - 1);
                        string Message = NewMessage.Substring(NewMessage.LastIndexOf(':') + 1);

                        foreach (string IgnoredUser in IgnoredUserList)
                        {
                            if (User.Equals(IgnoredUser, StringComparison.CurrentCultureIgnoreCase))
                            {
                                // We Are Ignoring This User, Return Out.

                                return;
                            }
                        }

                        OverlayHandle.AddMessage(User + ": " + Message, Brush, OutlineBrush);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Brush = new SolidColorBrush(ChatColorPicker.SelectedColor ?? default(Color));
            OutlineBrush = new SolidColorBrush(ChatOutlineColorPicker.SelectedColor ?? default(Color));
            OverlayHandle.ChatFontSize = Double.Parse(ChatSizeBox.Text);
            OverlayHandle.ChatBold = BoldButton.IsChecked ?? false;
            OverlayHandle.WindowStartupLocation = WindowStartupLocation.Manual;
            OverlayHandle.Left = Double.Parse(XBox.Text);
            OverlayHandle.Top = Double.Parse(YBox.Text);
            OverlayHandle.Width = Double.Parse(WidthBox.Text);
            OverlayHandle.Height = Double.Parse(HeightBox.Text);

            OverlayHandle.Show();
            this.Hide();

            MessageQueue = new ConcurrentQueue<string>();
            IgnoredUserList = new List<string>();

            string Channel = ChannelBox.Text.ToLower();

            EventWaitHandle BootFinishedLock = new EventWaitHandle(false, EventResetMode.ManualReset);

            Thread IRCThread = new Thread(() =>
            {
                Bot BotHandle = new Bot();
                BotHandle.Boot(ref MessageQueue, ref IgnoredUserList, Channel, ref BootFinishedLock);
            });

            IRCThread.Start();

            BootFinishedLock.WaitOne();

            DispatcherTimer ProcessMessagesTimer = new DispatcherTimer();
            ProcessMessagesTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            ProcessMessagesTimer.Tick += ProcessMessages;
            ProcessMessagesTimer.IsEnabled = true;
        }
    }
}
