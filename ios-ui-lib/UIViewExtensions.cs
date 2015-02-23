#if __IOS__

using System;
using System.Drawing;

using Foundation;
using UIKit;
using CoreGraphics;
using general_solid_lib;

namespace ios_ui_lib
{
    //http://blog.adamkemp.com/2014/12/ios-layout-gotchas-redux.html
    public static class UIViewExtensions
    {
        public static CGSize get_text_size_with_font(this string text, UIFont font) 
        {
            return new NSString(text).StringSize(UIFont.SystemFontOfSize(font.PointSize));
        }

        public static void apply_simple_border(this UIView view, CGColor color = null)
        {
            view.Layer.BorderWidth = 1;
            view.Layer.BorderColor = color ?? UIColor.Black.CGColor;
        }

        public static void CenterXInParent(this UIView view)
        {
            var parent = view.Superview;
            Guard.IsNotNull(parent, "parent");

            var parentSize = parent.Bounds.Size;
            var viewSize = view.Bounds.Size;

            var f = new CGRect((parentSize.Width - viewSize.Width) / 2, 
                                view.Frame.Y, 
                                viewSize.Width, 
                                viewSize.Height);
            view.Frame = f;
            view.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
            //parent.AddSubview(view);
        }

        public static void CenterInParent(this UIView view)
        {
            var parent = view.Superview;

            Guard.IsNotNull(parent, "Cannot center a view in its parent unless it has a parent");

            var parentSize = parent.Bounds.Size;
            view.SafeSetCenter(new CGPoint(parentSize.Width / 2, parentSize.Height / 2));
        }

        public static void SafeSetCenter(this UIView view, CGPoint center)
        {
            var size = view.Bounds.Size;
            center = center.Floor();
            if ((int)size.Width % 2 != 0)
            {
                center.X += 0.5f;
            }
            if ((int)size.Height % 2 != 0)
            {
                center.Y += 0.5f;
            }

            view.Center = center;
        }

        public static CGPoint Floor(this CGPoint center)
        {
            return new CGPoint((nfloat)Math.Floor(center.X), (nfloat)Math.Floor(center.Y));
        }
    }
}

#endif