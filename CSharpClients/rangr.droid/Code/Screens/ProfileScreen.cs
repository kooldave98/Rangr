using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using rangr.common;

namespace rangr.droid
{
	[Activity (Label = "@string/app_name", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProfileActivity : FragmentActivity
	{
        private ProfileFragment the_fragment;
        private ProfileFragment Fragment {
            get{ 
                return the_fragment ?? (the_fragment = new ProfileFragment());
            }
        }


        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            PushFragment(Fragment);
        }

        protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult (requestCode, resultCode, data);
            if (resultCode == Result.Ok) {
                Fragment.nuke_data_and_refresh();
            }
        }

        public override bool OnCreateOptionsMenu (IMenu menu)
        {
            MenuInflater.Inflate (Resource.Menu.profile, menu);

            return base.OnCreateOptionsMenu (menu);
        }

        public override bool OnOptionsItemSelected (IMenuItem item)
        {
            switch (item.ItemId) { 
                case Resource.Id.edit_profile_menu_item:
                    StartActivityForResult (typeof(EditProfileActivity), 0);
                    break;
            }

            return base.OnOptionsItemSelected (item);
        }
    }

    public class ProfileFragment : VMFragment<ProfileViewModel>
    {
        public override string TitleLabel { 
            get {
                return "My profile";//GetString(Resource.String.new_post_title);
            } 
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Profile, null);

            //
            // Setup the UI
            //
            listView = view.FindViewById<ListView> (Resource.Id.ProfileList);
            listView.Divider = null;

            // create our adapter
            listAdapter = new ProfileAdapter (view_model.PropertyGroups);

            //Hook up our adapter to our ListView
            listView.Adapter = listAdapter;

            return view;
        }

        public void nuke_data_and_refresh()
        {
            //Nuke our cache: Unideal implementation

            view_model = null;
            view_model = new ProfileViewModel ();


            //refresh list view
            listView.Adapter = listAdapter = new ProfileAdapter (view_model.PropertyGroups);
            listAdapter.NotifyDataSetChanged ();
        }

		

		private ProfileAdapter listAdapter;
		private ListView listView;

        public ProfileFragment ()
		{

			if (view_model == null) {
				view_model = new ProfileViewModel ();
			}

		}
	}
}

