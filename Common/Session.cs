using System;
using App.Core.Portable.Models;
using App.Core.Portable.Device;
using App.Core.Portable.Persistence;

namespace App.Common.Shared
{
	public class Session : ISession
	{
		public ConnectionIdentity CurrentConnection { get; set;}

		public User GetCurrentUser(bool allowDefault = false)
		{
			var user = PersistentStorage.Load<User> (CurrentUserKey);

			if (!allowDefault && user == null) {
				throw new InvalidOperationException ("User Session does not exist");
			}

			return user;
		}

		public void PersistCurrentUser(User user)
		{
			PersistentStorage.Save(CurrentUserKey, user);
		}

		public static ISession GetInstance(IPersistentStorage persistent_storage_instance)
		{
			return _instance ?? (_instance = new Session(persistent_storage_instance));
		}

		private IPersistentStorage PersistentStorage;

		private const string CurrentUserKey = "currentuser";

		private static ISession _instance = null;

		private Session(IPersistentStorage persistent_storage_instance)
		{
			PersistentStorage = persistent_storage_instance;
		}
	}
}

