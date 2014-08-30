using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using App.Common;
using System.Threading.Tasks;

namespace App.Android
{
	public abstract class BaseActivity : Activity
	{
		private EventHandler isBusyChangedEventHandler;

		private EventHandler<EventArgs> ConnectionFailedHandler;

		private EventHandler<EventArgs> GeolocatorFailedHandler;

		private ViewModelBase the_view_model;

		protected abstract ViewModelBase init_view_model ();

		private ProgressDialog progress;

		protected override void OnCreate (Bundle bundle)
		{
			notify ("OnCreate");
			base.OnCreate (bundle);

			the_view_model = init_view_model ();
				
		}

		protected override void OnResume ()
		{
			notify ("OnResume");
			base.OnResume ();

			isBusyChangedEventHandler = (sender, e) => {

				if (the_view_model.IsBusy) {
					show_progress();
				} else {
					dismiss_progress();
				}
			};

			the_view_model.IsBusyChanged += isBusyChangedEventHandler;

			ConnectionFailedHandler = (sender, e) => {
				ShowToast ("Unable to connect..");
			};

			GeolocatorFailedHandler = (sender, e) => {
				ShowToast ("Unable to determine location..");
			};

			AppEvents.Current.ConnectionFailed += ConnectionFailedHandler;

			AppEvents.Current.GeolocatorFailed += GeolocatorFailedHandler;

			the_view_model.ResumeState ();

			if (AppGlobal.Current.CurrentUserAndConnectionExists) {
				//This is majorly to prevent not logged in activities from calling resume
				AppGlobal.Current.Resume ();
			}
		}

		protected override void OnPause ()
		{
			notify ("OnPause");
			base.OnPause ();

			the_view_model.IsBusyChanged -= isBusyChangedEventHandler;

			the_view_model.PauseState ();

			dismiss_progress ();

			AppEvents.Current.ConnectionFailed -= ConnectionFailedHandler;

			AppEvents.Current.GeolocatorFailed -= GeolocatorFailedHandler;

			AppGlobal.Current.Pause ();
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
			}

			return base.OnMenuItemSelected (featureId, item);
		}

		protected void ResurrectActivity (Type activityType)
		{
			var i = new Intent (Application.Context, activityType);
			i.SetFlags (ActivityFlags.ReorderToFront);
			StartActivity (i);
		}

		protected void show_progress(string title = "Loading...", string message = "Busy")
		{
			progress = ProgressDialog.Show (this, title, message, true);
		}

		protected void dismiss_progress()
		{
			if (progress != null)
				progress.Dismiss ();
		}

		protected void ShowToast (string message)
		{
			if (true) {
				var t = Toast.MakeText (this, message, ToastLength.Long);
				t.SetGravity (GravityFlags.Center, 0, 0);
				t.Show ();
			}
		}

		protected void notify (String methodName)
		{
			//just disable this for now by the if condition
			if (1 == 0) {
				var name = this.LocalClassName;

				var noti = new Notification.Builder (this)
				.SetContentTitle (methodName + " " + name).SetAutoCancel (true)
				.SetSmallIcon (Resource.Drawable.ic_action_logo)
				.SetContentText (name).Build ();

				var notificationManager = (NotificationManager)GetSystemService (NotificationService);
				notificationManager.Notify ((int)CurrentTimeMillis (), noti);
			}
		}

		private static readonly DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		//Search online for C# equivalent of Javas CurrentTimeMillis
		private static long CurrentTimeMillis ()
		{
			return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}
	}
}