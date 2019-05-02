using App.Library.CodeStructures.Behavioral;
using App.Services.Connections;
using App.Services.Connections.Create;
using App.Services.Connections.Get;
using App.Services.Connections.GetForUser;
using App.Services.Connections.Update;
using App.Services.Users;
using System;
using System.Collections.Generic;
using System.Web.Http;
using App.WebAPI.Controllers.Infrastructure;
using App.Library.DomainHelpers.Analytics;

namespace App.WebAPI.Controllers.Application
{
    public class ConnectionsController : BaseController
    {
        private readonly ICreateConnection new_connection_command;
        private readonly IUpdateConnection update_connection_command;
        private readonly IGetConnectionForUser get_connection_for_user_query;
        private readonly IGetConnections get_connections_query;

        /// <summary>
        /// Constructor to initialise dependencies 
        /// </summary>
        public ConnectionsController(ICreateConnection the_new_connection_command,
                                        IUpdateConnection the_update_connection_command,
                                        IGetConnectionForUser the_get_connection_for_user_query,
                                        IGetConnections the_get_connections_query)
        {
            new_connection_command = Guard.IsNotNull(the_new_connection_command, "the_new_connection_command");
            update_connection_command = Guard.IsNotNull(the_update_connection_command, "the_update_connection_command");
            get_connection_for_user_query = Guard.IsNotNull(the_get_connection_for_user_query, "the_get_connection_for_user_command");
            get_connections_query = Guard.IsNotNull(the_get_connections_query, "the_get_connections_command");
        }


        /// <summary>
        /// Gets a collection of Connections from the service based on the specified request params
        /// </summary>
        /// <param name="request">The GetConnectionsRequest parameters</param>
        /// <returns>A collection of Post Details</returns>
        public IEnumerable<ConnectionDetails> Get([FromUri]GetConnectionsRequest request)
        {
            return get_connections_query.execute(request);
        }



        /// <summary>
        /// Creates a Connection with the specified request parameters in the data store
        /// POST api/posts
        /// </summary>
        /// <param name="request">The 'Connection' Insert Request</param>
        public ConnectionIdentity Post([FromBody]CreateConnectionRequest request)
        {
            try
            {
                //Create new connection for user if none exists
                var id = new_connection_command.execute(request);
                return id;
            }
            catch (ArgumentOutOfRangeException)
            {
                //Get the existing connection that exists
                var connection = get_connection_for_user_query.execute(new UserIdentity() { user_id = request.user_id });

                //Update the connections geolocatio, sinc eit can be stale
                var id = update_connection_command.execute(new UpdateConnectionRequest()
                {
                    connection_id = connection.connection_id,
                    long_lat_acc_geo_string = request.long_lat_acc_geo_string
                });

                return id;
            }
        }

        /// <summary>
        /// Creates a Connection with the specified request parameters in the data store
        /// POST api/posts
        /// </summary>
        /// <param name="request">The 'Connection' Insert Request</param>
        public ConnectionIdentity Put([FromBody]UpdateConnectionRequest request)
        {
            //There is a regular optimistic concurrency exception
            //from the unit of work.commit method. Basically, I think it gets thrown,
            //when 2 or mor requests try to update the connection at the same time.
            //However, before I make any conclusions, I still need to properly investigate this.
            //Hence why I am catching it, but logging it to Elmah anyways. 
            try
            {
                var id = update_connection_command.execute(request);
                return id;
            }
            catch (Exception e)
            {
                //log to App Insights
                new AnalyticsProvider().TrackException(e);
                return request;
            }

        }
    }
}
