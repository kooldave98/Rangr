using System;
using UIKit;
using App.Common;
using BigTed;
using Foundation;
using CoreGraphics;
using general_solid_lib;
using ios_ui_lib;

namespace rangr.ios
{
    public abstract class BaseViewModelController<VM> : BaseViewController where VM : ViewModelBase
    {
        protected VM view_model;

        public override void LoadView()
        {
            base.LoadView();
            Guard.IsNotNull(view_model, "view_model");

            Theme.Primitive.Apply(View);

            Title = NSBundle.MainBundle.LocalizedString(TitleLabel, TitleLabel);
        }
    }

    public abstract class BaseViewController : UIViewController
    {
        public abstract string TitleLabel { get; }

//        private static CGSize textSize = new NSString("The quick brown fox jumped over the lazy dog" +
//                                        "The quick brown fox jumped over the lazy dog" +
//                                        "The quick brown fox jumped over the lazy dog" +
//                                        "The quick brown fox jumped over the lazy dog")
//            .StringSize(UIFont.SystemFontOfSize(12f));
//
//        protected nfloat one_sixty_character_size = textSize.Height;

        protected nfloat parent_child_margin = HumanInterface.parent_child_margin;

        protected nfloat sibling_sibling_margin = HumanInterface.sibling_sibling_margin;

        protected nfloat finger_tip_diameter = HumanInterface.finger_tip_diameter;

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
            BTProgressHUD.ShowToast(message, false, 5000);
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

