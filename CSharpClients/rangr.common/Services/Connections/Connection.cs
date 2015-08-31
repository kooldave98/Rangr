using System;

namespace rangr.common
{
	public class Connection : ConnectionIdentity, IUser
	{
		public string user_id { get; set; }

		public string user_display_name { get; set; }

		public string long_lat_acc_geo_string { get; set; }

		public string user_status_message { get; set; }

		public string telephone_number { get; set; }

		public string twitter_name { get; set; }

		public string image_url { get; set; }

	}
}