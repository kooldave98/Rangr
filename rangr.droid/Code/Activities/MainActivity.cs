using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using App.Common;

namespace rangr.droid
{
    [Activity(Label = "@string/app_name", 
        MainLauncher = true, 
        Icon = "@drawable/icon", 
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);

            //Retain fragments so don't set home if state is stored.
            if (FragmentManager.BackStackEntryCount == 0)
            {
                if (!AppGlobal.Current.CurrentUserAndConnectionExists)
                {
                    show_login();
                }
                else
                {
                    show_feed();
                }
            }
        }

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

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.settings_menu_item:
                    //ShowBasket();
                    return true;
                case Android.Resource.Id.Home:
                    //pop full backstack when going home.   
                    FragmentManager.PopBackStack(baseFragment, PopBackStackFlags.Inclusive);
                    SetupActionBar();
                    return true;
            }

            return base.OnMenuItemSelected(featureId, item);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            SetupActionBar(FragmentManager.BackStackEntryCount != 0);
        }

        public int SwitchScreens(Fragment fragment, bool animated = true, bool isRoot = false)
        {
            var transaction = FragmentManager.BeginTransaction();

            if (animated)
            {
                int animIn, animOut;
                GetAnimationsForFragment(fragment, out animIn, out animOut);
                transaction.SetCustomAnimations(animIn, animOut);
            }
            transaction.Replace(Resource.Id.content_area, fragment);
            if (!isRoot)
                transaction.AddToBackStack(null);

            SetupActionBar(!isRoot);

            return transaction.Commit();
        }

        private void GetAnimationsForFragment(Fragment fragment, out int animIn, out int animOut)
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

        public void SetupActionBar(bool showUp = false)
        {
            this.ActionBar.SetDisplayHomeAsUpEnabled(showUp);
            //this.ActionBar.SetDisplayShowHomeEnabled (showUp);
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
            var fragment = new SearchFragment(hash_tag);
            fragment.HashTagSelected += (ht) => show_search(ht);
            SwitchScreens(fragment, true);
        }

        private void show_detail(Post post)
        {
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
    }
}