using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Portable;
using App.Core.Portable.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace App.Common
{
	public class Posts
	{
		public async Task<List<Post>> Get (string connection_id, string start_index, bool backward_traversal = false, bool latest_entries = false)
		{
			var posts = new List<Post> ();

			var url = String.Format ("{0}/?connection_id={1}&start_index={2}", base_rest_url, connection_id, start_index);

			if (backward_traversal) {
				url = url + "&backward_traversal=true";
			}


			if (latest_entries) {
				url = url + "&return_latest_entries_ignoring_other_params=true";
			}

			try {

				string data = await _httpRequest.Get (url);
				posts = JsonConvert.DeserializeObject<List<Post>> (data);

			} catch (Exception e) {

				AppEvents.Current.TriggerConnectionFailedEvent (e.Message);
				Debug.WriteLine (e.Message);
				//Todo: Probably load any few posts that have been cached locally
			}

			return posts;
		}

		public async Task<PostIdentity> Create (string text, string connection_id)
		{
			PostIdentity post_id = null;

			var requestBody = new List<KeyValuePair<string, string>> () {
				new KeyValuePair<string, string> ("connection_id", connection_id),
				new KeyValuePair<string, string> ("text", text)
			};

			var url = String.Format ("{0}/", base_rest_url);

			try {

				var data = await _httpRequest.Post (url, requestBody);
				post_id = JsonConvert.DeserializeObject<PostIdentity> (data);

			} catch (Exception e) {

				AppEvents.Current.TriggerConnectionFailedEvent (e.Message);
				Debug.WriteLine (e.Message);
			}

			return post_id;
		}


		private const string restful_resource = "posts";

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
