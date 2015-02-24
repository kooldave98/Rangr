using System;
using UIKit;
using CoreGraphics;

namespace ios_ui_lib
{
    public class UIVLabel : UILabel
    {
        private UITextVerticalAlignment _textVerticalAlignment;

        public UIVLabel()
        {
            VerticalAlignment = UITextVerticalAlignment.Top;
        }

        public UITextVerticalAlignment VerticalAlignment
        {
            get
            {
                return _textVerticalAlignment;
            }
            set
            {
                _textVerticalAlignment = value;
                SetNeedsDisplay();
            }
        }

        public override void DrawText(CGRect rect)
        {
            var bound = TextRectForBounds(rect, Lines);
            base.DrawText(bound);
        }

        public override CGRect TextRectForBounds(CGRect bounds, nint numberOfLines)
        {
            var rect = base.TextRectForBounds(bounds, numberOfLines);
            CGRect resultRect;
            switch (VerticalAlignment)
            {
                case UITextVerticalAlignment.Top:
                    resultRect = new CGRect(bounds.X, bounds.Y, rect.Size.Width, rect.Size.Height);
                    break;
                case UITextVerticalAlignment.Middle:
                    resultRect = new CGRect(bounds.X,
                        bounds.Y + (bounds.Size.Height - rect.Size.Height)/2,
                        rect.Size.Width, rect.Size.Height);
                    break;
                case UITextVerticalAlignment.Bottom:
                    resultRect = new CGRect(bounds.X,
                        bounds.Y + (bounds.Size.Height - rect.Size.Height),
                        rect.Size.Width, rect.Size.Height);
                    break;

                default:
                    resultRect = bounds;
                    break;
            }

            return resultRect;
        }
    }

    public enum UITextVerticalAlignment
    {
        Top = 0, // default
        Middle,
        Bottom
    }


}

