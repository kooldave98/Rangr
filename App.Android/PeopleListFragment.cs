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

namespace App.Android
{
	public class PeopleListFragment : ListFragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			RetainInstance = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var peopleListView = inflater.Inflate (Resource.Layout.PeopleList, container, false);


			peopleListView.PivotY = 0;
			peopleListView.PivotX = container.Width;

			return peopleListView;
		}

		public async override void OnViewCreated (View view, Bundle savedInstanceState)
		{
			base.OnViewCreated (view, savedInstanceState);

			await view_model.RefreshConnectedUsers();

			ListView.DividerHeight = 0;
			ListView.Divider = null;
			list_adapter = new PeopleListAdapter (view.Context, view_model.ConnectedUsers);

			ListAdapter = list_adapter;

			view_model.OnConnectionsReceived += HandleOnConnectionsReceived;
		}


		private void HandleOnConnectionsReceived(object sender, EventArgs e)
		{
			list_adapter.NotifyDataSetChanged();
		}

		public PeopleListFragment(PeopleViewModel the_view_model)
		{
			view_model = the_view_model;
		}

		PeopleListAdapter list_adapter;
		PeopleViewModel view_model;
	}
}