using System;

namespace App.Core.Portable.Models
{
	public class Connection : ConnectionIdentity
	{
		public int user_id { get; set;}

		public string user_display_name { get; set;}

		public string geolocation_string { get; set;}

		public string geolocation_accuracy_in_metres { get; set;}

	}
}

