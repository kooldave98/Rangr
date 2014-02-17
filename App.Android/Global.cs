using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using System.IO;
using App.Core.Portable.Models;
using App.Common.Shared;

namespace App.Android
{
	[Application (Label = "GeoWalkr", Theme = "@style/CustomHoloTheme", Icon="@drawable/Icon")]
	public class Global : global::Android.App.Application {

		public static IList<Post> Posts { get; set;}

		public static CommonClient client { get; set;}

		public Global(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
			: base(handle, transfer) {
		}

		public override void OnCreate()
		{
			base.OnCreate();
		}

		public static DateTime LastUseTime { get; set; }

		public static readonly TimeSpan ForceLoginTimespan = TimeSpan.FromMinutes (5);
	}
}

