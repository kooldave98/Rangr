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

//See:below url
//http://stackoverflow.com/questions/8558732/listview-textview-with-linkmovementmethod-makes-list-item-unclickable
namespace CustomViews
{
    public class HyperTextView : TextView
    {
        public bool DontConsumeNonUrlClicks = true;
        public bool LinkHit;

        public override bool OnTouchEvent(MotionEvent e)
        {
            LinkHit = false;
            bool res = base.OnTouchEvent(e);
    
            if (DontConsumeNonUrlClicks)
                return LinkHit;
            return res;
        }

        public HyperTextView(Context context)
            : base(context)
        {
        }

        public HyperTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public HyperTextView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }
    
    }

    public class LocalLinkMovementMethod : LinkMovementMethod
    {
        public static LocalLinkMovementMethod sInstance;

    
        public static LocalLinkMovementMethod GetInstance()
        {
            if (sInstance == null)
                sInstance = new LocalLinkMovementMethod();
    
            return sInstance;
        }

        public override bool OnTouchEvent(TextView widget, Android.Text.ISpannable buffer, MotionEvent e)
        {
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
    
                var link = buffer.GetSpans(off, off, Java.Lang.Class.FromType(typeof(ClickableSpan)));
    
                if (link.Length > 0)
                {
                    if (action == MotionEventActions.Up)
                    {
                        ((ClickableSpan)link[0]).OnClick(widget);
                    }
                    else if (action == MotionEventActions.Down)
                    {
                        Selection.SetSelection(buffer, buffer.GetSpanStart((ClickableSpan)link[0]), buffer.GetSpanEnd((ClickableSpan)link[0]));
                    }
    
                    if (widget.GetType().Equals(typeof(HyperTextView)))
                    {
                        ((HyperTextView)widget).LinkHit = true;
                    }
                    return true;
                }
                else
                {
                    Selection.RemoveSelection(buffer);
                    base.OnTouchEvent(widget, buffer, e);
                    return false;
                }
            }
            return base.OnTouchEvent(widget, buffer, e);
        }
    }
        
}