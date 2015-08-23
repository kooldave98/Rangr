using System.Data.Spatial;

namespace App.GeoNow.Library.Utilities
{
    public static class StringExtensions
    {
        public static DbGeography ToDbGeography(this string value) {
            if (value == null)
            {
                return (DbGeography)null;
            }
            string[] latLongStr = value.Split(',');
            // TODO: More error handling here, what if there is more than 2 pieces or less than 2?
            //       Are we supposed to populate ModelState with errors here if we can't conver the value to a point?
            string point = string.Format("POINT ({0} {1})", latLongStr[1], latLongStr[0]);
            //4326 format puts LONGITUDE first then LATITUDE
            DbGeography result = DbGeography.FromText(point, 4326);
            return result;
        }
    }
}
