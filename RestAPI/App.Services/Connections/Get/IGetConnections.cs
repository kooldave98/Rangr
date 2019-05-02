
using App.Library.CodeStructures.Behavioral;
using System.Collections.Generic;

namespace App.Services.Connections.Get
{
    public interface IGetConnections : IQuery<GetConnectionsRequest, IEnumerable<ConnectionDetails>>
    {        
    }

    
}
