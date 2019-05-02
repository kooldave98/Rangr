
namespace App.Services.HashTags
{
    public class HashTagIdentity : IHashTagRequest
    {
        public string hash_tag_name { get; set; }
    }

    public interface IHashTagRequest
    {
        string hash_tag_name { get; set; }
    }
}
