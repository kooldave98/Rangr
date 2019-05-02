using App.Library.Persistence;
using System.Collections.Generic;

namespace App.Persistence.Main
{
    public class User : BaseEntity<int>
    {
        public string DisplayName { get; set; }

        public virtual ICollection<Post> Posts
        {
            get { return _posts ?? (_posts = new HashSet<Post>()); }
            set { _posts = value; }
        }
        private ICollection<Post> _posts;

        public string StatusMessage { get; set; }

        public string DeleteDBTrigger { get; set; }
    }
}
