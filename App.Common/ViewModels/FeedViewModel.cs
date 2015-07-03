using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using common_lib;

namespace App.Common
{
	public class FeedViewModel : ViewModelBase
	{
		public enum RefreshDirection
		{
			FORWARD,
			BACKWARD
		}

		public IList<Post> Posts { get; set; }

		private bool first_load = true;

		public async Task RefreshPosts ()
		{
			if (!first_load) {
				//Todo: Need to guard Get Current Connection
				var newer_posts =
                    await get_posts.Get (
                        new GetPostsByMutualRequest(){
                        user_id = user_id, 
                        start_index = forward_start_index, 
						first_load = first_load
                    });

				foreach (var post in newer_posts) {
					first_load = false;
					Posts.Insert (0, post);
				}

				if (OnNewPostsReceived != null) {
					OnNewPostsReceived (this, EventArgs.Empty);
				}
			} else {

				await OlderPosts ();			
			}
		}

		public async Task<bool> OlderPosts ()
		{
			var older_posts_remaining = true;

			if (first_load || backward_start_index > 0) {
				var older_posts = 
                    await get_posts
							.Get (
                            new GetPostsByMutualRequest(){
                            user_id = user_id, 
                            start_index = backward_start_index, 
						    first_load = first_load, 
                            collection_traversal = CollectionTraversal.Older
                        }
                        );

				foreach (var post in older_posts) {
					first_load = false;
					Posts.Add (post);
				}

				older_posts_remaining = older_posts.Any ();
			}

			if (OnNewPostsReceived != null) {
				OnNewPostsReceived (this, EventArgs.Empty);
			}

			return older_posts_remaining;
		}

		private long forward_start_index {
            get{ return Posts.FirstOrDefault () != null ? Posts.First ().epoch_id + 1 : 0; }
		}

		private long backward_start_index {
            get{ return Posts.LastOrDefault () != null ? Posts.Last ().epoch_id - 1 : 0; }
		}

		private string user_id {
            get{ return _sessionInstance.GetCurrentUser ().user_id; }
		}

		public event EventHandler<EventArgs> OnNewPostsReceived;

		public FeedViewModel ()
		{
			Posts = new List<Post> ();

			_sessionInstance = Session.Current;

            get_posts = new GetPosts ();
		}

		Session _sessionInstance;
		GetPosts get_posts;
	}
}

