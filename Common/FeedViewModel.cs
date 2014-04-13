using System;
using App.Common.Shared;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Core.Portable.Persistence;

namespace App.Common
{
	public class FeedViewModel : ViewModelBase
	{
		public int start_index { get; set; }

		public IList<SeenPost> Posts { get; set; }

		public async Task RefreshPosts ()
		{
			var posts = await SeenPostServices.Get (_sessionInstance.CurrentConnection.connection_id.ToString (), start_index.ToString ());

			foreach (var post in posts) {
				start_index = post.id + 1;

				Posts.Insert (0, post);
			}

			if (OnNewPostsReceived != null) {
				OnNewPostsReceived (this, EventArgs.Empty);
			}
		}

		public event EventHandler<EventArgs> OnNewPostsReceived;

		public async Task init ()
		{
			IsBusy = true;
			var location = await _geoLocationInstance.GetCurrentPosition ();

			var user = _sessionInstance.GetCurrentUser ();

			//CreateConnection here
			_sessionInstance.CurrentConnection = await ConnectionServices.Create (user.user_id.ToString (), location.geolocation_value, location.geolocation_accuracy.ToString ());

			await this.RefreshPosts ();

			IsBusy = false;

			//init heartbeat here

			_geoLocationInstance.OnGeoPositionChanged (async (geo_value) => {
				_sessionInstance.CurrentConnection = await ConnectionServices
					.Update (_sessionInstance.CurrentConnection.connection_id.ToString (), geo_value.geolocation_value, geo_value.geolocation_accuracy.ToString ());

			});	


			JavaScriptTimer.SetInterval (async () => {
				var position = await _geoLocationInstance.GetCurrentPosition ();		

				_sessionInstance.CurrentConnection = await ConnectionServices
					.Update (_sessionInstance.CurrentConnection.connection_id.ToString (), position.geolocation_value, position.geolocation_accuracy.ToString ());

			}, 270000);//4.5 minuets (4min 30sec) [since 1000 is 1 second]
		}

		public FeedViewModel (IGeoLocation the_geolocation_instance, IPersistentStorage the_persistent_storage_instance)
		{
			Posts = new List<SeenPost> ();

			_geoLocationInstance = the_geolocation_instance;


			_sessionInstance = Session.GetInstance (the_persistent_storage_instance);
			_httpRequest = HttpRequest.Current;

			ConnectionServices = new Connections (_httpRequest);
			SeenPostServices = new SeenPosts (_httpRequest);
		}

		IGeoLocation _geoLocationInstance;
		ISession _sessionInstance;
		Connections ConnectionServices;
		IHttpRequest _httpRequest;
		SeenPosts SeenPostServices;
	}
}

