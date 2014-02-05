using App.Core.Portable.Models;
using App.Core.Portable.Network;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App.Core.Portable;

namespace App.Common.Shared
{
	 class PostRepository
    {
        private IHttpRequest _httpRequest;

        //loose coupling
        public PostRepository(IHttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public async Task<List<Post>> GetPostsByLocation(int StartIndex, string GeoLocationString)
        {
            var url = String.Format("{0}/posts/?GeoLocationString={1}&StartIndex={2}", Resources.baseUrl, GeoLocationString, StartIndex);
            string data = await _httpRequest.Get(url);
            var posts = JsonConvert.DeserializeObject<List<Post>>(data);
            return posts;
        }

        public async Task CreatePost(string Text, string UserID, string GeoLocation)
        {
            var requestBody = new List<KeyValuePair<string, string>>();
            requestBody.Add(new KeyValuePair<string,string>("UserID",UserID));
            requestBody.Add(new KeyValuePair<string,string>("Text",Text));
            requestBody.Add(new KeyValuePair<string, string>("GeoLocationString", GeoLocation));
            
            
            
            var url = String.Format("{0}/posts/", Resources.baseUrl);
            await _httpRequest.Post(url, requestBody);            
        }
    }
}
