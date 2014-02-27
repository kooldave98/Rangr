using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using System.IO;
using App.Core.Portable.Models;
using App.Common.Shared;
using Android.Content;
using Android.Preferences;

namespace App.Android
{
	[Application (Label = "@string/app_name", Theme = "@style/CustomHoloTheme")]//, Icon="@drawable/Icon")]
	public class Global : global::Android.App.Application
	{
		public IList<Post> Posts { get; set; }

		public CommonClient client { get; set; }

		public static Global Current {
			get {
				return _instance;// ?? (_instance = new GeoLocation(activityContext));
			}
		}

		private static Global _instance = null;

		public Global (IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
			: base (handle, transfer)
		{
			_instance = this;
		}

		public override void OnCreate ()
		{
			base.OnCreate ();
		}

		public DateTime LastUseTime { get; set; }

		public readonly TimeSpan ForceLoginTimespan = TimeSpan.FromMinutes (5);

		public string GetString(string key)
		{
			var prefs = Global.Context.GetSharedPreferences(this.PackageName, FileCreationMode.Private);
			return prefs.GetString(key, string.Empty);
		}

		public void SaveString(string key, string value)
		{
			var prefs = Global.Context.GetSharedPreferences(this.PackageName, FileCreationMode.Private);
			var prefEditor = prefs.Edit();
			prefEditor.PutString(key, value);
			prefEditor.Commit();
		}

	}
}

