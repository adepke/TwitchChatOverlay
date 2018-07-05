/*
	Copyright (c) 2018 Andrew Depke
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TwitchChatOverlay
{
    class MovableThumb : Thumb
    {
        private ContentControl Owner;

        public MovableThumb()
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
                Canvas.SetLeft(Owner, Canvas.GetLeft(Owner) + e.HorizontalChange);
                Canvas.SetTop(Owner, Canvas.GetTop(Owner) + e.VerticalChange);
            }
        }
    }
}
