using System;
using System.IO;
using System.Collections.Generic;
using App.Core.Portable.Persistence;
using Newtonsoft.Json;
using Android.App;
using Android.Content;

namespace App.Common
{
	//TODO: Commonise this with the ios one, they are too similar to be duplicated
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
			SaveString (key, output);
			return true;
		}

		public T Load<T>(string key)
		{
			var value = GetString (key);

			if (string.IsNullOrWhiteSpace (value)) {
				return default(T);
			}

			var output = JsonConvert.DeserializeObject<T>(value);

			if (output == null) {
				return default(T);
			}

			return output;
		}


		private string GetString(string key)
		{
			var prefs = Application.Context.GetSharedPreferences(Application.Context.PackageName, FileCreationMode.Private);
			return prefs.GetString(key, string.Empty);
		}

		private void SaveString(string key, string value)
		{
			var prefs = Application.Context.GetSharedPreferences(Application.Context.PackageName, FileCreationMode.Private);
			var prefEditor = prefs.Edit();
			prefEditor.PutString(key, value);
			prefEditor.Commit();
		}


	}
}

