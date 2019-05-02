namespace App.Services.Connections
{
    public class ConnectionDetails : ConnectionIdentity
    {
        public int user_id { get; set; }
        public string user_display_name { get; set; }
        public string user_status_message { get; set; }
        public string long_lat_acc_geo_string { get; set; }
    }
}
