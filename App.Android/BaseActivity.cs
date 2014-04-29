using System;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace App.Android 
{
	public class BaseActivity : Activity 
	{
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

	}

	public class BaseListActivity : ListActivity 
	{
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

	}
}