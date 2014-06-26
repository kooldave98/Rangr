using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Portable;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using Newtonsoft.Json;

namespace App.Common
{
	public class Posts
    {        
		public async Task Create(string text, string connection_id)
        {
			var requestBody = new List<KeyValuePair<string, string>> () {
				new KeyValuePair<string, string>( "connection_id",connection_id ),
				new KeyValuePair<string, string>( "text",text )
			};

			var url = String.Format("{0}/", base_rest_url);
            await _httpRequest.Post(url, requestBody);
        }


		private const string restful_resource = "posts";
		private IHttpRequest _httpRequest;
		private string base_rest_url
		{
			get
			{
				return string.Format ("{0}/{1}", Resources.baseUrl, restful_resource);
			}
		}
		public Posts(IHttpRequest httpRequest)
		{
			_httpRequest = httpRequest;
		}

    }
}
