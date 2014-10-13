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
using System.Threading.Tasks;
using App.Common;
using CustomViews;
using App.Core.Portable.Models;
using Android.Support.V4.Widget;

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

			postListView = FindViewById<EndlessListView> (Resource.Id.list);

			//postListView.EmptyView = FindViewById<LinearLayout> (Resource.Id.empty);
			postListView.SetListEndLoadingView (Resource.Layout.loading_layout);


			#region setup list adapter
			postListAdapter = new PostFeedAdapter (this, view_model.Posts);
			//----------
			//Hook up our adapter to our ListView
			//---------
			postListView.Adapter = postListAdapter;
			#endregion


			#region Setup pull to refresh

			refresher = FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);
			refresher.SetColorScheme (Resource.Color.xam_dark_blue, Resource.Color.xam_purple, 
				Resource.Color.xam_gray, Resource.Color.xam_green);

			refresher.Refresh += async (sender, e) => {
				await view_model.RefreshPosts ();
				JavaScriptTimer.SetTimeout (delegate {
					RunOnUiThread (() => {
						refresher.Refreshing = false;
					});
				}, 1500);//1.5 secs

			};
				
			#endregion

			postListView.OnListEndReached += async (sender, e) => {
				await view_model.OlderPosts ();
				JavaScriptTimer.SetTimeout (delegate {
					RunOnUiThread (() => {
						((EndlessListView)sender).SetEndlessListLoaderComplete ();
					});
				}, 1500);//1.5 secs
			};

			// wire up post click handler
			postListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				var post = view_model.Posts [e.Position];
				var postDetails = PostDetailsActivity.CreateIntent (this, post);
				StartActivity (postDetails);
			};

		}

		private EventHandler<EventArgs> NewPostsReceivedHandler;

		private EventHandler<EventArgs> GeoLocatorRefreshedHandler;

		private void show_refresher ()
		{
			refresher.Refreshing = true;
		}

		private void dismiss_refresher ()
		{
			refresher.Refreshing = false;
		}

		protected override void OnResume ()
		{
			//Doing this before to prevent the blank screen
			//cause the base can take a while
			show_refresher ();

			base.OnResume ();

			if (AppGlobal.Current.CurrentUserAndConnectionExists) {

				NewPostsReceivedHandler = (object sender, EventArgs e) => {
					//Refresh list view data
					RunOnUiThread (() => {
						postListAdapter.NotifyDataSetChanged ();
					});
				};

				view_model.OnNewPostsReceived += NewPostsReceivedHandler;

				GeoLocatorRefreshedHandler = async (object sender, EventArgs e) => {
					await view_model.RefreshPosts ();

					JavaScriptTimer.SetTimeout (delegate {
						dismiss_refresher ();
					}, 500);//1/2 a second
				};

				AppGlobal.Current.GeoLocatorRefreshed += GeoLocatorRefreshedHandler;
			}

		}

		protected override void OnPause ()
		{
			base.OnPause ();
			dismiss_refresher ();
			view_model.OnNewPostsReceived -= NewPostsReceivedHandler;
			AppGlobal.Current.GeoLocatorRefreshed -= GeoLocatorRefreshedHandler;
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
				ShowToast ("Console not available");
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
		private EndlessListView postListView;
		private SwipeRefreshLayout refresher;
	}
}