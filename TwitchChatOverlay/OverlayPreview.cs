using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace TwitchChatOverlay
{
    class OverlayPreview : System.Windows.FrameworkElement
    {
        public double AbsoluteX
        {
            get
            {
                return (double)GetValue(AbsoluteXProperty);
            }

            set
            {
                SetValue(AbsoluteXProperty, value);
            }
        }

        public double AbsoluteY
        {
            get
            {
                return (double)GetValue(AbsoluteYProperty);
            }

            set
            {
                SetValue(AbsoluteYProperty, value);
            }
        }

        public double AbsoluteWidth
        {
            get
            {
                return (double)GetValue(AbsoluteWidthProperty);
            }

            set
            {
                SetValue(AbsoluteWidthProperty, value);
            }
        }

        public double AbsoluteHeight
        {
            get
            {
                return (double)GetValue(AbsoluteHeightProperty);
            }

            set
            {
                SetValue(AbsoluteHeightProperty, value);
            }
        }

        public static readonly DependencyProperty AbsoluteXProperty = DependencyProperty.Register("AbsoluteX", typeof(double), typeof(OverlayPreview), new FrameworkPropertyMetadata(10d, new PropertyChangedCallback(OnAbsolutePropertyChanged)));
        public static readonly DependencyProperty AbsoluteYProperty = DependencyProperty.Register("AbsoluteY", typeof(double), typeof(OverlayPreview), new FrameworkPropertyMetadata(10d, new PropertyChangedCallback(OnAbsolutePropertyChanged)));
        public static readonly DependencyProperty AbsoluteWidthProperty = DependencyProperty.Register("AbsoluteWidth", typeof(double), typeof(OverlayPreview), new FrameworkPropertyMetadata(300d, new PropertyChangedCallback(OnAbsolutePropertyChanged)));
        public static readonly DependencyProperty AbsoluteHeightProperty = DependencyProperty.Register("AbsoluteHeight", typeof(double), typeof(OverlayPreview), new FrameworkPropertyMetadata(400d, new PropertyChangedCallback(OnAbsolutePropertyChanged)));

        public double AnchorX;
        public double AnchorY;
        public double Scale;

        internal Rectangle InternalRect;

        public OverlayPreview()
        {
            InternalRect = new Rectangle();
        }

        public static void OnAbsolutePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as OverlayPreview).UpdateInternalRect();
        }

        public void UpdateInternalRect()
        {
            Canvas.SetLeft(InternalRect, AnchorX + AbsoluteX * Scale);
            Canvas.SetTop(InternalRect, AnchorY + AbsoluteY * Scale);
            InternalRect.Width = AbsoluteWidth * Scale;
            InternalRect.Height = AbsoluteHeight * Scale;
        }
    }
}
