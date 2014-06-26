using System;
using System.Drawing;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using App.Common;

namespace App.iOS
{
	public partial class LoginViewController : UIViewController
	{
		private LoginViewModel view_model;

		private Action init_callback;

		public LoginViewController (Action the_init_callback)
		{
			view_model = new LoginViewModel ();

			init_callback = the_init_callback;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);			 

			UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;

			InitView ();		

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}

		private async void Login ()
		{
			if (string.IsNullOrEmpty (username.Text)) {
				var view = new UIAlertView ("Oops", "Please enter a username.", null, "Ok");
				view.Dismissed += (sender, e) => username.BecomeFirstResponder ();
				view.Show ();
				return;
			}
			if (string.IsNullOrEmpty (password.Text)) {
				var view = new UIAlertView ("Oops", "Please enter a password.", null, "Ok");
				view.Dismissed += (sender, e) => password.BecomeFirstResponder ();
				view.Show ();
				return;
			}

			username.ResignFirstResponder ();
			password.ResignFirstResponder ();

			indicator.StartAnimating ();
			//indicator.StopAnimating ();

			view_model.UserDisplayName = username.Text;

			await view_model.CreateUser ();

			init_callback ();

			DismissViewController (true, null);
		}
	}
}