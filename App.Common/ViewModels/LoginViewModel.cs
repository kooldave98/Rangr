using System;
using App.Core.Portable.Persistence;
using App.Core.Portable.Models;
using System.Threading.Tasks;

namespace App.Common
{
	public class LoginViewModel : ViewModelBase
	{
		public string UserDisplayName { get; set; }

		public async Task<bool> CreateUser ()
		{
			if (string.IsNullOrWhiteSpace (UserDisplayName)) {
				throw new InvalidOperationException ("You must enter a Display Name to create a new user");
			}

			var userID = await UserServices.Create (UserDisplayName);

			if (userID == null) {
				return false;
			}


			var user = await UserServices.Get (userID.user_id.ToString ());

			if (user == null) {
				return false;
			}

			sessionInstance.PersistCurrentUser (user);


			return true;
		}

		public LoginViewModel ()
		{
			sessionInstance = Session.GetInstance ();

			UserServices = new Users ();
		}

		private Users UserServices;
		private Session sessionInstance;
	}
}

