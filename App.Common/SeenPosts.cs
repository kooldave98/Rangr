using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core;
using App.Core.Portable;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using Newtonsoft.Json;

namespace App.Common
{
	public class SeenPosts
    {
		public async Task<List<SeenPost>> Get(string connection_id, string start_index)
        {
			var url = String.Format("{0}/?connection_id={1}&start_index={2}", base_rest_url, connection_id, start_index);
            string data = await _httpRequest.Get(url);

			var seen_posts = JsonConvert.DeserializeObject<List<SeenPost>>(data);
			return seen_posts;
        }

		private const string restful_resource = "seenposts";
		private IHttpRequest _httpRequest;
		private string base_rest_url
		{
			get
			{
				return string.Format ("{0}/{1}", Resources.baseUrl, restful_resource);
			}
		}
		public SeenPosts(IHttpRequest httpRequest)
		{
			_httpRequest = httpRequest;
		}
    }
}
