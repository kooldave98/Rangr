using App.Library.CodeStructures.Behavioral;

namespace App.Services.Connections.Update
{
    public interface IUpdateConnectionLastSeen : ICommand<ConnectionIdentity, ConnectionIdentity>
    {
        
    }
}
