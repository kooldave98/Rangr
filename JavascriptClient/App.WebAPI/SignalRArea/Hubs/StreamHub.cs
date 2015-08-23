using App.GeoNow.Library.Realtime.Models;
using App.GeoNow.Library.Realtime.Persistence;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.WebAPI.SignalRArea.Hubs
{
    public class StreamHub : Hub
    {
        private InMemoryRepository _repository;

        public StreamHub()
        {
            _repository = InMemoryRepository.GetInstance();
        }

        /// <summary>
        /// Fired when a client joins the chat. Here round trip state is available and we can register the user in the list
        /// </summary>
        public override Task OnConnected()
        {
            string userID = Context.QueryString["userID"];//replace with mac address or some unique ID later
            string geoLocation = Context.QueryString["geoLocation"];

            var user = new User()
            {
                //Id = Context.ConnectionId,                
                UserID = userID,
                GeoLocation = geoLocation
                //Guid.NewGuid().ToString(),
                //UserName = Clients.Caller.username
            };
            _repository.Add(user);
            _repository.AddMapping(Context.ConnectionId, user.UserID);
            //Clients.All.joins(user.Id, Clients.Caller.username, DateTime.Now);

            return base.OnConnected();
        }

        /// <summary>
        /// Invoked when a client connects. Retrieves the list of all currently connected users
        /// </summary>
        /// <returns></returns>
        public ICollection<User> GetConnectedUsers()
        {
            return _repository.Users.ToList<User>();
        }

        /// <summary>
        /// Fired when a client disconnects from the system. The user associated with the client ID gets deleted from the list of currently connected users.
        /// </summary>
        /// <returns></returns>
        public override Task OnDisconnected()
        {
            var userId = _repository.GetUserByConnectionId(Context.ConnectionId);
            if (userId != null)
            {
                var user = _repository.Users.Where(u => u.UserID == userId).FirstOrDefault();
                if (user != null)
                {
                    _repository.Remove(user);
                    return Clients.All.leaves(user.UserID, user.UserName, DateTime.Now);
                }
            }

            return base.OnDisconnected();
        }

        public void UpdateLocation(string geolocation)
        {
            string userID = Context.QueryString["userID"];
            
            _repository.UpdateUserGeoLocation(userID, geolocation);
        }
    }

    //public class Post : Hub
    //{
    //    private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

    //    public override Task OnConnected()
    //    {
    //        string name = Context.QueryString["name"];//replace with mac address or some unique ID later

    //        _connections.Add(name, Context.ConnectionId);

    //        return base.OnConnected();
    //    }

    //    public override Task OnDisconnected()
    //    {
    //        string name = Context.QueryString["name"];

    //        _connections.Remove(name, Context.ConnectionId);

    //        return base.OnDisconnected();
    //    }
    //}
}