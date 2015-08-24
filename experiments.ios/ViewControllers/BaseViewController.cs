using System;
using UIKit;
using BigTed;
using Foundation;
using CoreGraphics;

namespace experiments.ios
{
    public abstract class BaseViewController : UIViewController
    {
        public abstract string TitleLabel { get; }

        public override void LoadView()
        {
            base.LoadView();

            Title = NSBundle.MainBundle.LocalizedString(TitleLabel, TitleLabel);
        }

        protected void show_progress(string title = "Loading...", string message = "Busy")
        {                
            BTProgressHUD.Show(title);
        }

        protected void dismiss_progress()
        {
            BTProgressHUD.Dismiss();
        }

        protected void show_toast(string message)
        {
            BTProgressHUD.ShowToast(message, false, 10000);
        }

        protected void show_alert(string title, string message, string ok_button_text = "Ok", Action ok_button_action = null)
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

        private static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }
    }
}

