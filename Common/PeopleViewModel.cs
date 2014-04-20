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

		public async Task RefreshConnectedUsers ()
		{
			var result = await ConnectionServices.Get (_sessionInstance.CurrentConnection.connection_id.ToString ());

			ConnectedUsers = ConnectedUsers.Union (result, new ConnectionComparer ()).ToList();

			if (OnConnectionsReceived != null) {
				OnConnectionsReceived (this, EventArgs.Empty);
			}
		}

		public event EventHandler<EventArgs> OnConnectionsReceived;

		public PeopleViewModel (IPersistentStorage the_persistent_storage_instance)
		{
			ConnectedUsers = new List<Connection> ();

			_sessionInstance = Session.GetInstance (the_persistent_storage_instance);
			_httpRequest = HttpRequest.Current;

			ConnectionServices = new Connections (_httpRequest);
		}

		ISession _sessionInstance;
		Connections ConnectionServices;
		IHttpRequest _httpRequest;
	}
}

