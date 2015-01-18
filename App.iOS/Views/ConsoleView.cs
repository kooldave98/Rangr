using System;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace App.iOS
{
	[Register("ConsoleView")]
	public partial class ConsoleView : UIView
	{
		public UITextView Console;

		public ConsoleView(IntPtr h): base(h)
		{
		}

		public ConsoleView ()
		{
			var arr = NSBundle.MainBundle.LoadNib("ConsoleView", this, null);
			var v = Runtime.GetNSObject(arr.ValueAt(0)) as UIView;
			v.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
			AddSubview(v);

			Console = ConsoleTextView;
		}
	}
}