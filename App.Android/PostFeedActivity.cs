using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Xamarin.ActionBarPullToRefresh.Library;
using System.Threading.Tasks;
using App.Common;

namespace App.Android
{
	[Activity (Label = "@string/app_name", 
				MainLauncher = true, 
				ScreenOrientation = ScreenOrientation.Portrait)]

	public class PostFeedActivity : BaseActivity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Title = "Feed";		

			SetContentView (Resource.Layout.PostList);

			postListView = FindViewById<ListView> (Resource.Id.list);

			postListView.EmptyView = FindViewById<LinearLayout> (Resource.Id.empty);

			#region setup list adapter
			postListAdapter = new PostFeedAdapter (this, view_model.Posts);
			//----------
			//Hook up our adapter to our ListView
			//---------
			postListView.Adapter = postListAdapter;
			#endregion

			#region Setup pull to refresh
			mPullToRefreshAttacher = new PullToRefreshAttacher (this, postListView);

			mPullToRefreshAttacher.Refresh += async (sender, e) => {
				await view_model.RefreshPosts ();
				JavaScriptTimer.SetTimeout (delegate {
					RunOnUiThread (() => {
						mPullToRefreshAttacher.SetRefreshComplete ();
					});
				}, 1500);//1.5 secs

			};
			#endregion

			// wire up post click handler
			postListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				var post = view_model.Posts [e.Position];
				var postDetails = PostDetailsActivity.CreateIntent (this, post);
				StartActivity (postDetails);
			};

		}

		private EventHandler<EventArgs> NewPostsReceivedHandler;

		private EventHandler<EventArgs> ConnectionInitialisedHandler;

		protected override void OnResume ()
		{
			base.OnResume ();

			if (AppGlobal.Current.CurrentUserExists) {

				NewPostsReceivedHandler = (object sender, EventArgs e) => {
					//Refresh list view data
					postListAdapter.NotifyDataSetChanged ();
				};

				view_model.OnNewPostsReceived += NewPostsReceivedHandler;

				ConnectionInitialisedHandler = async (object sender, EventArgs e) => {
					//N/B: In [AppGlobal.update_connection], what happens if the update connection executes before binding this event ?
					//Well, Refresh posts in here will never happen, that's what.
					view_model.IsBusy = true;

					await view_model.RefreshPosts ();

					view_model.IsBusy = false;
				};

				AppGlobal.Current.OnConnectionInitialized += ConnectionInitialisedHandler;

			}
			else{
				StartActivity (typeof(LoginActivity));
			}

		}

		protected override void OnPause ()
		{
			base.OnPause ();

			view_model.OnNewPostsReceived -= NewPostsReceivedHandler;
			AppGlobal.Current.OnConnectionInitialized -= ConnectionInitialisedHandler;

		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok) {
				//await view_model.RefreshPosts ();
			}
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			menu.Add ("New Post").SetShowAsAction (ShowAsAction.IfRoom);

			MenuInflater.Inflate (Resource.Menu.menu, menu);

			menu.FindItem (Resource.Id.feed_menu_item).SetEnabled (false);

			return base.OnCreateOptionsMenu (menu);

			//return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.TitleFormatted.ToString ()) { 
			case "New Post":
				StartActivityForResult (typeof(NewPostActivity), 0);
				break;
			case "Console":
				ShowToast (false);
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		private FeedViewModel view_model;

		protected override ViewModelBase init_view_model ()
		{
			if (view_model == null) {
				view_model = new FeedViewModel ();
			}

			return view_model;		
		}

		private PostFeedAdapter postListAdapter;
		private ListView postListView;
		private PullToRefreshAttacher mPullToRefreshAttacher;
	}
}