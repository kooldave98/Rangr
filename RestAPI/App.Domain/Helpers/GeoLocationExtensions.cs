using App.Library.CodeStructures.Behavioral;
using App.Persistence.Main;
using App.Services.Static;

namespace App.Domain.Helpers
{
    public static class GeoLocationExtensions
    {
        public static bool intersects(this GeoLocation locationA, GeoLocation locationB)
        {
            Guard.IsNotNull(locationA, "locationA");
            Guard.IsNotNull(locationB, "locationB");

            var point_a = locationA.Geoposition;
            var radius_a = Resources.determine_radius(locationA.AccuracyInMetres);

            var point_b = locationB.Geoposition;
            var radius_b = Resources.determine_radius(locationB.AccuracyInMetres);

            var circleA = point_a.Buffer(radius_a + 0.0);
            var circleB = point_b.Buffer(radius_b + 0.0);

            return circleA.Intersects(circleB);
        }
    }
}
