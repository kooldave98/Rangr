using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using rangr.common;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Content.Res;
using Android.Support.V4.View;


/// <todo>
/// 0. Don't relload Feed when it is selected in drawer
/// 1. Stabilise app, make it error proof.
/// 2. Sort out titles from fragments.
/// 3. Sort out menus
/// 4. Introduce action bar spinner on Posts Fragment
/// </todo>
using droid_ui_lib;


namespace rangr.droid
{
    [Activity(Label = "@string/app_name", 
        //MainLauncher = true, 
        Icon = "@drawable/icon", 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity, ListView.IOnItemClickListener, ActionBar.IOnNavigationListener
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Analytics.Current.Initialize(ApplicationContext);

            SetContentView(Resource.Layout.main_with_drawer);

            setup_navigation_drawer();

            if (!AppGlobal.Current.CurrentUserAndConnectionExists)
            {
                show_login();
            }
            else
            {
                select_drawer_item(0);
            }

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {

            var drawerOpen = drawer_layout.IsDrawerOpen((int)GravityFlags.Left);
            //when open don't show anything
            var size = menu.Size();

            for (int i = 0; i < size; i++)
            {
                var item = menu.GetItem(i);
                item.SetVisible(!drawerOpen);
            }

            //The line below may not be very effective, as it would only hide those of the current fragment
            //CurrentFragment.HideMenus();

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
                case Android.Resource.Id.Home:
                    //either pop full backstack when going home.
                    //FragmentManager.PopBackStack(baseFragment, PopBackStackFlags.Inclusive);

                    //or go back to previous fragment
                    OnBackPressed();

                    return true;
                case Resource.Id.new_post_menu_item:
                    show_new_post();

                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void show_login()
        {
            var login_fragment = new LoginFragment();
            login_fragment.LoginSucceeded += () => select_drawer_item(0);
            login_fragment.SetupActionBarNavigation = () =>
            {
                unset_drawer();
                unset_up_caret();
            };
            switch_fragments(login_fragment, true, true);
        }

        private void show_feed(int selected_sub_item = 0)
        {
            if (selected_sub_item == 0)
            {
                show_feed_list();
            }
            else
            {
                show_feed_map();
            }
        }

        private void show_feed_list()
        {
            if (feed_fragment == null)
            {
                feed_fragment = new PostListFragment();
                feed_fragment.HashTagSelected += (ht) => show_search(ht);
                feed_fragment.PostItemSelected += (p) => show_detail(p);
                feed_fragment.SetupActionBarNavigation = () =>
                {
                    ActionBar.Title = "";
                    set_feed_spinner();
                    set_up_caret();
                    set_drawer();
                };
            }

            switch_fragments(feed_fragment, true, true);
        }

        private void show_feed_map()
        {
            var fragment = new PostDetailMapFragment(new Post()
                {
                    post_id = 222,
                    text = "fake post",
                    user_display_name = "fake user",
                    user_id = "444",
                    date = DateTime.Now.Date,
                    long_lat_acc_geo_string = "-2.2275587999999997,53.478498699999996,1",
                });

            switch_fragments(fragment, true, true);
        }

        private void show_new_post()
        {
            new_post_fragment = new NewPostFragment();
            new_post_fragment.SetupActionBarNavigation = () =>
            {
                drawer_toggle.DrawerIndicatorEnabled = false;
                unset_spinner();
                set_up_caret();
            };
            new_post_fragment.NewPostCreated += () =>
            {
                OnBackPressed();
            };

            switch_fragments(new_post_fragment, true);
        }

        private void show_search(string hash_tag)
        {
            search_fragment = new SearchFragment(hash_tag);
            search_fragment.HashTagSelected += (ht) => show_search(ht);
            search_fragment.SetupActionBarNavigation = () =>
            {
                unset_spinner();
                set_up_caret();
            };

            switch_fragments(search_fragment, true);
        }

        private void show_detail(Post post)
        {
            post_detail_fragment = new PostDetailFragment(post);
            post_detail_fragment.SetupActionBarNavigation = () =>
            {
                drawer_toggle.DrawerIndicatorEnabled = false;
                unset_spinner();
                set_up_caret();
            };

            switch_fragments(post_detail_fragment, true);
        }

        private int switch_fragments(Android.App.Fragment fragment, bool animated = true, bool isRoot = false)
        {
            var transaction = FragmentManager.BeginTransaction();

            if (animated)
            {
                int animIn, animOut;
                get_fragment_animations(fragment, out animIn, out animOut);
                transaction.SetCustomAnimations(animIn, animOut);
            }
            transaction.Replace(Resource.Id.content_frame, fragment);

            if (!isRoot)
            {
                transaction.AddToBackStack(null);
            }

            return transaction.Commit();
        }

        private void get_fragment_animations(Android.App.Fragment fragment, out int animIn, out int animOut)
        {
            //set default anims
            animIn = Resource.Animation.enter;
            animOut = Resource.Animation.exit;

            //set specific anims
            switch (fragment.GetType().Name)
            {
                case "PostDetailsFragment":
                    animIn = Resource.Animation.post_detail_in;
                    animOut = Resource.Animation.post_detail_out;
                    break;
            }
        }

        private void set_up_caret()
        {
            drawer_toggle.DrawerIndicatorEnabled = false;
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        private void set_drawer()
        {
            drawer_toggle.DrawerIndicatorEnabled = true;
            //ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        private void set_feed_spinner()
        {
            ActionBar.NavigationMode = ActionBarNavigationMode.List;

            var adapter = new ActionBarSpinnerAdapter(this, Resource.Layout.action_bar_spinner, feed_sub_items);

            ActionBar.SetListNavigationCallbacks(adapter, this);
        }

        private void unset_spinner()
        {
            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
        }

        private void unset_drawer()
        {
            drawer_toggle.DrawerIndicatorEnabled = false;
            ActionBar.SetHomeButtonEnabled(false);
        }

        private void unset_up_caret()
        {
            ActionBar.SetDisplayHomeAsUpEnabled(false);
            ActionBar.SetHomeButtonEnabled(false);
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            select_drawer_item(position);
        }

        //IS THIS REALLY NEEDED ????
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            // Pass any configuration change to the drawer toggls
            drawer_toggle.OnConfigurationChanged(newConfig);
        }

        private void select_drawer_item(int position)
        {
            switch (position)
            {
                case 0:
                    show_feed();
                    break;
                case 1:
                    drawer_list.SetItemChecked(current_drawer_item, true);
                    StartActivity(typeof(PeopleActivity));
                    return;
                case 2:
                    drawer_list.SetItemChecked(current_drawer_item, true);
                    StartActivity(typeof(ProfileActivity));
                    return;
                case 3:
                    drawer_list.SetItemChecked(current_drawer_item, true);
                    StartActivity(typeof(AboutAppActivity));
                    return;
            }

            // Highlight the selected item, update the title, and close the drawer
            current_drawer_item = position;
            drawer_list.SetItemChecked(position, true);

            drawer_layout.CloseDrawer(drawer_list);
        }

        private void setup_navigation_drawer()
        {
            drawer_layout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer_list = FindViewById<ListView>(Resource.Id.left_drawer);

            // set a custom shadow that overlays the main content when the drawer opens
            drawer_layout.SetDrawerShadow(Resource.Drawable.drawer_shadow, GravityCompat.Start);
            // set up the drawer's list view with items and click listener
            drawer_list.Adapter = new ArrayAdapter<String>(this,
                Resource.Layout.main_drawer_list_item, navigation_items);
            drawer_list.ChoiceMode = ChoiceMode.Single;

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

            //Display the current fragments title and update the options menu
            drawer_toggle.DrawerClosed += (o, args) =>
            {
                this.InvalidateOptionsMenu();
            };

            //Display the drawer title and update the options menu
            drawer_toggle.DrawerOpened += (o, args) =>
            {
                this.InvalidateOptionsMenu();
            };

//            OpenCallback = () => {
//                ActionBar.Title = Title;
//                CurrentFragment.HasOptionsMenu = false;
//                CurrentFragment.View.Enabled = false;
//                InvalidateOptionsMenu ();
//            },
//            CloseCallback = () => {
//                var currentFragment = CurrentFragment;
//                if (currentFragment != null) {
//                    ActionBar.Title = ((IMoyeuSection)currentFragment).Title;
//                    currentFragment.HasOptionsMenu = true;
//                    currentFragment.View.Enabled = true;
//                }
//                InvalidateOptionsMenu ();
//            },

            drawer_layout.SetDrawerListener(drawer_toggle);

            drawer_toggle.SyncState();
        }

        public bool OnNavigationItemSelected(int position, long itemId)
        {
            show_feed(position);
            return true;
        }

        private Android.App.Fragment current_fragment
        {
            get
            {
                return new Android.App.Fragment[]
                {
                    feed_fragment,
                    post_detail_fragment,
                    search_fragment,
                    new_post_fragment
                }.FirstOrDefault(f => f != null && f.IsAdded && f.IsVisible);
            }
        }

        private PostListFragment feed_fragment { get; set; }

        private PostDetailFragment post_detail_fragment { get; set; }

        private SearchFragment search_fragment { get; set; }

        private NewPostFragment new_post_fragment { get; set; }



        private string[] feed_sub_items = new string[]{ "List", "Map" };

        private DrawerLayout drawer_layout;
        private ListView drawer_list;
        private MyActionBarDrawerToggle drawer_toggle;

        private int current_drawer_item = -1;
        private string[] navigation_items = new string[]{ "Feed", "People", "Profile", "Settings" };
    }

    //See below url for styling the spinner properly
    //http://stackoverflow.com/questions/17613912/styling-actionbar-spinner-navigation-to-look-like-its-title-and-subtitle#_=_
    public class ActionBarSpinnerAdapter : ArrayAdapter<string>, ISpinnerAdapter
    {
        private Activity context;
        //private int textViewResourceId;
        private string[] items;

        public ActionBarSpinnerAdapter(Activity context, int textViewResourceId, string[] items)
            : base(context, textViewResourceId, items)
        {
            this.context = context; 
            //this.textViewResourceId = textViewResourceId;
            this.items = items; 
        }

        public string this [int position]
        {
            get { return items[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for position
            var item = items[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // gives us some performance gains by not always inflating a new view
            // will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                       context.LayoutInflater.Inflate(
                           Resource.Layout.action_bar_spinner,
                           parent,
                           false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var txtName = view.FindViewById<TextView>(Resource.Id.action_bar_title);
            var txtDescription = view.FindViewById<TextView>(Resource.Id.action_bar_subtitle);

            //Assign item's values to the various subviews
            txtName.SetText(context.Title, TextView.BufferType.Normal);
            txtDescription.SetText(item, TextView.BufferType.Normal);

            return view;
        }


        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            DropDownViewHolder holder = null;

            if (convertView == null)
            {
                LayoutInflater inflater = (context).LayoutInflater;
                convertView = inflater.Inflate(Resource.Layout.action_bar_spinner_item, parent, false);

                holder = new DropDownViewHolder();
                holder.mTitle = (TextView)convertView.FindViewById(Resource.Id.spinner_title);

                convertView.Tag = (Java.Lang.Object)holder;
            }
            else
            {
                holder = (DropDownViewHolder)convertView.Tag;
            }

            holder.mTitle.SetText(items[position], TextView.BufferType.Normal);

            return convertView;
        }

        public class DropDownViewHolder : Java.Lang.Object
        {
            public TextView mTitle { get; set; }
        }

        public override int Count
        {
            get { return items.Length; }
        }

    }

}