using System;

namespace App.Common
{
    public class Session : SingletonBase<Session>
	{
		public User GetCurrentUser (bool allowDefault = false)
		{
			var user = persistentStorage.Load<User> (CurrentUserKey);

			if (!allowDefault && user == null) {
				throw new InvalidOperationException ("User Session does not exist");
			}

			return user;
		}

		public void PersistCurrentUser (User user)
		{
			persistentStorage.Save (CurrentUserKey, user);
		}

		private PersistentStorage persistentStorage;

		private const string CurrentUserKey = "current_user";

		private Session ()
		{
			persistentStorage = PersistentStorage.Current;
		}
	}
}

