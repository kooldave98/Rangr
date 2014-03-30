using System;
using System.Drawing;
using App.Common.Shared;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace App.iOS
{
	public partial class LoginViewController : UIViewController
	{
		private ISession _sessionInstance;
		private IHttpRequest _httpRequest;
		private Users UserServices;
		private Action init_callback;

		public LoginViewController (Action the_init_callback)
		{
			_sessionInstance = Session.GetInstance (PersistentStorage.Current);
			_httpRequest = HttpRequest.Current;
			UserServices = new Users (_httpRequest);
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


			var userID = await UserServices.Create (username.Text);
			var user = await UserServices.Get (userID.user_id.ToString());
			_sessionInstance.PersistCurrentUser (user);

			init_callback ();

			DismissViewController (true, null);
		}
	}
}