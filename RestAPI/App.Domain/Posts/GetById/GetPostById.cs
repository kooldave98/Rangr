
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Posts;
using App.Services.Posts.GetById;
using System;
using System.Linq;

namespace App.Domain.Posts.GetById
{
    public class GetPostById : IGetPostById
    {
        public GetPostById(IQueryRepository<Post> the_post_repository)
        {
            post_repository = Guard.IsNotNull(the_post_repository, "the_post_repository");
        }

        public PostDetails execute(PostIdentity request)
        {

            var post = post_repository.Entities.SingleOrDefault(d => d.ID == request.post_id);

            if (post == null)
            {
                throw new Exception("Post with this ID doesn't exist");
            }

            return new PostDetails
            {
                date = post.DateTime,
                long_lat_acc_geo_string = string.Format("{0},{1}", post.GeoLocation.Geoposition.Longitude, post.GeoLocation.Geoposition.Latitude),
                text = post.Text,
                user_display_name = post.User.DisplayName,
                user_id = post.User.ID,
                post_id = post.ID
            };
        }

        private readonly IQueryRepository<Post> post_repository;
    }
}
