// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace App.iOS
{
	partial class ConsoleView
	{
		[Outlet]
		MonoTouch.UIKit.UITextView ConsoleTextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ConsoleTextView != null) {
				ConsoleTextView.Dispose ();
				ConsoleTextView = null;
			}
		}
	}
}
