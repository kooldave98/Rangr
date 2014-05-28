using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Android.App;
using App.Common;
using App.Common.Shared;
using App.Core.Portable.Models;
using Android.Content;

namespace App.Android
{

	[Application (Label = "@string/app_name", Theme = "@style/CustomHoloTheme")]//, Icon="@drawable/Icon")]
	public class Global : global::Android.App.Application
	{
		public LoginViewModel Login_View_Model { get; set; }

		public FeedViewModel Feed_View_Model { get; set; }

		public PeopleViewModel People_View_Model { get; set; }

		public ProfileViewModel Profile_View_Model { get; set; }

		public static Global Current {
			get {
				return _instance;
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

	}

}

