using System;

namespace App.Common
{
	public class Post : PostIdentity
	{
		public string text { get; set; }

		public DateTime date { get; set; }

		public int user_id { get; set; }

		public string user_display_name{ get; set; }

		public string long_lat_acc_geo_string { get; set; }

        public string image_url { get; set; }
	}
}

