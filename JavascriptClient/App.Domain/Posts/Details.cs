using System;
using System.Collections.Generic;

namespace App.Domain.Posts
{
    /// <summary>
    /// Details of a Post exposed to the client
    /// </summary>
    public class Details
    {
        /// <summary>
        /// The ID of the Post
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// The text of the Post
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// The Date and Time the post was made
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// The ID of the User who made the Post
        /// </summary>
        public int UserID { get; set; }
        
        /// <summary>
        /// The UserName of the User who made the POST
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// The Geolocation point where the post was made
        /// </summary>
        public string GeoLocation { get; set; }

    }
}