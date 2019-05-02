using App.Library.CodeStructures.Behavioral;

namespace App.Services.Connections.Remove
{
    public interface IRemoveConnection : ICommand<ConnectionIdentity, ConnectionIdentity>
    {
    }
}
