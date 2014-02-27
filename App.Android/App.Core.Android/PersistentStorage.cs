using System;
using System.IO;
using System.Collections.Generic;
using App.Core.Portable.Persistence;
using Newtonsoft.Json;

namespace App.Android
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
			throw new NotImplementedException ("Todo");
//			if (null == key)
//				return false;
//
//			return true;
		}

		public bool Save(string key, object value)
		{
			if (null == value)
				return false;

			var output = JsonConvert.SerializeObject(value);
			Global.Current.SaveString (key, output);
			return true;
		}

		public T Load<T>(string key)
		{
			var value = Global.Current.GetString (key);

			if (string.IsNullOrWhiteSpace (value)) {
				return default(T);
			}

			var output = JsonConvert.DeserializeObject<T>(value);

			if (output == null) {
				return default(T);
			}

			return output;
		}
	}
}

