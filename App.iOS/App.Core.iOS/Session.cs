using System;
using App.Core.Portable.Models;
using App.Core.Portable.Device;

namespace App.iOS
{
	public class Session : ISession
	{
		private const string CurrentUserKey = "currentuser";
		private static ISession _instance = null;
		public static ISession GetInstance()
		{
			return _instance ?? (_instance = new Session());
		}

		private Session()
		{
		}

		public User GetCurrentUser(){
			var user = PersistentStorage.Current.Load<User> (CurrentUserKey);
			return user;
		}

		public void AddCurrentUser(User user){
			PersistentStorage.Current.Save(CurrentUserKey, user);
		}
	}
}

