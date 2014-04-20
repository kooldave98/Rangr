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

		public void Deserialize(byte[] postBytes)
		{
			var serializer = new System.Xml.Serialization.XmlSerializer (typeof(SeenPost));

			CurrentPost = (SeenPost)serializer.Deserialize (new MemoryStream (postBytes));
		}

		public static byte[] Serialize(SeenPost post)
		{
			var serializer = new System.Xml.Serialization.XmlSerializer (typeof(SeenPost));
			var postStream = new MemoryStream ();
			serializer.Serialize (postStream, post);

			return postStream.ToArray ();
		}

		public PostDetailsViewModel ()
		{
			CurrentPost = new SeenPost ();
		}
	}
}

