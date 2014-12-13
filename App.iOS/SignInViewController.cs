
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using App.Common;

namespace App.iOS
{
	public partial class SignInViewController : UIViewController
	{
		private LoginViewModel view_model;
		AppDelegate del;

		public SignInViewController () : base ("SignInViewController", null)
		{
			view_model = new LoginViewModel ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.View.BackgroundColor = UIColor.Blue;

			//remove navigation bar
			del = UIApplication.SharedApplication.Delegate as AppDelegate;
			del.NavigationController.SetNavigationBarHidden (true, false);

			btnLogin.TouchUpInside += Login;
			//bottomAlignConstraint.Constant += 100;
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		private async void Login (object s, EventArgs ev)
		{
			del.NavigationController.PushViewController (new StreamViewController(), true);
			return;

			if (string.IsNullOrEmpty (txtDisplayName.Text)) {
				var view = new UIAlertView ("Oops", "Please enter a username.", null, "Ok");
				view.Dismissed += (sender, e) => txtDisplayName.BecomeFirstResponder ();
				view.Show ();
				return;
			}
			if (string.IsNullOrEmpty (txtBetaKey.Text)) {
				var view = new UIAlertView ("Oops", "Please enter a password.", null, "Ok");
				view.Dismissed += (sender, e) => txtBetaKey.BecomeFirstResponder ();
				view.Show ();
				return;
			}

			txtDisplayName.ResignFirstResponder ();
			txtBetaKey.ResignFirstResponder ();

			//indicator.StartAnimating ();
			//indicator.StopAnimating ();

			view_model.UserDisplayName = txtDisplayName.Text;

			var create_user_successful = await view_model.CreateUser ();

			if (create_user_successful) {
				await AppGlobal.Current.CreateNewConnectionFromLogin ();
				Console.WriteLine ("Osheey");
				DismissViewController (true, null);
			}
		}
	}
}

