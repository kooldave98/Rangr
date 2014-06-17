using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.Portable.Device
{
	public interface IGeoLocation
	{
		Task<GeoValue> GetCurrentPosition();
		void StartListening ();
		void StopListening ();
		event EventHandler<StatusChangedEventArgs> OnStatusChanged;
		event EventHandler<GeoPositionChangedEventArgs> OnGeoPositionChanged;
	}

	public enum Status
	{
		ERROR,
		RELIABLE,
		UNRELIABLE
	}

	public class GeoPositionChangedEventArgs : EventArgs
	{
		public GeoValue position { get; private set;}

		public GeoPositionChangedEventArgs(GeoValue the_position)
		{
			position = the_position;
		}
	}

	public class StatusChangedEventArgs : EventArgs
	{
		public Status status { get; private set;}

		public StatusChangedEventArgs(Status the_status)
		{
			status = the_status;
		}
	}

	public class GeoValue
	{
		public string geolocation_value { get; set;}

		public int geolocation_accuracy { get; set;}
	}


}

