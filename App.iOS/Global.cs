using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using App.Core.Portable.Models;
using App.Common.Shared;
using System.Globalization;
using MonoTouch.Foundation;

namespace App.iOS
{
	public class Global
	{
		//public IList<Post> Posts { get; set;}

		public CommonClient client { get; set;}

		private static Global _instance = null;
		public static Global Current { get { return _instance ?? (_instance = new Global ()); } }
		private Global ()
		{
		}

		public static readonly TimeSpan ForceLoginTimespan = TimeSpan.FromMinutes (5);

		public static DateTime? LastUseTime {
			get {
				return GetDateTime ("LastUseTime");
			}
			set {
				SetDateTime ("LastUseTime", value);
			}
		}

		private static DateTime? GetDateTime (string key)
		{
			var s = NSUserDefaults.StandardUserDefaults.StringForKey (key);
			if (string.IsNullOrEmpty (s)) {
				return null;
			}
			else {
				DateTime dt;
				if (DateTime.TryParse (s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt)) {
					return dt;
				}
				else {
					return null;
				}
			}
		}

		private static void SetDateTime (string key, DateTime? value)
		{
			if (value.HasValue) {
				NSUserDefaults.StandardUserDefaults.SetString (
					value.Value.ToString ("o", CultureInfo.InvariantCulture),
					key);
			}
			else {
				NSUserDefaults.StandardUserDefaults.RemoveObject (key);
			}
			NSUserDefaults.StandardUserDefaults.Synchronize ();
		}

	}
}