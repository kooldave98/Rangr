using System;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using App.Core.Portable.Models;
using App.Common.Shared;

namespace App.iOS
{
	public class Global
	{
		//public IList<Post> Posts { get; set;}

		public CommonClient client { get; set;}

		private static Global _instance = null;
		public static Global Current { get { return _instance ?? (_instance = new Global ()); } }
		private Global ()
		{
		}

	}
}