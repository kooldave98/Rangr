using System;
using System.Linq;
using App.Domain.HashTaggedPosts.Create;
using App.Domain.HashTags.Get;
using App.Library.CodeStructures.Behavioral;
using App.Persistence.Main;

namespace App.Domain.Posts.Analyse
{
    public class AnalysePost
    {
        public void execute(Post request)
        {
            Guard.IsNotNull(request, "request");

            if (string.IsNullOrWhiteSpace(request.Text))
            {
                throw new Exception("Hash tag cannot be empty");
            }

            request.Text.Split(null)
                        .Where(w => w.StartsWith("#"))
                        .Do(h =>
                        {
                            var hash_tag = create_hash_tag.execute(new CreateHashTagRequest() { hash_tag_name = h });
                            create_hash_tagged_post.execute(new CreateHashTaggedPostRequest() { post = request, hash_tag = hash_tag });
                        });

        }

        public AnalysePost(FindOrCreateAndGetHashTag the_create_hash_tag,
                            CreateHashTaggedPost the_create_hash_tagged_post)
        {
            create_hash_tag = Guard.IsNotNull(the_create_hash_tag, "the_create_hash_tag");
            create_hash_tagged_post = Guard.IsNotNull(the_create_hash_tagged_post, "the_create_hash_tagged_post");
        }

        private readonly FindOrCreateAndGetHashTag create_hash_tag;
        private readonly CreateHashTaggedPost create_hash_tagged_post;
    }
}
