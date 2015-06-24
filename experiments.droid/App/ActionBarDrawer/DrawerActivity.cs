using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Content.Res;

namespace experiments.droid
{
    [Activity(Label = "Home", MainLauncher = false, Icon = "@drawable/icon")]
    public class DrawerActivity : Activity, ListView.IOnItemClickListener
    {
        private DrawerLayout mDrawerLayout;
        private ListView mDrawerList;
        private ActionBarDrawerToggle mDrawerToggle;

        //private string mDrawerTitle;
        private string mTitle;
        private string[] mPlanetTitles;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.drawer_layout);

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            mTitle = Title;//mDrawerTitle = Title;
            mPlanetTitles = Resources.GetStringArray(Resource.Array.planets_array);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mDrawerList = FindViewById<ListView>(Resource.Id.left_drawer);

            // set a custom shadow that overlays the main content when the drawer opens
            mDrawerLayout.SetDrawerShadow(Resource.Drawable.drawer_shadow, GravityCompat.Start);
            // set up the drawer's list view with items and click listener
            mDrawerList.Adapter = new ArrayAdapter<String>(this,
                Resource.Layout.drawer_list_item, mPlanetTitles);
            mDrawerList.OnItemClickListener = this;

            // enable ActionBar app icon to behave as action to toggle nav drawer
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            // ActionBarDrawerToggle ties together the the proper interactions
            // between the sliding drawer and the action bar app icon
            mDrawerToggle = new MyActionBarDrawerToggle(
                this,                  /* host Activity */
                mDrawerLayout,         /* DrawerLayout object */
                Resource.Drawable.ic_drawer,  /* nav drawer image to replace 'Up' caret */
                Resource.String.drawer_open,  /* "open drawer" description for accessibility */
                Resource.String.drawer_close
            );
            mDrawerLayout.SetDrawerListener(mDrawerToggle);

            if (bundle == null)
            {
                selectItem(0);
            }
				
        }


        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        /* Called whenever we call invalidateOptionsMenu() */

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
            // If the nav drawer is open, hide action items related to the content view
            var drawerOpen = mDrawerLayout.IsDrawerOpen(mDrawerList);
            menu.FindItem(Resource.Id.action_websearch).SetVisible(!drawerOpen);
            return base.OnPrepareOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // The action bar home/up action should open or close the drawer.
            // ActionBarDrawerToggle will take care of this.
            if (mDrawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }
            // Handle action buttons
            switch (item.ItemId)
            {
                case Resource.Id.action_websearch:
				// create intent to perform web search for this planet
                    var intent = new Intent(Intent.ActionWebSearch);
                    intent.PutExtra(SearchManager.Query, ActionBar.Title);
				// catch event that there's no activity to handle intent
                    if (intent.ResolveActivity(PackageManager) != null)
                    {
                        StartActivity(intent);
                    }
                    else
                    {
                        Toast.MakeText(this, Resource.String.app_not_available, ToastLength.Long).Show();
                    }
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }


        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            selectItem(position);
        }

        /** Swaps fragments in the main content view */
        private void selectItem(int position)
        {
            // Create a new fragment and specify the planet to show based on position
            Android.App.Fragment fragment = new PlanetFragment();
            Bundle args = new Bundle();
            args.PutInt(PlanetFragment.ARG_PLANET_NUMBER, position);
            fragment.Arguments = args;

            // Insert the fragment by replacing any existing fragment

            FragmentManager.BeginTransaction()
				.Replace(Resource.Id.content_frame, fragment)
				.Commit();

            // Highlight the selected item, update the title, and close the drawer
            mDrawerList.SetItemChecked(position, true);
            SetTitle(mPlanetTitles[position]);
            mDrawerLayout.CloseDrawer(mDrawerList);
        }


        public void SetTitle(string title)
        {
            mTitle = title;
            ActionBar.Title = mTitle;
        }


        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            // Sync the toggle state after onRestoreInstanceState has occurred.
            mDrawerToggle.SyncState();
        }


        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            // Pass any configuration change to the drawer toggls
            mDrawerToggle.OnConfigurationChanged(newConfig);
        }
    }

    public class PlanetFragment : Android.App.Fragment
    {
        public const string ARG_PLANET_NUMBER = "planet_number";

        public PlanetFragment()
        {
            // Empty constructor required for fragment subclasses
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
                                          Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.fragment_planet, container, false);
            int i = Arguments.GetInt(ARG_PLANET_NUMBER);
            string planet = Resources.GetStringArray(Resource.Array.planets_array)[i];

            int imageId = Resources.GetIdentifier(planet.ToLower(),
                              "drawable", Activity.PackageName);
            (rootView.FindViewById<ImageView>(Resource.Id.image)).SetImageResource(imageId);
            Activity.Title = planet;
            return rootView;
        }
    }


}


