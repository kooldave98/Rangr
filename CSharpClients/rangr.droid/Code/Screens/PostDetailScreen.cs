
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using rangr.common;
using Android.Content.PM;
using Android.Gms.Maps.Model;
using Android.Gms.Maps;

namespace rangr.droid
{
    [Activity (Label = "@string/app_name"
            , ScreenOrientation = ScreenOrientation.Portrait)]         
    public class PostDetailFragmentActivity : FragmentActivity
    {        
        public override bool OnNavigateUp ()
        {
            base.OnNavigateUp ();

            Finish ();

            return true;
        }

        private PostDetailFragment the_fragment;
        private PostDetailFragment Fragment
        {
            get{
                if (the_fragment != null)
                    return the_fragment;
                

                Post post;
                if (Intent.HasExtra("Post"))
                {
                    var postBytes = Intent.GetByteArrayExtra("Post");
                    post = PostDetailsViewModel.Deserialize(postBytes);
                }
                else
                {
                    post = new Post();
                }

                return the_fragment = PostDetailFragment.newInstance(post);
            }
        }

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate(bundle);

            PushFragment(Fragment);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public static Intent CreateIntent (Context context, Post post)
        {
            var postStream = PostDetailsViewModel.Serialize (post);

            var intent = new Intent (context, typeof(PostDetailFragmentActivity))
                .PutExtra ("Post", postStream.ToArray ());
            return intent;
        }
    }

    public class PostDetailFragment : VMFragment<PostDetailsViewModel>
    {
        public override string TitleLabel { 
            get {
                return string.Empty;
            } 
        }



        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.post_detail, null);

            var map_fragment = new MapFragment();

            map_fragment.OnMapReady(map=>{
                //See: http://docs.xamarin.com/guides/android/platform_features/maps_and_location/maps/part_2_-_maps_api/ for more info on configuring Google maps
                //Solution:http://stackoverflow.com/questions/13733299/initialize-mapfragment-programmatically-with-maps-api-v2
                map.MapType = GoogleMap.MapTypeNormal;

                map.MyLocationEnabled = false;

                map.UiSettings.SetAllGesturesEnabled(false);

                map.AddMarker(SetMarker(post_geocoordinate, post.text));

                map.AddCircle(GetUncertaintyRadius(post_geocoordinate));


                var map_centre_location = GetPosition(post_geocoordinate);

                // We create an instance of CameraUpdate, and move the map to it.
                CameraPosition cameraPosition = new CameraPosition.Builder()
                    .Target(map_centre_location)      // Sets the center of the map to Mountain View
                    .Zoom(15)                 // Sets the zoom
                    //.Bearing(90)                // Sets the orientation of the camera to east
                    //.Tilt(30)                   // Sets the tilt of the camera to 30 degrees
                    .Build();                   // Creates a CameraPosition from the builder

                map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));

            });

            FragmentManager.BeginTransaction()
                .Chain(t=>t.Replace(Resource.Id.fragmentContainer, map_fragment))
                            .Chain(t=>t.Commit());

            view.FindViewById<TextView>(Resource.Id.UserNameText).SetText(view_model.CurrentPost.user_display_name, TextView.BufferType.Normal);

            postTextLabel = view.FindViewById<TextView>(Resource.Id.PostTextLabel);

            postTextLabel.Text = view_model.CurrentPost.text;

            return view;
        }

        private MarkerOptions SetMarker(GeoCoordinate geo_value, string caption)
        {

            return new MarkerOptions()
                .SetPosition(GetPosition(geo_value))
                .SetTitle(caption);
            //.InvokeIcon(BitmapDescriptorFactory
            //.DefaultMarker(BitmapDescriptorFactory
            //.HueCyan)));;
        }

        private CircleOptions GetUncertaintyRadius(GeoCoordinate geo_value)
        {
            // Instantiates a new CircleOptions object and defines the center and radius
            CircleOptions circleOptions = new CircleOptions();
            circleOptions.InvokeCenter(GetPosition(geo_value));
            circleOptions.InvokeRadius(geo_value.Accuracy.Value); // In meters
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

        private LatLng GetPosition(GeoCoordinate geo_value)
        {
            return new LatLng(geo_value.Latitude, geo_value.Longitude);
        }

        private TextView postTextLabel;

        protected override void Initialise()
        {
            var postBytes = Arguments.GetByteArray(INIT_ARG_KEY);
            post = PostDetailsViewModel.Deserialize (postBytes);
            view_model = new PostDetailsViewModel(post);
        }

        public static PostDetailFragment newInstance(Post a_post) 
        {
            var post_bytes = PostDetailsViewModel.Serialize(a_post).ToArray();

            var fragment = new PostDetailFragment(){
                Arguments = new Bundle()
                    .Chain(b=>b.PutByteArray(INIT_ARG_KEY, post_bytes))
            };
            return fragment;
        }

        private GeoCoordinate post_geocoordinate {get{ return post.long_lat_acc_geo_string.ToGeoCoordinateFromLongLatAccString();}}

        private Post post;


    }
}

