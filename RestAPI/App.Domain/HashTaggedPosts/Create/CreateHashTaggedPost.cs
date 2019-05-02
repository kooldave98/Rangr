using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;

namespace App.Domain.HashTaggedPosts.Create
{
    public class CreateHashTaggedPost
    {
        public HashTaggedPost execute(CreateHashTaggedPostRequest request)
        {
            Guard.IsNotNull(request.hash_tag, "request.hash_tag");
            Guard.IsNotNull(request.post, "request.post");

            var hash_tagged_post = new HashTaggedPost() { HashTag = request.hash_tag, Post = request.post };

            hash_tagged_post_repository.add(hash_tagged_post);

            return hash_tagged_post;
        }

        public CreateHashTaggedPost(IEntityRepository<HashTaggedPost> the_hash_tagged_post_repository)
        {
            hash_tagged_post_repository = Guard.IsNotNull(the_hash_tagged_post_repository, "the_hash_tagged_post_repository");
        }

        private readonly IEntityRepository<HashTaggedPost> hash_tagged_post_repository;
    }

    public class CreateHashTaggedPostRequest
    {
        public HashTag hash_tag { get; set; }
        public Post post { get; set; }
    }
}
