using SQLite;
using System;
using System.IO;
using System.Collections.Generic;

namespace App.Common
{
	/// <summary>
	/// This class was how I was storing user prefrences using a SQLite DB
	/// because I didn't know how to use App prefrences on ANdroid
	/// I am keeping this file as a reference for when I need to remember how to use SQLite
	/// </summary>
	public class DbORM
	{

		EntityDatabase db;

		private DbORM ()
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

		//private static PersistentStorage _instance = null;

	}
}

