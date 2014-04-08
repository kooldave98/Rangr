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
		public ConnectionIdentity current_connection { get; set; }

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
			//Incase of any problems see Employee Directory app
			var s = PersistentStorage.Current.Load<DateTime?> (key);
			return s;
		}

		private static void SetDateTime (string key, DateTime? value)
		{
			//Incase of any problems see Employee Directory app
			PersistentStorage.Current.Save (key, value);			
		}
	}
}