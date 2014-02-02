using App.Core.Portable.Persistence;
using SQLite;
using System;
using System.IO;
using System.Collections.Generic;

namespace App.Android
{
	public class PersistentStorage
	{
		EntityDatabase db;

		private PersistentStorage ()
		{
			var sqliteFilename = "Walkr.db3";
			string libraryPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine (libraryPath, sqliteFilename);
			var conn = new SQLiteConnection (dbPath);
			db = new EntityDatabase (conn);
		}

		public KeyValue GetKeyValue (int id)
		{
			return db.GetItem<KeyValue> (id);
		}

		public IEnumerable<KeyValue> GetKeyValues ()
		{
			return db.GetItems<KeyValue> ();
		}

		public int SaveKeyValue (KeyValue item)
		{
			return db.SaveItem<KeyValue> (item);
		}

		public int DeleteKeyValue (int id)
		{
			return db.DeleteItem<KeyValue> (id);
		}

		private static PersistentStorage _instance = null;

		public static PersistentStorage Current {
			get {
				return _instance ?? (_instance = new PersistentStorage ());
			}
		}
	}
}

