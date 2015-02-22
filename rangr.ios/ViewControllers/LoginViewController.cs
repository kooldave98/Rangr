
using Foundation;
using UIKit;
using System.Drawing;
using System;
using BigTed;
using App.Common;

namespace rangr.ios
{
    public class LoginViewController : BaseViewModelController<LoginViewModel>
    {
        public override string TitleLabel
        {
            get
            {
                return "Login";
            }
        }

        private UIView ContentView;
        private LoginView LoginView;
        private UIScrollView scrollView;
        private float keyboardOffset = 0;

        public event Action LoginSucceeded = delegate {};

        public LoginViewController()
        {
            view_model = new LoginViewModel();

            //N/B: This should not be done here. How do we know we are in a navigation controller ? ;)
            //This hides the back button text when you leave this View Controller
            //this.NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, handler: null);
            AutomaticallyAdjustsScrollViewInsets = false;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.AddSubview(scrollView = new UIScrollView());

            LoginView = new LoginView();
            LoginView.UserDidLogin += _ => Login(LoginView.EmailField.Text, LoginView.PasswordField.Text);
            scrollView.Add(ContentView = LoginView);

        }

        public override void ViewDidLayoutSubviews()
        {
            var bounds = View.Bounds;
            ContentView.Frame = bounds;
            scrollView.ContentSize = bounds.Size;
            //Resize Scroller for keyboard;
            bounds.Height -= keyboardOffset;
            scrollView.Frame = bounds;
        }

        private void OnKeyboardNotification(NSNotification notification)
        {
            if (IsViewLoaded)
            {

                //Check if the keyboard is becoming visible
                bool visible = notification.Name == UIKeyboard.WillShowNotification;
                UIView.Animate(UIKeyboard.AnimationDurationFromNotification(notification), () =>
                    {
                        UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));
                        var frame = UIKeyboard.FrameEndFromNotification(notification);
                        keyboardOffset = (float)(visible ? frame.Height : 0); 
                        ViewDidLayoutSubviews();
                    });
            }
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            NSNotificationCenter.DefaultCenter.RemoveObservers(new []{ UIKeyboard.WillHideNotification, UIKeyboard.WillShowNotification });
        }

        private void Login(string username, string password)
        {
            view_model.UserDisplayName = username;

            if (!string.IsNullOrEmpty(username))
            {

                if (!string.IsNullOrEmpty(password) && password == "wertyc")
                {



                    show_alert("Disclaimer", "This app is in beta, it may be subject to changes, loss of data and unavailability.", "Ok", async delegate
                    {
                        show_progress("creating user...");
                        var create_user_successful = await view_model.CreateUser();
                        dismiss_progress();
                        if (create_user_successful)
                        {
                            show_progress("logging in...");
                            await AppGlobal.Current.CreateNewConnectionFromLogin();
                            dismiss_progress();
                            LoginSucceeded();
                        }
                        else
                        {
                            show_toast("An error occurred.");
                        }
                    });


                }
                else
                {
                    show_alert("Error", "Invalid code entered. Please request a test code by emailing walkr@davidolubajo.com. Thanks", "Ok", delegate
                    {
                        LoginView.PasswordField.SelectAll(this);
                        LoginView.PasswordField.BecomeFirstResponder();
                    });
                }
            }

        }
    }
}



