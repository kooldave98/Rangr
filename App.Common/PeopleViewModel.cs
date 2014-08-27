using System;
using System.Linq;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Core.Portable.Persistence;

namespace App.Common
{
	public class PeopleViewModel : ViewModelBase
	{

		public IList<Connection> ConnectedUsers { get; set; }

		public GeoValue CurrentLocation { get; private set; }

		public async Task RefreshConnectedUsers ()
		{
			var result = await ConnectionServices.Get (_sessionInstance.GetCurrentConnection().connection_id.ToString ());

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
			_httpRequest = HttpRequest.Current;
			_geoLocationInstance = GeoLocation.GetInstance();

			ConnectionServices = new Connections (_httpRequest);

			setup_location_updates ();

		}

		private async void setup_location_updates ()
		{
			CurrentLocation = await _geoLocationInstance.GetCurrentPosition ();

			geoPositionChangedEventHandler = (sender, e) => { 
				CurrentLocation = e.position;
			};

			_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;
		}

		public override void ResumeState()
		{
			_geoLocationInstance.OnGeoPositionChanged += geoPositionChangedEventHandler;
		}

		public override void PauseState()
		{
			_geoLocationInstance.OnGeoPositionChanged -= geoPositionChangedEventHandler;
		}

		private EventHandler<GeoPositionChangedEventArgs> geoPositionChangedEventHandler;

		IGeoLocation _geoLocationInstance;
		ISession _sessionInstance;
		Connections ConnectionServices;
		IHttpRequest _httpRequest;
	}
}

