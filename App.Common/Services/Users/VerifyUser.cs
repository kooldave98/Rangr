using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace App.Common
{
	public class VerifyUser
	{
        public async Task<UserIdentity> execute (VerifyUserRequest request)
		{
			UserIdentity user_identity = null;

			var requestBody = new List<KeyValuePair<string, string>> () {
                new KeyValuePair<string, string> ("user_id", request.user_id),
                new KeyValuePair<string, string> ("secret_code", request.secret_code)
			};

            var url = string.Format ("{0}/verify", UserResources.base_rest_url);

			try {

                var data = await UserResources.httpRequest.Post (url, requestBody);        
				user_identity = JsonConvert.DeserializeObject<UserIdentity> (data);

			} catch (Exception e) {

				AppEvents.Current.TriggerConnectionFailedEvent ();
				Debug.WriteLine (e.Message);
			}

			return user_identity;
		}		
	}

    public class VerifyUserRequest
    {
        public string user_id { get; set; }

        public string secret_code { get; set; }
    }
}
