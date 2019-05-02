
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Connections;
using App.Services.Connections.GetForUser;
using App.Services.Users;
using System.Linq;

namespace App.Domain.Connections.GetForUser
{
    /// <summary>
    /// For the forseeable future this will always return one item
    /// I thought of a case where there might be multiple devices for the same user,
    /// in that case we will return a collection
    /// </summary>
    public class GetConnectionForUser : IGetConnectionForUser
    {
        public ConnectionDetails execute(UserIdentity request)
        {
            var cu = repository.Entities.Single(c => c.User.ID == request.user_id);

            return new ConnectionDetails()
            {
                connection_id = cu.ID,
                user_display_name = cu.User.DisplayName,
                user_status_message = cu.User.StatusMessage,
                long_lat_acc_geo_string = string.Format("{0},{1},{2}", cu.GeoLocation.Geoposition.Longitude, cu.GeoLocation.Geoposition.Latitude, cu.GeoLocation.AccuracyInMetres),
                user_id = cu.User.ID
            };
        }



        public GetConnectionForUser(IQueryRepository<Connection> the_repository)
        {
            repository = Guard.IsNotNull(the_repository, "the_repository");
        }

        private readonly IQueryRepository<Connection> repository;

    }


}
