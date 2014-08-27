using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Core.Portable;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using Newtonsoft.Json;

namespace App.Common
{
	public class Connections
	{
		public async Task<List<Connection>> Get (string connection_id)
		{
			var connections = new List<Connection> ();

			var url = String.Format ("{0}/?connection_id={1}", base_rest_url, connection_id);

			try {

				var data = await _httpRequest.Get (url);
				connections = JsonConvert.DeserializeObject<List<Connection>> (data);

			} catch (Exception) {
				AppEvents.Current.TriggerConnectionFailedEvent ();
				//Todo: log exception
			}

			return connections;
		}

		public async Task<ConnectionIdentity> Create (string user_id, string geolocation_string, string geolocation_accuracy_in_metres)
		{
			ConnectionIdentity connection_identity = null;

			var requestBody = new List<KeyValuePair<string, string>> () {
				new KeyValuePair<string, string> ("user_id", user_id),
				new KeyValuePair<string, string> ("geolocation_string", geolocation_string),
				new KeyValuePair<string, string> ("geolocation_accuracy_in_metres", geolocation_accuracy_in_metres)
			};            
            
			var url = String.Format ("{0}/", base_rest_url);

			try {

				var data = await _httpRequest.Post (url, requestBody);  
				connection_identity = JsonConvert.DeserializeObject<ConnectionIdentity> (data);

			} catch (Exception) {

				AppEvents.Current.TriggerConnectionFailedEvent ();
				//Todo: log exception
			}

			return connection_identity;
		}

		public async Task<ConnectionIdentity> Update (string connection_id, string geolocation_string, string geolocation_accuracy_in_metres)
		{
			ConnectionIdentity connection_identity = null;

			var requestBody = new List<KeyValuePair<string, string>> () {
				new KeyValuePair<string, string> ("connection_id", connection_id),
				new KeyValuePair<string, string> ("geolocation_string", geolocation_string),
				new KeyValuePair<string, string> ("geolocation_accuracy_in_metres", geolocation_accuracy_in_metres)
			};


			var url = String.Format ("{0}/", base_rest_url);

			try {

				string data = await _httpRequest.Put (url, requestBody);
				connection_identity = JsonConvert.DeserializeObject<ConnectionIdentity> (data);

			} catch (Exception) {

				AppEvents.Current.TriggerConnectionFailedEvent ();
				//Todo: log exception
			}

			return connection_identity;
		}


		private const string restful_resource = "connections";
		private IHttpRequest _httpRequest;

		private string base_rest_url {
			get {
				return string.Format ("{0}/{1}", Resources.baseUrl, restful_resource);
			}
		}

		public Connections (IHttpRequest httpRequest)
		{
			_httpRequest = httpRequest;
		}
	}
}
