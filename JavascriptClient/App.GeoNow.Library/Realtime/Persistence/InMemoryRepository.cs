using App.GeoNow.Library.Realtime.Models;
using App.GeoNow.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.GeoNow.Library.Realtime.Persistence
{
    public class InMemoryRepository
    {
        private static ICollection<User> _connectedUsers;
        private static Dictionary<string, string> _mappings;
        private static InMemoryRepository _instance = null;
        private static readonly int max_random = 3;

        public static InMemoryRepository GetInstance()
        {
            return _instance ?? (_instance = new InMemoryRepository());
        }

        #region Private methods

        private InMemoryRepository()
        {
            _connectedUsers = new List<User>();
            _mappings = new Dictionary<string, string>();
        }

        #endregion

        #region Repository methods

        public IQueryable<User> Users { get { return _connectedUsers.AsQueryable(); } }

        public IEnumerable<User> NearbyUsers(string geoLocation, int distanceInMetres) {
            return Users.Where(u => u.GeoLocation.ToDbGeography().Distance(geoLocation.ToDbGeography()) <= distanceInMetres).ToList();
        }

        public void UpdateUserGeoLocation(string userID, string geoLocation)
        {
            var user = _connectedUsers.Where(u => u.UserID == userID).FirstOrDefault();
            if (user != null)
            {
                user.GeoLocation = geoLocation;
            }
        }

        public void Add(User user)
        {
            _connectedUsers.Add(user);
        }

        public void Remove(User user)
        {
            _connectedUsers.Remove(user);
        }

        public string GetRandomizedUsername(string username)
        {
            string tempUsername = username;
            int newRandom = max_random, oldRandom = 0;
            int loops = 0;
            Random random = new Random();
            do
            {
                if (loops > newRandom)
                {
                    oldRandom = newRandom;
                    newRandom *= 2;
                }
                username = tempUsername + "_" + random.Next(oldRandom, newRandom).ToString();
                loops++;
            } while (GetInstance().Users.Where(u => u.UserName.Equals(username)).ToList().Count > 0);

            return username;
        }

        public void AddMapping(string connectionId, string userId)
        {
            if (!string.IsNullOrEmpty(connectionId) && !string.IsNullOrEmpty(userId))
            {
                _mappings.Add(connectionId, userId);
            }
        }

        public string GetUserByConnectionId(string connectionId)
        {
            string userId = null;
            _mappings.TryGetValue(connectionId, out userId);
            return userId;
        }

        public string GetConnectionIdByUser(string userID)
        {
            var key = _mappings.FirstOrDefault(k => k.Value == userID).Key;
            return key;

        }

        #endregion
    }
}
