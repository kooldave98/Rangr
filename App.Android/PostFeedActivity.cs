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
	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
	//Remember to use the Tab Layout from the Standard/Content Controls in the Sandbox as a test first.
	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
	[Activity (Label = "Walkr", ScreenOrientation = ScreenOrientation.Portrait)]
	public class PostFeedActivity : Activity
	{
		protected async override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Main);

			//ActionBar.SetDisplayHomeAsUpEnabled (true);

			_postListView = FindViewById<ListView> (Resource.Id.PostList);

			//App logic

			view_model = new FeedViewModel (GeoLocation.GetInstance (this), PersistentStorage.Current);

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

		private void refreshGrid ()
		{
			// create our adapter
			_postListAdapter = new PostListAdapter (this, view_model.Posts);

			//Hook up our adapter to our ListView
			_postListView.Adapter = _postListAdapter;
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
			menu.Add ("Console");
			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.TitleFormatted.ToString ()) { 
			case "New Post":
				StartActivityForResult (typeof(NewPostActivity), 0);
				break;
			case "Console":
				MenuItemClicked (item);
				break;
			case "Stream":
				MenuItemClicked (item);
				break;
			}

			return base.OnOptionsItemSelected (item);
		}

		private void MenuItemClicked (IMenuItem menu_item)
		{
			//var menu_item_string = menu_item.TitleFormatted.ToString ();

			//			if (consoleLayout.Visibility == ViewStates.Gone) {
			//				//consoleLayout.LayoutParameters.Height = 1000;
			//				streamLayout.Visibility = ViewStates.Gone;
			//				consoleLayout.Visibility = ViewStates.Visible;
			//				menu_item.SetTitle ("Stream");
			//			} else {
			//				consoleLayout.Visibility = ViewStates.Gone;
			//				streamLayout.Visibility = ViewStates.Visible;
			//				menu_item.SetTitle ("Console");
			//			}
			//			Console.WriteLine(menu_item_string + " option menuitem clicked");
			//			var t = Toast.MakeText(this, "Options Menu '"+menu_item_string+"' clicked", ToastLength.Short);
			//			t.SetGravity(GravityFlags.Center, 0, 0);
			//			t.Show();
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
		private PostListAdapter _postListAdapter;
		private ListView _postListView;
		private PullToRefreshAttacher mPullToRefreshAttacher;
	}
}


