using System;
using App.Core.Portable.Models;
using App.Core.Portable.Device;
using App.Core.Portable.Persistence;

namespace App.Common
{
	public class Session : ISession
	{
		public ConnectionIdentity CurrentConnection { get; set;}

		public User GetCurrentUser(bool allowDefault = false)
		{
			var user = persistentStorage.Load<User> (CurrentUserKey);

			if (!allowDefault && user == null) {
				throw new InvalidOperationException ("User Session does not exist");
			}

			return user;
		}

		public void PersistCurrentUser(User user)
		{
			persistentStorage.Save(CurrentUserKey, user);
		}

		public static ISession GetInstance()
		{
			return _instance ?? (_instance = new Session());
		}

		private IPersistentStorage persistentStorage;

		private const string CurrentUserKey = "currentuser";

		private static ISession _instance = null;

		private Session()
		{
			persistentStorage = PersistentStorage.Current;
		}
	}
}

