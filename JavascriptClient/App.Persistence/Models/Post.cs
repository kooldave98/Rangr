using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Spatial;

namespace App.Persistence.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public DbGeography GeoLocation { get; set; }

        public virtual User User { get; set; }
        public int UserID { get; set; }    
    }
}
