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
using App.Common;
using Android.Graphics;

namespace rangr.droid
{
    //See: http://docs.xamarin.com/guides/android/platform_features/maps_and_location/maps/part_2_-_maps_api/ for more info on configuring Google maps
    //Solution:http://stackoverflow.com/questions/13733299/initialize-mapfragment-programmatically-with-maps-api-v2
    public class PostDetailMapFragment : MapFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Map.MapType = GoogleMap.MapTypeNormal;

            Map.MyLocationEnabled = false;

            Map.UiSettings.SetAllGesturesEnabled(false);

            Map.AddMarker(GetMarker(post));

            Map.AddCircle(GetUncertaintyRadius(post));


            var map_centre_location = GetPosition(post.long_lat_acc_geo_string);

            // We create an instance of CameraUpdate, and move the map to it.
            CameraPosition cameraPosition = new CameraPosition.Builder()
                .Target(map_centre_location)      // Sets the center of the map to Mountain View
                .Zoom(15)                 // Sets the zoom
                                            //.Bearing(90)                // Sets the orientation of the camera to east
                                            //.Tilt(30)                   // Sets the tilt of the camera to 30 degrees
                .Build();                   // Creates a CameraPosition from the builder

            Map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
        }

        private MarkerOptions GetMarker(Post the_post)
        {

            return new MarkerOptions()
				.SetPosition(GetPosition(the_post.long_lat_acc_geo_string))
				.SetTitle(the_post.text);
            //.InvokeIcon(BitmapDescriptorFactory
            //.DefaultMarker(BitmapDescriptorFactory
            //.HueCyan)));;
        }

        private CircleOptions GetUncertaintyRadius(Post the_post)
        {
            // Instantiates a new CircleOptions object and defines the center and radius
            CircleOptions circleOptions = new CircleOptions();
            circleOptions.InvokeCenter(GetPosition(the_post.long_lat_acc_geo_string));
            circleOptions.InvokeRadius(GetAccurracy(the_post.long_lat_acc_geo_string)); // In meters
            circleOptions.InvokeStrokeWidth(1.0f);
            circleOptions.InvokeStrokeColor(0x550000FF);
            // Fill color of the circle
            // 0x represents, this is an hexadecimal code
            // 55 represents percentage of transparency. For 100% transparency, specify 00.
            // For 0% transparency ( ie, opaque ) , specify ff
            // The remaining 6 characters(00ff00) specify the fill color
            circleOptions.InvokeFillColor(0x550000FF);
            return circleOptions;
        }

        private LatLng GetPosition(string geo_string)
        {
            var array = geo_string.Split(',');
            return new LatLng(double.Parse(array[1]), double.Parse(array[0]));
        }

        private int GetAccurracy(string geo_string)
        {
            var array = geo_string.Split(',');
            return int.Parse(array[2]);
        }

        public PostDetailMapFragment(Post the_post)
        {
            post = the_post;
        }

        public PostDetailMapFragment()
        {

        }

        private Post post;
    }
}

