using System;
using App.Library.Persistence;

namespace App.Persistence.Main
{
    public class Connection : BaseEntity<int>
    {
        public virtual User User { get; set; }

        public virtual GeoLocation GeoLocation { get; set; }

        public string SignalRConnectionID { get; set; }

        public DateTime LastSeen { get; set; }
    }
}
