using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Connections;
using App.Services.Connections.Get;
using App.Services.Static;
using System.Collections.Generic;
using System.Linq;
using App.Domain.Helpers;

namespace App.Domain.Connections.Get
{
    public class GetConnections : IGetConnections
    {
        public IEnumerable<ConnectionDetails> execute(GetConnectionsRequest the_request)
        {
            set_request(the_request)
                .get_connection();



            var result =
                connection_repository
                .Entities
                .AsEnumerable()
                .Where(cu => cu.ID != request.connection_id
                            && cu.GeoLocation.intersects(connection.GeoLocation))
                .OrderBy(cu => cu.GeoLocation.Geoposition.Distance(connection.GeoLocation.Geoposition))
                //.AsEnumerable()
                //Beacuse datetime queries cannot be executed on SQL Server, so this has to be done in memory
                .Where(cu => (Resources.current_date_time - cu.LastSeen).TotalMinutes < request.last_seen_age_in_minutes)
                .Select(cu => new ConnectionDetails()
                {
                    connection_id = cu.ID,
                    user_display_name = cu.User.DisplayName,
                    user_status_message = cu.User.StatusMessage,
                    long_lat_acc_geo_string = string.Format("{0},{1},{2}", cu.GeoLocation.Geoposition.Longitude, cu.GeoLocation.Geoposition.Latitude, cu.GeoLocation.AccuracyInMetres),
                    user_id = cu.User.ID
                });

            return result;
        }

        private GetConnections set_request(GetConnectionsRequest the_request)
        {
            request = Guard.IsNotNull(the_request, "the_request");

            return this;
        }

        private GetConnections get_connection()
        {
            Guard.IsNotNull(request, "request");

            connection = connection_repository.Entities.Single(cu => cu.ID == request.connection_id);

            return this;
        }


        public GetConnections(IQueryRepository<Connection> the_connection_repository)
        {
            connection_repository = Guard.IsNotNull(the_connection_repository, "the_connection_repository");
        }

        private readonly IQueryRepository<Connection> connection_repository;


        private Connection connection;
        private GetConnectionsRequest request;
    }


}
