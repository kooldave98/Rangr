using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Android.App;
using App.Common;
using App.Common.Shared;
using App.Core.Portable.Models;

namespace App.Android
{
	[Application (Label = "@string/app_name", Theme = "@style/CustomHoloTheme")]//, Icon="@drawable/Icon")]
	public class Global : global::Android.App.Application
	{
		public FeedViewModel Feed { get; set; }

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

	}
}

