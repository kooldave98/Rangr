﻿using System;
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
using App.Core.Portable.Models;

namespace App.Android
{
	public class PeopleMapFragment : Fragment
	{
		private static readonly LatLng Passchendaele = new LatLng(53.478498699999996, -2.2275587999999997);
		private static readonly LatLng VimyRidge = new LatLng(53.4785183, -2.2274819);
//		private GoogleMap _map;

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);				
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);

			var view = inflater.Inflate(Resource.Layout.PeopleMap, null);
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

		public override void OnResume()
		{
			base.OnResume();
			SetupMap();
		}

		public override void OnDestroyView() 
		{

			base.OnDestroyView ();
			Fragment fragment = Activity.FragmentManager.FindFragmentById (Resource.Id.map) as MapFragment;  
			FragmentTransaction ft = FragmentManager.BeginTransaction();

			ft.Remove(fragment);
			ft.Commit();
		}

		private async void SetupMap()
		{

			await view_model.RefreshConnectedUsers();

			GoogleMap _map = null;

			if (_map == null)
			{
				_map = (Activity.FragmentManager.FindFragmentById (Resource.Id.map) as MapFragment).Map;

				if (_map != null)
				{
					_map.MapType = GoogleMap.MapTypeNormal;

					_map.MyLocationEnabled = true;

					foreach (var connection in view_model.ConnectedUsers) {
						_map.AddMarker (GetMarker(connection));								
					}

					// We create an instance of CameraUpdate, and move the map to it.
					CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(VimyRidge, 15);
					_map.MoveCamera(cameraUpdate);
				}
			}
		}

		private MarkerOptions GetMarker(Connection connected_user)
		{

			return new MarkerOptions ()
				.SetPosition (GetPosition (connected_user.geolocation_string))
				.SetTitle (connected_user.user_display_name);
			//.InvokeIcon(BitmapDescriptorFactory
			//.DefaultMarker(BitmapDescriptorFactory
			//.HueCyan)));;
		}

		private LatLng GetPosition(string geo_string)
		{
			var array = geo_string.Split (',');
			return new LatLng (double.Parse(array [1]), double.Parse(array [0]));
		}

		public PeopleMapFragment(PeopleViewModel the_view_model)
		{
			view_model = the_view_model;
		}

		PeopleViewModel view_model;
	}
}

