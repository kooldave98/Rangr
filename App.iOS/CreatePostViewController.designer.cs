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
	[Register ("CreatePostViewController")]
	partial class CreatePostViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton CreatePostBtn { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextView NewPostTbx { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (NewPostTbx != null) {
				NewPostTbx.Dispose ();
				NewPostTbx = null;
			}

			if (CreatePostBtn != null) {
				CreatePostBtn.Dispose ();
				CreatePostBtn = null;
			}
		}
	}
}
