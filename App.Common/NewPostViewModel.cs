using System;
using System.Threading.Tasks;
using App.Core.Portable.Persistence;
using App.Core.Portable.Models;

namespace App.Common
{
	public class NewPostViewModel : ViewModelBase
	{
		public User CurrentUser { get; private set; }

		public string PostText { get; set; }

		public async Task<bool> CreatePost ()
		{
			if (string.IsNullOrWhiteSpace (PostText)) {
				throw new InvalidOperationException ("Cannot create an empty post");
			}				

			var result = await post_services.Create (PostText, SessionInstance.GetCurrentConnection ().connection_id.ToString ());

			if (result != null) {
				return true;
			}

			return false;
		}

		public NewPostViewModel (IPersistentStorage the_persistent_storage_instance)
		{
			SessionInstance = Session.GetInstance ();
			post_services = new Posts ();

			CurrentUser = SessionInstance.GetCurrentUser ();
		}

		private Session SessionInstance;
		private Posts post_services;
	}
}

