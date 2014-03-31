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
using App.Core.Portable.Network;

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
		Connections ConnectionServices;
		IHttpRequest _httpRequest;


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
			//var menu_item_string = menu_item.TitleFormatted.ToString ();

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


		protected async override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);


			//ActionBar.SetDisplayHomeAsUpEnabled (true);

			_global = Global.Current;
			_global.Posts = new List<SeenPost> ();

			_geoLocationInstance = GeoLocation.GetInstance (this);
			_sessionInstance = Session.GetInstance(PersistentStorage.Current);
			_httpRequest = HttpRequest.Current;

			ConnectionServices = new Connections (_httpRequest);



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
					postDetails.PutExtra("PostID", _global.Posts [e.Position].id);
					StartActivity (postDetails);
				};
			}


			var location = await _geoLocationInstance.GetCurrentPosition ();

			var user = _sessionInstance.GetCurrentUser ();

			//CreateConnection here
			_global.current_connection = await ConnectionServices.Create(user.user_id.ToString(), location.geolocation_value, location.geolocation_accuracy.ToString());


			//init heartbeat here

			_geoLocationInstance.OnGeoPositionChanged (async (geo_value)=>{
				_global.current_connection = await ConnectionServices
					.Update(_global.current_connection.connection_id.ToString(), geo_value.geolocation_value, geo_value.geolocation_accuracy.ToString());

			});	


			JavaScriptTimer.SetInterval(async () =>
				{
					var position = await _geoLocationInstance.GetCurrentPosition();		

					_global.current_connection = await ConnectionServices
						.Update(_global.current_connection.connection_id.ToString(), position.geolocation_value, position.geolocation_accuracy.ToString());

				}, 270000);//4.5 minuets (4min 30sec) [since 1000 is 1 second]

//			_textView = FindViewById<TextView> (Resource.Id.textView);
//
//
//			var traceWriter = new TextViewWriter (SynchronizationContext.Current, _textView);
//
//
//			Action<SeenPost> routine = (post) => {
//				_global.Posts.Insert (0, post);
//
//				refreshGrid ();
//			};
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


