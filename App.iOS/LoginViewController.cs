using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using App.Core.Portable.Device;
using App.Common.Shared;
using App.Core.Portable.Models;

namespace App.iOS
{
	public partial class LoginViewController : UIViewController
	{
		ISession _sessionInstance = Session.GetInstance();
		HttpRequest _httpRequest;
		User user;


		public LoginViewController ()
		{

			//Check if the user exists first before populating the view
			user = _sessionInstance.GetCurrentUser ();
			if (user != null) {
				DismissViewController (true, null);
			}

			InitView ();

			_httpRequest = new HttpRequest ();
		}


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
		}

		private async void Login ()
		{
			if (string.IsNullOrEmpty(username.Text))
			{
				var view = new UIAlertView("Oops", "Please enter a username.", null, "Ok");
				view.Dismissed += (sender, e) => username.BecomeFirstResponder();
				view.Show();
				return;
			}
			if (string.IsNullOrEmpty(password.Text))
			{
				var view = new UIAlertView("Oops", "Please enter a password.", null, "Ok");
				view.Dismissed += (sender, e) => password.BecomeFirstResponder();
				view.Show();
				return;
			}

			username.ResignFirstResponder ();
			password.ResignFirstResponder ();

			indicator.StartAnimating ();

			//indicator.StopAnimating ();


			var userID = await new UserRepository(_httpRequest).CreateUser(username.Text);
			user = await new UserRepository(_httpRequest).GetUserById(userID.ID);
			_sessionInstance.AddCurrentUser(user);

			DismissViewController (true, null);
		}
	}
}