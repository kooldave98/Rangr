using App.Core.Portable.Models;
using App.Core.Portable.Network;
using App.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App.Core.Portable;

namespace App.Common.Shared
{
    public class UserRepository
    {
        private IHttpRequest _httpRequest;

        //loose coupling
        public UserRepository(IHttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public async Task<User> GetUserById(int userId)
        {
            var url = String.Format("{0}/users/?ID={1}", Resources.baseUrl, userId);
            string data = await _httpRequest.Get(url);
            var user = JsonConvert.DeserializeObject<User>(data);
            return user;
        }

        public async Task<UserIdentity> CreateUser(string displayName)
        {
            var requestBody = new List<KeyValuePair<string, string>>();
            requestBody.Add(new KeyValuePair<string, string>("DisplayName", displayName));

            var url = String.Format("{0}/users/", Resources.baseUrl);
            string data = await _httpRequest.Post(url, requestBody);
        
            var userID = JsonConvert.DeserializeObject<UserIdentity>(data);
            return userID;
        }
    }
}
