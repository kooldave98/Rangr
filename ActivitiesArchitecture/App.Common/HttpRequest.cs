using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Xamarin;
using System.Linq;
using ModernHttpClient;
using System.Net.Http;
using Fusillade;

namespace App.Common
{
    public class HttpRequest
    {
        public async Task<string> Put(string baseUrl, List<KeyValuePair<string, string>> data)
        {
            var request_string = "PUT: " + baseUrl;

            Insights.Track(request_string, data.ToDictionary(i => i.Key, i => i.Value));

            string responseString = null;

            try
            {
                using (var handle = Insights.TrackTime(request_string))
                {	
                    var response = await httpClient.PutAsync(baseUrl, new FormUrlEncodedContent(data));
                    responseString = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception e)
            { 
                Insights.Report(e, new Dictionary<string, string>()
                    {
                        { "Request", request_string },
                        { "Response", responseString }
                    });

                throw;
            }

            return responseString;
        }


        public async Task<string> Post(string baseUrl, List<KeyValuePair<string, string>> data)
        {
            var request_string = "POST: " + baseUrl;

            Insights.Track(request_string, data.ToDictionary(i => i.Key, i => i.Value));

            string responseString = null;

            try
            {
                using (var handle = Insights.TrackTime(request_string))
                {	
                    var response = await httpClient.PostAsync(baseUrl, new FormUrlEncodedContent(data));
                    responseString = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception e)
            { 
                Insights.Report(e, new Dictionary<string, string>()
                    {
                        { "Request", request_string },
                        { "Response", responseString }
                    });

                throw;
            }

            return responseString;
        }

        public async Task<string> Get(string baseUrl)
        {
            var request_string = "GET: " + baseUrl;

            Insights.Track(request_string);

            string responseString = null;

            try
            {
                using (var handle = Insights.TrackTime(request_string))
                {				
                    var response = await httpClient.GetAsync(baseUrl);
                    responseString = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception e)
            { 
                Insights.Report(e, new Dictionary<string, string>()
                    {
                        { "Request", request_string },
                        { "Response", responseString }
                    });

                throw;
            }

            return responseString;
        }


        private static HttpRequest _instance;

        public static HttpRequest Current
        {
            get
            {
                return _instance ?? (_instance = new HttpRequest());
                //return (_instance = new HttpRequest ());
            }
        }

        private static HttpClient httpClient;

        private HttpRequest()
        {
            httpClient = new HttpClient(NetCache.UserInitiated);
            httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

    }
}