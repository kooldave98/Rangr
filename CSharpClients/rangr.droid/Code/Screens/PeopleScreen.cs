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
using solid_lib;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace rangr.droid
{
    [Activity(Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]         
    public class PeopleListFragmentActivity : FragmentActivity
    {
        private PeopleViewModel people_vm = new PeopleViewModel();

        public PeopleListFragment PeopleListFragment {            
            get{ return new PeopleListFragment().Chain(f => f.set_vm(people_vm)); }
        }

        public MapFragment PeopleMapFragment {            
            get { 
                return new MapFragment().Chain(f => {
                    f.OnMapReady(map => {
                        map.MapType = GoogleMap.MapTypeNormal;

                        map.MyLocationEnabled = true;

                        foreach (var connection in people_vm.ConnectedUsers)
                        {
                            map.AddMarker(GetMarker(connection));                                
                        }

                        var my_location = GetPosition(new GeoCoordinate(0, 0));

                        if (people_vm.CurrentLocation != null)
                        {
                            my_location = GetPosition(people_vm.CurrentLocation);
                        }

                        // We create an instance of CameraUpdate, and move the map to it.
                        CameraPosition cameraPosition = new CameraPosition.Builder()
                                .Target(my_location)      // Sets the center of the map to Mountain View
                                .Zoom(15)                   // Sets the zoom
                                                            //.Bearing(90)                // Sets the orientation of the camera to east
                                                            //.Tilt(30)                   // Sets the tilt of the camera to 30 degrees
                                .Build();                   // Creates a CameraPosition from the builder

                        map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
                    });
                });
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

        protected override void OnCreate(Bundle bundle)
        {
            Title = "People";
            base.OnCreate(bundle);

            //setup the action bar for tabs mode
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;


            AddTab("List", PeopleListFragment);
            AddTab("Map", PeopleMapFragment);

            if (bundle != null)
                this.ActionBar.SelectTab(this.ActionBar.GetTabAt(bundle.GetInt("tab")));
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("tab", this.ActionBar.SelectedNavigationIndex);

            base.OnSaveInstanceState(outState);
        }

        private void AddTab(string tabText, Fragment view)
        {
            var tab = this.ActionBar.NewTab()
                .SetText(tabText);

            // must set event handler before adding tab
            tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e) {
                var ContainerID = Android.Resource.Id.Content;
                FragmentManager.BeginTransaction().Add(ContainerID, view, tabText).Commit();
            };

            tab.TabUnselected += (object sender, ActionBar.TabEventArgs e) => {
                e.FragmentTransaction.Remove (view);
            };

            this.ActionBar.AddTab(tab);
        }

        //      private void AddTab (string tabText, Fragment view)
        //      {
        //          var tab = this.ActionBar.NewTab ();
        //          tab.SetText (tabText);
        //          //tab.SetIcon (Resource.Drawable.ic_tab_white);
        //
        //          // must set event handler before adding tab
        //          tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e) {
        //              var fragment = this.FragmentManager.FindFragmentById (Resource.Id.fragmentContainer);
        //              if (fragment != null) {
        //                  e.FragmentTransaction.Remove (fragment);
        //              }
        //              e.FragmentTransaction.Add (Resource.Id.fragmentContainer, view);
        //
        //          };
        //          tab.TabUnselected += (object sender, ActionBar.TabEventArgs e) => {
        //              e.FragmentTransaction.Remove (view);
        //          };
        //
        //          this.ActionBar.AddTab (tab);
        //      }


    }


    public class PeopleListFragment : VMFragment<PeopleViewModel>
    {
        public void set_vm(PeopleViewModel vm)
        {
            view_model = Guard.IsNotNull(vm, "vm");
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.PeopleList, container, false);


            view.PivotY = 0;
            view.PivotX = container.Width;

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            people_list_view = view.FindViewById<ListView>(Resource.Id.list);

            people_list_view.DividerHeight = 0;
            people_list_view.Divider = null;

            HandleOnConnectionsReceived = (object sender, EventArgs e) => {
                list_adapter = new PeopleListAdapter(view.Context, view_model.ConnectedUsers, view_model.CurrentLocation);

                Activity.RunOnUiThread(() => {
                    people_list_view.Adapter = list_adapter;
                    list_adapter.NotifyDataSetChanged();
                });
            };
        }

        private EventHandler<EventArgs> GeoLocatorRefreshedHandler;

        public override void OnResume()
        {
            base.OnResume();

            view_model.OnConnectionsReceived += HandleOnConnectionsReceived;
				
            GeoLocatorRefreshedHandler = async (object sender, EventArgs e) => {
                await view_model.RefreshConnectedUsers();
            };

            AppGlobal.Current.GeoLocatorRefreshed += GeoLocatorRefreshedHandler;
        }

        public override void OnPause()
        {
            base.OnPause();
            view_model.OnConnectionsReceived -= HandleOnConnectionsReceived;
            AppGlobal.Current.GeoLocatorRefreshed -= GeoLocatorRefreshedHandler;
        }

        public override string TitleLabel {
            get {
                return "People";
            }
        }

        protected override void Initialise()
        {
            if (view_model == null)
            {
                view_model = new PeopleViewModel();
            }
        }


        private EventHandler<EventArgs> HandleOnConnectionsReceived;


        PeopleListAdapter list_adapter;
        ListView people_list_view;
    }
}