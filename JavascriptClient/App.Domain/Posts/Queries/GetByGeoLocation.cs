using App.Domain.Geolocation;
using App.Domain.Service.Framework;
using App.Persistence.EF.Infrastructure;
using App.Service.Framework.Queries;
using System.Collections.Generic;
using App.GeoNow.Library.Utilities;
using System.Linq;

namespace App.Domain.Posts.Queries
{
    public class GetByGeoLocation : IQuery<IEnumerable<Details>, GetByGeolocationRequest>
    {
        private readonly RepositorySet repositories;

        public GetByGeoLocation()
        {
            this.repositories = new RepositorySet();
        }

        public IEnumerable<Details> Execute(GetByGeolocationRequest request)
        {
            var geolocation = request.GeoLocationString.ToDbGeography();
            //100 metres ??
            var posts = repositories.Posts.Entries
            .Where(p => p.GeoLocation.Distance(geolocation) <= 100 && p.ID >= request.StartIndex);

            return posts.Select(p => new Details
            {
                Text = p.Text,
                Date = p.Date,
                ID = p.ID,
                UserID = p.UserID,
                UserEmail = p.User.Email,
                GeoLocation = p.GeoLocation.AsText()
            });
        }
    }


    /// <summary>
    /// The request parameters for retrieving a collection of Posts from the data store
    /// </summary>
    public class GetByGeolocationRequest : Geolocation.Identity, ICollectionRequest
    {
        public GetByGeolocationRequest()
        {
            StartIndex = 0;
        }

        /// <summary>
        /// The index to start querying for results from
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// The maximum number of items to return
        /// </summary>
        public int MaxResults { get; set; }
    }
}
