using System;
using UIKit;
using BigTed;
using Foundation;
using CoreGraphics;

namespace experiments.ios
{
    public abstract class BaseViewController : UIViewController
    {
        public BaseViewController(string title)
        {
            TitleLabel = title;
        }

        public string TitleLabel { get; private set; }

        public override void LoadView()
        {
            base.LoadView();

            Title = NSBundle.MainBundle.LocalizedString(TitleLabel, TitleLabel);
        }

        protected void AddCentered(UIView view)
        {
            AddCentered(view, view.Frame.Y, view.Frame.Width, view.Frame.Height);
        }

        protected void AddCentered(UIView view, nfloat y, nfloat width, nfloat height)
        {
            var f = new CGRect((View.Frame.Width - width) / 2, y, width, height);
            view.Frame = f;
            view.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
            View.AddSubview(view);
        }

        protected void hide_keyboard_for()
        {
            //hide keyboard for a soft keyboard    
        }

        protected void show_progress(string title = "Loading...", string message = "Busy")
        {                
            BTProgressHUD.Show(title);
        }

        protected void dismiss_progress()
        {
            BTProgressHUD.Dismiss();
        }

        protected void ShowToast(string message)
        {
            BTProgressHUD.ShowToast(message, false, 10000);
        }

        protected void ShowAlert(string title, string message, string ok_button_text = "Ok", Action ok_button_action = null)
        {
            var alert = new UIAlertView(title, message, null, ok_button_text);                

            alert.Clicked += delegate
            {
                if (ok_button_action != null)
                {
                    ok_button_action();
                }
            };

            alert.Show();
        }

        protected void notify(String methodName)
        {
            //lock screen notifications
        }

        private static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }
    }
}

