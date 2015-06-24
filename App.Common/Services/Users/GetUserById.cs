using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace App.Common
{
	public class GetUserById
	{
        public async Task<User> Get (UserIdentity request)
		{
			User user = null;

            var url = string.Format ("{0}/?user_id={1}", UserResources.base_rest_url, request.user_id);

			try {

                string data = await UserResources.httpRequest.Get (url);
				user = JsonConvert.DeserializeObject<User> (data);

			} catch (Exception e) {

				AppEvents.Current.TriggerConnectionFailedEvent ();
				Debug.WriteLine (e.Message);
			}
			return user;
		}		
	}
}
