using System;

namespace App.Common
{
	public class Post : PostIdentity
	{
		public string text { get; set; }

		public DateTime date { get; set; }

		public int user_id { get; set; }

		public string user_display_name{ get; set; }

		public string geolocation { get; set; }

		public int geolocation_accuracy_in_metres { get; set; }
	}
}

