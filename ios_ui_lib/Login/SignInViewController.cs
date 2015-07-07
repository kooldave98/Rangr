using System;
using CoreGraphics;
using Foundation;
using UIKit;
using common_lib;

namespace ios_ui_lib
{
    public class SignInViewController : SimpleViewController
    {
        public event Action LoginSucceeded = delegate {};

        public override string TitleLabel
        { 
            get{ return "Login"; } 
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            populate_view();
        }

        protected void populate_view()
        {
            View.AddSubview(login_view = new LoginView()
                                        .Init(l=> l.UserDidLogin += o => Login())
            );
        }

        public override void ViewDidLayoutSubviews()
        {
            var parent_child_margin = HumanInterface.parent_child_margin;
            var sibling_sibling_margin = HumanInterface.sibling_sibling_margin;
            var finger_tip_diameter = HumanInterface.finger_tip_diameter;

            View.ConstrainLayout(() => 

                //login_view.Frame.GetCenterX() == View.Frame.GetCenterX() &&
                login_view.Frame.Left == View.Frame.Left + parent_child_margin &&
                login_view.Frame.Right == View.Frame.Right - parent_child_margin &&
                login_view.Frame.Top == View.Frame.Top + parent_child_margin &&
                login_view.Frame.Bottom == View.Frame.Bottom - parent_child_margin 

            );
        }



        private void Login()
        {
            if (string.IsNullOrEmpty(login_view.EmailField.Text))
            {
                show_alert("Oops", "Please enter a valid email.", "Ok", () => {
                    login_view.EmailField.BecomeFirstResponder();
                });

                return;
            }
            if (string.IsNullOrEmpty(login_view.PasswordField.Text))
            {
                show_alert("Oops", "Please enter a password.", "Ok", () => {
                    login_view.PasswordField.BecomeFirstResponder();
                });

                return;
            }

            login_view.EmailField.ResignFirstResponder();
            login_view.PasswordField.ResignFirstResponder();

            LoginSucceeded();
        }

        private LoginView login_view;

    }
}