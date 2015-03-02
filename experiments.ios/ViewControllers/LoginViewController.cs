using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace experiments.ios
{
    public partial class LoginViewController : BaseViewController
    {
        public event Action LoginSucceeded = delegate {};

        public override string TitleLabel { 
            get{ return "Login"; } 
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
            View.BackgroundColor = UIColor.White;

            populate_view();
        }

        public override void ViewDidLayoutSubviews()
        {
            layout_with_simple_contraints();
        }

        private void Login()
        {
            if (string.IsNullOrEmpty(username.Text))
            {
                show_alert("Oops", "Please enter a username.", "Ok", () => {
                    username.BecomeFirstResponder();
                });

                return;
            }
            if (string.IsNullOrEmpty(password.Text))
            {
                show_alert("Oops", "Please enter a password.", "Ok", () => {
                    password.BecomeFirstResponder();
                });

                return;
            }

            username.ResignFirstResponder();
            password.ResignFirstResponder();

            indicator.StartAnimating();
            //indicator.StopAnimating ();

            LoginSucceeded();
        }

    }
}