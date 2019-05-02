using System.Linq;
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;

namespace App.Domain.Posts.Get
{
    public class GetPostsByHashTag
    {
        public IQueryable<Post> execute(GetPostsByHashTagRequest request)
        {
            Guard.IsNotNull(request, "request");
            return hash_tagged_post_repository.Entities
                                                .Where(htp => htp.HashTag.ID == request.hash_tag_id)
                                                .Select(v => v.Post);
        }

        public GetPostsByHashTag(IQueryRepository<HashTaggedPost> the_hash_tagged_post_repository)
        {
            hash_tagged_post_repository = Guard.IsNotNull(the_hash_tagged_post_repository, "the_hash_tagged_post_repository");
        }

        private readonly IQueryRepository<HashTaggedPost> hash_tagged_post_repository;
    }

    public class GetPostsByHashTagRequest
    {
        public int hash_tag_id { get; set; }
    }
}
