using System.Data.Entity.Spatial;
using App.Library.Persistence;

namespace App.Persistence.Main
{
    public class GeoLocation : BaseEntity<int>
    {
        public DbGeography Geoposition { get; set; }

        public int AccuracyInMetres { get; set; }
    }
}
