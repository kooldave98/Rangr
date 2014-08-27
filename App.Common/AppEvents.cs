using System;

namespace App.Common
{
	public class AppEvents
	{
		#region "GeolocatorFailed"
		public event EventHandler<EventArgs> GeolocatorFailed;

		public void TriggerGeolocatorFailedEvent()
		{
			if (GeolocatorFailed != null) {
				GeolocatorFailed (this, EventArgs.Empty);
			}
		}
		#endregion

		#region "ConnectionFailed"
		public event EventHandler<EventArgs> ConnectionFailed;

		public void TriggerConnectionFailedEvent()
		{
			if (ConnectionFailed != null) {
				ConnectionFailed (this, EventArgs.Empty);
			}
		}
		#endregion

		#region"Instance plumbing"
		public static AppEvents Current {
			get {
				return current ?? (current = new AppEvents ());
			}
		}
		private static AppEvents current;
		protected AppEvents(){}
		#endregion
	}
}

