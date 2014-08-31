using System;

namespace App.Core.Portable.Models
{
	public class Connection : ConnectionIdentity, IUser
	{
		public int user_id { get; set;}

		public string user_display_name { get; set;}

		public string geolocation_string { get; set;}

		public string geolocation_accuracy_in_metres { get; set;}

		public double? distance_from_context_in_metres { get; set;}

		public string user_status_message { get; set;}

		public string telephone_number { get; set;}

		public string twitter_name { get; set;}

		public string image_url { get; set;}

	}
}