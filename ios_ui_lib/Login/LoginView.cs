using System;
using UIKit;
using System.Drawing;
using Foundation;
using common_lib;

namespace ios_ui_lib
{
    /// <summary>
    /// TODO: Consider consolidating this LoginView with the LoginViewController
    /// Decide on a re-useable pattern to go forward with.
    /// ----------------------------------------------------------->>>>>>>>>>>>>>>>>>>>
    /// UPDATE: I'm thinking I may as well leave this in here as an example of how to create UIViews ??
    /// The LoginViewController can just embbed this viewlike any other view and handle the area where it asks for a username and password.
    /// </summary>
    public class LoginView : UIView
    {
        public readonly UIImageView GravatarView;

        //TODO: Add this to the Layout standard sizes as Standard
        static readonly SizeF GravatarSize = new SizeF(85f, 85f);

        public readonly UITextField UserIDField;
        public readonly UITextField PasswordField;

        public event EventHandler<OnLoginRequestedEventArgs> OnLoginRequested = delegate{};

        public LoginView()
        {
            BackgroundColor = UIColor.White;

            Add(GravatarView = new UIImageView(new RectangleF(PointF.Empty, GravatarSize))
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    //TODO: Put resources that will be local to the library csproj
                    Image = UIImage.FromBundle("user-default-avatar"),
                });

            GravatarView.Layer.CornerRadius = GravatarSize.Width / 2;
            GravatarView.Layer.MasksToBounds = true;


            AddConstraint(NSLayoutConstraint.Create(
                    GravatarView,
                    NSLayoutAttribute.Top,
                    NSLayoutRelation.Equal,
                    this,
                    NSLayoutAttribute.Top,
                    1f, 90f
                ));

            AddConstraint(NSLayoutConstraint.Create(
                    GravatarView,
                    NSLayoutAttribute.CenterX,
                    NSLayoutRelation.Equal,
                    this,
                    NSLayoutAttribute.CenterX,
                    1f, 0
                ));

            AddConstantSizeConstraints(GravatarView, GravatarSize);

            Add(UserIDField = new UITextField(new RectangleF(10, 10, 300, 30))
                {
                    BorderStyle = UITextBorderStyle.RoundedRect,
                    Placeholder = "user id",
                    TranslatesAutoresizingMaskIntoConstraints = false
                });

            AddConstraint(NSLayoutConstraint.Create(
                    UserIDField,
                    NSLayoutAttribute.Top,
                    NSLayoutRelation.Equal,
                    GravatarView,
                    NSLayoutAttribute.Bottom,
                    1f, 30f
                ));

            AddConstraint(NSLayoutConstraint.Create(
                    UserIDField,
                    NSLayoutAttribute.CenterX,
                    NSLayoutRelation.Equal,
                    GravatarView,
                    NSLayoutAttribute.CenterX,
                    1f, 0
                ));

            UserIDField.EditingChanged += (sender, e) => {
                DisplayGravatar(UserIDField.Text);
            };

            var textSize = new NSString("hello").StringSize(UIFont.SystemFontOfSize(12f));

            AddConstantSizeConstraints(UserIDField, new SizeF(260, (float)textSize.Height + 16));

            Add(PasswordField = new UITextField(new RectangleF(10, 10, 300, 30))
                {
                    BorderStyle = UITextBorderStyle.RoundedRect,
                    SecureTextEntry = true,
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    ReturnKeyType = UIReturnKeyType.Go,
                    Placeholder = "password"
                });

            AddConstraint(NSLayoutConstraint.Create(
                    PasswordField,
                    NSLayoutAttribute.Top,
                    NSLayoutRelation.Equal,
                    UserIDField,
                    NSLayoutAttribute.Bottom,
                    1f, 10f
                ));

            AddConstraint(NSLayoutConstraint.Create(
                    PasswordField,
                    NSLayoutAttribute.CenterX,
                    NSLayoutRelation.Equal,
                    UserIDField,
                    NSLayoutAttribute.CenterX,
                    1f, 0
                ));

            AddConstantSizeConstraints(PasswordField, new SizeF(260, (float)textSize.Height + 16));

            PasswordField.ShouldReturn = field =>
            {
                field.ResignFirstResponder();
                    OnLoginRequested(this, new OnLoginRequestedEventArgs(UserIDField.Text, PasswordField.Text));
                return true;
            };
        }

        async void DisplayGravatar(string email)
        {
            NSData data;

            try
            {
                data = await Gravatar.GetImageData(email, (int)GravatarSize.Width * 2);
            }
            catch
            {
                return;
            }

            GravatarView.Image = UIImage.LoadFromData(data);
        }

        void AddConstantSizeConstraints(UIView view, SizeF size)
        {
            AddConstraint(NSLayoutConstraint.Create(
                    view,
                    NSLayoutAttribute.Width,
                    NSLayoutRelation.Equal,
                    null,
                    NSLayoutAttribute.NoAttribute,
                    1f, size.Width
                ));

            AddConstraint(NSLayoutConstraint.Create(
                    view,
                    NSLayoutAttribute.Height,
                    NSLayoutRelation.Equal,
                    null,
                    NSLayoutAttribute.NoAttribute,
                    1f, size.Height
                ));
        }

        public class OnLoginRequestedEventArgs : EventArgs
        {
            public string login_id { get; private set; }

            public string login_password { get; private set; }

            public OnLoginRequestedEventArgs (string id, string password)
            {
                login_id = id;
                login_password = password;
            }
        }
    }


}

