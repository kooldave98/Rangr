using System;
using App.Core.Portable.Models;
using App.Core.Portable.Device;
using SQLite;
using System.Linq;

namespace App.Android
{
	public class Session : ISession
	{
		private const string CurrentUserKey = "currentuser";


		public User GetCurrentUser(){
			User user = null;

			var keyValue = PersistentStorage.Current.GetKeyValues ().SingleOrDefault(k=>k.Key == CurrentUserKey);

			if (keyValue != null) {
				user = new User () {
					ID = keyValue.UserID,
					DisplayName = keyValue.UserDisplayName
				};
			}

			return user;
		}

		public void AddCurrentUser(User user){

			var data = PersistentStorage.Current.GetKeyValues ().SingleOrDefault(k=>k.Key == CurrentUserKey);

			if (data != null) {
				//Delete existing one
				PersistentStorage.Current.DeleteKeyValue (data.ID);
			}

			var keyValue = new KeyValue () { 
				Key = CurrentUserKey,
				UserDisplayName = user.DisplayName,
				UserID = user.ID
			};

			PersistentStorage.Current.SaveKeyValue (keyValue);

		}

		private static ISession _instance = null;

		public static ISession Current
		{
			get{
				return _instance ?? (_instance = new Session ());
			}
		}
	}


}

