using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public bool ChatBold;
        public double OutlineThickness;

        public Overlay()
        {
            InitializeComponent();
        }

        public void AddMessage(string FormattedMessage, SolidColorBrush Brush, SolidColorBrush OutlineBrush)
        {
            var NewMessage = new OutlinedTextBlock
            {
                Text = FormattedMessage,
                FontSize = ChatFontSize,
                Fill = Brush,
                Stroke = OutlineBrush,
                StrokeThickness = OutlineThickness,
                TextWrapping = TextWrapping.WrapWithOverflow,
                FontWeight = ChatBold ? FontWeights.Bold : FontWeights.Regular,
            };

            NewMessage.OwnerOverlay = this;

            ChatStack.Children.Add(NewMessage);
        }

        public void TrimChatStack()
        {
            while (true)
            {
                if (ChatStack.Children.Count > 1)
                {
                    double StackHeight = 0d;

                    foreach (UIElement Element in ChatStack.Children)
                    {
                        StackHeight += (Element as OutlinedTextBlock).ActualHeight;
                    }

                    if (StackHeight > Height)
                    {
                        ChatStack.Children.RemoveAt(0);  // Stack Too High, Remove the Top Message and Recheck Height.
                    }

                    else
                    {
                        break;  // Message Stack Fits on Screen, Break.
                    }
                }

                else
                {
                    break;  // Only 1 Message in the Stack, Break.
                }
            }
        }

        private void Application_Exitting(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
