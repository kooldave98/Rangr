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
		public bool CurrentUserExists 
		{ 
			get
			{ 
				var user = sessionInstance.GetCurrentUser (true);
				if (user != null) {
					return true;
				}

				return false;
			}
		}

		public void InitConnection ()
		{
			new Task (async() => {

				var user = sessionInstance.GetCurrentUser ();

				var location = await _geoLocationInstance.GetCurrentPosition ();

				sessionInstance.CurrentConnection = 
					await ConnectionServices.Create (user.user_id.ToString (), location.geolocation_value, location.geolocation_accuracy.ToString ());

				InitHeartBeat();

			}).Start ();

		}

		private void InitHeartBeat()
		{
			geoPositionChangedEventHandler = async (object sender, GeoPositionChangedEventArgs geo_value) => {
				sessionInstance.CurrentConnection = await ConnectionServices
					.Update (sessionInstance.CurrentConnection.connection_id.ToString (), geo_value.position.geolocation_value, geo_value.position.geolocation_accuracy.ToString ());

			};

			_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;

			_geoLocationInstance.StartListening ();

			start_timer ();
		}

		private Timer TimerDisposable { get; set;}

		private void start_timer ()
		{
			TimerDisposable = (Timer)JavaScriptTimer.SetInterval (async () => {
				var position = await _geoLocationInstance.GetCurrentPosition ();		

				sessionInstance.CurrentConnection = await ConnectionServices
					.Update (sessionInstance.CurrentConnection.connection_id.ToString (), position.geolocation_value, position.geolocation_accuracy.ToString ());

			}, 270000);//4.5 minuets (4min 30sec) [since 1000 is 1 second]
		}

		public void Pause()
		{
			//Todo need to Null Guard the timer and stuff generally.
			if (!paused) {
				TimerDisposable.Stop ();
				TimerDisposable.Dispose ();
				TimerDisposable = null;

				_geoLocationInstance.StopListening ();
				_geoLocationInstance.OnGeoPositionChanged -= geoPositionChangedEventHandler;

				paused = true;
			}

		}

		public void Resume()
		{
			if (paused) {

				InitHeartBeat ();

				paused = false;
			}

		}

		// declarations
		public event EventHandler<EventArgs> Initialized = delegate {};
		protected readonly string logTag = "!!!!!!! App";

		// properties
		public bool IsInitialized { get; set; }

		public static AppGlobal Current
		{
			get { return current; }
		} private static AppGlobal current;

		static AppGlobal ()
		{
			current = new AppGlobal();
		}
		protected AppGlobal () 
		{
			_geoLocationInstance = GeoLocation.GetInstance ();
			ConnectionServices = new Connections (HttpRequest.Current);
			sessionInstance = Session.GetInstance();

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


				this.IsInitialized = true;

				this.Initialized (this, new EventArgs ());
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

