using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace TwitchChatOverlay
{
    class ResizableThumb : Thumb
    {
        private ContentControl Owner;

        public ResizableThumb()
        {
            DragDelta += new DragDeltaEventHandler(OnDragDelta);
        }

        public void BindOwner(ContentControl InOwner)
        {
            Owner = InOwner;
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Owner != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange,
                            Owner.ActualHeight - Owner.MinHeight);
                        Owner.Height -= deltaVertical;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange,
                            Owner.ActualHeight - Owner.MinHeight);
                        Canvas.SetTop(Owner, Canvas.GetTop(Owner) + deltaVertical);
                        Owner.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange,
                            Owner.ActualWidth - Owner.MinWidth);
                        Canvas.SetLeft(Owner, Canvas.GetLeft(Owner) + deltaHorizontal);
                        Owner.Width -= deltaHorizontal;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange,
                            Owner.ActualWidth - Owner.MinWidth);
                        Owner.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
            }

            e.Handled = true;
        }
    }
}
