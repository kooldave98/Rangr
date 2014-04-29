using System;
using System.Linq;
using App.Common.Shared;
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
			var result = await ConnectionServices.Get (_sessionInstance.CurrentConnection.connection_id.ToString ());

			ConnectedUsers = result;

			if (OnConnectionsReceived != null) {
				OnConnectionsReceived (this, EventArgs.Empty);
			}
		}

		public event EventHandler<EventArgs> OnConnectionsReceived;

		public PeopleViewModel (IGeoLocation the_geolocation_instance, IPersistentStorage the_persistent_storage_instance)
		{
			ConnectedUsers = new List<Connection> ();
			_sessionInstance = Session.GetInstance (the_persistent_storage_instance);
			_httpRequest = HttpRequest.Current;
			_geoLocationInstance = the_geolocation_instance;

			ConnectionServices = new Connections (_httpRequest);

			setup_location_updates ();

		}

		private async void setup_location_updates ()
		{
			CurrentLocation = await _geoLocationInstance.GetCurrentPosition ();

			_geoLocationInstance.OnGeoPositionChanged ((geo_value)=>{ 
				CurrentLocation = geo_value;
			});
		}

		IGeoLocation _geoLocationInstance;
		ISession _sessionInstance;
		Connections ConnectionServices;
		IHttpRequest _httpRequest;
	}
}

