﻿using System;
using System.Timers;
using System.Threading.Tasks;
using App.Core.Portable.Device;
using System.Collections.Generic;

#if __ANDROID__
using Android.App;


#else

#endif

namespace App.Common
{
	/// <summary>
	/// Singleton class for Application wide objects. 
	/// </summary>
	public class AppGlobal
	{
		public bool CurrentUserAndConnectionExists { 
			get { 
				var connection = sessionInstance.GetCurrentConnection (true);
				var user = sessionInstance.GetCurrentUser (true);

				if (connection != null && user != null) {
					return true;
				}

				return false;
			}
		}

		public async Task CreateNewConnectionFromLogin ()
		{
			if (sessionInstance.GetCurrentUser (true) == null) {
				throw new InvalidOperationException ("User doesn't exist");
			}
			//new Task (async() => {

			var user = sessionInstance.GetCurrentUser ();

			var location = await _geoLocationInstance.GetCurrentPosition ();

			var connection_id = await ConnectionServices.Create (user.user_id.ToString (), location.geolocation_value, location.geolocation_accuracy.ToString ());

			if (connection_id != null) {
				sessionInstance.PersistCurrentConnection (connection_id);

				InitPositionChangedListener ();
			}				

			//}).Start ();
		}

		private void InitPositionChangedListener ()
		{
			//This should really be a Guard though
			if (!CurrentUserAndConnectionExists) {
				throw new InvalidOperationException ("User / or connection doesn't exist");
			}

			#region"init_geo_listener"
			geoPositionChangedEventHandler = async (object sender, GeoPositionChangedEventArgs geo_value) => {
				await update_connection (geo_value.position);
			};

			_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;

			_geoLocationInstance.StartListening ();
			#endregion
		}

		private async Task update_connection (GeoValue position)
		{
			//var conn_id = 
			await ConnectionServices.Update (sessionInstance.GetCurrentConnection ().connection_id.ToString (), 
				position.geolocation_value, position.geolocation_accuracy.ToString ());
			//conn_id will be null if the Service command failed for some unknown reason
		}

		#if __ANDROID__
		private List<Activity> running_activities = new List<Activity> ();

		public void Resume (Activity the_activity)
		{
			lock (running_activities) {
				running_activities.Add (the_activity);
			}
			Resume ();
		}

		public void Pause (Activity the_activity)
		{
			lock (running_activities) {
				running_activities.Remove (the_activity);
			}

			JavaScriptTimer.SetTimeout (delegate {
				if (running_activities.Count == 0) {
					Pause ();
				}
			}, 5000);//5 seconds
		}
		#endif

		public void Resume ()
		{
			if (CurrentUserAndConnectionExists) {
				InitPositionChangedListener ();

				JavaScriptTimer.SetTimeout (async delegate {
					//has been put into a callback to definitely happen after
					//activities have been registered
					var position = await _geoLocationInstance.GetCurrentPosition ();

					if (position != null) {
						this.IsGeoLocatorRefreshed = true;
						this.GeoLocatorRefreshed (this, new EventArgs ());
					} else {
						AppEvents.Current.TriggerGeolocatorFailedEvent ();
					}
				}, 1000);//1 second

			}
		}

		public void Pause ()
		{
			#region"suspend_geolocator"
			_geoLocationInstance.StopListening ();
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

		static AppGlobal ()
		{
			current = new AppGlobal ();
		}

		protected AppGlobal ()
		{
			_geoLocationInstance = GeoLocation.GetInstance ();
			ConnectionServices = new Connections (HttpRequest.Current);
			sessionInstance = Session.GetInstance ();

			// any work here is likely to be blocking (static constructors run on whatever thread that first 
			// access its instance members, which in our case is an activity doing an initialization check),
			// so we want to do it on a background thread
			new Task (async () => { 

				// add a little wait time, to illustrate a loading event
				// TODO: Replace this with real work in your app, such as starting services,
				// database init, web calls, etc.
				//Thread.Sleep (2500);
				//TODO: Loop until a position is fixed
				//as positioning is fundamental to our business
				//pre-fetch current position

				//preload the location
				await _geoLocationInstance.GetCurrentPosition ();

				//Log.Debug (logTag, "App initialized, setting Initialized = true");
			}).Start ();
		}

		private EventHandler<GeoPositionChangedEventArgs> geoPositionChangedEventHandler;

		private ISession sessionInstance;
		private Connections ConnectionServices;
		private IGeoLocation _geoLocationInstance;
	}
}

