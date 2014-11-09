using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using Xamarin;

namespace App.Common
{
	/// <summary>
	/// https://github.com/xamarin/Xamarin.Mobile/blob/master/MonoDroid/Samples/GeolocationSample/MainActivity.cs
	/// </summary>
	public partial class GeoLocation
	{
		//private string sample_geoposition =  "-2.2275587999999997,53.478498699999996";
		private static GeoLocation _instance = null;
		private Geolocator geolocator;

		public event EventHandler<GeoPositionChangedEventArgs> OnGeoPositionChanged;
		public event EventHandler<StatusChangedEventArgs> OnStatusChanged;
		//private SynchronizationContext _context;
		private static bool positionReliable = false;
		private static Position geoPosition;
		private static Position simulated_geoPosition;
		private static bool is_simulation = false;

		private void Init ()
		{
			geolocator.DesiredAccuracy = 10;

			geolocator.PositionError += (sender, args) => {
				var errorMessage = args.Error.ToString ();

				NotifyStatusChanged (Status.ERROR, errorMessage);

			};


			geolocator.PositionChanged += (sender, args) => {
				geoPosition = args.Position;

				if (PositionIsInvalid) {
					NotifyInaccurate ();
				} else {
					if (!is_simulation) {
						pre_notify_position_changed ();
					}
				}
			};



			AppEvents.Current.LocationSimulated += (sender, args) => {

				if (args.Message == "L") {

					is_simulation = false;
				
				} else {

					simulate_position (args.Message);

				}

				pre_notify_position_changed ();
			};

			var persisted_sim = PersistentStorage.Current.Load<string> ("simulation");
			persisted_sim = string.IsNullOrWhiteSpace (persisted_sim) ? "L" : persisted_sim;
			if (persisted_sim != "L") {
				simulate_position (persisted_sim);
			}
		
		}

		private void simulate_position (string key)
		{
			var locations = new Dictionary<string, string> () {
				{ "A","-2.22872, 53.47863" },
				{ "B","-2.22819, 53.47925" },
				{ "C","-2.22695, 53.47922" },
				{ "D","-2.22633, 53.47869" },
			};

			is_simulation = true;
			var lat_lng = GetLatLng (locations [key]);

			var pos = new Position ();

			pos.Latitude = lat_lng.Item1;
			pos.Longitude = lat_lng.Item2;
			pos.Accuracy = 5;

			simulated_geoPosition = pos;
		}

		private Tuple<double, double> GetLatLng (string geo_string)
		{
			var array = geo_string.Split (',');
			return new Tuple<double, double> (double.Parse (array [1]), double.Parse (array [0]));
		}

		private void pre_notify_position_changed ()
		{
			var geolocationValue = string.Format ("{0},{1}", geoPosition.Longitude, geoPosition.Latitude);
			//geolocationValue = sample_geoposition;//TO BE REMOVED

			if (is_simulation) {
				geolocationValue = string.Format ("{0},{1}", simulated_geoPosition.Longitude, simulated_geoPosition.Latitude);
			}

			NotifyAccurate ();

			var geo_value = new GeoValue {
				geolocation_value = geolocationValue,
				geolocation_accuracy = Convert.ToInt32 (geoPosition.Accuracy)
			};

			NotifyPositionChanged (geo_value);
		}

