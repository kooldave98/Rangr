using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace App.Common
{
	public class CreateUser
	{
        public async Task<UserIdentity> Create (CreateUserRequest request)
		{
			UserIdentity user_identity = null;

			var requestBody = new List<KeyValuePair<string, string>> () {
                new KeyValuePair<string, string> ("mobile_number", request.mobile_number.ToString())
			};

            var url = string.Format ("{0}/create", UserResources.base_rest_url);

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

    public class CreateUserRequest
    {
        public long mobile_number { get; set; }
    }
}
