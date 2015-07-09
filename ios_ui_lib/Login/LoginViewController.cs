using System;
using CoreGraphics;
using Foundation;
using UIKit;
using common_lib;

namespace ios_ui_lib
{
    public sealed class LoginViewController : SimpleViewController
    {
        public event Action LoginSucceeded = delegate {};

        public override string TitleLabel
        { 
            get{ return "Login"; } 
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //What does this line below FUCKING DO ?? -->>> INVESTIGATE
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;

            populate_view();
        }

        public override void ViewDidLayoutSubviews()
        {
            var logo_image_width = user_image.Image.Size.Width;
            var logo_image_height = user_image.Image.Size.Height;
            var parent_child_margin = HumanInterface.parent_child_margin;
            var sibling_sibling_margin = HumanInterface.sibling_sibling_margin;
            var finger_tip_diameter = HumanInterface.finger_tip_diameter;

            View.ConstrainLayout(() => 
                user_image.Frame.Width == logo_image_width &&
                user_image.Frame.Height == logo_image_height &&
                user_image.Frame.Top == View.Frame.Top + parent_child_margin &&
                user_image.Frame.GetCenterX() == View.Frame.GetCenterX() &&

                username.Frame.Left == View.Frame.Left + parent_child_margin &&
                username.Frame.Right == View.Frame.Right - parent_child_margin &&
                username.Frame.Top == user_image.Frame.Bottom + sibling_sibling_margin &&
                username.Frame.Height == finger_tip_diameter &&

                password.Frame.Left == View.Frame.Left + parent_child_margin &&
                password.Frame.Right == View.Frame.Right - parent_child_margin &&
                password.Frame.Top == username.Frame.Bottom + sibling_sibling_margin &&
                password.Frame.Height == finger_tip_diameter &&

                login.Frame.Height == finger_tip_diameter &&
                login.Frame.Width == login.Frame.Height * 2.0f &&
                login.Frame.Top == password.Frame.Bottom + sibling_sibling_margin &&
                login.Frame.GetCenterX() == View.Frame.GetCenterX() &&

                help.Frame.Height == finger_tip_diameter &&
                help.Frame.Width == help.Frame.Height * 2.0f &&
                help.Frame.Top == login.Frame.Top &&
                help.Frame.Left == login.Frame.Right + sibling_sibling_margin &&

                indicator.Frame.Height == finger_tip_diameter &&
                indicator.Frame.Width == indicator.Frame.Height * 2.0f &&
                indicator.Frame.Top == login.Frame.Bottom + sibling_sibling_margin &&
                indicator.Frame.Left == login.Frame.Left
            );
        }

        UIImageView user_image;
        UITextField username;
        UITextField password;
        UIButton login;
        UIButton help;
        UIActivityIndicatorView indicator;

        protected void populate_view()
        {
            View.AddSubview(user_image = new UIImageView(UIImage.FromBundle("user-default-avatar.png")));

            View.AddSubview(username = new UITextField
                {
                    Placeholder = "Username",
                    BorderStyle = UITextBorderStyle.RoundedRect,
                    VerticalAlignment = UIControlContentVerticalAlignment.Center,
                    AutocorrectionType = UITextAutocorrectionType.No,
                    AutocapitalizationType = UITextAutocapitalizationType.None,
                    ClearButtonMode = UITextFieldViewMode.WhileEditing,
                    LeftView = new UIView(new CGRect(0, 0, 8, 8)),
                    LeftViewMode = UITextFieldViewMode.Always,
                    ReturnKeyType = UIReturnKeyType.Next,
                    ShouldReturn = delegate
                    {
                        password.BecomeFirstResponder();
                        return true;
                    },
                });


            View.AddSubview(password = new UITextField
                {
                    Placeholder = "Password",
                    SecureTextEntry = true,
                    BorderStyle = UITextBorderStyle.RoundedRect,
                    VerticalAlignment = UIControlContentVerticalAlignment.Center,
                    AutocorrectionType = UITextAutocorrectionType.No,
                    AutocapitalizationType = UITextAutocapitalizationType.None,
                    ClearButtonMode = UITextFieldViewMode.WhileEditing,
                    LeftView = new UIView(new CGRect(0, 0, 8, 8)),
                    LeftViewMode = UITextFieldViewMode.Always,
                    ReturnKeyType = UIReturnKeyType.Go,
                    ShouldReturn = delegate
                    {
                        Login();
                        return true;
                    },
                });


            login = UIButton.FromType(UIButtonType.RoundedRect);
            login.SetTitle("Login", UIControlState.Normal);
            login.TouchUpInside += delegate
            {
                Login();
            };
            View.AddSubview(login);


            help = UIButton.FromType(UIButtonType.InfoDark);
            help.TouchUpInside += (sender, e) =>
            {
                new UIAlertView("Need Help?", "Enter any username or password to login.", null, "Ok").Show();
            };
            View.AddSubview(help);


            View.AddSubview(indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge)
                {
                    HidesWhenStopped = true,
                    Hidden = true,
                });
        }

        private void Login()
        {
            if (string.IsNullOrEmpty(username.Text))
            {
                show_alert("Oops", "Please enter a username.", "Ok", () =>
                    {
                        username.BecomeFirstResponder();
                    });

                return;
            }
            if (string.IsNullOrEmpty(password.Text))
            {
                show_alert("Oops", "Please enter a password.", "Ok", () =>
                    {
                        password.BecomeFirstResponder();
                    });

                return;
            }

            username.ResignFirstResponder();
            password.ResignFirstResponder();

            indicator.StartAnimating();
            //indicator.StopAnimating ();//Is ther no need for this any longer ??? --->>> INVESTIGATE IT MOFO !!

            LoginSucceeded();
        }

        public void set_user_name(string value)
        {
            username.Text = value;
        }

    }
}