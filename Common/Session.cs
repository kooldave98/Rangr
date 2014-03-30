using System;
using App.Core.Portable.Models;
using App.Core.Portable.Device;
using App.Core.Portable.Persistence;

namespace App.Common.Shared
{
	public class Session : ISession
	{
		private IPersistentStorage PersistentStorage;

		private const string CurrentUserKey = "currentuser";

		private static ISession _instance = null;
		public static ISession GetInstance(IPersistentStorage persistent_storage_instance)
		{
			return _instance ?? (_instance = new Session(persistent_storage_instance));
		}

		private Session(IPersistentStorage persistent_storage_instance)
		{
			PersistentStorage = persistent_storage_instance;
		}

		public User GetCurrentUser(){
			var user = PersistentStorage.Load<User> (CurrentUserKey);
			return user;
		}

		public void PersistCurrentUser(User user){
			PersistentStorage.Save(CurrentUserKey, user);
		}
	}
}

