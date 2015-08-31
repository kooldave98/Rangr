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
using Android.Graphics;

namespace rangr.droid
{
	public class PostMapFragment : Fragment
	{
		private GoogleMap _map;

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);				
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);

			var view = inflater.Inflate (Resource.Layout.GoogleMap, null);
			return view;

		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);
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

			if (_map == null) {
				_map = (Activity.FragmentManager.FindFragmentById (Resource.Id.map) as MapFragment).Map;

				if (_map != null) {
					_map.MapType = GoogleMap.MapTypeNormal;

					_map.MyLocationEnabled = false;

					_map.UiSettings.SetAllGesturesEnabled (false);

					_map.AddMarker (GetMarker (post));

					_map.AddCircle (GetUncertaintyRadius (post));


					var map_centre_location = GetPosition (post.long_lat_acc_geo_string);

					// We create an instance of CameraUpdate, and move the map to it.
					CameraPosition cameraPosition = new CameraPosition.Builder ()
						.Target (map_centre_location)      // Sets the center of the map to Mountain View
						.Zoom (15)                 // Sets the zoom
					                                //.Bearing(90)                // Sets the orientation of the camera to east
					                                //.Tilt(30)                   // Sets the tilt of the camera to 30 degrees
						.Build ();                   // Creates a CameraPosition from the builder

					_map.AnimateCamera (CameraUpdateFactory.NewCameraPosition (cameraPosition));
				}
			}




		}

		private MarkerOptions GetMarker (Post the_post)
		{

			return new MarkerOptions ()
				.SetPosition (GetPosition (the_post.long_lat_acc_geo_string))
				.SetTitle (the_post.text);
			//.InvokeIcon(BitmapDescriptorFactory
			//.DefaultMarker(BitmapDescriptorFactory
			//.HueCyan)));;
		}

		private CircleOptions GetUncertaintyRadius (Post the_post)
		{
			// Instantiates a new CircleOptions object and defines the center and radius
			CircleOptions circleOptions = new CircleOptions ();
			circleOptions.InvokeCenter (GetPosition (the_post.long_lat_acc_geo_string));
			circleOptions.InvokeRadius (GetAccurracy (the_post.long_lat_acc_geo_string)); // In meters
			circleOptions.InvokeStrokeWidth (1.0f);
			circleOptions.InvokeStrokeColor (0x550000FF);
			// Fill color of the circle
			// 0x represents, this is an hexadecimal code
			// 55 represents percentage of transparency. For 100% transparency, specify 00.
			// For 0% transparency ( ie, opaque ) , specify ff
			// The remaining 6 characters(00ff00) specify the fill color
			circleOptions.InvokeFillColor (0x550000FF);
			return circleOptions;
		}

		private LatLng GetPosition (string geo_string)
		{
			var array = geo_string.Split (',');
			return new LatLng (double.Parse (array [1]), double.Parse (array [0]));
		}

		private int GetAccurracy (string geo_string)
		{
			var array = geo_string.Split (',');
			return int.Parse (array [2]);
		}

		public PostMapFragment (Post the_post)
		{
			post = the_post;
		}

		Post post;
	}
}

