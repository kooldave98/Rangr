

using App.Library.EntityFramework.Configuration;
using App.Persistence.Main;

namespace App.Persistence.EF.Domain.Main.GeoLocations
{
    public class ModelBuilder : ModelConfiguration<GeoLocation>
    {

        public ModelBuilder(string schema)
        {

            Map(m => m.ToTable("GeoLocation", schema));
        }
    }
}
