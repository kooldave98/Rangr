using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using System.IO;
using App.Core.Portable.Models;
using App.Common.Shared;

namespace App.Android
{
	[Application (Label = "@string/app_name", Theme = "@style/CustomHoloTheme")]//, Icon="@drawable/Icon")]
	public class Global : global::Android.App.Application
	{
		public IList<SeenPost> Posts { get; set; }

		public ConnectionIdentity current_connection { get; set;}

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

