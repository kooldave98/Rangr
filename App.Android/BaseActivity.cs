﻿using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using App.Common;

namespace App.Android 
{
	public abstract class BaseActivity : Activity 
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			the_view_model.IsBusyChanged +=	(sender, e) => {
				if (the_view_model.IsBusy) {
					progress = ProgressDialog.Show (this, "Loading...", "Busy", true);
				} else {
					progress.Dismiss ();
				}
			};
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


		protected void ResurrectActivity(Type activityType)
		{
			var i = new Intent(Global.Current, activityType);
			i.SetFlags(ActivityFlags.ReorderToFront);
			StartActivity(i);
		}


		protected void ShowToast(bool really)
		{
			if(really)
			{
				var t = Toast.MakeText(this, "A toast", ToastLength.Short);
				t.SetGravity(GravityFlags.Center, 0, 0);
				t.Show();
			}
		}

		protected abstract ViewModelBase the_view_model { get;}


		private ProgressDialog progress;


		protected override void OnPause ()
		{
			base.OnPause ();
			Global.Current.suspend ();
		}

		protected override void OnStop ()
		{
			base.OnStop ();

		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			Finish ();

		}

		protected override void OnStart ()
		{
			base.OnStart ();

		}

		protected override void OnResume ()
		{
			base.OnResume ();
			Global.Current.resume ();
			notify ("OnResume");
		}

		private void notify(String methodName) {
			var name = this.LocalClassName;

			var noti = new Notification.Builder(this)
				.SetContentTitle(methodName + " " + name).SetAutoCancel(true)
										.SetSmallIcon(Resource.Drawable.ic_action_logo)
										.SetContentText(name).Build();

			var notificationManager = (NotificationManager) GetSystemService(NotificationService);
			notificationManager.Notify((int) CurrentTimeMillis(), noti);
		}

		private static readonly DateTime Jan1st1970 = new DateTime
			(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static long CurrentTimeMillis()
		{
			return (long) (DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}

	}

	/// <summary>
	/// Base list activity.
	/// </summary>
	public abstract class BaseListActivity : ListActivity 
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			the_view_model.IsBusyChanged +=	(sender, e) => {
				if (the_view_model.IsBusy) {
					progress = ProgressDialog.Show (this, "Loading...", "Busy", true);
				} else {
					progress.Dismiss ();
				}
			};
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


		protected void ResurrectActivity(Type activityType)
		{
			var i = new Intent(Global.Current, activityType);
			i.SetFlags(ActivityFlags.ReorderToFront);
			StartActivity(i);
		}


		protected void ShowToast(bool really)
		{
			if(really)
			{
				var t = Toast.MakeText(this, "A toast", ToastLength.Short);
				t.SetGravity(GravityFlags.Center, 0, 0);
				t.Show();
			}
		}

		protected abstract ViewModelBase the_view_model { get;}


		private ProgressDialog progress;


		protected override void OnPause ()
		{
			base.OnPause ();
			Global.Current.suspend ();
		}

		protected override void OnStop ()
		{
			base.OnStop ();

		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();

			Finish ();

		}

		protected override void OnStart ()
		{
			base.OnStart ();

		}

		protected override void OnResume ()
		{
			base.OnResume ();
			Global.Current.resume ();
		}

	}
}