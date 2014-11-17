using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Portable;
using App.Core.Portable.Models;
using Newtonsoft.Json;
using System.Diagnostics;

namespace App.Common
{
	//category={category}
	//&start_index={start_index}
	//&max_results={max_results}
	//&collection_traversal={collection_traversal}
	//&first_load={first_load}
	//&hash_tag_name={hash_tag_name}
	//&connection_id={connection_id}
	public class Posts
	{

		public async Task<List<Post>> Get (string connection_id, 
		                                   string start_index,		                                   
		                                   bool first_load = false, 
		                                   CollectionTraversal traversal = CollectionTraversal.Newer,
		                                   GetPostsQueryCategory category = GetPostsQueryCategory.ByRadius,
		                                   string hash_tag_name = "")
		{
			var posts = new List<Post> ();

			var url = String.Format ("{0}/?connection_id={1}&start_index={2}", base_rest_url, connection_id, start_index);

			if (traversal == CollectionTraversal.Older) {
				url = url + "&collection_traversal=Older";
			}

			if (first_load) {
				url = url + "&first_load=true";
			}

			if (category == GetPostsQueryCategory.ByHashTag) {
				url = url + string.Format ("&category=ByHashTag&hash_tag_name={0}", hash_tag_name);
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

	public enum GetPostsQueryCategory
	{
		ByRadius,
		//Our default category, which is 200 metres
		ByHashTag,
	}

	public enum CollectionTraversal
	{
		Newer,
		//Our default traversal
		Older
	}
}
