
using System;
using System.Drawing;

using Foundation;
using UIKit;
using CoreGraphics;

namespace ios_ui_lib
{
    public class UIMultiLineLabelCell : UILabel
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            base.PreferredMaxLayoutWidth = this.Bounds.Width;

            base.LayoutSubviews();
        }
    }
}

