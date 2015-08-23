using System;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace App.WebAPI.Controllers
{
    public abstract class HubController<THub> : ApiController where THub : IHub
    {
        Lazy<IHubContext> hub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<THub>());

        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
    }
}
