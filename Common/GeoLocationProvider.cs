using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Portable.Device;

namespace App.Common.Shared
{
	public partial class GeoLocation : IGeoLocation
	{
		private string sample_geoposition =  "-2.2275587999999997,53.478498699999996";
		private static GeoLocation _instance = null;
		private Geolocator geolocator;
		private List<Action<string>> geoPositionChangedCallbacks = new List<Action<string>> ();
		private List<Action<PositionStatus>> statusChangedCallbacks = new List<Action<PositionStatus>> ();
//		public event EventHandler<GeoPositionChangedEventArgs> OnGeoPositionChanged;
//		public event EventHandler<StatusChangedEventArgs> OnStatusChanged;
		private SynchronizationContext _context;
		private static bool positionReliable = false;
		private static Position geoPosition;

		private void Init ()
		{
			geolocator.DesiredAccuracy = 10;

			geolocator.PositionError += (sender, args) => {
				var errorMessage = args.Error.ToString ();

				NotifyStatusChanged (PositionStatus.ERROR, errorMessage);

			};


			geolocator.PositionChanged += (sender, args) => {
				geoPosition = args.Position;

				if(PositionIsInvalid){

					NotifyInaccurate ();


				} else {
					var geolocationValue = string.Format ("{0},{1}", geoPosition.Longitude, geoPosition.Latitude);
					geolocationValue = sample_geoposition;//TO BE REMOVED

					NotifyAccurate ();

					NotifyPositionChanged (geolocationValue);

				}


			};
		
			ToggleListening ();
		
		
		}


		public void OnGeoPositionChanged (Action<string> handler)
		{
			geoPositionChangedCallbacks.Add (handler);
		}
		public void OnStatusChanged (Action<PositionStatus> handler)
		{
			statusChangedCallbacks.Add (handler);
		}



		public async Task<String> GetCurrentPosition ()
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
			geolocationValue = sample_geoposition;
			return geolocationValue;
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

				NotifyStatusChanged (PositionStatus.UNRELIABLE, "Position is not reliable");
			}
		}

		private void NotifyAccurate ()
		{
			if (!positionReliable) {
				positionReliable = true;

				NotifyStatusChanged (PositionStatus.RELIABLE, "Position is reliable");
			}
		}

		private void NotifyStatusChanged (PositionStatus status, string statusMessage)
		{
			//OnStatusChanged (this, new StatusChangedEventArgs(status));
			foreach (var callback in statusChangedCallbacks) {
				callback (status);
			}
		}

		private void NotifyPositionChanged (string position)
		{
			//OnGeoPositionChanged (this, new GeoPositionChangedEventArgs (position));

			foreach (var callback in geoPositionChangedCallbacks) {
				callback (position);
			}
		}

		private async Task GetPosition ()
		{

			var t = geolocator.GetPositionAsync (10000);

			if (t.IsFaulted) {
				NotifyStatusChanged (PositionStatus.ERROR, ((GeolocationException)t.Exception.InnerException).Error.ToString ());
			} else if (t.IsCanceled) {
				NotifyStatusChanged (PositionStatus.ERROR, "Geolocator was cancelled");
			} else {
				geoPosition = await t;
			}
		
		}

		private void ToggleListening ()
		{

			if (!this.geolocator.IsListening) {
				this.geolocator.StartListening (minTime: 30000, minDistance: 10, includeHeading: true);
			} else {
				this.geolocator.StopListening ();
			}
		}
		//		private TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
		//		private CancellationTokenSource cancelSource;
		//
		//		private void Setup()
		//		{
		//
		//		}
		//
		//		public void GetPosition ()
		//		{
		//			Setup();
		//
		//			this.cancelSource = new CancellationTokenSource();
		//
		//
		//			this.geolocator.GetPositionAsync (timeout: 10000, cancelToken: this.cancelSource.Token, includeHeading: true)
		//				.ContinueWith (t =>
		//					{
		//						if (t.IsFaulted)
		//							PositionStatus.Text = ((GeolocationException)t.Exception.InnerException).Error.ToString();
		//						else if (t.IsCanceled)
		//							PositionStatus.Text = "Canceled";
		//						else
		//						{
		//							PositionStatus.Text = t.Result.Timestamp.ToString("G");
		//							PositionLatitude.Text = "La: " + t.Result.Latitude.ToString("N4");
		//							PositionLongitude.Text = "Lo: " + t.Result.Longitude.ToString("N4");
		//						}
		//
		//					}, scheduler);
		//		}
		//
		//		public void CancelPosition ()
		//		{
		//			CancellationTokenSource cancel = this.cancelSource;
		//			if (cancel != null)
		//				cancel.Cancel();
		//		}
		//		private void OnListeningError (object sender, PositionErrorEventArgs e)
		//		{
		//			BeginInvokeOnMainThread (() => {
		//				ListenStatus.Text = e.Error.ToString();
		//			});
		//		}
		//
		//		private void OnPositionChanged (object sender, PositionEventArgs e)
		//		{
		//			BeginInvokeOnMainThread (() => {
		//				ListenStatus.Text = e.Position.Timestamp.ToString("G");
		//				ListenLatitude.Text = "La: " + e.Position.Latitude.ToString("N4");
		//				ListenLongitude.Text = "Lo: " + e.Position.Longitude.ToString("N4");
		//			});
		//		}
	}


}

