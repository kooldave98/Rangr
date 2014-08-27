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

		public async Task<bool> UpdateUser ()
		{
			if (CurrentUserToBeEdited == null) {
				throw new InvalidOperationException ("User is null");
			}

			var result = await user_services.Update (CurrentUserToBeEdited.user_id.ToString(), CurrentUserToBeEdited.user_display_name, CurrentUserToBeEdited.user_status_message);

			if(result == null)
			{
				return false;
			}

			var user = CurrentUserToBeEdited = await user_services.Get (CurrentUserToBeEdited.user_id.ToString ());

			if (user != null) {
				//if the refresher query failed dont persist it
				SessionInstance.PersistCurrentUser (user);
			}

			return true;
		}

		public EditProfileViewModel (IPersistentStorage the_persistent_storage_instance)
		{
			SessionInstance = Session.GetInstance ();
			user_services = new Users (HttpRequest.Current);

			CurrentUserToBeEdited = SessionInstance.GetCurrentUser ();
		}

		private ISession SessionInstance;
		private Users user_services;
	}
}

