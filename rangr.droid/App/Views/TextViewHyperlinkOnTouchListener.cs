using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;

namespace CustomViews
{
    public class TextViewHyperlinkOnTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        public bool OnTouch(View v, MotionEvent e)
        {
            var ret = false;

            TextView widget = (TextView)v;

            var buffer = (ISpanned)widget.TextFormatted;

            MotionEventActions action = e.Action;

            if (action == MotionEventActions.Up || action == MotionEventActions.Down)
            {
                int x = (int)e.GetX();
                int y = (int)e.GetY();

                x -= widget.PaddingLeft;
                y -= widget.PaddingTop;

                x += widget.ScrollX;
                y += widget.ScrollY;

                Layout layout = widget.Layout;
                int line = layout.GetLineForVertical(y);
                int off = layout.GetOffsetForHorizontal(line, x);


                var link = buffer.GetSpans(off, off, Java.Lang.Class.FromType(typeof(AClickableSpan)));

                if (link.Length != 0)
                {
                    if (action == MotionEventActions.Up)
                    {
                        ((AClickableSpan)link[0]).OnClick(widget);
                    }
                    ret = true;
                }
            }
            return ret;
        }

    }

}

