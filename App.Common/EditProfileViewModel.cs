using System;
using System.Threading.Tasks;
using App.Core.Portable.Device;
using App.Core.Portable.Persistence;
using App.Core.Portable.Models;

namespace App.Common
{
	public class EditProfileViewModel : ViewModelBase
	{
		public User CurrentUserToBeEdited { get; set;}

		public async Task UpdateUser ()
		{
			if (CurrentUserToBeEdited == null) {
				throw new InvalidOperationException ("User is null");
			}

			await user_services.Update (CurrentUserToBeEdited.user_id.ToString(), CurrentUserToBeEdited.user_display_name, CurrentUserToBeEdited.user_status_message);

			var user = CurrentUserToBeEdited = await user_services.Get (CurrentUserToBeEdited.user_id.ToString ());
			SessionInstance.PersistCurrentUser (user);
		}

		public EditProfileViewModel (IPersistentStorage the_persistent_storage_instance)
		{
			SessionInstance = Session.GetInstance ();
			user_services = new Users (HttpRequest.Current);

			CurrentUserToBeEdited = SessionInstance.GetCurrentUser ();
		}

		public override void ResurrectViewModel()
		{
			//throw new NotImplementedException ();
		}

		public override void TombstoneViewModel()
		{
			//throw new NotImplementedException ();
		}

		private ISession SessionInstance;
		private Users user_services;
	}
}

