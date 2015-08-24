using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

#if __ANDROID__
using Android.App;
using Android.Content;


#else
using Foundation;
#endif


namespace App.Common
{
    //TODO: Commonise this with the ios one, they are too similar to be duplicated
    public class PersistentStorage : SingletonBase<PersistentStorage>
    {
        private PersistentStorage() {  }

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
            SaveString(key, output);
            return true;
        }

        public T Load<T>(string key)
        {
            var value = GetString(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                return default(T);
            }

            var output = JsonConvert.DeserializeObject<T>(value);

            if (output == null)
            {
                return default(T);
            }

            return output;
        }

        #if __ANDROID__
		private string GetString (string key)
		{
			var prefs = Application.Context.GetSharedPreferences (Application.Context.PackageName, FileCreationMode.Private);
			return prefs.GetString (key, string.Empty);
		}

		private void SaveString (string key, string value)
		{
			var prefs = Application.Context.GetSharedPreferences (Application.Context.PackageName, FileCreationMode.Private);
			var prefEditor = prefs.Edit ();
			prefEditor.PutString (key, value);
			prefEditor.Commit ();
		}
		
		
#else
        private static string GetString(string key)
        {
            var s = NSUserDefaults.StandardUserDefaults.StringForKey(key);
            return s;
        }

        private static void SaveString(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                NSUserDefaults.StandardUserDefaults.SetString(value, key);
            }
            else
            {
                NSUserDefaults.StandardUserDefaults.RemoveObject(key);
            }
            NSUserDefaults.StandardUserDefaults.Synchronize();
        }
        #endif






    }
}

