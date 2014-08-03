using System;

namespace App.Core.Portable
{
	public interface IUser
	{
		string user_display_name { get; set;}

		string user_status_message { get; set;}

		string telephone_number { get; set;}

		string twitter_name { get; set;}

		string image_url { get; set;}
	}
}

