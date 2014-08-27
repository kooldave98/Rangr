﻿using System;
using System.Timers;
using System.Threading.Tasks;
using App.Core.Portable.Device;

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
				var user = sessionInstance.GetCurrentUser (true);

				var connection = sessionInstance.GetCurrentConnection (true);

				if (user != null && connection != null) {
					return true;
				}

				return false;
			}
		}

		public async Task CreateConnection ()
		{
			//new Task (async() => {

			if (!CurrentUserAndConnectionExists) {
				var user = sessionInstance.GetCurrentUser ();

				var location = await _geoLocationInstance.GetCurrentPosition ();

				var connection_id = await ConnectionServices.Create (user.user_id.ToString (), location.geolocation_value, location.geolocation_accuracy.ToString ());

				if (connection_id == null) {
					sessionInstance.PersistCurrentConnection (connection_id);
				
					ConnectionInitialised = true;
					OnConnectionInitialized (this, new EventArgs ());

					InitHeartBeat (false);
				}

			} else {
				InitHeartBeat (true);
			}

			//}).Start ();
		}

		private void InitHeartBeat (bool eagerly = false)
		{
			//This should really be a Guard though
			if (!CurrentUserAndConnectionExists) {
				throw new InvalidOperationException ("User / or connection doesn't exist");
			}

			#region"init_timer"
			TimerDisposable = (Timer)JavaScriptTimer.SetInterval (async () => {

				var position = await _geoLocationInstance.GetCurrentPosition ();		
				//@@@@--------->>>>>>>>>>>>>>>>>WHEN IN FLIGHT MODE THE ABOVE LINE THROWS EXCEPTIONS AT TIMES
				//NEED TO ADD ERROR HANDLING TO GEOLOCATOR TOO
				await update_connection (position);

				//4.5 minuets (4min 30sec) [since 1000 is 1 second]
			}, 270000, eagerly);//Eager timer interval polling
			#endregion

			#region"init_geo_listener"
			geoPositionChangedEventHandler = async (object sender, GeoPositionChangedEventArgs geo_value) => {
				await update_connection (geo_value.position);
			};

			_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;

			_geoLocationInstance.StartListening ();
			#endregion
		}

		private Timer TimerDisposable;

		private async Task update_connection (GeoValue position)
		{

			var conn_id = await ConnectionServices.Update (sessionInstance.GetCurrentConnection ().connection_id.ToString (), 
				              position.geolocation_value, position.geolocation_accuracy.ToString ());

			//conn_id will be null if the Service command failed for some unknown reason

			 
			if (!ConnectionInitialised) {
				if (conn_id != null) {
					//we don't wanna initialise if the conn_id was null
					ConnectionInitialised = true;
					OnConnectionInitialized (this, new EventArgs ());
				}
			}
		}

		public void Pause ()
		{

			#region"pause timer"
			if (TimerDisposable != null) {
				TimerDisposable.Stop ();
				TimerDisposable.Dispose ();
				TimerDisposable = null;
			}
			#endregion

			#region"suspend_geolocator"
			_geoLocationInstance.StopListening ();
			if (geoPositionChangedEventHandler != null)
				_geoLocationInstance.OnGeoPositionChanged -= geoPositionChangedEventHandler;
			#endregion

			ConnectionInitialised = false;

		}

		public void Resume ()
		{
			if (!ConnectionInitialised) {
				InitHeartBeat (true);
			}
		}

		// declarations
		//public event EventHandler<EventArgs> Initialized = delegate {};

		protected readonly string logTag = "!!!!!!! App";

		//public bool IsInitialized { get; set; }


		public event EventHandler<EventArgs> OnConnectionInitialized = delegate {};

		public bool ConnectionInitialised { get; private set; }

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
			new Task (() => { 

				//pre-fetch current position
				_geoLocationInstance.GetCurrentPosition ();
				// add a little wait time, to illustrate a loading event
				// TODO: Replace this with real work in your app, such as starting services,
				// database init, web calls, etc.
				//Thread.Sleep (2500);


				//this.IsInitialized = true;

				//this.Initialized (this, new EventArgs ());

				//Log.Debug (logTag, "App initialized, setting Initialized = true");
			}).Start ();
		}

		private EventHandler<GeoPositionChangedEventArgs> geoPositionChangedEventHandler;

		private ISession sessionInstance;
		private Connections ConnectionServices;
		private IGeoLocation _geoLocationInstance;
	}
}

