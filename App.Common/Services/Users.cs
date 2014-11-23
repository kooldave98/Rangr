using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core;
using Newtonsoft.Json;
using System.Diagnostics;

namespace App.Common
{
	public class Users
	{
		public async Task<User> Get (string user_id)
		{
			User user = null;

			var url = string.Format ("{0}/?user_id={1}", base_rest_url, user_id);

			try {

				string data = await _httpRequest.Get (url);
				user = JsonConvert.DeserializeObject<User> (data);

			} catch (Exception e) {

				AppEvents.Current.TriggerConnectionFailedEvent ();
				Debug.WriteLine (e.Message);
			}
			return user;
		}

		public async Task<UserIdentity> Create (string user_display_name)
		{
			UserIdentity user_identity = null;

			var requestBody = new List<KeyValuePair<string, string>> () {
				new KeyValuePair<string, string> ("user_display_name", user_display_name)
			};

			var url = string.Format ("{0}/", base_rest_url);

			try {

				var data = await _httpRequest.Post (url, requestBody);        
				user_identity = JsonConvert.DeserializeObject<UserIdentity> (data);

			} catch (Exception e) {

				AppEvents.Current.TriggerConnectionFailedEvent ();
				Debug.WriteLine (e.Message);
			}

			return user_identity;
		}

		public async Task<UserIdentity> Update (string user_id, string user_display_name, string status_message)
		{
			UserIdentity user_identity = null;

			var requestBody = new List<KeyValuePair<string, string>> () {
				new KeyValuePair<string, string> ("user_id", user_id),
				new KeyValuePair<string, string> ("user_display_name", user_display_name),
				new KeyValuePair<string, string> ("user_status_message", status_message)
			};


			var url = String.Format ("{0}/", base_rest_url);

			try {

				string data = await _httpRequest.Put (url, requestBody);
				user_identity = JsonConvert.DeserializeObject<UserIdentity> (data);

			} catch (Exception e) {

				AppEvents.Current.TriggerConnectionFailedEvent ();
				Debug.WriteLine (e.Message);
			}
			return user_identity;
		}


		private const string restful_resource = "users";

		private HttpRequest _httpRequest {
			get {
				return HttpRequest.Current;
			}
		}

		private string base_rest_url {
			get {
				return string.Format ("{0}/{1}", Resources.baseUrl, restful_resource);
			}
		}
	}
}
