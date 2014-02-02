using System;
using App.Core.Portable.Models;
using App.Core.Portable.Device;

namespace App.iOS
{
	public class Session : ISession
	{
		private User _user;
		private static ISession _instance = null;
		public static ISession GetInstance()
		{
			return _instance ?? (_instance = new Session());
		}

		private Session()
		{
			_user = new User () {
				ID = 1,
				DisplayName = "SeededUser"
			};
		}

		public User GetCurrentUser(){
			return _user;
		}

		public void AddCurrentUser(User user){
			_user = user;
		}
	}
}

