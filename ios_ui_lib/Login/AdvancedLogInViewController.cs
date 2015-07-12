using System;
using CoreGraphics;
using Foundation;
using UIKit;
using common_lib;

namespace ios_ui_lib
{
    public class AdvancedLoginViewController : SimpleViewController
    {
        nfloat keyboardOffset = 0;

        public event Action LoginSucceeded = delegate {};

        public override string TitleLabel
        { 
            get{ return "Login"; } 
        }

        //Make this an abstract method in the base class.
        //so we won't have to derive from LoadView just to populate the view;
        public override void WillPopulateView()
        {
            View.AddSubview(scroll_view = new UIScrollView());

            scroll_view.AddSubview(login_view = new LoginView()
                                        .Init(l => l.UserDidLogin += o => Login())
            );
        }

        private NSLayoutConstraint[] added_constraints;

        public override void WillAddConstraints()
        {
            //            To use the pure autolayout approach do the following:
            //
            //            Set translatesAutoresizingMaskIntoConstraints to NO on all views involved.
            //            Position and size your scroll view with constraints external to the scroll view.
            //            Use constraints to lay out the subviews within the scroll view, being sure that 
            //            the constraints tie to all four edges of the scroll view and do not rely on the scroll view to get their size.
            //            A simple example would be a large image view, which has an intrinsic content size 
            //            derived from the size of the image. In the viewDidLoad method of your view controller, 
            //            you would include code similar to the code shown in the listing below:
            //See: https://developer.apple.com/library/ios/technotes/tn2154/_index.html

            added_constraints =
                
            View.ConstrainLayout(() => 
                scroll_view.Frame.Left == View.Bounds.Left &&
                scroll_view.Frame.Right == View.Bounds.Right &&
                scroll_view.Frame.Top == View.Bounds.Top &&
                scroll_view.Frame.Bottom == View.Bounds.Bottom - keyboardOffset && //--->this tweaks the height of scroll view  based on the offset??
            
            
                login_view.Frame.Height == View.Bounds.Height - double_parent_child_margin &&
            
                login_view.Frame.Left == View.Bounds.Left + parent_child_margin &&
                login_view.Frame.Right == View.Bounds.Right - parent_child_margin
        
            
            );
            
            scroll_view.ConstrainLayout(() =>             
            
                login_view.Frame.Top >= scroll_view.Frame.Top + parent_child_margin &&
                login_view.Frame.Bottom <= scroll_view.Frame.Bottom - parent_child_margin 
            
            );
        }

        public override void WillUpdateConstraints()
        {
            added_constraints[3].Constant = -keyboardOffset;
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
                        keyboardOffset = visible ? (nfloat)frame.Height : 0.0f; 
                        ApplyConstraints(true);
                    });
            }

            //However: I think the scrolling is done automatically for you, see:
            //http://stackoverflow.com/a/2703756/502130

            //if not.... then see below

//            //see https://developer.apple.com/library/ios/documentation/StringsTextFonts/Conceptual/TextAndWebiPhoneOS/KeyboardManagement/KeyboardManagement.html
//            //for how to scroll rect to visible
//            //ideally the below code should be called in the Keyboard.DidShowNotification
//            if (!View.Frame.Contains(login_view.EmailField.Frame.Location))
//            {
//                scroll_view.ScrollRectToVisible(login_view.EmailField.Frame, true);
//            }


        }

        public void set_user_name(string username)
        {
            login_view.EmailField.Text = username;
        }



        private void Login()
        {
            if (string.IsNullOrEmpty(login_view.EmailField.Text))
            {
                show_alert("Oops", "Please enter a valid email.", "Ok", () =>
                    {
                        login_view.EmailField.BecomeFirstResponder();
                    });

                return;
            }
            if (string.IsNullOrEmpty(login_view.PasswordField.Text))
            {
                show_alert("Oops", "Please enter a password.", "Ok", () =>
                    {
                        login_view.PasswordField.BecomeFirstResponder();
                    });

                return;
            }

            login_view.EmailField.ResignFirstResponder();
            login_view.PasswordField.ResignFirstResponder();

            LoginSucceeded();
        }

        private LoginView login_view;

        private UIScrollView scroll_view;

    }
}