using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using System.Linq;

namespace App.Domain.Geo
{
    
    public class FindOrCreateGeoLocation
    {
        public GeoLocation execute(GeoLocation request)
        {
            Guard.IsNotNull(request, "request");
            var result = geolocation_repository.Entities.FirstOrDefault(g => g.Geoposition.Latitude == request.Geoposition.Latitude
                                                    && g.Geoposition.Longitude == request.Geoposition.Longitude
                                                    && g.AccuracyInMetres == request.AccuracyInMetres);


            var returned_result = result ?? request;

            Guard.IsNotNull(returned_result, "returned_result");

            return returned_result;
        }

        public FindOrCreateGeoLocation(IEntityRepository<GeoLocation> the_geolocation_repository)
        {
            geolocation_repository = Guard.IsNotNull(the_geolocation_repository, "the_geolocation_repository");
        }

        private readonly IEntityRepository<GeoLocation> geolocation_repository;
    }
}
