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
using rangr.common;

namespace rangr.droid
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]
	public class ProfileActivity : BaseActivity
	{

		protected override void OnCreate (Bundle bundle)
		{
			Title = "My Profile";
			base.OnCreate (bundle);

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

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok) {

				//Nuke our cache: Unideal implementation

				view_model = null;
				init_view_model ();


				//refresh list view
				listView.Adapter = listAdapter = new ProfileAdapter (view_model.PropertyGroups);
				listAdapter.NotifyDataSetChanged ();
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
            MenuInflater.Inflate (Resource.Menu.profile, menu);

			return base.OnCreateOptionsMenu (menu);
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
            switch (item.ItemId) { 
                case Resource.Id.edit_profile_menu_item:
				StartActivityForResult (typeof(EditProfileActivity), 0);
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		private ProfileAdapter listAdapter;
		private ListView listView;

		private ProfileViewModel view_model;

		protected override ViewModelBase init_view_model ()
		{

			if (view_model == null) {
				view_model = new ProfileViewModel ();
			}

			return view_model;

		}
	}
}

