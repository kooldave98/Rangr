using System;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Core.Portable.Persistence;

namespace App.Common
{
	//The ios workarounds are because I didn't want to be repopulating the list each time.
	//So I am caching the latest posts every time a pull is made.
	public class FeedViewModel : ViewModelBase
	{
		public int start_index { get; set; }

		//1.1non android breaking ios workaround
		public IList<SeenPost> LatestPosts { get; set; }
		//end workaround

		public IList<SeenPost> Posts { get; set; }

		public async Task RefreshPosts ()
		{
			//Todo: Need to guard Get Current Connection

			LatestPosts = await SeenPostServices.Get (_sessionInstance.GetCurrentConnection ().connection_id.ToString (), start_index.ToString ());
			

			foreach (var post in LatestPosts) {
				start_index = post.id + 1;

				Posts.Insert (0, post);
			}

			if (OnNewPostsReceived != null) {
				OnNewPostsReceived (this, EventArgs.Empty);
			}
		}

		public event EventHandler<EventArgs> OnNewPostsReceived;

		public FeedViewModel ()
		{
			Posts = new List<SeenPost> ();

			//1.3 non android breaking ios workaround
			LatestPosts = new List<SeenPost> ();
			//end workaround


			_sessionInstance = Session.GetInstance ();

			SeenPostServices = new SeenPosts (HttpRequest.Current);

		}

		ISession _sessionInstance;
		SeenPosts SeenPostServices;
	}
}

