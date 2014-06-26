using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using App.Core.Portable.Network;

namespace App.Common
{
	public class HttpRequest : IHttpRequest
    {
		private static HttpClient httpClient;
        

		public async Task<string> Put(string baseUrl, List<KeyValuePair<string, string>> data)
		{
			var response = await httpClient.PutAsync(baseUrl, new FormUrlEncodedContent(data));
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
		}


        public async Task<string> Post(string baseUrl, List<KeyValuePair<string, string>> data)
        {
			var response = await httpClient.PostAsync(baseUrl, new FormUrlEncodedContent(data));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        public async Task<string> Get(string baseUrl)
        {
			var response = await httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }


		private static IHttpRequest _instance = null;
		public static IHttpRequest Current
		{
			get{
				return _instance ?? (_instance = new HttpRequest ());
			}
		}

		private HttpRequest()
		{
			httpClient = new HttpClient(new HttpClientHandler());
		}

    }
}
