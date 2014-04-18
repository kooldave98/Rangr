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

namespace App.Android
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]
	public class ProfileActivity : BaseActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.menu, menu);

			menu.FindItem (Resource.Id.profile_menu_item).SetEnabled(false);

			return base.OnCreateOptionsMenu (menu);
		}
	}
}

