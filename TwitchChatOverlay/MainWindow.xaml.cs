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
using System.ComponentModel;

namespace TwitchChatOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Thread IRCThread;

        private Overlay OverlayHandle = new Overlay();

        public ConcurrentQueue<string> MessageQueue;
        public List<string> IgnoredUserList;

        public SolidColorBrush Brush;
        public SolidColorBrush OutlineBrush;

        public MainWindow()
        {
            InitializeComponent();

            BuildPreview();
        }

        private void BuildPreview()
        {
            double CenterPoint = Canvas.GetLeft(Screen) + Screen.Width / 2d;

            double Ratio = WpfScreenHelper.Screen.PrimaryScreen.Bounds.Width / WpfScreenHelper.Screen.PrimaryScreen.Bounds.Height;

            Screen.Width = Screen.Height * Ratio;
            Canvas.SetLeft(Screen, CenterPoint - Screen.Width / 2d);

            var OverlayPreviewBindX = new Binding("Text");
            OverlayPreviewBindX.Source = XBox;
            var OverlayPreviewBindY = new Binding("Text");
            OverlayPreviewBindY.Source = YBox;
            var OverlayPreviewBindWidth = new Binding("Text");
            OverlayPreviewBindWidth.Source = WidthBox;
            var OverlayPreviewBindHeight = new Binding("Text");
            OverlayPreviewBindHeight.Source = HeightBox;

            var _OverlayPreview = new OverlayPreview();
            BaseCanvas.Children.Add(_OverlayPreview);
            BaseCanvas.Children.Add(_OverlayPreview.InternalRect);
            _OverlayPreview.AnchorX = Canvas.GetLeft(Screen);
            _OverlayPreview.AnchorY = Canvas.GetTop(Screen);
            _OverlayPreview.Scale = Screen.Width / WpfScreenHelper.Screen.PrimaryScreen.Bounds.Width;
            _OverlayPreview.SetBinding(OverlayPreview.AbsoluteXProperty, OverlayPreviewBindX);
            _OverlayPreview.SetBinding(OverlayPreview.AbsoluteYProperty, OverlayPreviewBindY);
            _OverlayPreview.SetBinding(OverlayPreview.AbsoluteWidthProperty, OverlayPreviewBindWidth);
            _OverlayPreview.SetBinding(OverlayPreview.AbsoluteHeightProperty, OverlayPreviewBindHeight);
            _OverlayPreview.InternalRect.Fill = new SolidColorBrush(Colors.Gray);

            _OverlayPreview.UpdateInternalRect();

            PreviewLabel.Content = WpfScreenHelper.Screen.PrimaryScreen.Bounds.Width.ToString() + " x " + WpfScreenHelper.Screen.PrimaryScreen.Bounds.Height.ToString() + " Preview";

            var TextPreviewBindFill = new Binding("SelectedColor");
            TextPreviewBindFill.Source = ChatColorPicker;
            TextPreviewBindFill.Converter = new NullableColorSolidBrushConverter();
            var TextPreviewBindStroke = new Binding("SelectedColor");
            TextPreviewBindStroke.Source = ChatOutlineColorPicker;
            TextPreviewBindStroke.Converter = new NullableColorSolidBrushConverter();
            var TextPreviewBindSize = new Binding("Text");
            TextPreviewBindSize.Source = ChatSizeBox;
            var TextPreviewBindStrokeSize = new Binding("Text");
            TextPreviewBindStrokeSize.Source = ChatOutlineThicknessBox;
            var TextPreviewBindBold = new Binding("IsChecked");
            TextPreviewBindBold.Source = BoldButton;
            TextPreviewBindBold.Converter = new NullableBooleanFontWeightConverter();

            TextPreview.SetBinding(OutlinedTextBlock.FillProperty, TextPreviewBindFill);
            TextPreview.SetBinding(OutlinedTextBlock.StrokeProperty, TextPreviewBindStroke);
            TextPreview.SetBinding(OutlinedTextBlock.FontSizeProperty, TextPreviewBindSize);
            TextPreview.SetBinding(OutlinedTextBlock.StrokeThicknessProperty, TextPreviewBindStrokeSize);
            TextPreview.SetBinding(OutlinedTextBlock.FontWeightProperty, TextPreviewBindBold);
        }

        private void ProcessMessages(object sender, EventArgs e)
        {
            if (MessageQueue.Count > 0)
            {
                string NewMessage;

                if (MessageQueue.TryDequeue(out NewMessage))
                {
                    if (NewMessage != null)
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Brush = new SolidColorBrush(ChatColorPicker.SelectedColor ?? default(Color));
            OutlineBrush = new SolidColorBrush(ChatOutlineColorPicker.SelectedColor ?? default(Color));
            OverlayHandle.ChatFontSize = Double.Parse(ChatSizeBox.Text);
            OverlayHandle.ChatBold = BoldButton.IsChecked ?? false;
            OverlayHandle.OutlineThickness = Double.Parse(ChatOutlineThicknessBox.Text);
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

            IRCThread = new Thread(() =>
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

        private void Application_Exitting(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
