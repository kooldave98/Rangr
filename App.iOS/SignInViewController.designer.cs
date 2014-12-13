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
	[Register ("SignInViewController")]
	partial class SignInViewController
	{
		[Outlet]
		MonoTouch.UIKit.NSLayoutConstraint bottomAlignConstraint { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnLogin { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtBetaKey { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtDisplayName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnLogin != null) {
				btnLogin.Dispose ();
				btnLogin = null;
			}

			if (bottomAlignConstraint != null) {
				bottomAlignConstraint.Dispose ();
				bottomAlignConstraint = null;
			}

			if (txtBetaKey != null) {
				txtBetaKey.Dispose ();
				txtBetaKey = null;
			}

			if (txtDisplayName != null) {
				txtDisplayName.Dispose ();
				txtDisplayName = null;
			}
		}
	}
}
