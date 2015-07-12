using System;
using CoreGraphics;
using Foundation;
using UIKit;
using common_lib;

namespace ios_ui_lib
{
    public class ExtendableLoginViewController : SimpleViewController
    {
        nfloat keyboardOffset = 0;

        public event Action LoginSucceeded = delegate {};

        public override string TitleLabel
        { 
            get{ return "Login"; } 
        }

        public override void LoadView()
        {
            Console.WriteLine("LoadView()");
            base.LoadView();

            populate_view();
        }

        public override void ViewDidLoad()
        {
            Console.WriteLine("ViewDidLoad()");
            base.ViewDidLoad();
        }

        //Make this an abstract method in the base class.
        //so we won't have to derive from LoadView just to populate the view;
        protected void populate_view()
        {
            View.apply_simple_border(UIColor.Yellow.CGColor);

            View.AddSubview(scroll_view = new UIScrollView()
                                        .Init(v => v.apply_simple_border(UIColor.Red.CGColor)));

            scroll_view.AddSubview(login_view = new LoginView()
                                        .Init(l => l.UserDidLogin += o => Login())
                                        .Init(l => l.apply_simple_border(UIColor.Brown.CGColor))
            );
        }

        private NSLayoutConstraint[] added_constraints;

        public override void UpdateViewConstraints()
        {
            Console.WriteLine("UpdateViewConstraints()");


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



            if (!have_constraints_been_added)
            {

                //TODO: When the keyboard pops up, dynamically adjust the scroll views bottom constraints
                added_constraints =
                View.ConstrainLayout(() =>
                    scroll_view.Frame.Left == View.Bounds.Left &&
                    scroll_view.Frame.Right == View.Bounds.Right &&
                    scroll_view.Frame.Top == View.Bounds.Top &&
                    scroll_view.Frame.Bottom == View.Bounds.Bottom - keyboardOffset &&//--->this tweaks the height of scroll view  based on the offset??
            
            
                    login_view.Frame.Height == View.Bounds.Height - double_parent_child_margin &&
            
                    login_view.Frame.Left == View.Bounds.Left + parent_child_margin &&
                    login_view.Frame.Right == View.Bounds.Right - parent_child_margin
            
            
                );
            
                scroll_view.ConstrainLayout(() => 
            
            
                    login_view.Frame.Top >= scroll_view.Frame.Top + parent_child_margin &&
                    login_view.Frame.Bottom <= scroll_view.Frame.Bottom - parent_child_margin 
            
                );

                have_constraints_been_added = true;
            }

            added_constraints[3].Constant = -keyboardOffset;

            base.UpdateViewConstraints();
        }

        public override void ViewDidAppear(bool animated)
        {
            Console.WriteLine("ViewDidAppear()");
            base.ViewDidAppear(animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            Console.WriteLine("ViewWillAppear()");
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
                        View.SetNeedsUpdateConstraints();
                        View.UpdateConstraintsIfNeeded();
                    });
            }

            //see https://developer.apple.com/library/ios/documentation/StringsTextFonts/Conceptual/TextAndWebiPhoneOS/KeyboardManagement/KeyboardManagement.html
            //for how to scroll rect to visible
            //ideally the below code should be called in the Keyboard.DidShowNotification
            if(!View.Frame.Contains(login_view.EmailField.Frame.Location))
            {
                scroll_view.ScrollRectToVisible(login_view.EmailField.Frame, true);
            }
        }

        public override void ViewWillLayoutSubviews()
        {
            Console.WriteLine("ViewWillLayoutSubviews()");
            base.ViewWillLayoutSubviews();
        }

        public override void ViewDidLayoutSubviews()
        {
            Console.WriteLine("ViewDidLayoutSubviews()");
            base.ViewDidLayoutSubviews();

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