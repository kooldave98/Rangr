using App.Domain.HashTags.Get;
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services;
using App.Services.Posts;
using App.Services.Posts.Get;
using System.Collections.Generic;
using System.Linq;

namespace App.Domain.Posts.Get
{
    public class GetPosts : IGetPosts
    {
        public IEnumerable<PostDetails> execute(GetPostsRequest the_request)
        {
            set_request(the_request)
                .get_connection();


            if (request.first_load)
            {
                var latest_post = post_repository.Entities.AsEnumerable().LastOrDefault();
                if (latest_post != null)
                {
                    request.start_index = latest_post.ID;
                    request.collection_traversal = CollectionTraversal.Older;
                }
            }

            var query_repository = post_repository.Entities;

            if (request.category == GetPostsQueryCategory.ByRadius)
            {
                query_repository = get_posts_by_radius.execute(new GetPostsByRadiusRequest() { geo_location = connection.GeoLocation });
            }
            else if (request.category == GetPostsQueryCategory.ByHashTag)
            {
                var hash_tag = get_hash_tag_by_name.execute(new GetHashTagByNameRequest() { hash_tag_name = request.hash_tag_name });
                query_repository = get_posts_by_hash_tag.execute(new GetPostsByHashTagRequest() { hash_tag_id = hash_tag.ID });
            }


            return
               query_repository
                        .AsEnumerable()
                        .Where(p => (request.collection_traversal == CollectionTraversal.Older ? p.ID <= request.start_index : p.ID >= request.start_index))
                        .Order(p => request.category == GetPostsQueryCategory.ByRadius 
                                                        ? p.ID 
                                                        : p.GeoLocation.Geoposition.Distance(connection.GeoLocation.Geoposition)
                                    , request.category == GetPostsQueryCategory.ByRadius
                                                        ? request.collection_traversal != CollectionTraversal.Older
                                                        :true)
                        .Take(request.max_results > 0 ? request.max_results : 10)
                //.AsEnumerable()
                        .Select(p => new PostDetails
                        {
                            image_id = p.image_id,
                            text = p.Text,
                            date = p.DateTime,
                            post_id = p.ID,
                            user_id = p.User.ID,
                            user_display_name = p.User.DisplayName,
                            long_lat_acc_geo_string = string.Format("{0},{1},{2}", p.GeoLocation.Geoposition.Longitude, p.GeoLocation.Geoposition.Latitude, p.GeoLocation.AccuracyInMetres) 
                        });
        }

        private GetPosts set_request(GetPostsRequest the_request)
        {
            request = Guard.IsNotNull(the_request, "the_request");

            return this;
        }

        private GetPosts get_connection()
        {
            Guard.IsNotNull(request, "request");

            connection = connection_repository.Entities.Single(cu => cu.ID == request.connection_id);

            return this;
        }



        public GetPosts(IQueryRepository<Post> the_post_repository,
                        GetPostsByRadius the_get_posts_by_radius,
                        GetPostsByHashTag the_get_posts_by_hash_tag,
                        GetHashTagByName the_get_hash_tag_by_name,
                        IQueryRepository<Connection> the_connection_repository)
        {
            post_repository = Guard.IsNotNull(the_post_repository, "the_post_repository");
            connection_repository = Guard.IsNotNull(the_connection_repository, "the_connection_repository");

            get_posts_by_radius = Guard.IsNotNull(the_get_posts_by_radius, "the_get_posts_by_radius");
            get_posts_by_hash_tag = Guard.IsNotNull(the_get_posts_by_hash_tag, "the_get_posts_by_hash_tag");

            get_hash_tag_by_name = Guard.IsNotNull(the_get_hash_tag_by_name, "the_get_hash_tag_by_name");
        }

        private readonly IQueryRepository<Post> post_repository;
        private readonly IQueryRepository<Connection> connection_repository;
        private readonly GetPostsByRadius get_posts_by_radius;
        private readonly GetPostsByHashTag get_posts_by_hash_tag;
        private readonly GetHashTagByName get_hash_tag_by_name;

        private Connection connection;
        private GetPostsRequest request;

    }
}
