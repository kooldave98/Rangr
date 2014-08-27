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
		private bool isBusyHandlerSet = false;

		private EventHandler isBusyChangedEventHandler;

		private EventHandler<EventArgs> ConnectionFailedHandler;

		private ViewModelBase the_view_model;

		protected abstract ViewModelBase init_view_model ();

		private ProgressDialog progress;

		protected override void OnCreate (Bundle bundle)
		{
			notify ("OnCreate");
			base.OnCreate (bundle);

			the_view_model = init_view_model ();

			isBusyChangedEventHandler = (sender, e) => {

				if (the_view_model.IsBusy) {
					progress = ProgressDialog.Show (this, "Loading...", "Busy", true);
				} else {
					progress.Dismiss ();
				}
			};

			the_view_model.IsBusyChanged += isBusyChangedEventHandler;
			isBusyHandlerSet = true;
		}

		protected override void OnResume ()
		{
			notify ("OnResume");
			base.OnResume ();

			if (!isBusyHandlerSet) {
				the_view_model.IsBusyChanged += isBusyChangedEventHandler;
			}

			ConnectionFailedHandler = (sender, e) => {
				ShowToast("Unable to connect..");
			};

			AppEvents.Current.ConnectionFailed += ConnectionFailedHandler;

			the_view_model.ResumeState ();

			if (AppGlobal.Current.CurrentUserAndConnectionExists) {
				AppGlobal.Current.Resume ();
			}
		}

		protected override void OnPause ()
		{
			notify ("OnPause");
			base.OnPause ();

			the_view_model.IsBusyChanged -= isBusyChangedEventHandler;

			isBusyHandlerSet = false;

			the_view_model.PauseState ();

			if (progress != null)
				progress.Dismiss ();

			AppEvents.Current.ConnectionFailed -= ConnectionFailedHandler;

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