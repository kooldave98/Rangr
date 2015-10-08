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
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]			
    public class PeopleFragmentActivity : FragmentActivity<PeopleFragment>
	{        
        private PeopleViewModel people_vm = new PeopleViewModel();

        public override PeopleFragment InitFragment()
        {
            if (Fragment == null)
            {
                return new PeopleListFragment().Chain(f=>f.set_vm(people_vm));
            }
            
            if (typeof(PeopleListFragment) == Fragment.GetType())
            {
                return new PeopleMapFragment().Chain(f=>f.set_vm(people_vm));
            }

            if (typeof(PeopleMapFragment) == Fragment.GetType())
            {
                return new PeopleListFragment().Chain(f => f.set_vm(people_vm));
            }

            return new PeopleListFragment().Chain(f=>f.set_vm(people_vm));
        }

        protected override int ContainerID { 
            get { 
                return Resource.Id.fragmentContainer; 
            } 
        }

        protected override int ContentViewID {
            get{
                return Resource.Layout.People;
            }
        }

		protected override void OnCreate (Bundle bundle)
		{
			Title = "People";
			base.OnCreate (bundle);

			//setup the action bar for tabs mode
			this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;


			AddTab ("List");
			AddTab ("Map");

			if (bundle != null)
				this.ActionBar.SelectTab (this.ActionBar.GetTabAt (bundle.GetInt ("tab")));
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			outState.PutInt ("tab", this.ActionBar.SelectedNavigationIndex);

			base.OnSaveInstanceState (outState);
		}

        private void AddTab (string tabText)
        {
            var tab = this.ActionBar.NewTab ()
                .SetText (tabText);

            // must set event handler before adding tab
            tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e) {
                LoadNewFragment();
            };

            tab.TabUnselected += (object sender, ActionBar.TabEventArgs e) => {
                e.FragmentTransaction.Remove (Fragment);
            };

            this.ActionBar.AddTab (tab);
        }

//		private void AddTab (string tabText, Fragment view)
//		{
//			var tab = this.ActionBar.NewTab ();            
//			tab.SetText (tabText);
//			//tab.SetIcon (Resource.Drawable.ic_tab_white);
//
//			// must set event handler before adding tab
//			tab.TabSelected += delegate(object sender, ActionBar.TabEventArgs e) {
//				var fragment = this.FragmentManager.FindFragmentById (Resource.Id.fragmentContainer);
//				if (fragment != null) {
//					e.FragmentTransaction.Remove (fragment);         
//				}
//				e.FragmentTransaction.Add (Resource.Id.fragmentContainer, view);
//
//			};
//			tab.TabUnselected += (object sender, ActionBar.TabEventArgs e) => {
//				e.FragmentTransaction.Remove (view);
//			};
//
//			this.ActionBar.AddTab (tab);
//		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.menu, menu);

			menu.FindItem (Resource.Id.people_menu_item).SetEnabled (false);

			return base.OnCreateOptionsMenu (menu);
		}
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
            if (view_model == null)
            {
                view_model = new PeopleViewModel();
            }
        }
    }
}

