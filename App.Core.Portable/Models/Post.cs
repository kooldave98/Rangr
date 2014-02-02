using System;

namespace App.Core.Portable.Models
{
	public class Post
	{
		public int ID { get; set;}

		public string Text { get; set;}

		public DateTime Date { get; set;}

		public int UserID { get; set;}

		public string UserDisplayName{ get; set;}

		public string GeoLocationString { get; set;}
	}
}

