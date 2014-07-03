using System;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Core.Portable.Network;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Core.Portable.Persistence;

namespace App.Common
{
	//The ios workarounds are because I didn't want to be repopulating the list each time.
	//So I am caching the latest posts every time a pull is made.
	public class FeedViewModel : ViewModelBase
	{
		public int start_index { get; set; }

		//1.1non android breaking ios workaround
		public IList<SeenPost> LatestPosts { get; set; }
		//end workaround

		public IList<SeenPost> Posts { get; set; }

		public async Task RefreshPosts ()
		{
			try{
				//var posts =
				//We are using latest posts here because we need a variable outside the try block
				//so we just decided to make use of the latest posts variable
				LatestPosts = await SeenPostServices.Get (_sessionInstance.GetCurrentConnection().connection_id.ToString (), start_index.ToString ());
			}catch(Exception e){
				//We get exceptions here sometimes because the 5 minute idle time on the server has elapsed
			}

			foreach (var post in LatestPosts) {
				start_index = post.id + 1;

				Posts.Insert (0, post);
			}

			if (OnNewPostsReceived != null) {
				OnNewPostsReceived (this, EventArgs.Empty);
			}
		}

		public event EventHandler<EventArgs> OnNewPostsReceived;

//		public async Task init ()
//		{
//			IsBusy = true;
//			var location = await _geoLocationInstance.GetCurrentPosition ();
//
//			var user = _sessionInstance.GetCurrentUser ();
//
//			//CreateConnection here
//			_sessionInstance.CurrentConnection = await ConnectionServices.Create (user.user_id.ToString (), location.geolocation_value, location.geolocation_accuracy.ToString ());
//
//			await this.RefreshPosts ();
//
//			IsBusy = false;
//
//			//init heartbeat here
//
//			_geoLocationInstance.OnGeoPositionChanged (async (geo_value) => {
//				_sessionInstance.CurrentConnection = await ConnectionServices
//					.Update (_sessionInstance.CurrentConnection.connection_id.ToString (), geo_value.geolocation_value, geo_value.geolocation_accuracy.ToString ());
//
//			});	
//
//
//			JavaScriptTimer.SetInterval (async () => {
//				var position = await _geoLocationInstance.GetCurrentPosition ();		
//
//				_sessionInstance.CurrentConnection = await ConnectionServices
//					.Update (_sessionInstance.CurrentConnection.connection_id.ToString (), position.geolocation_value, position.geolocation_accuracy.ToString ());
//
//			}, 270000);//4.5 minuets (4min 30sec) [since 1000 is 1 second]
//		}

		public override void ResurrectViewModel()
		{
			//throw new NotImplementedException ();
		}

		public override void TombstoneViewModel()
		{
			//throw new NotImplementedException ();
		}


		public FeedViewModel ()
		{
			Posts = new List<SeenPost> ();

			//1.3 non android breaking ios workaround
			LatestPosts = new List<SeenPost> ();
			//end workaround


			_sessionInstance = Session.GetInstance ();

			SeenPostServices = new SeenPosts (HttpRequest.Current);
		}
			
		ISession _sessionInstance;
		SeenPosts SeenPostServices;
	}
}

