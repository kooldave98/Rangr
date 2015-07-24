// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace ios_ui_lib
{
	[Register ("TextDisplayViewController")]
	partial class TextDisplayViewController
	{
		[Outlet]
		UIKit.UILabel DisplayLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DisplayLabel != null) {
				DisplayLabel.Dispose ();
				DisplayLabel = null;
			}
		}
	}
}
