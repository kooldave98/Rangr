using System;
using System.Collections.Generic;

namespace App.Core.Portable.Models
{
	public class ConnectionIdentity
	{
		public int connection_id { get; set;}
	}

	public class ConnectionComparer : IEqualityComparer<Connection>
	{
		public bool Equals (Connection x, Connection y)
		{
			return (x != null || y != null) ? x.connection_id.Equals(y.connection_id) : false;
		}
		public int GetHashCode (Connection obj)
		{
			return obj.connection_id.GetHashCode();
		}
	}
}

