using System;
using System.Data.Spatial;

namespace App.Domain.Posts
{
    /// <summary>
    /// The request parameters for inserting a new Post
    /// </summary>
    public class InsertRequest
    {
        /// <summary>
        /// The ID of the User making the Post
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The GeoLocation where the post was made
        /// </summary>
        public string GeoLocation { get; set; }
        
        /// <summary>
        /// The Text of the Post
        /// </summary>
        public string Text { get; set; }
    }
}
