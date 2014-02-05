using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.Portable.Device
{
	public interface IGeoLocation
	{
		Task<String> GetCurrentPosition();
		void OnGeoPositionChanged (Action<string> handler);
		void OnStatusChanged (Action<PositionStatus> handler);
//		event EventHandler<StatusChangedEventArgs> OnStatusChanged;
//		event EventHandler<GeoPositionChangedEventArgs> OnGeoPositionChanged;
	}

	public enum PositionStatus
	{
		ERROR,
		RELIABLE,
		UNRELIABLE
	}

	public class GeoPositionChangedEventArgs : EventArgs
	{
		public string position { get; private set;}

		public GeoPositionChangedEventArgs(string the_position)
		{
			position = the_position;
		}
	}

	public class StatusChangedEventArgs : EventArgs
	{
		public PositionStatus status { get; private set;}

		public StatusChangedEventArgs(PositionStatus the_status)
		{
			status = the_status;
		}
	}


}

