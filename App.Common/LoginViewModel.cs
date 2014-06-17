using System;
using App.Core.Portable.Persistence;
using App.Common.Shared;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using System.Threading.Tasks;
using System.Timers;

namespace App.Common
{
	public class LoginViewModel : ViewModelBase
	{
		public string UserDisplayName { get; set; }

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

		public async Task Login()
		{
			if (!logged_in && CurrentUserExists) {

				await InitHeartBeat ();
				logged_in = true;
			} 
			else if (!string.IsNullOrWhiteSpace (UserDisplayName))
			{
				var userID = await UserServices.Create (UserDisplayName);
				var user = await UserServices.Get (userID.user_id.ToString ());
				sessionInstance.PersistCurrentUser (user);

				await InitHeartBeat ();
			}
			else if(!logged_in) {
				throw new InvalidOperationException ("You must enter a Display Name to create a new user");
			}

		}

		public override void TombstoneViewModel()
		{
			if (paused.HasValue && !paused.Value) {
				TimerDisposable.Stop ();
				TimerDisposable.Dispose ();
				TimerDisposable = null;

				_geoLocationInstance.StopListening ();
				_geoLocationInstance.OnGeoPositionChanged -= geoPositionChangedEventHandler;
			}
			paused = true;
		}

		public override void ResurrectViewModel()
		{
			if (paused.HasValue && paused.Value) {

				_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;
				_geoLocationInstance.StartListening ();

				initTimer ();
			}
			paused = false;
		}

		private Timer TimerDisposable;


		private async Task InitHeartBeat ()
		{
			IsBusy = true;
			var location = await _geoLocationInstance.GetCurrentPosition ();

			var user = sessionInstance.GetCurrentUser ();

			//CreateConnection here
			sessionInstance.CurrentConnection = await ConnectionServices.Create (user.user_id.ToString (), location.geolocation_value, location.geolocation_accuracy.ToString ());


			IsBusy = false;

			//init heartbeat here

			geoPositionChangedEventHandler = async (object sender, GeoPositionChangedEventArgs geo_value) => {
				sessionInstance.CurrentConnection = await ConnectionServices
					.Update (sessionInstance.CurrentConnection.connection_id.ToString (), geo_value.position.geolocation_value, geo_value.position.geolocation_accuracy.ToString ());

			};

			_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;

			_geoLocationInstance.StartListening ();

			initTimer ();
		}

		private void initTimer ()
		{
			TimerDisposable = (Timer)JavaScriptTimer.SetInterval (async () => {
				var position = await _geoLocationInstance.GetCurrentPosition ();		

				sessionInstance.CurrentConnection = await ConnectionServices
					.Update (sessionInstance.CurrentConnection.connection_id.ToString (), position.geolocation_value, position.geolocation_accuracy.ToString ());

			}, 270000);//4.5 minuets (4min 30sec) [since 1000 is 1 second]
		}

		public LoginViewModel (IGeoLocation the_geolocation_instance, IPersistentStorage the_persistent_storage_instance)
		{
			sessionInstance = Session.GetInstance(the_persistent_storage_instance);

			_geoLocationInstance = the_geolocation_instance;


			ConnectionServices = new Connections (HttpRequest.Current);

			UserServices = new Users (HttpRequest.Current);
		}

		private EventHandler<GeoPositionChangedEventArgs> geoPositionChangedEventHandler;

		private IGeoLocation _geoLocationInstance;
		private Connections ConnectionServices;
		private Users UserServices;
		private ISession sessionInstance;
		private bool logged_in = false;
		private bool? paused;
	}
}

