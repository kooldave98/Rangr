using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using common_lib;

namespace App.Common
{
    public class CreatePost
    {
        public async Task<PostIdentity> execute(CreatePostRequest request)
        {
            PostIdentity post_id = null;

            var requestBody = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("text", request.text),
                new KeyValuePair<string, string>("user_id", request.user_id.ToString()),
                new KeyValuePair<string, string>("long_lat_acc_geo_string", request.long_lat_acc_geo_string),
            };

            var binary_request_body = new List<KeyValuePair<string, HttpFile>>()
            {
                new KeyValuePair<string, HttpFile>("image_data", request.image_data)
            }; 

            var url = String.Format("{0}/create", PostResources.base_rest_url);

            try
            {

                var data = await PostResources.httpRequest.Post(url, requestBody, binary_request_body);
                post_id = JsonConvert.DeserializeObject<PostIdentity>(data);

            }
            catch (Exception e)
            {

                AppEvents.Current.TriggerConnectionFailedEvent(e.Message);
                Debug.WriteLine(e.Message);
            }

            return post_id;
        }
    }

    public class CreatePostRequest : UserIdentity
    {
        public string text { get; set; }

        public string long_lat_acc_geo_string { get; set; }

        public HttpFile image_data { get; set; }
    }
}
