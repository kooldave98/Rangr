using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using App.Library.Azure.Storage;
using App.Library.CodeStructures.Behavioral;
using App.Services.Posts;
using App.Services.Posts.Create;
using App.Services.Posts.Get;
using App.WebAPI.Controllers.Infrastructure;

namespace App.WebAPI.Controllers.Application
{
    
    public class CreatePostController : BaseController
    {
        private readonly ICreatePost new_post_command;

         /// <summary>
         /// Constructor to initialise dependencies
         /// </summary>
        public CreatePostController(ICreatePost the_new_post_command)
        {
            new_post_command = Guard.IsNotNull(the_new_post_command, "the_new_post_command");
        }

        /// <summary>
        /// Persists a Post with the specified request parameters in the data store
        /// POST api/posts
        /// </summary>
        /// <param name="request">The 'Post' Insert Request</param>
        [Route("api/posts/create")]
        [HttpPost]
        public async Task<PostIdentity> Post([FromBody]CreatePostRequest request)
        {
            var azure = new AzureStorageService();
            request.image_id = await azure.upload(request.image_data.Buffer);

            var id = new_post_command.execute(request);
            return id;
            //var post = get_post_by_id_query.execute(id);
            //NotifyConnectedClients(request.geolocation_string, post);
        }

        private void NotifyConnectedClients(string geolocation, PostDetails post)
        {
            //var nearbyUsers = _repository.NearbyUsers(geolocation, 100);
            //foreach (var user in nearbyUsers)
            //{
            //    var connectionID = _repository.GetConnectionIdByUser(user.UserID);
            //    Hub.Clients.Client(connectionID).AppendPost(post);
            //}
        }

        private async Task NotifyClientsNearAsync(string geolocation)
        {
            await Task.Factory.StartNew(() =>
            {
                
            });
        }

    }


    public class GetPostsController : BaseController
    {
        private readonly IGetPosts get_posts_query;

        public GetPostsController(IGetPosts the_get_posts_query)
        {
            get_posts_query = Guard.IsNotNull(the_get_posts_query, "the_get_posts_query");
        }

        /// <summary>
        /// Gets a collection of Posts from the service based on the specified request params
        /// </summary>
        /// <param name="request">The GetPostsRequest parameters</param>
        /// <returns>A collection of Post Details</returns>
        [Route("api/posts")]
        [HttpGet]
        public IEnumerable<PostDetails> Get([FromUri]GetPostsRequest request)
        {
            var data = get_posts_query.execute(request);
            return data;
        }
    }
}