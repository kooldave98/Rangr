using System;
using common_lib;

namespace App.Common
{
	public class PostIdentity : IPostIdentity
    {
        public long epoch_id { get; set; }

        public string user_id { get; set; }
    }

	public interface IPostIdentity
	{
        long epoch_id { get; set; }

        string user_id { get; set; }
	}
}

