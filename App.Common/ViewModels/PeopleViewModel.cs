using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace App.Common
{
	public class PeopleViewModel : ViewModelBase
	{

		public IList<Connection> ConnectedUsers { get; set; }

		public GeoCoordinate CurrentLocation { get; private set; }

		public async Task RefreshConnectedUsers ()
		{
			var result = await ConnectionServices.Get (_sessionInstance.GetCurrentConnection ().connection_id.ToString ());

			ConnectedUsers = result;

			if (OnConnectionsReceived != null) {
				OnConnectionsReceived (this, EventArgs.Empty);
			}
		}

		public event EventHandler<EventArgs> OnConnectionsReceived;

		public PeopleViewModel ()
		{
			ConnectedUsers = new List<Connection> ();
			_sessionInstance = Session.GetInstance ();
			_geoLocationInstance = GeoLocation.GetInstance ();

			ConnectionServices = new Connections ();

			setup_location_updates ();

		}

		private async void setup_location_updates ()
		{
			CurrentLocation = await _geoLocationInstance.GetCurrentPosition ();

//			geoPositionChangedEventHandler = (sender, e) => { 
//				CurrentLocation = e.position;
//			};
//
//			_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;
		}

		public override void ResumeState ()
		{
//			_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;
		}

		public override void PauseState ()
		{
//			_geoLocationInstance.OnGeoPositionChanged -= geoPositionChangedEventHandler;
		}

		//		private EventHandler<GeoPositionChangedEventArgs> geoPositionChangedEventHandler;

		GeoLocation _geoLocationInstance;
		Session _sessionInstance;
		Connections ConnectionServices;
	}
}

