using System;
using App.Library.Persistence;

namespace App.Persistence.Main
{
    public class HashTag : BaseEntity<int>
    {
        public string HashTagName { get; set; }

        public DateTime DateTimeFirstCreated { get; set; }
    }
}
