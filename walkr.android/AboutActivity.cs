
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
using Android.Preferences;
using Android.Content.PM;

namespace walkr.android
{
	[Activity (Label = "About", MainLauncher = true, Icon = "@drawable/ic_launcher",
		ScreenOrientation = ScreenOrientation.Portrait,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]			
	public class SettingsActivity : PreferenceActivity
	{

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			AddPreferencesFromResource (Resource.Xml.preferences_general);

			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.SetDisplayShowHomeEnabled (true);
		}


		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			case Android.Resource.Id.Home:
				Finish ();
				break;
			}
			return base.OnOptionsItemSelected (item);
		}

	}
}

