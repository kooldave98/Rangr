
using App.Library.CodeStructures.Behavioral;
using App.Library.DomainHelpers.Spatial;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Connections;
using App.Services.Connections.Update;
using System.Linq;
using App.Services.Static;
using App.Domain.Geo;

namespace App.Domain.Connections.Update
{
    public class UpdateConnection : IUpdateConnection
    {
        private UpdateConnectionRequest request;
        private readonly IEntityRepository<Connection> connected_user_repository;
        private readonly IUnitOfWork unit_of_work;
        private readonly FindOrCreateGeoLocation find_or_create_geolocation;

        private Connection connected_user;

        public UpdateConnection(IEntityRepository<Connection> the_connected_user_repository,
                                    FindOrCreateGeoLocation the_find_or_create_geolocation,
                                    IUnitOfWork the_unit_of_work)
        {
            connected_user_repository = Guard.IsNotNull(the_connected_user_repository, "the_connected_user_repository");
            unit_of_work = Guard.IsNotNull(the_unit_of_work, "the_unit_of_work");
            find_or_create_geolocation = Guard.IsNotNull(the_find_or_create_geolocation, "the_find_or_create_geolocation");
        }

        public ConnectionIdentity execute(UpdateConnectionRequest update_request)
        {
            set_request(update_request)
                .find_connected_user()
                .update_connected_user_in_repository()
                .Commit();

            return new ConnectionIdentity() { connection_id = connected_user.ID };
        }

        private UpdateConnection set_request(UpdateConnectionRequest create_request)
        {
            request = Guard.IsNotNull(create_request, "create_request");
            return this;
        }

        private UpdateConnection find_connected_user()
        {
            connected_user = connected_user_repository.Entities.Single(cu => cu.ID == request.connection_id);
            return this;
        }

        private UpdateConnection update_connected_user_in_repository()
        {
            Guard.IsNotNull(connected_user, "connected_user");
            Guard.IsNotNull(request, "request");


            var request_geolocation = request.long_lat_acc_geo_string.ToDbGeographyFromLongLatAccString();

            Guard.IsNotNull(request_geolocation, "request_geolocation");

            connected_user.GeoLocation = find_or_create_geolocation.execute(new GeoLocation()
                                                    {
                                                        Geoposition = request_geolocation,
                                                        AccuracyInMetres = request.long_lat_acc_geo_string.ToAccuracyInMetresFromLongLatAccString()
                                                    });
            connected_user.LastSeen = Resources.current_date_time;

            return this;
        }
        /// <summary>
        /// See below urls for how to resolve recurring Optimistic concurrency exception
        /// http://stackoverflow.com/questions/6819813/solution-for-store-update-insert-or-delete-statement-affected-an-unexpected-n
        /// http://msdn.microsoft.com/library/bb896255(v=vs.110).aspx
        /// </summary>
        /// <returns></returns>
        private UpdateConnection Commit()
        {
            unit_of_work.Commit();
            return this;
        }
    }
}
