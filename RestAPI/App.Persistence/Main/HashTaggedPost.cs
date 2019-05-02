using App.Library.Persistence;

namespace App.Persistence.Main
{
    public class HashTaggedPost : BaseEntity<int>
    {
        public HashTag HashTag { get; set; }

        public Post Post { get; set; }
    }
}
