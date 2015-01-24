using System;
using CoreGraphics;
using Foundation;
using UIKit;
using App.Common;

namespace App.iOS
{
    public partial class LoginViewController : UIViewController
    {
        public event Action LoginSucceeded = delegate {};

        private LoginViewModel view_model;

        public LoginViewController()
        {
            view_model = new LoginViewModel();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);			 

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;

            InitView();		

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //del.NavigationController.SetNavigationBarHidden(true, false);
        }

        private async void Login()
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

            username.ResignFirstResponder();
            password.ResignFirstResponder();

            indicator.StartAnimating();
            //indicator.StopAnimating ();

            view_model.UserDisplayName = username.Text;

            var create_user_successful = await view_model.CreateUser();

            if (create_user_successful)
            {
                await AppGlobal.Current.CreateNewConnectionFromLogin();

                LoginSucceeded();
            }
        }

    }
}