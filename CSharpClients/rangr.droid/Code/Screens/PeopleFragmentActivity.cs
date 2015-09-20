using System;
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
using rangr.common;

namespace rangr.droid
{
    //Need to think about..
    //1. The fragment container
    //2. The fragment tag
    //3. How to properly swap out fragments
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]			
    public class PeopleFragmentActivity : FragmentActivity<PeopleFragment>
	{        
        public override PeopleFragment InitFragment()
        {
            throw new NotImplementedException();
        }

		protected override void OnCreate (Bundle bundle)
		{
			Title = "People";
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.People);


			//setup the action bar for tabs mode
			this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			list_fragment = new PeopleListFragment ();
			map_fragment = new PeopleMapFragment ();


			AddTab ("List", list_fragment);
			AddTab ("Map", map_fragment);

			if (bundle != null)
				this.ActionBar.SelectTab (this.ActionBar.GetTabAt (bundle.GetInt ("tab")));
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			outState.PutInt ("tab", this.ActionBar.SelectedNavigationIndex);

			base.OnSaveInstanceState (outState);
		}

		private void AddTab (string tabText, Fragment view)
		{
			var tab = this.ActionBar.NewTab ();            
			tab.SetText (tabText);
			//tab.SetIcon (Resource.Drawable.ic_tab_white);

			// must set event handler before adding tab
			tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e) {
				var fragment = this.FragmentManager.FindFragmentById (Resource.Id.fragmentContainer);
				if (fragment != null) {
					e.FragmentTransaction.Remove (fragment);         
				}
				e.FragmentTransaction.Add (Resource.Id.fragmentContainer, view);

			};
			tab.TabUnselected += (object sender, ActionBar.TabEventArgs e) => {
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

		private PeopleListFragment list_fragment;
		private PeopleMapFragment map_fragment;
	}

    public abstract class PeopleFragment : VMFragment<PeopleViewModel>
    {
        public override string TitleLabel {
            get {
                return "People";
            }
        }

        protected override void Initialise()
        {
            view_model = new PeopleViewModel();
        }
    }
}

