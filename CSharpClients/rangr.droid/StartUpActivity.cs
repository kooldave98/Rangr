
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
using rangr.common;

namespace rangr.droid
{
    [Activity(Label = "@string/app_name", 
        MainLauncher = true,
        NoHistory = true,
        Theme = "@android:style/Theme.Translucent.NoTitleBar.Fullscreen")]			
    public class StartUpActivity : Activity
    {
        //Concept got from here....
        //http://stackoverflow.com/a/15698430/502130
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Analytics.Current.Initialize(ApplicationContext); 
                
            start_activity_architecture();

            //start_fragment_architecture();

            Finish();
        }



        /// <summary>
        /// The all activities based architecture
        /// </summary>
        private void start_activity_architecture()
        {
            if (AppGlobal.Current.CurrentUserAndConnectionExists) {
                StartActivity (typeof(PostListFragmentActivity));
            } else {
                StartActivity (typeof(LoginFragmentActivity));
            }
        }

        /// <summary>
        /// The all fragments hosted in one Activity based architecture
        /// </summary>
        private void start_fragment_architecture()
        {
            StartActivity(typeof(FragmentHostActivity));
        }
    }
}

