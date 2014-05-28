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
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Title = "Feed";		

			SetContentView (Resource.Layout.PostList);


			_postListView = ListView;//FindViewById<ListView> (Resource.Id.PostList);


			//setup list adapter
			setupAdapter ();


			// wire up post click handler
			if (_postListView != null) {

				setup_pull_to_refresh_on_list (_postListView);

				bind_list_item_click (_postListView);
			}



		}

		protected override async void OnResume ()
		{
			base.OnResume ();

			view_model.OnNewPostsReceived += HandleOnNewPostsReceived;

			await view_model.RefreshPosts ();

		}

		protected override void OnPause ()
		{
			base.OnPause ();

			view_model.OnNewPostsReceived -= HandleOnNewPostsReceived;

		}

		protected override async void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok) {
				//await view_model.RefreshPosts ();
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

		private FeedViewModel view_model
		{
			get {
				return (FeedViewModel)the_view_model;
			}
		}

		protected override ViewModelBase the_view_model {
			get 
			{
				if(Global.Current.Feed_View_Model == null)
				{
					Global.Current.Feed_View_Model = new FeedViewModel (GeoLocation.GetInstance (Global.Current), PersistentStorage.Current);
				}

				return Global.Current.Feed_View_Model;
			}
		}

		//private ProgressDialog progress;
		private PostFeedAdapter _postListAdapter;
		private ListView _postListView;
		private PullToRefreshAttacher mPullToRefreshAttacher;
	}
}


