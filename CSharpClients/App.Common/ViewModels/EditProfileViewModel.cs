using System;
using System.Threading.Tasks;

namespace App.Common
{
	public class EditProfileViewModel : ViewModelBase
	{
		public User CurrentUserToBeEdited { get; set; }

		public async Task<bool> UpdateUser ()
		{
			if (CurrentUserToBeEdited == null) {
				throw new InvalidOperationException ("User is null");
			}

			var result = await user_services.Update (CurrentUserToBeEdited.user_id.ToString (), CurrentUserToBeEdited.user_display_name, CurrentUserToBeEdited.user_status_message);

			if (result == null) {
				return false;
			}

			var user = CurrentUserToBeEdited = await user_services.Get (CurrentUserToBeEdited.user_id.ToString ());

			if (user != null) {
				//if the refresher query failed dont persist it
				SessionInstance.PersistCurrentUser (user);
			}

			return true;
		}

		public EditProfileViewModel ()
		{
			SessionInstance = Session.Current;
			user_services = new Users ();

			CurrentUserToBeEdited = SessionInstance.GetCurrentUser ();
		}

		private Session SessionInstance;
		private Users user_services;
	}
}

