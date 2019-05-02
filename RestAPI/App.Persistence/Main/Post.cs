using System;
using App.Library.Persistence;

namespace App.Persistence.Main
{
    public class Post : BaseEntity<int>
    {
        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public virtual GeoLocation GeoLocation { get; set; }

        public virtual User User { get; set; }

        public string image_id { get; set; }
    }
}
