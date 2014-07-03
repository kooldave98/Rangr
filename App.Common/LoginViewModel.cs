using System;
using App.Core.Portable.Persistence;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using System.Threading.Tasks;

namespace App.Common
{
	public class LoginViewModel : ViewModelBase
	{
		public string UserDisplayName { get; set; }

		public async Task CreateUser()
		{
			if (!string.IsNullOrWhiteSpace (UserDisplayName))
			{
				var userID = await UserServices.Create (UserDisplayName);
				var user = await UserServices.Get (userID.user_id.ToString ());
				sessionInstance.PersistCurrentUser (user);

				await AppGlobal.Current.InitConnection ();
			}
			else {
				throw new InvalidOperationException ("You must enter a Display Name to create a new user");
			}

		}

		public override void TombstoneViewModel()
		{

		}

		public override void ResurrectViewModel()
		{

		}

		public LoginViewModel ()
		{
			sessionInstance = Session.GetInstance();

			UserServices = new Users (HttpRequest.Current);
		}

		private Users UserServices;
		private ISession sessionInstance;
	}
}

