
using App.Library.CodeStructures.Behavioral;

namespace App.Services.Connections.Update
{
    public interface IUpdateSignalrConnection : ICommand<UpdateSignalrConnectionRequest, ConnectionIdentity>
    {
        
    }
}
