using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Xamarin;
using System.Linq;
using ModernHttpClient;
using System.Net.Http;
using Fusillade;
using System.Net.Http.Headers;

namespace App.Common
{
    //TO-REFACTOR
    //This class violates SOLID in so many ways.
    //Most importantly it is not re-useable in other apps due 
    //to its high coupling with with external dependencises
    public class HttpRequest : SingletonBase<HttpRequest>
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

        public async Task<string> Post(string baseUrl, 
                                        List<KeyValuePair<string, string>> data, 
                                        List<KeyValuePair<string, HttpFile>> binary_data)
        {
            //Multipart setup
            var httpContent = new MultipartFormDataContent("testnewboundary");
            foreach (var item in data)
            {
                httpContent.Add(new StringContent(item.Value), item.Key);
            }
            add_to_multipart_form_data_content(httpContent, binary_data);
            //end of multipart setup


            var request_string = "POST: " + baseUrl;

            Insights.Track(request_string, data.ToDictionary(i => i.Key, i => i.Value));

            string responseString = null;

            try
            {
                using (var handle = Insights.TrackTime(request_string))
                {   
                    var response = await httpClient.PostAsync(baseUrl, httpContent);
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

        private void add_to_multipart_form_data_content(MultipartFormDataContent content, 
                                                        List<KeyValuePair<string, HttpFile>> data)
        {
            foreach (var item in data)
            {
                var fileContent = new ByteArrayContent(item.Value.Buffer);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(item.Value.MediaType);

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = item.Value.FileName,
                    Name = item.Key
                };
                content.Add(fileContent,item.Key);
            }
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

        private static HttpClient httpClient;

        private HttpRequest()
        {
            httpClient = new HttpClient(NetCache.UserInitiated);
            //httpClient.Timeout = TimeSpan.FromSeconds(100);
        }

    }
}
