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
			if (CurrentUserExists) {

				await InitHeartBeat ();
			} 
			else if (!string.IsNullOrWhiteSpace (UserDisplayName))
			{
				var userID = await UserServices.Create (UserDisplayName);
				var user = await UserServices.Get (userID.user_id.ToString ());
				sessionInstance.PersistCurrentUser (user);

				await InitHeartBeat ();
			}
			else {
				throw new InvalidOperationException ("You must enter a Display Name to create a new user");
			}

		}

		public void SuspendMemoryIntensiveResources()
		{
			var timer = (Timer)TimerDisposable;
			timer.Stop ();
			timer.Dispose ();

			_geoLocationInstance.StopListening ();
		}

		public void ResumeMemoryIntensiveResources()
		{
			_geoLocationInstance.StartListening ();

			initTimer ();
		}

		private IDisposable TimerDisposable { get; set;}


		private async Task InitHeartBeat ()
		{
			IsBusy = true;
			var location = await _geoLocationInstance.GetCurrentPosition ();

			var user = sessionInstance.GetCurrentUser ();

			//CreateConnection here
			sessionInstance.CurrentConnection = await ConnectionServices.Create (user.user_id.ToString (), location.geolocation_value, location.geolocation_accuracy.ToString ());


			IsBusy = false;

			//init heartbeat here

			_geoLocationInstance.StartListening ();

			_geoLocationInstance.OnGeoPositionChanged (async (geo_value) => {
				sessionInstance.CurrentConnection = await ConnectionServices
					.Update (sessionInstance.CurrentConnection.connection_id.ToString (), geo_value.geolocation_value, geo_value.geolocation_accuracy.ToString ());

			});	

			initTimer ();
		}

		private void initTimer ()
		{
			TimerDisposable = JavaScriptTimer.SetInterval (async () => {
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

		private IGeoLocation _geoLocationInstance;
		private Connections ConnectionServices;
		private Users UserServices;
		private ISession sessionInstance;
	}
}

