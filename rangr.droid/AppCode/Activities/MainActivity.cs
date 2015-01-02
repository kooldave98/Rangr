using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using App.Common;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Content.Res;
using Android.Support.V4.View;

namespace rangr.droid
{
    [Activity(Label = "@string/app_name", 
        MainLauncher = true, 
        Icon = "@drawable/icon", 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity, ListView.IOnItemClickListener
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main_with_drawer);

            drawer_layout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            action_bar_title = Title = "@string/app_name";
            //navigation_items = Resources.GetStringArray(Resource.Array.navigation_drawer_list);
            drawer_layout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer_list = FindViewById<ListView>(Resource.Id.left_drawer);

            // set a custom shadow that overlays the main content when the drawer opens
            drawer_layout.SetDrawerShadow(Resource.Drawable.drawer_shadow, GravityCompat.Start);
            // set up the drawer's list view with items and click listener
            drawer_list.Adapter = new ArrayAdapter<String>(this,
                Resource.Layout.main_drawer_list_item, navigation_items);
            drawer_list.OnItemClickListener = this;

            // enable ActionBar app icon to behave as action to toggle nav drawer
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            // ActionBarDrawerToggle ties together the the proper interactions
            // between the sliding drawer and the action bar app icon
            drawer_toggle = new MyActionBarDrawerToggle(
                this,                  /* host Activity */
                drawer_layout,         /* DrawerLayout object */
                Resource.Drawable.ic_drawer,  /* nav drawer image to replace 'Up' caret */
                Resource.String.drawer_open,  /* "open drawer" description for accessibility */
                Resource.String.drawer_close
            );
            drawer_layout.SetDrawerListener(drawer_toggle);

//            if (bundle == null)
//            {
//                selectItem(0);
//            }

            //Retain fragments so don't set home if state is stored.
            if (FragmentManager.BackStackEntryCount == 0)
            {
                if (!AppGlobal.Current.CurrentUserAndConnectionExists)
                {
                    show_login();
                }
                else
                {
                    selectItem(0);
                    //show_feed();
                }
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
            var drawerOpen = drawer_layout.IsDrawerOpen(drawer_list);

            menu.FindItem(Resource.Id.settings_menu_item).SetVisible(!drawerOpen);
            menu.FindItem(Resource.Id.boom_menu_item).SetVisible(!drawerOpen);

            return base.OnPrepareOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // The action bar home/up action should open or close the drawer.
            // ActionBarDrawerToggle will take care of this.
            if (drawer_toggle.DrawerIndicatorEnabled && drawer_toggle.OnOptionsItemSelected(item))
            {
                return true;
            }
            // Handle action buttons

            switch (item.ItemId)
            {
                case Resource.Id.settings_menu_item:
                    StartActivity(typeof(AboutAppActivity));
                    return true;
                case Android.Resource.Id.Home:
                    //either pop full backstack when going home.   

                    //FragmentManager.PopBackStack(baseFragment, PopBackStackFlags.Inclusive);
                    //drawer_toggle.DrawerIndicatorEnabled = true;

                    //or go back to previous fragment
                    OnBackPressed();

                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();

            drawer_toggle.DrawerIndicatorEnabled = true;
        }

        public int SwitchScreens(Android.App.Fragment fragment, bool animated = true, bool isRoot = false)
        {
            var transaction = FragmentManager.BeginTransaction();

            if (animated)
            {
                int animIn, animOut;
                GetAnimationsForFragment(fragment, out animIn, out animOut);
                transaction.SetCustomAnimations(animIn, animOut);
            }
            transaction.Replace(Resource.Id.content_frame, fragment);

            if (!isRoot)
            {
                transaction.AddToBackStack(null);
            }

            return transaction.Commit();
        }

        private void GetAnimationsForFragment(Android.App.Fragment fragment, out int animIn, out int animOut)
        {
            animIn = Resource.Animation.enter;
            animOut = Resource.Animation.exit;

            switch (fragment.GetType().Name)
            {
                case "PostDetailsFragment":
                    animIn = Resource.Animation.post_detail_in;
                    animOut = Resource.Animation.post_detail_out;
                    break;
            }
        }

        private void show_feed()
        {
            var fragment = new PostsFragment();
            fragment.HashTagSelected += (ht) => show_search(ht);
            fragment.PostItemSelected += (p) => show_detail(p);
            baseFragment = fragment.Id;
            SwitchScreens(fragment, true, true);
        }

        private void show_search(string hash_tag)
        {
            drawer_toggle.DrawerIndicatorEnabled = false;

            var fragment = new SearchFragment(hash_tag);
            fragment.HashTagSelected += (ht) => show_search(ht);
            SwitchScreens(fragment, true);
        }

        private void show_detail(Post post)
        {
            drawer_toggle.DrawerIndicatorEnabled = false;

            var fragment = new PostDetailFragment(post);
            SwitchScreens(fragment, true);
        }

        private void show_login()
        {
            var fragment = new LoginFragment();
            baseFragment = fragment.Id;
            fragment.LoginSucceeded += () => show_feed();
            SwitchScreens(fragment, true, true);
        }

        private int baseFragment;

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("baseFragment", baseFragment);
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            baseFragment = savedInstanceState.GetInt("baseFragment");
        }


        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            selectItem(position);
        }

        /** Swaps fragments in the main content view */
        private void selectItem(int position)
        {
            switch (position)
            {
                case 0:
                    show_feed();
                    break;
                default:
                    break;
            }

            // Highlight the selected item, update the title, and close the drawer
            drawer_list.SetItemChecked(position, true);
            SetTitle(navigation_items[position]);
            drawer_layout.CloseDrawer(drawer_list);
        }


        public void SetTitle(string title)
        {
            action_bar_title = title;
            ActionBar.Title = action_bar_title;
        }


        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            // Sync the toggle state after onRestoreInstanceState has occurred.
            drawer_toggle.SyncState();
        }


        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            // Pass any configuration change to the drawer toggls
            drawer_toggle.OnConfigurationChanged(newConfig);
        }

        private DrawerLayout drawer_layout;
        private ListView drawer_list;
        private ActionBarDrawerToggle drawer_toggle;

        private string action_bar_title;
        private string[] navigation_items = new string[]{ "Feed", "People", "Profile", "Settings" };
    }
}