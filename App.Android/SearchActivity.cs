
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using CustomViews;
using AndroidResource = Android.Resource;
using App.Common;
using Android.Support.V4.Widget;

namespace App.Android
{
	[Activity (Label = "Search", 
		ScreenOrientation = ScreenOrientation.Portrait)]			
	public class SearchActivity : BaseActivity
	{
		private const string intent_name = "hashtag";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.PostList);

			Title = "Search";

			if (Intent.HasExtra (intent_name)) {
				var hash_tag_name = Intent.GetStringExtra (intent_name);
				Title = hash_tag_name;
				view_model.hash_tag_search_keyword = hash_tag_name;
			} else {
				Finish ();
			}

			ActionBar.SetDisplayHomeAsUpEnabled (true);		



			postListView = FindViewById<EndlessListView> (Resource.Id.list);

			postListView.EmptyView = FindViewById<View> (AndroidResource.Id.Empty);
			postListView.InitEndlessness (Resource.Layout.loading_layout, Resource.Id.loadMoreButton, Resource.Id.loadMoreProgress);

			postListView.OnLoadMoreTriggered += async (sender, e) => {
				await view_model.OlderPosts ();
				JavaScriptTimer.SetTimeout (delegate {
					RunOnUiThread (() => {
						((EndlessListView)sender).SetEndlessListLoaderComplete ();
					});
				}, 1500);//1.5 secs
			};

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


			// wire up post click handler
			postListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				var post = view_model.Posts [e.Position];
				var postDetails = PostDetailsActivity.CreateIntent (this, post);
				StartActivity (postDetails);
			};
		}

		public override bool OnNavigateUp ()
		{
			base.OnNavigateUp ();

			Finish ();

			return true;
		}

		public static Intent CreateIntent (Context context, string hash_tag)
		{
			return new Intent (context, typeof(SearchActivity)).PutExtra (intent_name, hash_tag);
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
			//Simulation
			var persisted_simulation = PersistentStorage.Current.Load<string> ("simulation");
			if (!string.IsNullOrWhiteSpace (persisted_simulation) && persisted_simulation != "L") {
				ShowToast ("Simulated location" + persisted_simulation);
			}

		}

		protected override void OnPause ()
		{
			base.OnPause ();
			dismiss_refresher ();
			view_model.OnNewPostsReceived -= NewPostsReceivedHandler;
			AppGlobal.Current.GeoLocatorRefreshed -= GeoLocatorRefreshedHandler;
		}

		private SearchViewModel view_model;

		protected override ViewModelBase init_view_model ()
		{
			if (view_model == null) {
				view_model = new SearchViewModel ();
			}

			return view_model;		
		}

		private PostFeedAdapter postListAdapter;
		private EndlessListView postListView;
		private SwipeRefreshLayout refresher;
	}
}

