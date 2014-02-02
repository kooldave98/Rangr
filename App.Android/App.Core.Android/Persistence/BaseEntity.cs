using System;
using SQLite;

namespace App.Android
{
	/// <summary>
	/// Business entity base class. Provides the ID property.
	/// </summary>
	public abstract class BaseEntity : IBaseEntity {
		public BaseEntity ()
		{
		}

		/// <summary>
		/// Gets or sets the Database ID.
		/// </summary>
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
	}
}

