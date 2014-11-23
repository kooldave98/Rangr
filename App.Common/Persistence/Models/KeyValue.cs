using System;
using SQLite;

namespace App.Common
{
	public class KeyValue : BaseEntity
	{
		public string Key { get; set; }

		public string UserDisplayName { get; set; }

		public int UserID { get; set; }
	}
}

