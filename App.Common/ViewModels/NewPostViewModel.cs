using System;
using System.Threading.Tasks;
using common_lib;

namespace App.Common
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

            var location = await GeoLocation.GetInstance().GetCurrentPosition();

            var request = new CreatePostRequest(){ 
                user_id = SessionInstance.GetCurrentUser().user_id,
                text = PostText,
                long_lat_acc_geo_string = location.ToLongLatAccString(),
                image_data = PostImage
            };

            var result = await create_post.execute (request);

			if (result != null) {
				return true;
			}

			return false;
		}

		public NewPostViewModel ()
		{
			SessionInstance = Session.Current;
            create_post = new CreatePost ();

			CurrentUser = SessionInstance.GetCurrentUser ();
		}

		private Session SessionInstance;
		private CreatePost create_post;
	}
}

