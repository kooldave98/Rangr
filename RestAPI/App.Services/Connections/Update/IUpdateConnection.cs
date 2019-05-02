
using App.Library.CodeStructures.Behavioral;

namespace App.Services.Connections.Update
{
    public interface IUpdateConnection : ICommand<UpdateConnectionRequest, ConnectionIdentity>
    {
        
    }
}
