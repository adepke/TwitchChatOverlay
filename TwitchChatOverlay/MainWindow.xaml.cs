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

namespace TwitchChatOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Overlay OverlayHandle = new Overlay();
            OverlayHandle.Foreground = new SolidColorBrush(ChatColorPicker.SelectedColor ?? default(Color));
            OverlayHandle.TwitchChat.FontSize = Double.Parse(ChatSizeBox.Text);
            OverlayHandle.WindowStartupLocation = WindowStartupLocation.Manual;
            OverlayHandle.Left = Double.Parse(XBox.Text);
            OverlayHandle.Top = Double.Parse(YBox.Text);
            OverlayHandle.Width = Double.Parse(WidthBox.Text);
            OverlayHandle.Height = Double.Parse(HeightBox.Text);

            TwitchOverlayBot.Boot(ChannelBox.Text);

            OverlayHandle.Show();
            this.Hide();
        }
    }
}
