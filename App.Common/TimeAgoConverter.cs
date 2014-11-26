﻿using System;
using System.IO;

/// <summary>
/// See also another implementation here:
/// http://aeykay.blogspot.co.uk/2012/10/facebook-like-time-ago-function.html
/// </summary>
namespace App.Common
{
	public class TimeAgoConverter
	{

		private static TimeAgoConverter _instance;

		public static TimeAgoConverter Current {
			get {
				return _instance ?? (_instance = new TimeAgoConverter ());
			}
		}

		private TimeAgoConverter ()
		{
		}

		protected virtual string GetString (TimeAgo timeAgo, int time)
		{
			return timeAgo == TimeAgo.Now
				? "Just Now"
					: String.Format ("{0} {1} ago", time, timeAgo.ToString ().ToLower ());
		}


		public string Convert (object value)
		{
			DateTime when;
			if (!this.TryGetDateTime (value, out when))
				return String.Empty;

			var difference = (DateTime.UtcNow - when).TotalSeconds;
			var format = TimeAgo.Days;
			var time = 0;

			if (difference < 30.0) {
				format = TimeAgo.Now;
			} else if (difference < 100) {
				format = TimeAgo.Seconds;
				time = (int)difference;
			} else if (difference < 3600) {
				format = TimeAgo.Minutes;
				time = (int)(difference / 60);
			} else if (difference < 24 * 3600) {
				format = TimeAgo.Hours;
				time = (int)(difference / (3600));
			} else {
				format = TimeAgo.Days;
				time = (int)(difference / (3600 * 24));
			}
			// TODO: future dates
			// TODO: weeks, months, years
			return this.GetString (format, time);
		}


		protected virtual bool TryGetDateTime (object value, out DateTime date)
		{
			date = DateTime.MinValue;

			if (value == null)
				return true;

			if (value is DateTime) {
				date = (DateTime)value;

				return true;
			}

			if (value is DateTimeOffset) {
				var offset = (DateTimeOffset)value;
				date = offset.UtcDateTime;
				return true;
			}
			throw new InvalidDataException ("Invalid data type - " + value.GetType ());
		}
	}

	public enum TimeAgo
	{
		Now,
		Seconds,
		Minutes,
		Hours,
		Days
	}
}

