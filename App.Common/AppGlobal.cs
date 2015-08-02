using System;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

using System.Net.Http;
using common_lib;

#if __ANDROID__
using AndroidApp = Android.App;



#else

#endif

namespace App.Common
{
    /// <summary>
    /// Singleton class for Application wide objects. 
    /// </summary>
    public class AppGlobal
    {
        public bool CurrentUserExists
        { 
            get
            { 
                var user = sessionInstance.GetCurrentUser(true);

                if (user != null)
                {
                    return true;
                }

                return false;
            }
        }

        #if __ANDROID__
        private List<AndroidApp.Activity> running_activities = new List<AndroidApp.Activity>();

        public void Resume(AndroidApp.Activity the_activity)
        {
            lock (running_activities)
            {
                running_activities.Add(the_activity);
            }
            Resume();
        }

        public void Pause(AndroidApp.Activity the_activity)
        {
            lock (running_activities)
            {
                running_activities.Remove(the_activity);
            }

            JSTimer.SetTimeout(delegate
                {
                    if (running_activities.Count == 0)
                    {
                        Pause();
                    }
                }, 5000);//5 seconds
        }
        #endif

        public void Resume()
        {
            //Resume any AppGlobal resources here
            //none for now
        }

        public void Pause()
        {
            //Pause any AppGlobal resources here
            //none for now
        }

        protected readonly string logTag = "!!!!!!! App";


        public static AppGlobal Current
        {
            get { return current; }
        }

        private static AppGlobal current;

        static AppGlobal()
        {
            current = new AppGlobal();
        }

        protected AppGlobal()
        {
            sessionInstance = Session.Current;

            // any work here is likely to be blocking (static constructors run on whatever thread that first 
            // access its instance members, which in our case is an activity doing an initialization check),
            // so we want to do it on a background thread
//            Task.Run(()=>{
//                //Ths may be better than below
//                //investigate
//            });
            new Task(async () =>
                { 

                    // add a little wait time, to illustrate a loading event
                    // TODO: Replace this with real work in your app, such as starting services,
                    // database init, web calls, etc.
                    //Thread.Sleep (2500);
                    //TODO: Loop until a position is fixed
                    //as positioning is fundamental to our business
                    //pre-fetch current position

                    await ContactsProvider.Current.get_contacts();

                    //preload the location
                    await GeoLocation.GetInstance().GetCurrentPosition();

                    //Log.Debug (logTag, "App initialized, setting Initialized = true");
                }).Start();
        }

        private Session sessionInstance;
    }
}

