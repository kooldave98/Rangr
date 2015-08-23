using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Geolocation
{
    /// <summary>
    /// A wrapper object to Identify a specific point in the world
    /// </summary>
    public class Identity
    {
        /// <summary>
        /// The string format of the DBGeography location
        /// </summary>
        public string GeoLocationString { get; set; }
    }
}
