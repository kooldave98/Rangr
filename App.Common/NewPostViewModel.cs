using System;
using System.Threading.Tasks;
using App.Core.Portable.Device;
using App.Common.Shared;
using App.Core.Portable.Persistence;
using App.Core.Portable.Models;

namespace App.Common
{
	public class NewPostViewModel : ViewModelBase
	{
		public User CurrentUser { get; private set;}

		public string PostText { get; set; }

		public async Task CreatePost ()
		{
			if (string.IsNullOrWhiteSpace (PostText)) {
				throw new InvalidOperationException ("Cannot create an empty post");
			}
				
			await post_services.Create (PostText, SessionInstance.CurrentConnection.connection_id.ToString ());
		}

		public NewPostViewModel (IPersistentStorage the_persistent_storage_instance)
		{
			SessionInstance = Session.GetInstance (the_persistent_storage_instance);
			post_services = new Posts (HttpRequest.Current);

			CurrentUser = SessionInstance.GetCurrentUser ();
		}

		public override void ResurrectViewModel()
		{
			throw new NotImplementedException ();
		}

		public override void TombstoneViewModel()
		{
			throw new NotImplementedException ();
		}

		private ISession SessionInstance;
		private Posts post_services;
	}
}

