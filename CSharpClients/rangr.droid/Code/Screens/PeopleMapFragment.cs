using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using rangr.common;

namespace rangr.droid
{
    public class PeopleMapFragment : PeopleFragment
	{
		private GoogleMap _map;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);

			var view = inflater.Inflate (Resource.Layout.GoogleMap, null);
			return view;

		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{

			base.OnViewCreated (view, savedInstanceState);

			//ListView.DividerHeight = 0;
//			ListView.Divider = null;
//			var peopleListAdapter = new PeopleListAdapter (view.Context, view_model.ConnectedUsers);
//
//			ListAdapter = peopleListAdapter;
//
//			view_model.OnConnectionsReceived += (sender, e) => {
//				peopleListAdapter.NotifyDataSetChanged();
//			};
		}

		public override void OnResume ()
		{
			base.OnResume ();
			SetupMap ();
		}

		public override void OnPause ()
		{
			base.OnPause ();
			_map = null;

			cleanup_map ();
		}

		public override void OnDestroyView ()
		{

			base.OnDestroyView ();
			//cleanup_map ();
		}

		private void cleanup_map ()
		{
			Fragment fragment = Activity.FragmentManager.FindFragmentById (Resource.Id.map) as MapFragment;  
			FragmentTransaction ft = FragmentManager.BeginTransaction ();

			ft.Remove (fragment);
			ft.Commit ();
		}

		//See: http://docs.xamarin.com/guides/android/platform_features/maps_and_location/maps/part_2_-_maps_api/
		//for more info on configuring Google maps
		private void SetupMap ()
		{
			//No need to refresh here again, as it would have been refreshed in the list fragment
			//This would also save us the trouble of Initialising the connection if the app has been 
			//idle for too long(> 4.5 mins) and this fragment is the first thing that wakes the app up

			//await view_model.RefreshConnectedUsers();

			if (_map == null) {
				_map = (Activity.FragmentManager.FindFragmentById (Resource.Id.map) as MapFragment).Map;

				if (_map != null) {
					_map.MapType = GoogleMap.MapTypeNormal;

					_map.MyLocationEnabled = true;

					foreach (var connection in view_model.ConnectedUsers) {
						_map.AddMarker (GetMarker (connection));								
					}

					var my_location = GetPosition (new GeoCoordinate (0, 0));

					if (view_model.CurrentLocation != null) {
						my_location = GetPosition (view_model.CurrentLocation);
					}

					// We create an instance of CameraUpdate, and move the map to it.
					CameraPosition cameraPosition = new CameraPosition.Builder ()
						.Target (my_location)      // Sets the center of the map to Mountain View
						.Zoom (15)                   // Sets the zoom
					                                //.Bearing(90)                // Sets the orientation of the camera to east
					                                //.Tilt(30)                   // Sets the tilt of the camera to 30 degrees
						.Build ();                   // Creates a CameraPosition from the builder

					_map.AnimateCamera (CameraUpdateFactory.NewCameraPosition (cameraPosition));
				}
			}


		}

		private MarkerOptions GetMarker (Connection connected_user)
		{

			return new MarkerOptions ()
				.SetPosition (GetPosition (connected_user.long_lat_acc_geo_string.ToGeoCoordinateFromLongLatAccString ()))
				.SetTitle (connected_user.user_display_name);
			//.InvokeIcon(BitmapDescriptorFactory
			//.DefaultMarker(BitmapDescriptorFactory
			//.HueCyan)));;
		}

		private LatLng GetPosition (GeoCoordinate geo_value)
		{
			return new LatLng (geo_value.Latitude, geo_value.Longitude);
		}
	}
}

