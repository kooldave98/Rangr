﻿using System;

namespace App.Core.Portable.Models
{
	public class Post : PostIdentity
	{
		public string text { get; set;}

		public DateTime date { get; set;}

		public int user_id { get; set;}

		public string user_display_name{ get; set;}

		public string geolocation { get; set;}
	}
}

