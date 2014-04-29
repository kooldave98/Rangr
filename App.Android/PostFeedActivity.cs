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
using App.Common.Shared;
using App.Core.Android;

namespace App.Android
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]
	public class PostFeedActivity : BaseListActivity
	{
		protected async override void OnCreate (Bundle bundle)
		{
			Title = "Feed";
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.PostList);


			_postListView = ListView;//FindViewById<ListView> (Resource.Id.PostList);
//
//			_postListView.PivotY = 0;
//			_postListView.PivotX =  container.Width;

			//App logic

			view_model = new FeedViewModel (GeoLocation.GetInstance (this), PersistentStorage.Current);

			//setup list adapter
			setupAdapter ();

			view_model.IsBusyChanged +=	(sender, e) => {
				if (view_model.IsBusy) {
					progress = ProgressDialog.Show (this, "Loading...", "Busy", true);
				} else {
					progress.Dismiss ();
				}
			};

			view_model.OnNewPostsReceived += HandleOnNewPostsReceived;

			// wire up post click handler
			if (_postListView != null) {

				setup_pull_to_refresh_on_list (_postListView);

				bind_list_item_click (_postListView);
			}

			await view_model.init ();

		}

		protected override void OnStart ()
		{
			base.OnStart ();
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			refreshGrid ();
		}

		protected override async void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok) {
				await view_model.RefreshPosts ();
			}
		}

		private void HandleOnNewPostsReceived (object sender, EventArgs e)
		{
			refreshGrid ();
		}

		private void setupAdapter()
		{
			// create our adapter
			ListAdapter = _postListAdapter = new PostFeedAdapter (this, view_model.Posts);

			

			//Hook up our adapter to our ListView
			_postListView.Adapter = _postListAdapter;
		}

		private void refreshGrid ()
		{
			_postListAdapter.NotifyDataSetChanged ();

		}

		private void setup_pull_to_refresh_on_list (ListView list_view)
		{
			mPullToRefreshAttacher = new PullToRefreshAttacher (this, list_view);

			// Set Listener to know when a refresh should be started
			mPullToRefreshAttacher.Refresh += async (sender, e) => {
				await view_model.RefreshPosts ();
				JavaScriptTimer.SetTimeout(delegate{
					RunOnUiThread(()=>{
						mPullToRefreshAttacher.SetRefreshComplete ();
					});
				},1500);//1.5 secs

			};
		}

		private void bind_list_item_click (ListView list_view)
		{
			list_view.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				var post = view_model.Posts [e.Position];
				var postDetails = PostDetailsActivity.CreateIntent (this, post);
				StartActivity (postDetails);
			};
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			menu.Add ("New Post").SetShowAsAction (ShowAsAction.IfRoom);

			MenuInflater.Inflate (Resource.Menu.menu, menu);

			menu.FindItem (Resource.Id.feed_menu_item).SetEnabled(false);

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

		private FeedViewModel view_model {
			get {
				return Global.Current.Feed;
			}
			set {
				Global.Current.Feed = value;
			}
		}

		private ProgressDialog progress;
		private PostFeedAdapter _postListAdapter;
		private ListView _postListView;
		private PullToRefreshAttacher mPullToRefreshAttacher;
	}
}


