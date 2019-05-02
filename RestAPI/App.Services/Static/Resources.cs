using System;

namespace App.Services.Static
{
    public class Resources
    {
        /// <summary>
        /// The radial distance that will be reachable for a post
        /// N/B: BEFORE WHEN WE WERE CALCULATING DISTANCE FROM POINT TO POINT
        /// WE USED 100Metres; NOW WE ARE USING 50Metres BECAUSE WE ARE NOW USING POLYGONS
        /// IN LAME TERMS CIRCLES :)
        /// </summary>
        public const int nearby_radius_in_metres = 100;

        /// <summary>
        /// The connection age limit for a valid connection
        /// </summary>
        public const int max_connection_age_in_minutes = 5;

        /// <summary>
        /// The current universal time
        /// </summary>
        public static DateTime current_date_time { get { return DateTime.UtcNow; } }


        public static int determine_radius(int location_accurracy)
        {
            var radius_range = nearby_radius_in_metres;

            if (radius_range < location_accurracy)
            {
                radius_range = location_accurracy;
            }

            return radius_range;
        }
    }
}
