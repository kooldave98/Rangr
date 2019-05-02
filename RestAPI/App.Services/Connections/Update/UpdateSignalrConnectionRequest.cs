using App.Services.Users;

namespace App.Services.Connections.Update
{
    public class UpdateSignalrConnectionRequest : ConnectionIdentity
    {
        public string signalr_connection_id { get; set; }
    }
}
