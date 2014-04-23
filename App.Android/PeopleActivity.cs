﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App.Common;
using App.Core.Android;

namespace App.Android
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]			
	public class PeopleActivity : BaseActivity
	{
		protected async override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.People);

			//load our viewmodel
			view_model = new PeopleViewModel (PersistentStorage.Current);

			await view_model.RefreshConnectedUsers ();

			//setup the action bar for tabs mode
			this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			AddTab ("List", new PeopleListFragment (view_model));
			AddTab ("Map", new PeopleMapFragment (view_model));

			if (bundle != null)
				this.ActionBar.SelectTab (this.ActionBar.GetTabAt (bundle.GetInt ("tab")));
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			outState.PutInt ("tab", this.ActionBar.SelectedNavigationIndex);

			base.OnSaveInstanceState (outState);
		}

		void AddTab (string tabText, Fragment view)
		{
			var tab = this.ActionBar.NewTab ();            
			tab.SetText (tabText);
			//tab.SetIcon (Resource.Drawable.ic_tab_white);

			// must set event handler before adding tab
			tab.TabSelected += async delegate(object sender, ActionBar.TabEventArgs e) {
				var fragment = this.FragmentManager.FindFragmentById (Resource.Id.fragmentContainer);
				if (fragment != null) {
					e.FragmentTransaction.Remove (fragment);         
				}
				e.FragmentTransaction.Add (Resource.Id.fragmentContainer, view);

				//reload the data
				await view_model.RefreshConnectedUsers ();
			};
			tab.TabUnselected += delegate(object sender, ActionBar.TabEventArgs e) {
				e.FragmentTransaction.Remove (view);
			};

			this.ActionBar.AddTab (tab);
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.menu, menu);

			menu.FindItem (Resource.Id.people_menu_item).SetEnabled (false);

			return base.OnCreateOptionsMenu (menu);
		}

		private PeopleViewModel view_model;
	}
}

