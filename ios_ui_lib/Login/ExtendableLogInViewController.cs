using System;
using CoreGraphics;
using Foundation;
using UIKit;
using common_lib;

namespace ios_ui_lib
{
    public class ExtendableLoginViewController : SimpleViewController
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
            //View.apply_simple_border(UIColor.Yellow.CGColor);

            View.AddSubview(scroll_view = new UIScrollView()
                                        .Init(v=>v.apply_simple_border(UIColor.Red.CGColor)));

            scroll_view.AddSubview(login_view = new LoginView()
                                        .Init(l=> l.UserDidLogin += o => Login())
                                        .Init(l=>l.apply_simple_border(UIColor.Brown.CGColor))
            );
        }

        public override void ViewDidLayoutSubviews()
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



            View.ConstrainLayout(()=>
                scroll_view.Frame.Left == View.Bounds.Left &&
                scroll_view.Frame.Right == View.Bounds.Right &&
                scroll_view.Frame.Top == View.Bounds.Top &&
                scroll_view.Frame.Bottom == View.Bounds.Bottom - 60 &&//--->height of key board offset??


                login_view.Frame.Height == View.Bounds.Height - double_parent_child_margin &&

                login_view.Frame.Left == View.Bounds.Left + parent_child_margin &&
                login_view.Frame.Right == View.Bounds.Right - parent_child_margin


            );

            scroll_view.ConstrainLayout(() => 


                login_view.Frame.Top >= scroll_view.Frame.Top + parent_child_margin &&
                login_view.Frame.Bottom <= scroll_view.Frame.Bottom - parent_child_margin 

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

        private UIScrollView scroll_view;

    }
}