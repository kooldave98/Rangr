using System;
using App.Core.Portable.Persistence;
using App.Common.Shared;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using System.Threading.Tasks;

namespace App.Common
{
	public class LoginViewModel : ViewModelBase
	{
		public string UserDisplayName { get; set; }

		public bool CurrentUserExists 
		{ 
			get
			{ 
				var user = sessionInstance.GetCurrentUser (true);
				if (user != null) {
					return true;
				}

				return false;
			}
		}

		public async Task Login()
		{
			if (string.IsNullOrWhiteSpace (UserDisplayName)) {
				throw new InvalidOperationException ("You must enter a Display Name to create a new user");
			}

			var userID = await UserServices.Create (UserDisplayName);
			var user = await UserServices.Get (userID.user_id.ToString ());
			sessionInstance.PersistCurrentUser (user);
		}


		public LoginViewModel (IPersistentStorage the_persistent_storage_instance)
		{
			sessionInstance = Session.GetInstance(the_persistent_storage_instance);

			UserServices = new Users (HttpRequest.Current);
		}

		private Users UserServices;
		private ISession sessionInstance;
	}
}

