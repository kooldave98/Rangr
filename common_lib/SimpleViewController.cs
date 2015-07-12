#if __IOS__
using System;
using UIKit;
using BigTed;
using Foundation;
using CoreGraphics;

namespace common_lib
{
    public abstract class SimpleViewController : UIViewController
    {
        protected bool have_constraints_been_added { get; set;}

        public override void LoadView()
        {
            base.LoadView();
            Title = NSBundle.MainBundle.LocalizedString(TitleLabel, TitleLabel);

            View.TranslatesAutoresizingMaskIntoConstraints = false;
            have_constraints_been_added = false;
            View.SetNeedsUpdateConstraints();


            View.BackgroundColor = UIColor.White;
        }

        public abstract string TitleLabel { get; }

        //        private static CGSize textSize = new NSString("The quick brown fox jumped over the lazy dog" +
        //                                        "The quick brown fox jumped over the lazy dog" +
        //                                        "The quick brown fox jumped over the lazy dog" +
        //                                        "The quick brown fox jumped over the lazy dog")
        //            .StringSize(UIFont.SystemFontOfSize(12f));
        //
        //        protected nfloat one_sixty_character_size = textSize.Height;

        protected nfloat parent_child_margin = HumanInterface.parent_child_margin;

        protected nfloat double_parent_child_margin = HumanInterface.parent_child_margin * 2;

        protected nfloat sibling_sibling_margin = HumanInterface.sibling_sibling_margin;

        protected nfloat double_sibling_sibling_margin = HumanInterface.sibling_sibling_margin * 2;

        protected nfloat finger_tip_diameter = HumanInterface.finger_tip_diameter;

        protected nfloat double_finger_tip_diameter = HumanInterface.finger_tip_diameter * 2;

        protected void hide_keyboard_for()
        {
            //hide keyboard for a soft keyboard    
        }

        protected void show_progress(string title = "Loading...", string message = "Busy")
        {                
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
            BTProgressHUD.Show(title);
        }

        protected void dismiss_progress()
        {
            UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
            BTProgressHUD.Dismiss();
        }

        protected void show_toast(string message)
        {
            BTProgressHUD.ShowToast(message, false, 5000);
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
#endif
