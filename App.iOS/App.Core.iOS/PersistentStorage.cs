using System;
using System.Globalization;
using MonoTouch.Foundation;
using App.Core.Portable.Persistence;
using Newtonsoft.Json;

namespace App.iOS
{
	public class PersistentStorage : IPersistentStorage
	{
		private static IPersistentStorage _instance = null;

		public static IPersistentStorage Current
		{
			get{
				return _instance ?? (_instance = new PersistentStorage ());
			}
		}

		private PersistentStorage()
		{

		}

		public bool Clear(string key)
		{
			if (null == key)
				return false;

			return true;
		}

		public bool Save(string key, object value)
		{
			if (null == value)
				return false;

			var output = JsonConvert.SerializeObject(value);
			ios_set_item (key, output);
			return true;
		}

		public T Load<T>(string key)
		{
			var value = ios_get_item (key);

			if (string.IsNullOrWhiteSpace (value)) {
				return default(T);
			}

			var output = JsonConvert.DeserializeObject<T>(value);

			if (output == null) {
				return default(T);
			}

			return output;
		}


		private static string ios_get_item (string key)
		{
			var s = NSUserDefaults.StandardUserDefaults.StringForKey (key);
			return s;
		}

		private static void ios_set_item (string key, string value)
		{
			if (!string.IsNullOrWhiteSpace(value)) {
				NSUserDefaults.StandardUserDefaults.SetString (value, key);
			}else {
				NSUserDefaults.StandardUserDefaults.RemoveObject (key);
			}
			NSUserDefaults.StandardUserDefaults.Synchronize ();
		}


	}
}