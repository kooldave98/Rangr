using System;
using System.Linq;
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;

namespace App.Domain.HashTags.Get
{
    public class GetHashTagByName
    {
        public HashTag execute(GetHashTagByNameRequest request)
        {
            Guard.IsNotNull(request, "request");

            if (string.IsNullOrWhiteSpace(request.hash_tag_name))
            {
                throw new Exception("Hash tag cannot be empty");
            }

            return hash_tag_repository.Entities
                                        .Single(ht => ht.HashTagName == request.hash_tag_name);
        }

        public GetHashTagByName(IQueryRepository<HashTag> the_hash_tag_repository)
        {
            hash_tag_repository = Guard.IsNotNull(the_hash_tag_repository, "the_hash_tag_repository");
        }

        private readonly IQueryRepository<HashTag> hash_tag_repository;
    }

    public class GetHashTagByNameRequest
    {
        public string hash_tag_name { get; set; }
    }
}