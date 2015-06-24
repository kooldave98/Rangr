using System;

namespace App.Common
{
	public class PostIdentity : BaseEntity, IPostIdentity
    {
        public long epoch_id { get; set; }

        public long user_id { get; set; }
    }

	public interface IPostIdentity
	{
        long epoch_id { get; set; }

        long user_id { get; set; }
	}
}

