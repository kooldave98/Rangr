using App.Domain.Geo;
using App.Library.CodeStructures.Behavioral;
using App.Library.DomainHelpers.Spatial;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Connections;
using App.Services.Connections.Create;
using App.Services.Static;
using System;
using System.Linq;

namespace App.Domain.Connections.Create
{
    public class CreateConnection : ICreateConnection
    {
        private CreateConnectionRequest request;
        private readonly IEntityRepository<Connection> connected_user_repository;
        private readonly IEntityRepository<User> user_repository;
        private readonly IUnitOfWork unit_of_work;

        private readonly FindOrCreateGeoLocation find_or_create_geolocation;

        private Connection connected_user;
        private User user;

        public CreateConnection(IEntityRepository<Connection> the_connected_user_repository,
                                IEntityRepository<User> the_user_repository,
                                FindOrCreateGeoLocation the_find_or_create_geolocation,
                                IUnitOfWork the_unit_of_work)
        {
            connected_user_repository = Guard.IsNotNull(the_connected_user_repository, "the_connected_user_repository");
            user_repository = Guard.IsNotNull(the_user_repository, "the_user_repository");
            unit_of_work = Guard.IsNotNull(the_unit_of_work, "the_unit_of_work");
            find_or_create_geolocation = Guard.IsNotNull(the_find_or_create_geolocation, "the_find_or_create_geolocation");

        }

        public ConnectionIdentity execute(CreateConnectionRequest create_request)
        {
            set_request(create_request)
                .find_user()
                .validate()
                .add_connected_user_to_repository()
                .Commit();

            return new ConnectionIdentity() { connection_id = connected_user.ID };
        }

        private CreateConnection set_request(CreateConnectionRequest create_request)
        {
            request = Guard.IsNotNull(create_request, "create_request");
            return this;
        }

        private CreateConnection find_user()
        {
            Guard.IsNotNull(request, "request");
            Guard.IsNotNull(user_repository, "user_repository");

            user = user_repository.Entities.Single(u => u.ID == request.user_id);
            return this;
        }

        private CreateConnection validate()
        {
            var connection = connected_user_repository.Entities.SingleOrDefault(cu => cu.User.ID == request.user_id);
            if (connection != null)
            {
                throw new ArgumentOutOfRangeException("request.user_id", "A connection with this user_id already exists");
            }

            return this;
        }

        private CreateConnection add_connected_user_to_repository()
        {
            Guard.IsNotNull(connected_user_repository, "connected_user_repository");
            Guard.IsNotNull(request, "request");
            
            Guard.IsNotNull(user, "user");

            var request_geolocation = request.long_lat_acc_geo_string.ToDbGeographyFromLongLatAccString();

            Guard.IsNotNull(request_geolocation, "request_geolocation");

            connected_user = new Connection
            {
                User = user,
                GeoLocation = find_or_create_geolocation.execute(new GeoLocation()
                {
                    Geoposition = request_geolocation,
                    AccuracyInMetres = request.long_lat_acc_geo_string.ToAccuracyInMetresFromLongLatAccString()
                }),
                LastSeen = Resources.current_date_time
            };

            connected_user_repository.add(connected_user);
            return this;
        }

        private CreateConnection Commit()
        {
            unit_of_work.Commit();
            return this;
        }
    }
}
