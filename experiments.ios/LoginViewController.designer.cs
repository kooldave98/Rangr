using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace experiments.ios
{
    public partial class LoginViewController
    {
        UITextField username;
        UITextField password;
        UIActivityIndicatorView indicator;
        UIButton login;
        UIButton help;

        protected void InitView()
        {
            View.BackgroundColor = UIColor.White;

            var logo = new UIImageView(UIImage.FromBundle("user-default-avatar.png"));

            AddCentered(logo, 33, logo.Image.Size.Width, logo.Image.Size.Height);

            username = new UITextField
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
            };
            AddCentered(username, 80, 200, 44);

            password = new UITextField
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
            };
            AddCentered(password, 132, 200, 44);

            login = UIButton.FromType(UIButtonType.RoundedRect);
            login.SetTitle("Login", UIControlState.Normal);
            login.TouchUpInside += delegate
            {
                Login();
            };
            AddCentered(login, 184, 100, 51);

            help = UIButton.FromType(UIButtonType.InfoDark);
            help.TouchUpInside += (sender, e) =>
            {
                new UIAlertView("Need Help?", "Enter any username or password to login.", null, "Ok").Show();
            };
            AddCentered(help, 194, 30, 31);

            //Adjust frame of help button
            var frame = help.Frame;
            frame.X = login.Frame.Right + 8;
            help.Frame = frame;

            indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge)
            {
                HidesWhenStopped = true,
                Hidden = true,
            };
            frame = indicator.Frame;
            frame.X = login.Frame.X - indicator.Frame.Width - 8;
            frame.Y = login.Frame.Y + 6;
            indicator.Frame = frame;
            View.AddSubview(indicator);
        }
    }
}
