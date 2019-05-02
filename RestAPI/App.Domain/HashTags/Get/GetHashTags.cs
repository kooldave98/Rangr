using System.Linq;
using System.Collections.Generic;
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.HashTags;
using App.Services.HashTags.Get;

namespace App.Domain.HashTags.Get
{
    public class GetHashTags : IGetHashTags
    {
        public IEnumerable<HashTagDetails> execute(GetHashTagsRequest request)
        {
            //return hash_tagged_posts_repository.Entities.

            return null;
        }

        public GetHashTags(IEntityRepository<HashTaggedPost> the_hash_tagged_posts_repository)
        {
            hash_tagged_posts_repository = Guard.IsNotNull(the_hash_tagged_posts_repository, "the_hash_tagged_posts_repository");
        }


        private IEntityRepository<HashTaggedPost> hash_tagged_posts_repository;
    }
}
