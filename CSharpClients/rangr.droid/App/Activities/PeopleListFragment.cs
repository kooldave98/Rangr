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
using App.Common;

namespace rangr.droid
{
    public class PeopleListFragment : ListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var peopleListView = inflater.Inflate(Resource.Layout.PeopleList, container, false);


            peopleListView.PivotY = 0;
            peopleListView.PivotX = container.Width;

            return peopleListView;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ListView.DividerHeight = 0;
            ListView.Divider = null;

            HandleOnConnectionsReceived = (object sender, EventArgs e) =>
            {
                list_adapter = new PeopleListAdapter(view.Context, view_model.ConnectedUsers, view_model.CurrentLocation);

                Activity.RunOnUiThread(() =>
                    {
                        ListAdapter = list_adapter;
                        list_adapter.NotifyDataSetChanged();
                    });
            };
        }

        private EventHandler<EventArgs> GeoLocatorRefreshedHandler;

        public override void OnResume()
        {
            base.OnResume();
            //For some reason, doing the refresh posts OnResume doesn't work
            //So doing it in OnViewCreated

            view_model.OnConnectionsReceived += HandleOnConnectionsReceived;
				
            GeoLocatorRefreshedHandler = async (object sender, EventArgs e) =>
            {
                await view_model.RefreshConnectedUsers();
            };

            AppGlobal.Current.GeoLocatorRefreshed += GeoLocatorRefreshedHandler;
        }

        public override void OnPause()
        {
            base.OnPause();
            view_model.OnConnectionsReceived -= HandleOnConnectionsReceived;
            AppGlobal.Current.GeoLocatorRefreshed -= GeoLocatorRefreshedHandler;
            //cleanup_map ();
        }

        private void cleanup_map()
        {
            Fragment fragment = this;
            FragmentTransaction ft = FragmentManager.BeginTransaction();

            ft.Remove(fragment);
            ft.Commit();
        }


        private EventHandler<EventArgs> HandleOnConnectionsReceived;

        public PeopleListFragment(PeopleViewModel the_view_model)
        {
            view_model = the_view_model;
        }

        PeopleListAdapter list_adapter;
        PeopleViewModel view_model;
    }
}