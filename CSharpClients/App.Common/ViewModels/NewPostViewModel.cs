using System;
using System.Threading.Tasks;
using solid_lib;

namespace rangr.common
{
	public class NewPostViewModel : ViewModelBase
	{
		public User CurrentUser { get; private set; }

		public string PostText { get; set; }

        public HttpFile PostImage { get; set; }

		public async Task<bool> CreatePost ()
		{
			if (string.IsNullOrWhiteSpace (PostText)) {
				throw new InvalidOperationException ("Cannot create an empty post");
			}

            Guard.IsNotNull(PostImage, "PostImage");


            var result = await post_services.Create (PostText, SessionInstance.GetCurrentConnection ().connection_id, PostImage);

			if (result != null) {
				return true;
			}

			return false;
		}

		public NewPostViewModel ()
		{
			SessionInstance = Session.Current;
			post_services = new Posts ();

			CurrentUser = SessionInstance.GetCurrentUser ();
		}

		private Session SessionInstance;
		private Posts post_services;
	}
}

