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
	partial class PostCellView
	{
		[Outlet]
		MonoTouch.UIKit.UILabel TimeAgoView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView UserImageView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel UserNameView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel UserPostTextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (UserImageView != null) {
				UserImageView.Dispose ();
				UserImageView = null;
			}

			if (UserNameView != null) {
				UserNameView.Dispose ();
				UserNameView = null;
			}

			if (UserPostTextView != null) {
				UserPostTextView.Dispose ();
				UserPostTextView = null;
			}

			if (TimeAgoView != null) {
				TimeAgoView.Dispose ();
				TimeAgoView = null;
			}
		}
	}
}
