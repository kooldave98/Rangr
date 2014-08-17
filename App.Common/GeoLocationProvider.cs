using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Portable.Device;

namespace App.Common
{
	/// <summary>
	/// https://github.com/xamarin/Xamarin.Mobile/blob/master/MonoDroid/Samples/GeolocationSample/MainActivity.cs
	/// </summary>
	public partial class GeoLocation : IGeoLocation
	{
		//private string sample_geoposition =  "-2.2275587999999997,53.478498699999996";
		private static GeoLocation _instance = null;
		private Geolocator geolocator;
		public event EventHandler<GeoPositionChangedEventArgs> OnGeoPositionChanged;
		public event EventHandler<StatusChangedEventArgs> OnStatusChanged;
		//private SynchronizationContext _context;
		private static bool positionReliable = false;
		private static Position geoPosition;

		private void Init ()
		{
			geolocator.DesiredAccuracy = 10;

			geolocator.PositionError += (sender, args) => {
				var errorMessage = args.Error.ToString ();

				NotifyStatusChanged (Status.ERROR, errorMessage);

			};


			geolocator.PositionChanged += (sender, args) => {
				geoPosition = args.Position;

				if(PositionIsInvalid){

					NotifyInaccurate ();


				} else {
					var geolocationValue = string.Format ("{0},{1}", geoPosition.Longitude, geoPosition.Latitude);
					//geolocationValue = sample_geoposition;//TO BE REMOVED

					NotifyAccurate ();

					var geo_value = new GeoValue { geolocation_value = geolocationValue, geolocation_accuracy = Convert.ToInt32(geoPosition.Accuracy)};

					NotifyPositionChanged (geo_value);

				}


			};
		
		
		}



		public async Task<GeoValue> GetCurrentPosition ()
		{
			if (geoPosition == null) {
				await GetPosition ();
			}

			if (PositionIsInvalid) {
				await GetPosition ();
			}

			if (PositionIsInvalid) {
				NotifyInaccurate ();
				throw new ArgumentOutOfRangeException ("geoPosition", "GeoPosition is invalid");
			}

			var geolocationValue = String.Format ("{0},{1}", geoPosition.Longitude, geoPosition.Latitude);
			//geolocationValue = sample_geoposition;
			return new GeoValue { geolocation_value = geolocationValue, geolocation_accuracy = Convert.ToInt32(geoPosition.Accuracy)};
		}

		private bool PositionIsInvalid{
			get{
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

		private async Task GetPosition ()
		{

			var t = geolocator.GetPositionAsync (10000);

			if (t.IsFaulted) {
				NotifyStatusChanged (Status.ERROR, ((GeolocationException)t.Exception.InnerException).Error.ToString ());
			} else if (t.IsCanceled) {
				NotifyStatusChanged (Status.ERROR, "Geolocator was cancelled");
			} else {
				geoPosition = await t;
			}
		
		}

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

		public void StartListening()
		{
			if (!this.geolocator.IsListening) {
				this.geolocator.StartListening (minTime: 30000, minDistance: 10, includeHeading: false);
			}
		}
	}


}
