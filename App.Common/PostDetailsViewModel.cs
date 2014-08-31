using System;
using App.Core.Portable.Models;
using System.IO;
using App.Core.Portable.Device;
using App.Core.Portable.Persistence;

namespace App.Common
{
	public class PostDetailsViewModel : ViewModelBase
	{
		public Post CurrentPost { get; set;}

		public PostDetailsViewModel (Post the_seen_post)
		{
			CurrentPost = the_seen_post;
		}

		public PostDetailsViewModel ()
		{
			CurrentPost = new Post();
		}

		#region ReUseableHelpers
		public static Post Deserialize(byte[] postBytes)
		{
			var serializer = new System.Xml.Serialization.XmlSerializer (typeof(Post));

			return (Post)serializer.Deserialize (new MemoryStream (postBytes));
		}

		public static byte[] Serialize(Post post)
		{
			var serializer = new System.Xml.Serialization.XmlSerializer (typeof(Post));
			var postStream = new MemoryStream ();
			serializer.Serialize (postStream, post);

			return postStream.ToArray ();
		}
		#endregion
	}
}

