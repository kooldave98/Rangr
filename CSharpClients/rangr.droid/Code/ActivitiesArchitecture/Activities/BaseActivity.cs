using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using rangr.common;
using System.Threading.Tasks;

namespace rangr.droid
{
	public abstract class BaseActivity : Activity
	{
		private bool is_paused = false;

		private EventHandler isBusyChangedEventHandler;

		private EventHandler<AppEventArgs> ConnectionFailedHandler;

		private EventHandler<AppEventArgs> GeolocatorFailedHandler;

		private ViewModelBase the_view_model;

		protected abstract ViewModelBase init_view_model ();

		private ProgressDialog progress;

		protected override void OnCreate (Bundle bundle)
		{
			//notify ("OnCreate");
			base.OnCreate (bundle);

			the_view_model = init_view_model ();
				
		}

		protected override void OnResume ()
		{
			//notify ("OnResume");
			base.OnResume ();

			isBusyChangedEventHandler = (sender, e) => {

				if (the_view_model.IsBusy) {
					show_progress ();
				} else {
					dismiss_progress ();
				}
			};

			the_view_model.IsBusyChanged += isBusyChangedEventHandler;

			ConnectionFailedHandler = (sender, e) => {
				ShowToast ("An attempt to establish a network connection failed.");
			};

			GeolocatorFailedHandler = (sender, e) => {
				ShowToast ("An attempt to retrieve your geolocation failed. " +
				"\n Please set your location mode on your phone's location settings to HIGH ACCURRACY");
			};

			AppEvents.Current.ConnectionFailed += ConnectionFailedHandler;

			AppEvents.Current.GeolocatorFailed += GeolocatorFailedHandler;

			the_view_model.ResumeState ();

			AppGlobal.Current.Resume (this);

			if (this.GetType () != typeof(LoginActivity) && !AppGlobal.Current.CurrentUserAndConnectionExists) {
				//This if condition allows me to make any activity the startup activity
				//Basically, the startup Activity should not be the Login, what should happen is..
				//if the app is not logged in, then we popup the Login Activity to authenticate the user.
				//So, each activity still needs to do its individual UserAndConnection checks (see below)
				//[if (AppGlobal.Current.CurrentUserAndConnectionExists)]
				//See the PostFeedActivity OnResume for an example of what I mean
				StartActivity (typeof(LoginActivity));
			} 

			is_paused = false;
		}

		protected override void OnPause ()
		{
			//notify ("OnPause");
			base.OnPause ();

			the_view_model.IsBusyChanged -= isBusyChangedEventHandler;

			the_view_model.PauseState ();

			dismiss_progress ();

			//Potentially easier way to unsubscribe event handlers
			//http://www.h3mm3.com/2011/06/unsubscribing-to-events-in-c.html

			AppEvents.Current.ConnectionFailed -= ConnectionFailedHandler;

			AppEvents.Current.GeolocatorFailed -= GeolocatorFailedHandler;

			AppGlobal.Current.Pause (this);

			is_paused = true;
		}

		//OnMenuItemSelected is the generic version of all menus (Options Menu, Context Menu)
		//http://stackoverflow.com/questions/7059572/difference-between-onmenuitemselected-and-onoptionsitemselected
		public override bool OnMenuItemSelected (int featureId, IMenuItem item)
		{
			switch (item.ItemId) {
			case Resource.Id.feed_menu_item:
				ResurrectActivity (typeof(PostFeedActivity));
				break;
			case Resource.Id.people_menu_item:
				ResurrectActivity (typeof(PeopleActivity));
				break;
			case Resource.Id.profile_menu_item:
				ResurrectActivity (typeof(ProfileActivity));
				break;
			case Resource.Id.simulation_menu_item:
				ResurrectActivity (typeof(SimulationActivity));
				break;
			case Resource.Id.settings_menu_item:
				ResurrectActivity (typeof(AboutAppActivity));
				break;
			}

			return base.OnMenuItemSelected (featureId, item);
		}

		protected void ResurrectActivity (Type activityType)
		{
			var i = new Intent (Application.Context, activityType);
			i.SetFlags (ActivityFlags.ReorderToFront);
			StartActivity (i);
		}

		protected void show_progress (string title = "Loading...", string message = "Busy")
		{
			RunOnUiThread (() => {
				progress = ProgressDialog.Show (this, title, message, true);
			});

		}

		protected void dismiss_progress ()
		{
			RunOnUiThread (() => {
				if (progress != null)
					progress.Dismiss ();
			});

		}

		protected void ShowToast (string message)
		{
			if (!is_paused) {
				RunOnUiThread (() => {
					if (true) {
						var t = Toast.MakeText (this, message, ToastLength.Long);
						t.SetGravity (GravityFlags.Center, 0, 0);
						t.Show ();
					}
				});
			}

		}

		protected void ShowAlert (string title, string message, string ok_button_text = "Ok", Action ok_button_action = null)
		{
			if (!is_paused) {
				var builder = new AlertDialog.Builder (this)
				.SetTitle (title)
				.SetMessage (message)
				.SetPositiveButton (ok_button_text, (innerSender, innere) => {
					RunOnUiThread (() => {
						if (ok_button_action != null) {
							ok_button_action ();
						}
					});
				
				});
				var dialog = builder.Create ();
				dialog.Show ();
			}
		}

		protected void notify (String methodName)
		{
			//just disable this for now by the if condition

			var name = this.LocalClassName;

			var noti = new Notification.Builder (this)
				.SetContentTitle (methodName + " " + name).SetAutoCancel (true)
				.SetSmallIcon (Resource.Drawable.Icon)
				.SetContentText (name).Build ();

			var notificationManager = (NotificationManager)GetSystemService (NotificationService);
			notificationManager.Notify ((int)CurrentTimeMillis (), noti);
		}

		private static readonly DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		//Search online for C# equivalent of Javas CurrentTimeMillis
		private static long CurrentTimeMillis ()
		{
			return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}
	}
}