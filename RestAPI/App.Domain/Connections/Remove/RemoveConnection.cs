using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Connections;
using App.Services.Connections.Remove;
using System.Linq;

namespace App.Domain.Connections.Remove
{
    public class RemoveConnection : IRemoveConnection
    {
        public ConnectionIdentity execute(ConnectionIdentity request)
        {
            var connected_user = connected_user_repository.Entities.SingleOrDefault(e => e.ID == request.connection_id);
            
            connected_user_repository.remove(connected_user);
            unit_of_work.Commit();            

            return request;
        }

        public RemoveConnection(IUnitOfWork the_unit_of_work, IEntityRepository<Connection> the_connected_user_repository)
        {
            unit_of_work = Guard.IsNotNull(the_unit_of_work, "the_unit_of_work");
            connected_user_repository = Guard.IsNotNull(the_connected_user_repository, "the_connected_user_repository");
        }

        private readonly IUnitOfWork unit_of_work;
        private readonly IEntityRepository<Connection> connected_user_repository;

    }
}
