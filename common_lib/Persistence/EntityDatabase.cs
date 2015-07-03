using System;
using System.IO;
using SQLite;
using System.Linq;
using System.Collections.Generic;

namespace common_lib
{
    public class EntityDatabase
    {
        private static object locker = new object();

        private SQLiteConnection database;

        private static EntityDatabase _instance = null;

        public static EntityDatabase Current
        {
            get
            {
                return _instance ?? (_instance = new EntityDatabase(new SQLiteConnection(dbPath())));
            }
        }

        private static string dbPath()
        {
            var sqliteFilename = "Walkr.db3";
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(libraryPath, sqliteFilename);
        }

        /// <summary>
        /// Initializes a new instance of the AppDatabase. 
        /// if the database doesn't exist, it will create the database and all the tables.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
        private EntityDatabase(SQLiteConnection conn)
        {
            database = conn;
            // create the Db tables here
            //database.CreateTable<Post>();
        }

        public IEnumerable<T> GetItems<T>() where T : IBaseEntity, new()
        {
            lock (locker)
            {
                return (from i in database.Table<T>()
                                    select i).ToList();
            }
        }

        public T GetItem<T>(int id) where T : IBaseEntity, new()
        {
            lock (locker)
            {
                return database.Table<T>().FirstOrDefault(x => x.ID == id);
                // Following throws NotSupportedException - thanks aliegeni
                //return (from i in Table<T> ()
                //        where i.ID == id
                //        select i).FirstOrDefault ();
            }
        }

        public int SaveItem<T>(T item) where T : IBaseEntity
        {
            lock (locker)
            {
                if (item.ID != 0)
                {
                    database.Update(item);
                    return item.ID;
                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        public int DeleteItem<T>(int id) where T : IBaseEntity, new()
        {
            lock (locker)
            {
                return database.Delete<T>(new T() { ID = id });
            }
        }
    }

    public interface IBaseEntity
    {
        int ID { get; set; }
    }

    public abstract class BaseEntity : IBaseEntity
    {
        public BaseEntity()
        {
        }

        /// <summary>
        /// Gets or sets the Database ID.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
    }
}

//        public KeyValue GetKeyValue(int id)
//        {
//            return db.GetItem<KeyValue>(id);
//        }
//
//        public IEnumerable<KeyValue> GetKeyValues()
//        {
//            return db.GetItems<KeyValue>();
//        }
//
//        public int SaveKeyValue(KeyValue item)
//        {
//            return db.SaveItem<KeyValue>(item);
//        }
//
//        public int DeleteKeyValue(int id)
//        {
//            return db.DeleteItem<KeyValue>(id);
//        }

