using System;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

using ModernHttpClient;
using System.Net.Http;
using solid_lib;

#if __ANDROID__
using AndroidApp = Android.App;



#else

#endif

namespace rangr.common
{
    /// <summary>
    /// Singleton class for Application wide objects. 
    /// </summary>
    public class AppGlobal
    {
        public bool CurrentUserAndConnectionExists { 
            get { 
                var connection = sessionInstance.GetCurrentConnection(true);
                var user = sessionInstance.GetCurrentUser(true);

                if (connection != null && user != null)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task CreateNewConnectionFromLogin()
        {
            if (sessionInstance.GetCurrentUser(true) == null)
            {
                throw new InvalidOperationException("User doesn't exist");
            }
            //new Task (async() => {

            var user = sessionInstance.GetCurrentUser();

            var location = await _geoLocationInstance.GetCurrentPosition();

            if (location != null)
            {
                var connection_id = await ConnectionServices.Create(user.user_id, location.ToLongLatAccString());

                if (connection_id != null)
                {
                    sessionInstance.PersistCurrentConnection(connection_id);

                    InitPositionChangedListener();
                }	
            }

            //}).Start ();
        }

        private void InitPositionChangedListener()
        {
            //This should really be a Guard though
            if (!CurrentUserAndConnectionExists)
            {
                throw new InvalidOperationException("User / or connection doesn't exist");
            }

            #region"init_geo_listener"
            geoPositionChangedEventHandler = async (object sender, GeoPositionChangedEventArgs geo_value) => {
                await update_connection(geo_value.position);
            };

            _geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;

            _geoLocationInstance.StartListening();
            #endregion
        }

        private async Task update_connection(GeoCoordinate position)
        { 
            await ConnectionServices.Update(sessionInstance.GetCurrentConnection().connection_id, 
                position.ToLongLatAccString());
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

            JSTimer.SetTimeout(delegate {
                if (running_activities.Count == 0)
                {
                    Pause();
                }
            }, 5000);//5 seconds
        }
        #endif

        public void Resume()
        {
            if (CurrentUserAndConnectionExists)
            {

                InitPositionChangedListener();				

                JSTimer.SetTimeout(async delegate {
                    //has been put into a callback to definitely happen after
                    //activities have been registered
                    var position = await _geoLocationInstance.GetCurrentPosition();

                    //Force the users current location to be refreshed on the server
                    await update_connection(position);

                    if (position != null)
                    {
                        this.IsGeoLocatorRefreshed = true;
                        this.GeoLocatorRefreshed(this, new EventArgs());
                    }
                    else
                    {
                        AppEvents.Current.TriggerGeolocatorFailedEvent("Could not refresh current location");
                    }
                }, 1000);//1 second

            }
        }

        public void Pause()
        {
            #region"suspend_geolocator"
            _geoLocationInstance.StopListening();
            if (geoPositionChangedEventHandler != null)
                _geoLocationInstance.OnGeoPositionChanged -= geoPositionChangedEventHandler;
            #endregion

            IsGeoLocatorRefreshed = false;
        }


        // declarations
        public event EventHandler<EventArgs> GeoLocatorRefreshed = delegate {};

        protected readonly string logTag = "!!!!!!! App";

        public bool IsGeoLocatorRefreshed { get; private set; }


        public static AppGlobal Current {
            get { return current; }
        }

        private static AppGlobal current;

        static AppGlobal()
        {
            current = new AppGlobal();
        }

        protected AppGlobal()
        {
            _geoLocationInstance = GeoLocation.GetInstance();
            ConnectionServices = new Connections();
            sessionInstance = Session.Current;

            // any work here is likely to be blocking (static constructors run on whatever thread that first 
            // access its instance members, which in our case is an activity doing an initialization check),
            // so we want to do it on a background thread
            new Task(async () => { 

                // add a little wait time, to illustrate a loading event
                // TODO: Replace this with real work in your app, such as starting services,
                // database init, web calls, etc.
                //Thread.Sleep (2500);
                //TODO: Loop until a position is fixed
                //as positioning is fundamental to our business
                //pre-fetch current position

                //preload the location
                await _geoLocationInstance.GetCurrentPosition();

                //Log.Debug (logTag, "App initialized, setting Initialized = true");
            }).Start();
        }

        private EventHandler<GeoPositionChangedEventArgs> geoPositionChangedEventHandler;

        private Session sessionInstance;
        private Connections ConnectionServices;
        private GeoLocation _geoLocationInstance;
    }
}