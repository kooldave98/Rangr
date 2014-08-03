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
	public class Users
    {
		public async Task<User> Get(string user_id)
        {
			var url = string.Format("{0}/?user_id={1}", base_rest_url, user_id);
            string data = await _httpRequest.Get(url);
            var user = JsonConvert.DeserializeObject<User>(data);
            return user;
        }

		public async Task<UserIdentity> Create(string user_display_name)
        {
            var requestBody = new List<KeyValuePair<string, string>>()
			{
				new KeyValuePair<string, string>("user_display_name", user_display_name)
			};

			var url = string.Format("{0}/", base_rest_url);
			var data = await _httpRequest.Post(url, requestBody);
        
			var user_identity = JsonConvert.DeserializeObject<UserIdentity>(data);
			return user_identity;
        }

		public async Task<UserIdentity> Update (string user_id, string user_display_name, string status_message)
		{
			var requestBody = new List<KeyValuePair<string, string>> () 
			{
				new KeyValuePair<string, string>( "user_id", user_id ),
				new KeyValuePair<string, string>( "user_display_name", user_display_name ),
				new KeyValuePair<string, string>( "user_status_message", status_message )
			};


			var url = String.Format ("{0}/", base_rest_url);
			string data = await _httpRequest.Put (url, requestBody);  

			var user_identity = JsonConvert.DeserializeObject<UserIdentity> (data);
			return user_identity;
		}


		private const string restful_resource = "users";
		private IHttpRequest _httpRequest;
		private string base_rest_url
		{
			get
			{
				return string.Format ("{0}/{1}", Resources.baseUrl, restful_resource);
			}
		}
		public Users(IHttpRequest httpRequest)
		{
			_httpRequest = httpRequest;
		}
    }
}
