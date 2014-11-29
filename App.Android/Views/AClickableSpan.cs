using System;
using Android.Text.Style;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace CustomViews
{
	public class AClickableSpan : ClickableSpan
	{
		private Action<string> on_click_handler;

		public AClickableSpan (Action<string> OnClickHandler) : base ()
		{
			on_click_handler = OnClickHandler;
		}

		public override void UpdateDrawState (TextPaint ds)
		{
			ds.Color = new global::Android.Graphics.Color (ds.LinkColor);
			ds.SetARGB (255, 30, 144, 255);
		}

		public override void OnClick (View widget)
		{
			var tv = (TextView)widget;
			var s = (ISpanned)tv.TextFormatted;
			var start = s.GetSpanStart (this);
			var end = s.GetSpanEnd (this);
			var span_word = s.SubSequenceFormatted (start + 1, end).ToString ();

			on_click_handler (span_word);
		}
	}
}

