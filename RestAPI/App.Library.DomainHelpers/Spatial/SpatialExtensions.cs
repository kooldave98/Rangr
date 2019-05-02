using System;
using System.Data.Entity.Spatial;

namespace App.Library.DomainHelpers.Spatial
{
    public static class SpatialExtensions
    {
        public static DbGeography ToDbGeographyFromLongLatAccString(this string long_lat_acc_geo_value)
        {
            if (long_lat_acc_geo_value == null)
            {
                return (DbGeography)null;
            }

            // TODO: More error handling here, what if there is more than 2 pieces or less than 2?
            // Are we supposed to populate ModelState with errors here if we can't conver the value to a point?
            var long_lat_str = long_lat_acc_geo_value.Split(',');


            var longitude = double.Parse(long_lat_str[0]);
            var latitude = double.Parse(long_lat_str[1]);
            const int coordinateSystemId = 4326;

            //4326 format puts LONGITUDE first then LATITUDE
            var point = string.Format("POINT ({0} {1})", longitude, latitude);


            var result = DbGeography.FromText(point, coordinateSystemId);
            return result;
        }

        public static int ToAccuracyInMetresFromLongLatAccString(this string long_lat_acc_geo_value)
        {
            var long_lat_str = long_lat_acc_geo_value.Split(',');

            if (long_lat_str.Length != 3)
            {
                throw new InvalidOperationException("String input is not a valid long, lat, acc string");
            }

            return int.Parse(long_lat_str[2]);
        }
    }
}
