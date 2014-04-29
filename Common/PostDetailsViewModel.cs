using System;
using App.Core.Portable.Models;
using System.IO;
using App.Core.Portable.Device;
using App.Core.Portable.Persistence;
using App.Common.Shared;

namespace App.Common
{
	public class PostDetailsViewModel : ViewModelBase
	{
		public SeenPost CurrentPost { get; set;}

		public PostDetailsViewModel (SeenPost the_seen_post)
		{
			CurrentPost = the_seen_post;
		}

		public PostDetailsViewModel ()
		{
			CurrentPost = new SeenPost();
		}

		#region ReUseableHelpers
		public static SeenPost Deserialize(byte[] postBytes)
		{
			var serializer = new System.Xml.Serialization.XmlSerializer (typeof(SeenPost));

			return (SeenPost)serializer.Deserialize (new MemoryStream (postBytes));
		}

		public static byte[] Serialize(SeenPost post)
		{
			var serializer = new System.Xml.Serialization.XmlSerializer (typeof(SeenPost));
			var postStream = new MemoryStream ();
			serializer.Serialize (postStream, post);

			return postStream.ToArray ();
		}
		#endregion
	}
}

