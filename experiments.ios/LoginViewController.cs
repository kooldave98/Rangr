using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace experiments.ios
{
    public partial class LoginViewController : BaseViewController
    {
        public override string TitleLabel
        {
            get
            {
                return "Login";
            }
        }

        public event Action LoginSucceeded = delegate {};

        public LoginViewController()
        {
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
        }

        private void Login()
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

            ShowToast("Hello");
            LoginSucceeded();
        }

    }
}