using System;
using System.Linq;
using App.Library.CodeStructures.Behavioral;
using App.Library.Persistence;
using App.Persistence.Main;
using App.Services.Static;

namespace App.Domain.HashTags.Get
{
    public class FindOrCreateAndGetHashTag
    {
        public HashTag execute(CreateHashTagRequest request)
        {
            Guard.IsNotNull(request, "request");

            if (string.IsNullOrWhiteSpace(request.hash_tag_name))
            {
                throw new Exception("Hash tag cannot be empty");
            }

            request.hash_tag_name = request.hash_tag_name.TrimStart('#');

            var hash_tag = hash_tag_repository.Entities.SingleOrDefault(h => h.HashTagName == request.hash_tag_name);

            if (hash_tag == null)
            {
                hash_tag = new HashTag()
                {
                    HashTagName = request.hash_tag_name,
                    DateTimeFirstCreated = Resources.current_date_time
                };

                hash_tag_repository.add(hash_tag);
            }
            
            return hash_tag;
        }

        public FindOrCreateAndGetHashTag(IEntityRepository<HashTag> the_hash_tag_repository)
        {
            hash_tag_repository = Guard.IsNotNull(the_hash_tag_repository, "the_hash_tag_repository");
        }

        private readonly IEntityRepository<HashTag> hash_tag_repository;
    }

    public class CreateHashTagRequest
    {
        public string hash_tag_name { get; set; }
    }
}
