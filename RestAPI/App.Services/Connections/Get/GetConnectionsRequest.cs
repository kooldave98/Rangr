using App.Services.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Connections.Get
{
    public class GetConnectionsRequest : ConnectionIdentity
    {
        public int last_seen_age_in_minutes { get; set; }


        public GetConnectionsRequest()
        {
            last_seen_age_in_minutes = Resources.max_connection_age_in_minutes;
        }
    }
}
