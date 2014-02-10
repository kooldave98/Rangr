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
		LinearLayout consoleLayout;
		LinearLayout streamLayout;

		ListView _postListView;
		TextView _textView;
		IGeoLocation _geoLocationInstance;
		ISession _sessionInstance;

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			menu.Add("New Post").SetShowAsAction(ShowAsAction.IfRoom);
			menu.Add("Console");
			return true;
		}

		//This is the Android
//		public boolean onOptionsItemSelected(MenuItem item) {
//			// Handle presses on the action bar items
//			switch (item.getItemId()) {
//			case R.id.action_search:
//				openSearch();
//				return true;
//			case R.id.action_compose:
//				composeMessage();
//				return true;
//			default:
//				return super.onOptionsItemSelected(item);
//			}
//		}


		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.TitleFormatted.ToString()) { 
			case "New Post":
				StartActivity (typeof(NewPostScreen)); break;
			case "Console":
				MenuItemClicked(item); break;
			case "Stream":
				MenuItemClicked(item); break;
			}

			return base.OnOptionsItemSelected(item);
		}

		void MenuItemClicked(IMenuItem menu_item)
		{
			var menu_item_string = menu_item.TitleFormatted.ToString ();

			if (consoleLayout.Visibility == ViewStates.Gone) {
				//consoleLayout.LayoutParameters.Height = 1000;
				streamLayout.Visibility = ViewStates.Gone;
				consoleLayout.Visibility = ViewStates.Visible;
				menu_item.SetTitle ("Stream");
			} else {
				consoleLayout.Visibility = ViewStates.Gone;
				streamLayout.Visibility = ViewStates.Visible;
				menu_item.SetTitle ("Console");
			}



//			Console.WriteLine(menu_item_string + " option menuitem clicked");
//			var t = Toast.MakeText(this, "Options Menu '"+menu_item_string+"' clicked", ToastLength.Short);
//			t.SetGravity(GravityFlags.Center, 0, 0);
//			t.Show();
		}


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);


			//ActionBar.SetDisplayHomeAsUpEnabled (true);

			_global = Global.Current;
			_global.Posts = new List<Post> ();

			_geoLocationInstance = GeoLocation.GetInstance (SynchronizationContext.Current, this);
			_sessionInstance = Session.Current;

			//Find our controls
			_postListView = FindViewById<ListView> (Resource.Id.PostList);

			consoleLayout = FindViewById<LinearLayout> (Resource.Id.ConsoleLayout);
			streamLayout = FindViewById<LinearLayout> (Resource.Id.StreamLayout);

			consoleLayout.Visibility = ViewStates.Gone;

//			consoleLayout.Touch += (object sender, View.TouchEventArgs e) => {
//				consoleLayout.LayoutParameters.Height = 1000;
//			};


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

			_global.client = CommonClient.GetInstance (traceWriter, _geoLocationInstance, _sessionInstance, SynchronizationContext.Current);

			Action<Post> routine = (post) => {
				_global.Posts.Insert (0, post);

				refreshGrid ();
			};

			_global.client.OnConnectionAborted ((client) => {
				client.Start(routine);
			});

			_global.client.Start (routine);
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


