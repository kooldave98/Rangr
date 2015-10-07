using System;
using UIKit;
using rangr.common;
using BigTed;
using Foundation;
using CoreGraphics;
using solid_lib;
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
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            isBusyChangedEventHandler = (sender, e) => {

                if (view_model.IsBusy)
                {
                    show_progress();
                }
                else
                {
                    dismiss_progress();
                }
            };

            view_model.IsBusyChanged += isBusyChangedEventHandler;

            ConnectionFailedHandler = (sender, e) => {
                show_toast("An attempt to establish a network connection failed.");
            };

            GeolocatorFailedHandler = (sender, e) => {
                show_toast("An attempt to retrieve your geolocation failed. " +
                    "\n Please set your location mode on your phone's location settings to HIGH ACCURRACY");
            };

            AppEvents.Current.ConnectionFailed += ConnectionFailedHandler;

            AppEvents.Current.GeolocatorFailed += GeolocatorFailedHandler;

            view_model.ResumeState();

            AppGlobal.Current.Resume();


            is_paused = false;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            view_model.IsBusyChanged -= isBusyChangedEventHandler;

            view_model.PauseState();

            dismiss_progress();

            //Potentially easier way to unsubscribe event handlers
            //http://www.h3mm3.com/2011/06/unsubscribing-to-events-in-c.html

            AppEvents.Current.ConnectionFailed -= ConnectionFailedHandler;

            AppEvents.Current.GeolocatorFailed -= GeolocatorFailedHandler;

            AppGlobal.Current.Pause();

            is_paused = true;
        }

        private bool is_paused = false;

        private EventHandler isBusyChangedEventHandler;

        private EventHandler<AppEventArgs> ConnectionFailedHandler;

        private EventHandler<AppEventArgs> GeolocatorFailedHandler;
    }

    public abstract class BaseViewController : UIViewController
    {
        public override void LoadView()
        {
            base.LoadView();
            Title = NSBundle.MainBundle.LocalizedString(TitleLabel, TitleLabel);
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

        protected nfloat sibling_sibling_margin = HumanInterface.sibling_sibling_margin;

        protected nfloat finger_tip_diameter = HumanInterface.finger_tip_diameter;

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

