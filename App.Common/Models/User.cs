using System;

namespace App.Common
{
	public class User : UserIdentity, IUser
	{
		public string user_display_name { get; set; }

		public string user_status_message { get; set; }

		public string telephone_number { get; set; }

		public string twitter_name { get; set; }

		public string image_url { get; set; }
	}
}

