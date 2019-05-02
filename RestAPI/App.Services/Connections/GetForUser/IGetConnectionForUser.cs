
using App.Library.CodeStructures.Behavioral;
using App.Services.Users;

namespace App.Services.Connections.GetForUser
{
    /// <summary>
    /// For now since our domain requirement is to only have 1 connection per user
    /// our return value will not be a collection of details. We can always change this later
    /// </summary>
    public interface IGetConnectionForUser : IQuery<UserIdentity, ConnectionDetails>
    {        
    }

    
}
