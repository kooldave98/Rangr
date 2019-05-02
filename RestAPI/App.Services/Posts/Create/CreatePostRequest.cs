using App.Library.DomainHelpers.Types;

namespace App.Services.Posts.Create
{
    public class CreatePostRequest
    {
        public int connection_id { get; set; }

        public string text { get; set; }

        public string image_id { get; set; }

        public RawFile image_data { get; set; }
    }
}
