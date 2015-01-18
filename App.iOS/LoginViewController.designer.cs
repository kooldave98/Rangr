using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace App.iOS
{
    public partial class LoginViewController
    {
        UITextField username;
        UITextField password;
        UIActivityIndicatorView indicator;
        UIButton login;
        UIButton help;

        static readonly UIImage TextFieldBackground = UIImage.FromBundle("login_textfield.png")
																.CreateResizableImage(new UIEdgeInsets(8, 8, 8, 8));

        public void InitView()
        {
            View.BackgroundColor = UIColor.FromPatternImage(UIImage.FromBundle("login_box.png"));


//			var logo = new UIImageView (UIImage.FromBundle ("logo.png"));
//			AddCentered (logo, 33, logo.Image.Size.Width, logo.Image.Size.Height);

            username = new UITextField
            {
                Placeholder = "Username",
                BorderStyle = UITextBorderStyle.None,
                VerticalAlignment = UIControlContentVerticalAlignment.Center,
                AutocorrectionType = UITextAutocorrectionType.No,
                AutocapitalizationType = UITextAutocapitalizationType.None,
                ClearButtonMode = UITextFieldViewMode.WhileEditing,
                Background = TextFieldBackground,
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
                BorderStyle = UITextBorderStyle.None,
                VerticalAlignment = UIControlContentVerticalAlignment.Center,
                AutocorrectionType = UITextAutocorrectionType.No,
                AutocapitalizationType = UITextAutocapitalizationType.None,
                ClearButtonMode = UITextFieldViewMode.WhileEditing,
                Background = TextFieldBackground,
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

            login = UIButton.FromType(UIButtonType.Custom);
            login.SetTitle("Login", UIControlState.Normal);
            login.SetBackgroundImage(UIImage.FromBundle("login_btn.png").CreateResizableImage(new UIEdgeInsets(8, 8, 8, 8)), UIControlState.Normal);
            login.TouchUpInside += delegate
            {
                Login();
            };
            AddCentered(login, 184, 100, 51);

            help = UIButton.FromType(UIButtonType.Custom);
            help.SetImage(UIImage.FromBundle("questionmark.png"), UIControlState.Normal);
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


        void AddCentered(UIView view, float y, float width, float height)
        {
            var f = new CGRect((320 - width) / 2, y, width, height);
            view.Frame = f;
            view.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
            View.AddSubview(view);
        }
    }
}
