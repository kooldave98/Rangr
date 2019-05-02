
using App.Library.CodeStructures.Behavioral;

namespace App.Services.Connections.Create
{
    public interface ICreateConnection : ICommand<CreateConnectionRequest, ConnectionIdentity>
    {
        
    }
}
