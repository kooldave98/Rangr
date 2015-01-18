using System;
using System.Runtime.Serialization;

namespace App.Common
{
	/// <summary>
	/// The coordinates class.
	/// </summary>
	public class GeoCoordinate
	{
		public double Latitude { get; set; }

		public double Longitude { get; set; }

		#region optional items

		public int? Accuracy { get; set; }

		public double? Altitude { get; set; }

		public double? Bearing { get; set; }

		public double? Speed { get; set; }

		#endregion

		public DateTime TimeStamp { get; set; }

		public GeoCoordinate ()
		{
		}

		public GeoCoordinate (double latitude, double longitude)
		{
			this.Latitude = latitude;
			this.Longitude = longitude;
		}

		/// <summary>
		/// Calculates distance between two locations.
		/// </summary>
		/// <returns>The <see cref="System.Double"/>The distance in meters</returns>
		/// <param name="a">Location a</param>
		/// <param name="b">Location b</param>
		public static double DistanceBetween (GeoCoordinate a, GeoCoordinate b)
		{
			const double R_in_metres = 6371000;
			var lat = (b.Latitude - a.Latitude).ToRadians ();
			var lng = (b.Longitude - a.Longitude).ToRadians ();
			var h1 = Math.Sin (lat / 2) * Math.Sin (lat / 2) +
			         Math.Cos (a.Latitude.ToRadians ()) * Math.Cos (b.Latitude.ToRadians ()) *
			         Math.Sin (lng / 2) * Math.Sin (lng / 2);
			var h2 = 2 * Math.Asin (Math.Min (1, Math.Sqrt (h1)));
			return R_in_metres * h2;
		}

		/// <summary>
		/// Calculates bearing between start and stop.
		/// </summary>
		/// <returns>The <see cref="System.Double"/>.</returns>
		/// <param name="start">Start coordinates.</param>
		/// <param name="stop">Stop coordinates.</param>
		public static double BearingBetween (GeoCoordinate start, GeoCoordinate stop)
		{
			var deltaLon = stop.Longitude - start.Longitude;
			var cosStop = Math.Cos (stop.Latitude);
			return Math.Atan2 (
				(Math.Cos (start.Latitude) * Math.Sin (stop.Latitude)) -
				(Math.Sin (start.Latitude) * cosStop * Math.Cos (deltaLon)),
				Math.Sin (deltaLon) * cosStop);
		}

		/// <summary>
		/// Calculates this locations distance to another coordicate.
		/// </summary>
		/// <returns>The distance to another coordicate</returns>
		/// <param name="other">Other coordinates.</param>
		public double DistanceFrom (GeoCoordinate other)
		{
			return DistanceBetween (this, other);
		}

		/// <summary>
		/// Calculates this locations bearing to another coordicate.
		/// </summary>
		/// <returns>Bearing degree.</returns>
		/// <param name="other">Other coordinates.</param>
		public double BearingFrom (GeoCoordinate other)
		{
			return BearingBetween (this, other);
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString ()
		{
			return string.Format ("({0:0.0000}, {1:0.0000})", Latitude, Longitude);
		}

	}

	public static class GeoCoordinateExtensions
	{
		public static GeoCoordinate ToGeoCoordinateFromLongLatAccString (this string long_lat_acc_string)
		{
			var split = long_lat_acc_string.Split (',');

			if (split.Length != 3) {
				throw new InvalidOperationException ("long lat acc string format is invalid");
			}

			var geo_value = new GeoCoordinate (double.Parse (split [1]), double.Parse (split [0]));
			geo_value.Accuracy = int.Parse (split [2]);

			return geo_value;
		}

		public static string ToLongLatAccString (this GeoCoordinate long_lat_acc_string)
		{
			var geo_string = string.Format ("{0},{1},{2}", long_lat_acc_string.Longitude, long_lat_acc_string.Latitude, long_lat_acc_string.Accuracy);

			return geo_string;
		}
	}


	public static class NumericExtensions
	{
		public static double ToRadians (this double val)
		{
			return (Math.PI / 180) * val;
		}
	}

}