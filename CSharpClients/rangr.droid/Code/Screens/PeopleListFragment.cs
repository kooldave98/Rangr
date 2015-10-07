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

namespace rangr.droid
{
	public class PeopleListFragment : PeopleFragment
	{
        public void set_vm(PeopleViewModel vm)
        {
            view_model = Guard.IsNotNull(vm, "vm");
        }
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.PeopleList, container, false);


			view.PivotY = 0;
			view.PivotX = container.Width;

			return view;
		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

            people_list_view = view.FindViewById<ListView>(Resource.Id.list);

            people_list_view.DividerHeight = 0;
            people_list_view.Divider = null;

			HandleOnConnectionsReceived = (object sender, EventArgs e) => {
				list_adapter = new PeopleListAdapter (view.Context, view_model.ConnectedUsers, view_model.CurrentLocation);

				Activity.RunOnUiThread (() => {
                    people_list_view.Adapter = list_adapter;
					list_adapter.NotifyDataSetChanged ();
				});
			};
		}

		private EventHandler<EventArgs> GeoLocatorRefreshedHandler;

		public override void OnResume ()
		{
			base.OnResume ();
			//For some reason, doing the refresh posts OnResume doesn't work
			//So doing it in OnViewCreated

			view_model.OnConnectionsReceived += HandleOnConnectionsReceived;
				
			GeoLocatorRefreshedHandler = async (object sender, EventArgs e) => {
				await view_model.RefreshConnectedUsers ();
			};

			AppGlobal.Current.GeoLocatorRefreshed += GeoLocatorRefreshedHandler;
		}

		public override void OnPause ()
		{
			base.OnPause ();
			view_model.OnConnectionsReceived -= HandleOnConnectionsReceived;
			AppGlobal.Current.GeoLocatorRefreshed -= GeoLocatorRefreshedHandler;
			//cleanup_map ();
		}

		private void cleanup_map ()
		{
			Fragment fragment = this;
			FragmentTransaction ft = FragmentManager.BeginTransaction ();

			ft.Remove (fragment);
			ft.Commit ();
		}


		private EventHandler<EventArgs> HandleOnConnectionsReceived;


		PeopleListAdapter list_adapter;
        ListView people_list_view;
	}
}