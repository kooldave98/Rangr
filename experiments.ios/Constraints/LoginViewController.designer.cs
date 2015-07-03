using System;
using CoreGraphics;
using Foundation;
using UIKit;
using ios_ui_lib;

namespace experiments.ios
{
    public partial class LoginViewController
    {

        UIImageView logo;
        UITextField username;
        UITextField password;
        UIButton login;
        UIButton help;
        UIActivityIndicatorView indicator;


        private void layout_with_simple_contraints()
        {

            var logo_image_width = logo.Image.Size.Width;
            var logo_image_height = logo.Image.Size.Height;
            var parent_child_margin = HumanInterface.parent_child_margin;
            var sibling_sibling_margin = HumanInterface.sibling_sibling_margin;
            var finger_tip_diameter = HumanInterface.finger_tip_diameter;

            View.ConstrainLayout(() => 
                logo.Frame.Width == logo_image_width &&
                logo.Frame.Height == logo_image_height &&
                logo.Frame.Top == View.Frame.Top + parent_child_margin &&
                logo.Frame.GetCenterX() == View.Frame.GetCenterX() &&

                username.Frame.Left == View.Frame.Left + parent_child_margin &&
                username.Frame.Right == View.Frame.Right - parent_child_margin &&         
                username.Frame.Top == logo.Frame.Bottom + sibling_sibling_margin &&
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
                indicator.Frame.Top == login.Frame.Bottom + sibling_sibling_margin&&
                indicator.Frame.Left == login.Frame.Left
            );
        }

        protected void populate_view()
        {
            View.AddSubview( logo = new UIImageView(UIImage.FromBundle("user-default-avatar.png")));

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
    }
}
