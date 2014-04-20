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
	public class PeopleMapFragment : Fragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView(inflater, container, savedInstanceState);

			var view = inflater.Inflate(Resource.Layout.TabExampleToBeDeleted, container, false);
			var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
			sampleTextView.Text = "sample fragment text 2";

			return view;
		}

		public PeopleMapFragment(PeopleViewModel the_view_model)
		{
			view_model = the_view_model;
		}

		PeopleViewModel view_model;
	}
}

