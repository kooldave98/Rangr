using System;
using Android.Text.Style;
using Android.Content;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace App.Android
{
	public class AClickableSpan : ClickableSpan
	{
		Context context;

		public AClickableSpan (Context ctx) : base ()
		{
			context = ctx;
		}

		public override void UpdateDrawState (TextPaint ds)
		{
			ds.Color = new global::Android.Graphics.Color (ds.LinkColor);
			ds.SetARGB (255, 30, 144, 255);
		}


		public override void OnClick (View widget)
		{
			TextView tv = (TextView)widget;
			ISpanned s = (ISpanned)tv.TextFormatted;
			int start = s.GetSpanStart (this);
			int end = s.GetSpanEnd (this);
			String theWord = s.SubSequenceFormatted (start + 1, end).ToString ();

			context.StartActivity (SearchActivity.CreateIntent (context, theWord));
		}
	}
}

