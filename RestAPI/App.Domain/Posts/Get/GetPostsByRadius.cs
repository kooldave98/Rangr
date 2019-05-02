using System.Linq;
using App.Domain.Helpers;
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;

namespace App.Domain.Posts.Get
{
    public class GetPostsByRadius
    {
        public IQueryable<Post> execute(GetPostsByRadiusRequest request)
        {
            Guard.IsNotNull(request.geo_location, "request.geo_location");

            return post_repository.Entities
                                    .AsEnumerable()
                                    .Where(p => p.GeoLocation.intersects(request.geo_location))
                                    .AsQueryable();
        }

        public GetPostsByRadius(IQueryRepository<Post> the_post_repository)
        {
            post_repository = Guard.IsNotNull(the_post_repository, "the_post_repository");
        }

        private readonly IQueryRepository<Post> post_repository;
    }

    public class GetPostsByRadiusRequest
    {
        public GeoLocation geo_location { get; set; }
    }
}
