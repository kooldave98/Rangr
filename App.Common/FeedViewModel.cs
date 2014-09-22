using System;
using System.Linq;
using App.Core.Portable.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Core.Portable.Persistence;

namespace App.Common
{
	//The ios workarounds are because I didn't want to be repopulating the list each time.
	//So I am caching the latest posts every time a pull is made.

	public class FeedViewModel : ViewModelBase
	{
		public enum RefreshDirection
		{
			FORWARD,
			BACKWARD
		}
		//public int start_index { get; set; }

		//1.1non android breaking ios workaround
		public IList<Post> LatestPosts { get; set; }
		//end workaround

		public IList<Post> Posts { get; set; }

		private bool first_load = true;

		public async Task RefreshPosts ()
		{
			if (!first_load) {
				//Todo: Need to guard Get Current Connection
				var newer_posts = 
					LatestPosts = 
						await PostServices.Get (connection_id, 
						forward_start_index.ToString (), 
						backward_traversal: false, 
						latest_entries: first_load);

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

		public async Task OlderPosts ()
		{
			if (first_load || backward_start_index > 0) {
				var older_posts = 
					await PostServices
							.Get (connection_id, 
						backward_start_index.ToString (), 
						backward_traversal: true, 
						latest_entries: first_load);

				foreach (var post in older_posts) {
					first_load = false;
					Posts.Add (post);
				}
			}

			if (OnNewPostsReceived != null) {
				OnNewPostsReceived (this, EventArgs.Empty);
			}
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

			//1.3 non android breaking ios workaround
			LatestPosts = new List<Post> ();
			//end workaround


			_sessionInstance = Session.GetInstance ();

			PostServices = new Posts ();
		}

		Session _sessionInstance;
		Posts PostServices;
	}
}

