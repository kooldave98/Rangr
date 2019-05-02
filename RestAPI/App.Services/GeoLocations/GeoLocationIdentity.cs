using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Geolocation
{
    /// <summary>
    /// A wrapper object to Identify a specific point in the world
    /// </summary>
    public class GeoLocationIdentity
    {

        public int geolocation_id { get; set; }

        /// <summary>
        /// The string format of the DBGeography location
        /// </summary>
        public string geolocation_string { get; set; }
    }
}
