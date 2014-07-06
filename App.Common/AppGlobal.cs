using System;
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
		public bool CurrentUserExists { 
			get { 
				var user = sessionInstance.GetCurrentUser (true);
				if (user != null) {
					return true;
				}

				return false;
			}
		}

		public void CreateConnection ()
		{
			new Task (async() => {

				var user = sessionInstance.GetCurrentUser ();

				var location = await _geoLocationInstance.GetCurrentPosition ();

				var connection_id = await ConnectionServices.Create (user.user_id.ToString (), location.geolocation_value, location.geolocation_accuracy.ToString ());

				sessionInstance.PersistCurrentConnection (connection_id);

				BroadcastConnectionEstablishedIfNeeded ();

				InitHeartBeat ();

			}).Start ();

		}

		private void InitHeartBeat ()
		{
			//This should really be a Guard though
			if (!CurrentUserExists) {
				throw new InvalidOperationException ("User / or connection doesn't exist");
			}


			#region"init_geo_listener"
			geoPositionChangedEventHandler = async (object sender, GeoPositionChangedEventArgs geo_value) => {
				await update_connection (geo_value.position);
			};

			_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;

			_geoLocationInstance.StartListening ();
			#endregion

			#region"init_timer"
			TimerDisposable = (Timer)JavaScriptTimer.SetInterval (async () => {

				var position = await _geoLocationInstance.GetCurrentPosition ();		

				await update_connection (position);

				BroadcastConnectionEstablishedIfNeeded ();


				//4.5 minuets (4min 30sec) [since 1000 is 1 second]
			}, 270000, !IsConnectionEstablished);
			#endregion
		}

		private Timer TimerDisposable;

		private async Task update_connection (GeoValue position)
		{
			await ConnectionServices
				.Update (sessionInstance.GetCurrentConnection ().connection_id.ToString (), 
				position.geolocation_value, position.geolocation_accuracy.ToString ());
		}

		private void BroadcastConnectionEstablishedIfNeeded ()
		{
			if (!IsConnectionEstablished) {
				IsConnectionEstablished = true;

				this.ConnectionEstablished (this, new EventArgs ());
			}
		}

		public void Pause ()
		{
			//Todo need to Null Guard the timer and stuff generally.
			if (!paused) {

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


				paused = true;
			}

		}

		public void Resume ()
		{
			if (paused) {

				if (CurrentUserExists) {
					InitHeartBeat ();
				}
				paused = false;
			}

		}

		// declarations
		//public event EventHandler<EventArgs> Initialized = delegate {};

		public event EventHandler<EventArgs> ConnectionEstablished = delegate {};

		protected readonly string logTag = "!!!!!!! App";

		// properties
		public bool IsConnectionEstablished { get; set; }

		//public bool IsInitialized { get; set; }

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
		private bool paused = true;
	}
}

