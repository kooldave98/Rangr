using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
					await PostServices
							.Get (connection_id, 
						forward_start_index.ToString (), 
						first_load: first_load);

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
					await PostServices
							.Get (connection_id, 
						backward_start_index.ToString (), 
						first_load: first_load, 
						traversal: CollectionTraversal.Older);

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

		private int forward_start_index {
			get{ return Posts.FirstOrDefault () != null ? Posts.First ().post_id + 1 : 0; }
		}

		private int backward_start_index {
			get{ return Posts.LastOrDefault () != null ? Posts.Last ().post_id - 1 : 0; }
		}

		private string connection_id {
			get{ return _sessionInstance.GetCurrentConnection ().connection_id.ToString (); }
		}

		public event EventHandler<EventArgs> OnNewPostsReceived;

		public FeedViewModel ()
		{
			Posts = new List<Post> ();

			_sessionInstance = Session.GetInstance ();

			PostServices = new Posts ();
		}

		Session _sessionInstance;
		Posts PostServices;
	}
}

