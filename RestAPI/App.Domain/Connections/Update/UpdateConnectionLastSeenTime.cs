
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Connections;
using App.Services.Connections.Update;
using System.Linq;
using App.Services.Static;

namespace App.Domain.Connections.Update
{
    public class UpdateConnectionLastSeen : IUpdateConnectionLastSeen
    {
        private readonly IEntityRepository<Connection> connected_user_repository;
        private readonly IUnitOfWork unit_of_work;
        private Connection connected_user;
        ConnectionIdentity request;

        public UpdateConnectionLastSeen(IEntityRepository<Connection> the_connected_user_repository,
                                        IUnitOfWork the_unit_of_work)
        {
            connected_user_repository = Guard.IsNotNull(the_connected_user_repository, "the_connected_user_repository");
            unit_of_work = Guard.IsNotNull(the_unit_of_work, "the_unit_of_work");
        }

        public ConnectionIdentity execute(ConnectionIdentity the_request)
        {
            set_request(the_request)
                .find_connected_user()
                .update_connected_user_in_repository()
                .Commit();

            return new ConnectionIdentity() { connection_id = request.connection_id };
        }

        private UpdateConnectionLastSeen set_request(ConnectionIdentity the_request)
        {
            request = Guard.IsNotNull(the_request, "the_request");
            return this;
        }

        private UpdateConnectionLastSeen find_connected_user()
        {
            connected_user = connected_user_repository.Entities.Single(cu => cu.ID == request.connection_id);
            return this;
        }

        private UpdateConnectionLastSeen update_connected_user_in_repository()
        {
            Guard.IsNotNull(connected_user, "connected_user");

            connected_user.LastSeen = Resources.current_date_time;

            return this;
        }

        private UpdateConnectionLastSeen Commit()
        {
            unit_of_work.Commit();
            return this;
        }
    }
}
