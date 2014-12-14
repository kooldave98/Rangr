using System;
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

        public static TimeAgoConverter Current
        {
            get
            {
                return _instance ?? (_instance = new TimeAgoConverter());
            }
        }

        private TimeAgoConverter()
        {
        }

        protected virtual string GetString(TimeAgo timeAgo, int time)
        {
            var plural_string = time > 1 ? "s" : string.Empty;

            return timeAgo == TimeAgo.Now
				? "Just Now"
                    : String.Format("{0} {1}{2} ago", time, timeAgo.ToString().ToLower(), plural_string);
        }


        public string Convert(object value)
        {
            DateTime when;
            if (!this.TryGetDateTime(value, out when))
                return String.Empty;

            var difference = (DateTime.UtcNow - when).TotalSeconds;
            var format = TimeAgo.Day;
            var time = 0;

            if (difference < 30.0)
            {
                format = TimeAgo.Now;
            }
            else if (difference < 100)
            {
                format = TimeAgo.Second;
                time = (int)difference;
            }
            else if (difference < 3600)
            {
                format = TimeAgo.Minute;
                time = (int)(difference / 60);
            }
            else if (difference < 24 * 3600)
            {
                format = TimeAgo.Hour;
                time = (int)(difference / (3600));
            }
            else
            {
                format = TimeAgo.Day;
                time = (int)(difference / (3600 * 24));
            }
            // TODO: future dates
            // TODO: weeks, months, years
            return this.GetString(format, time);
        }


        protected virtual bool TryGetDateTime(object value, out DateTime date)
        {
            date = DateTime.MinValue;

            if (value == null)
                return true;

            if (value is DateTime)
            {
                date = (DateTime)value;

                return true;
            }

            if (value is DateTimeOffset)
            {
                var offset = (DateTimeOffset)value;
                date = offset.UtcDateTime;
                return true;
            }
            throw new InvalidDataException("Invalid data type - " + value.GetType());
        }
    }

    public enum TimeAgo
    {
        Now,
        Second,
        Minute,
        Hour,
        Day
    }
}

