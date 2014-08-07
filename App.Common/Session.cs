using System;
using App.Core.Portable.Models;
using App.Core.Portable.Device;
using App.Core.Portable.Persistence;

namespace App.Common
{
	public class Session : ISession
	{

		public void PersistCurrentConnection(ConnectionIdentity connection_id)
		{
			persistentStorage.Save(CurrentConnectionKey, connection_id);
		}

		public ConnectionIdentity GetCurrentConnection(bool allowDefault = false)
		{
			var connection = persistentStorage.Load<ConnectionIdentity> (CurrentConnectionKey);

			if (!allowDefault && connection == null) {
				throw new InvalidOperationException ("Connection ID does not exist");
			}

			return connection;
		}

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

		private const string CurrentUserKey = "current_user";

		private const string CurrentConnectionKey = "current_connection";

		private static ISession _instance = null;

		private Session()
		{
			persistentStorage = PersistentStorage.Current;
		}
	}
}

