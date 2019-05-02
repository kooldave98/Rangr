using System;

namespace App.Services.Posts
{
    /// <summary>
    /// Details of a Post exposed to the client
    /// </summary>
    public class PostDetails : PostIdentity
    {
        
        /// <summary>
        /// The text of the Post
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// The Date and Time the post was made
        /// </summary>
        public DateTime date { get; set; }

        /// <summary>
        /// The ID of the User who made the Post
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// The UserName of the User who made the POST
        /// </summary>
        public string user_display_name { get; set; }

        /// <summary>
        /// The location as long, lat, acc where the post was made
        /// </summary>
        public string long_lat_acc_geo_string { get; set; }

        public string image_id { get; set; }
    }
}