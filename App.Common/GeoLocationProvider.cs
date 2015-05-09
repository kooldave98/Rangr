using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Geolocation;
using System.Threading;
using System.Threading.Tasks;

#if __ANDROID__
using Android.App;
using Android.Content;

#else
#endif

namespace App.Common
{
	/// <summary>
	/// https://github.com/xamarin/Xamarin.Mobile/blob/master/MonoDroid/Samples/GeolocationSample/MainActivity.cs
	/// </summary>
	public class GeoLocation
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

		#if __ANDROID__
		public static GeoLocation GetInstance ()
		{
			return _instance ?? (_instance = new GeoLocation (Application.Context));
		}

		private GeoLocation (Context appContext)
		{
			if (this.geolocator != null)
				return;

			geolocator = new Geolocator (appContext);

			Init ();
		}
		#else
		public static GeoLocation GetInstance ()
		{
		return _instance ?? (_instance = new GeoLocation ());
		}


		private GeoLocation ()
		{
		if (this.geolocator != null)
		return;

		geolocator = new Geolocator ();

		Init ();

		}
		#endif

		private void Init ()
		{
			geolocator.DesiredAccuracy = 100;

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
				{ "A","-2.229798, 53.477738" },
				{ "B","-2.229396, 53.479345" },
				{ "C","-2.227427, 53.480470" },
				{ "D","-2.224769, 53.480107" },
			};

			is_simulation = true;
			var lat_lng = GetLatLng (locations [key]);

			var pos = new Position ();

			pos.Latitude = lat_lng.Item1;
			pos.Longitude = lat_lng.Item2;
			pos.Accuracy = 1;

			simulated_geoPosition = pos;
		}

		private Tuple<double, double> GetLatLng (string geo_string)
		{
			var array = geo_string.Split (',');
			return new Tuple<double, double> (double.Parse (array [1]), double.Parse (array [0]));
		}

		private void pre_notify_position_changed ()
		{
			var geolocationValue = new GeoCoordinate (geoPosition.Latitude, geoPosition.Longitude);
			//geolocationValue = sample_geoposition;//TO BE REMOVED

			if (is_simulation) {
				geolocationValue = new GeoCoordinate (simulated_geoPosition.Latitude, simulated_geoPosition.Longitude);
			}

			NotifyAccurate ();

			geolocationValue.Accuracy = Convert.ToInt32 (geoPosition.Accuracy);

			NotifyPositionChanged (geolocationValue);
		}

		public async Task<GeoCoordinate> GetCurrentPosition ()
		{
			GeoCoordinate geo_value = null;

            using (var handle = Analytics.Current.TrackTime ("TimeToGetCurrentPosition")) {

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
                        Analytics.Current.Report (e);
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

				geo_value = new GeoCoordinate (geoPosition.Latitude, geoPosition.Longitude);

				if (is_simulation) {
					geo_value = new GeoCoordinate (simulated_geoPosition.Latitude, simulated_geoPosition.Longitude);
				}

				//geolocationValue = sample_geoposition;
				geo_value.Accuracy = Convert.ToInt32 (geoPosition.Accuracy);
			}
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

		private void NotifyPositionChanged (GeoCoordinate position)
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

				this.geolocator.StartListening (minTime: 60000, minDistance: 100, includeHeading: false);
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
		public GeoCoordinate position { get; private set; }

		public GeoPositionChangedEventArgs (GeoCoordinate the_position)
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

}

