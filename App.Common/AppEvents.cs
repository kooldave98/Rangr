using System;
using general_shared_lib;

namespace App.Common
{
    public class AppEvents : SingletonBase<AppEvents>
	{
		#region "GeolocatorFailed"

		public event EventHandler<AppEventArgs> GeolocatorFailed;

		public void TriggerGeolocatorFailedEvent (string message = "Unable to determine location..")
		{
			if (GeolocatorFailed != null) {
				GeolocatorFailed (this, new AppEventArgs (message));
			}
		}

		#endregion

		#region "ConnectionFailed"

		public event EventHandler<AppEventArgs> ConnectionFailed;

		public void TriggerConnectionFailedEvent (string message = "Unable to connect..")
		{
			if (ConnectionFailed != null) {
				ConnectionFailed (this, new AppEventArgs (message));
			}
		}

		#endregion

		#region "SimulationTriggered"

		public event EventHandler<AppEventArgs> LocationSimulated;

		public void TriggerLocationSimulatedEvent (string location_string)
		{
			if (LocationSimulated != null) {
				LocationSimulated (this, new AppEventArgs (location_string));
			}
		}

		#endregion


		#region"Instance plumbing"

		protected AppEvents ()
		{
		}

		#endregion
	}

	public class AppEventArgs : EventArgs
	{
		private readonly string message;

		public AppEventArgs (string message)
		{
			this.message = message;
		}

		public string Message {
			get { return this.message; }
		}
	}

}

