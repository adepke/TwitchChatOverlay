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
using System.Windows.Shapes;

namespace TwitchChatOverlay
{
    /// <summary>
    /// Interaction logic for Overlay.xaml
    /// </summary>
    public partial class Overlay : Window
    {
        public double ChatFontSize;

        public Overlay()
        {
            InitializeComponent();
        }

        public void AddMessage(string FormattedMessage, SolidColorBrush Brush)
        {
            int Index = (int)(Height * 0.3) / (int)ChatFontSize;

            if (ChatStack.Children.Count > Index)
            {
                ChatStack.Children.RemoveAt(0);
            }

            ChatStack.Children.Add(new Label
            {
                Content = FormattedMessage,
                FontSize = ChatFontSize,
                Foreground = Brush,
            });
        }
    }
}