		public async Task<GeoValue> GetCurrentPosition ()
		{
			GeoValue geo_value = null;


			if (!this.geolocator.IsGeolocationAvailable || !this.geolocator.IsGeolocationEnabled) {
				NotifyStatusChanged (Status.ERROR, "Location services are unavailable");
				return geo_value;
			}

			if (geoPosition == null) {
				try {
					geoPosition = await geolocator.GetPositionAsync (10000);
				} catch (Exception) {
					//NotifyStatusChanged (Status.ERROR, "Could not determine location, trying again");
					//return geo_value;TRY AGAIN FOR LONGER !!!!
				}
			}

			if (geoPosition == null) {
				try {
					geoPosition = await geolocator.GetPositionAsync (20000);
				} catch (Exception e) {
					Insights.Report (e);
					NotifyStatusChanged (Status.ERROR, "Could not determine location");
					return geo_value;
				}
			}

			if (PositionIsInvalid) {
				try {
					geoPosition = await geolocator.GetPositionAsync (10000);
				} catch (Exception) {
					NotifyStatusChanged (Status.ERROR, "Could not determine location");
					return geo_value;
				}
			}

			if (PositionIsInvalid) {
				NotifyInaccurate ();
				return geo_value;
			}

			var geolocationValue = String.Format ("{0},{1}", geoPosition.Longitude, geoPosition.Latitude);

			if (is_simulation) {
				geolocationValue = String.Format ("{0},{1}", simulated_geoPosition.Longitude, simulated_geoPosition.Latitude);
			}

			//geolocationValue = sample_geoposition;
			geo_value = new GeoValue {
				geolocation_value = geolocationValue,
				geolocation_accuracy = Convert.ToInt32 (geoPosition.Accuracy)
			};

			return geo_value;
		}

		private bool PositionIsInvalid {
			get {
				//var timespan = DateTime.UtcNow - geoPosition.Timestamp;
				//return geoPosition.Accuracy > 100 || timespan.Minutes > 1;
				return false;
			}
		}

		private void NotifyInaccurate ()
		{
			if (positionReliable) {
				positionReliable = false;

				NotifyStatusChanged (Status.UNRELIABLE, "Position is not reliable");
			}
		}

		private void NotifyAccurate ()
		{
			if (!positionReliable) {
				positionReliable = true;

				NotifyStatusChanged (Status.RELIABLE, "Position is reliable");
			}
		}

		private void NotifyStatusChanged (Status status, string statusMessage)
		{
			if (status == Status.ERROR) {
				AppEvents.Current.TriggerGeolocatorFailedEvent ();
			}

			if (OnStatusChanged != null) {
				OnStatusChanged (this, new StatusChangedEventArgs (status));
			}
		}

		private void NotifyPositionChanged (GeoValue position)
		{
			if (OnGeoPositionChanged != null) {
				OnGeoPositionChanged (this, new GeoPositionChangedEventArgs (position));
			}
		}

		//		private async Task GetPosition ()
		//		{
		//
		//			var t = geolocator.GetPositionAsync (10000);
		//
		//			if (t.IsFaulted) {
		//				NotifyStatusChanged (Status.ERROR, ((GeolocationException)t.Exception.InnerException).Error.ToString ());
		//			} else if (t.IsCanceled) {
		//				NotifyStatusChanged (Status.ERROR, "Geolocator was cancelled");
		//			} else {
		//				geoPosition = await t;
		//			}
		//
		//		}

		//		private void ToggleListening ()
		//		{
		//
		//			if (!this.geolocator.IsListening) {
		//				this.geolocator.StartListening (minTime: 30000, minDistance: 10, includeHeading: false);
		//			} else {
		//				this.geolocator.StopListening ();
		//			}
		//		}

		public void StopListening ()
		{

			if (this.geolocator.IsListening) {
				this.geolocator.StopListening ();
			} 
		}

		public void StartListening ()
		{
			if (!this.geolocator.IsListening) {

				if (!this.geolocator.IsGeolocationAvailable || !this.geolocator.IsGeolocationEnabled) {
					NotifyStatusChanged (Status.ERROR, "Location services are unavailable");
					return;
				}

				this.geolocator.StartListening (minTime: 30000, minDistance: 10, includeHeading: false);
			}
		}
	}

	public enum Status
	{
		ERROR,
		RELIABLE,
		UNRELIABLE
	}

	public class GeoPositionChangedEventArgs : EventArgs
	{
		public GeoValue position { get; private set; }

		public GeoPositionChangedEventArgs (GeoValue the_position)
		{
			position = the_position;
		}
	}

	public class StatusChangedEventArgs : EventArgs
	{
		public Status status { get; private set; }

		public StatusChangedEventArgs (Status the_status)
		{
			status = the_status;
		}
	}

	public class GeoValue
	{
		public string geolocation_value { get; set; }

		public int geolocation_accuracy { get; set; }
	}

}

