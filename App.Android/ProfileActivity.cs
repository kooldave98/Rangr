using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using App.Common;
using App.Core.Android;

namespace App.Android
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]
	public class ProfileActivity : BaseActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			Title = "My Profile";
			base.OnCreate (bundle);

			view_model = new ProfileViewModel (PersistentStorage.Current);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Profile);

			//
			// Setup the UI
			//
			listView = FindViewById<ListView> (Resource.Id.ProfileList);
			listView.Divider = null;

			// create our adapter
			listAdapter = new ProfileAdapter (view_model.PropertyGroups);

			//Hook up our adapter to our ListView
			listView.Adapter = listAdapter;

		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.menu, menu);

			menu.FindItem (Resource.Id.profile_menu_item).SetEnabled (false);

			return base.OnCreateOptionsMenu (menu);
		}

		private ProfileAdapter listAdapter;
		private ListView listView;

		private ProfileViewModel view_model;
	}
}

