using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using App.Core.Portable.Network;

namespace App.Common.Shared
{
	public class HttpRequest : IHttpRequest
    {
        private static HttpClient httpClient = new HttpClient(new HttpClientHandler());
        private static HttpResponseMessage response;
        

        public async Task<string> Post(string baseUrl, List<KeyValuePair<string, string>> data)
        {

            //var values = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("blah", "12345"),
            //    new KeyValuePair<string, string>("game_id", "123456")
            //};

            //var httpClient = new HttpClient(new HttpClientHandler());
            response = await httpClient.PostAsync(baseUrl, new FormUrlEncodedContent(data));
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }

        public async Task<string> Get(string baseUrl)
        {
            response = await httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}
