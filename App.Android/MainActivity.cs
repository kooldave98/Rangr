using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using App.Common.Shared;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using System.Linq;
using System.Collections.Generic;
using Android.Content.PM;

namespace App.Android
{
	[Activity (Label = "Walkr", ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{
		Global _global;
		PostListAdapter _postListAdapter;
		//IList<Post> _posts;
		Button _addPostButton;

		ListView _postListView;
		TextView _textView;
		IGeoLocation _geoLocationInstance;
		ISession _sessionInstance;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			_global = Global.Current;
			_global.Posts = new List<Post> ();

			_geoLocationInstance = GeoLocation.GetInstance (SynchronizationContext.Current, this);
			_sessionInstance = Session.Current;

			//Find our controls
			_postListView = FindViewById<ListView> (Resource.Id.PostList);
			_addPostButton = FindViewById<Button> (Resource.Id.AddButton);

			var consoleLayout = FindViewById<LinearLayout> (Resource.Id.ConsoleLayout);

			consoleLayout.Visibility = ViewStates.Gone;

			consoleLayout.Touch += (object sender, View.TouchEventArgs e) => {
				consoleLayout.LayoutParameters.Height = 1000;
			};


			// wire up add new post button handler
			if (_addPostButton != null) {
				_addPostButton.Click += (sender, e) => {
					StartActivity (typeof(NewPostScreen));
				};
			}


			// wire up post click handler
			if (_postListView != null) {
				_postListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
					var postDetails = new Intent (this, typeof(PostDetailsScreen));
					postDetails.PutExtra("PostID", _global.Posts [e.Position].ID);
					StartActivity (postDetails);
				};
			}

			_textView = FindViewById<TextView> (Resource.Id.textView);


			var traceWriter = new TextViewWriter (SynchronizationContext.Current, _textView);

			_global.client = new CommonClient (traceWriter, _geoLocationInstance, _sessionInstance, SynchronizationContext.Current);

			var task = _global.client.RunAsync ((post) => {
				_global.Posts.Insert(0, post);

				refreshGrid();
			});
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			refreshGrid ();


		}

		private void refreshGrid()
		{
			// create our adapter
			_postListAdapter = new PostListAdapter(this, _global.Posts);

			//Hook up our adapter to our ListView
			_postListView.Adapter = _postListAdapter;
		}
	}
}


