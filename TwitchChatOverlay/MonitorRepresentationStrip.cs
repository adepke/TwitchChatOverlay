using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TwitchChatOverlay
{
    class MonitorRepresentationStrip
    {
        public Canvas Build(double ReductionScalar)
        {
            var InternalCanvas = new Canvas();
            InternalCanvas.IsHitTestVisible = false;

            double NextStartingPosition = 0d;
            double MaxHeight = 0d;

            foreach (var Monitor in WpfScreenHelper.Screen.AllScreens)
            {
                MaxHeight = Monitor.Bounds.Height > MaxHeight ? Monitor.Bounds.Height : MaxHeight;

                Rectangle MonitorRect = new Rectangle();
                MonitorRect.IsHitTestVisible = false;

                InternalCanvas.Children.Add(MonitorRect);

                Canvas.SetTop(MonitorRect, Monitor.Bounds.Height * ReductionScalar / 2d);
                Canvas.SetLeft(MonitorRect, NextStartingPosition);
                MonitorRect.Width = Monitor.Bounds.Width * ReductionScalar;
                MonitorRect.Height = Monitor.Bounds.Height * ReductionScalar;

                MonitorRect.Fill = new SolidColorBrush(Colors.DarkGray);
                MonitorRect.Opacity = 0.4d;

                // 10 Pixel Spacers Between Monitors.
                NextStartingPosition += MonitorRect.Width + 10d;
            }

            InternalCanvas.Width = NextStartingPosition - 10d;
            InternalCanvas.Height = MaxHeight * ReductionScalar;

            return InternalCanvas;
        }
    }
}
