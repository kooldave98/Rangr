using App.Domain.Posts;
using App.Domain.Posts.Commands;
using App.Domain.Posts.Queries;
using App.GeoNow.Library.Realtime.Persistence;
using App.WebAPI.SignalRArea.Hubs;
using System.Collections.Generic;
using System.Web.Http;

namespace App.WebAPI.Controllers
{
    /// <summary>
    /// Restful controller actions for the 'Posts' API
    /// </summary>
    public class PostsController : HubController<StreamHub>
    {
        private readonly GetByGeoLocation GetPostsByGeoLocation;
        private readonly Insert InsertPostCommand;
        private InMemoryRepository _repository;

        /// <summary>
        /// Constructor to initialise dependencies
        /// </summary>
        public PostsController()
        {
            GetPostsByGeoLocation = new GetByGeoLocation();
            InsertPostCommand = new Insert();
            _repository = InMemoryRepository.GetInstance();
        }

        /// <summary>
        /// Gets a collection of Posts from the service based on the specified request params
        /// GET api/posts/?geolocation=-2.2275772,53.4785067
        /// </summary>
        /// <param name="request">The GetByGeoLocationRequest parameters</param>
        /// <returns>A collection of Post Details</returns>
        public IEnumerable<Details> Get([FromUri]GetByGeolocationRequest request)
        {
            return GetPostsByGeoLocation.Execute(request);
        }

        /// <summary>
        /// Persists a Post with the specified request parameters in the data store
        /// POST api/posts
        /// </summary>
        /// <param name="request">The 'Post' Insert Request</param>
        public void Post([FromBody]InsertRequest request)
        {
            InsertPostCommand.Execute(request);
            var nearbyUsers = _repository.NearbyUsers(request.GeoLocation, 100);
            foreach (var user in nearbyUsers)
            {
                var connectionID = _repository.GetConnectionIdByUser(user.UserID);
                Hub.Clients.Client(connectionID).PollServer();
            }
        }

    }
}