
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Connections;
using App.Services.Connections.Update;
using System.Linq;
using App.Services.Static;

namespace App.Domain.Connections.Update
{
    public class UpdateSignalrConnection : IUpdateSignalrConnection
    {
        private UpdateSignalrConnectionRequest request;
        private readonly IEntityRepository<Connection> connected_user_repository;
        private readonly IUnitOfWork unit_of_work;

        private Connection connected_user;

        public UpdateSignalrConnection(IEntityRepository<Connection> the_connected_user_repository
                                    , IUnitOfWork the_unit_of_work)
        {
            connected_user_repository = Guard.IsNotNull(the_connected_user_repository, "the_connected_user_repository");
            unit_of_work = Guard.IsNotNull(the_unit_of_work, "the_unit_of_work");
        }

        public ConnectionIdentity execute(UpdateSignalrConnectionRequest update_request)
        {
            set_request(update_request)
                .find_connected_user()
                .update_connected_user_in_repository()
                .Commit();

            return new ConnectionIdentity() { connection_id = connected_user.ID };
        }

        private UpdateSignalrConnection set_request(UpdateSignalrConnectionRequest update_request)
        {
            request = Guard.IsNotNull(update_request, "create_request");
            return this;
        }

        private UpdateSignalrConnection find_connected_user()
        {
            connected_user = connected_user_repository.Entities.Single(cu => cu.ID == request.connection_id);
            return this;
        }

        private UpdateSignalrConnection update_connected_user_in_repository()
        {
            Guard.IsNotNull(connected_user, "connected_user");

            connected_user.SignalRConnectionID = request.signalr_connection_id;
            connected_user.LastSeen = Resources.current_date_time;

            return this;
        }

        private UpdateSignalrConnection Commit()
        {
            unit_of_work.Commit();
            return this;
        }
    }
}
