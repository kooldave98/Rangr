
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using App.Common;

namespace rangr.droid
{
    public abstract class BaseFragment : Fragment
    {
        protected void show_progress(string title = "Loading...", string message = "Busy")
        {
            this.
            Activity.RunOnUiThread(() =>
                {
                    progress = ProgressDialog.Show(this.Activity, title, message, true);
                });

        }

        protected void dismiss_progress()
        {
            this.Activity.RunOnUiThread(() =>
                {
                    if (progress != null)
                        progress.Dismiss();
                });

        }

        protected void ShowToast(string message)
        {
//            if (!is_paused)
//            {
            this.Activity.RunOnUiThread(() =>
                {
                    if (true)
                    {
                        var t = Toast.MakeText(this.Activity, message, ToastLength.Long);
                        t.SetGravity(GravityFlags.Center, 0, 0);
                        t.Show();
                    }
                });
            //}

        }

        protected void ShowAlert(string title, string message, string ok_button_text = "Ok", Action ok_button_action = null)
        {
//            if (!is_paused)
//            {
            var builder = new AlertDialog.Builder(this.Activity)
                    .SetTitle(title)
                    .SetMessage(message)
                    .SetPositiveButton(ok_button_text, (innerSender, innere) =>
                {
                    Activity.RunOnUiThread(() =>
                        {
                            if (ok_button_action != null)
                            {
                                ok_button_action();
                            }
                        });

                });
            var dialog = builder.Create();
            dialog.Show();
            //}
        }

        protected void notify(String methodName)
        {
            var name = Activity.LocalClassName;

            var noti = new Notification.Builder(this.Activity)
                .SetContentTitle(methodName + " " + name).SetAutoCancel(true)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetContentText(name).Build();

            var notificationManager = (NotificationManager)Activity.GetSystemService(Activity.NotificationService);
            notificationManager.Notify((int)CurrentTimeMillis(), noti);
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        //Search online for C# equivalent of Javas CurrentTimeMillis
        private static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }


        private ProgressDialog progress;

    }

    public abstract class VMFragment<V> : BaseFragment where V : ViewModelBase
    {
        public override void OnCreate(Bundle bundle)
        {
            Guard.IsNotNull(view_model, "view_model");
            //notify ("OnCreate");
            base.OnCreate(bundle);
        }

        public override void OnResume()
        {
            //notify ("OnResume");
            base.OnResume();

            isBusyChangedEventHandler = (sender, e) =>
            {

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

            ConnectionFailedHandler = (sender, e) =>
            {
                ShowToast("An attempt to establish a network connection failed.");
            };

            GeolocatorFailedHandler = (sender, e) =>
            {
                ShowToast("An attempt to retrieve your geolocation failed. " +
                    "\n Please set your location mode on your phone's location settings to HIGH ACCURRACY");
            };

            AppEvents.Current.ConnectionFailed += ConnectionFailedHandler;

            AppEvents.Current.GeolocatorFailed += GeolocatorFailedHandler;

            view_model.ResumeState();

            AppGlobal.Current.Resume(this.Activity);


            is_paused = false;
        }

        public override void OnPause()
        {
            //notify ("OnPause");
            base.OnPause();

            view_model.IsBusyChanged -= isBusyChangedEventHandler;

            view_model.PauseState();

            dismiss_progress();

            //Potentially easier way to unsubscribe event handlers
            //http://www.h3mm3.com/2011/06/unsubscribing-to-events-in-c.html

            AppEvents.Current.ConnectionFailed -= ConnectionFailedHandler;

            AppEvents.Current.GeolocatorFailed -= GeolocatorFailedHandler;

            AppGlobal.Current.Pause(this.Activity);

            is_paused = true;
        }

        private bool is_paused = false;

        private EventHandler isBusyChangedEventHandler;

        private EventHandler<AppEventArgs> ConnectionFailedHandler;

        private EventHandler<AppEventArgs> GeolocatorFailedHandler;

        protected V view_model;
    }
}