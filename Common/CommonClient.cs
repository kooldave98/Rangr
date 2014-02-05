using App.Core.Portable.Device;
using App.Core.Portable.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace App.Common.Shared
{
	public class CommonClient
	{
		private IHubProxy hubProxy;
		private bool connectionReady = false;
		private ISession _session;
		private IGeoLocation _geolocation;
		private TextWriter _traceWriter;
		private SynchronizationContext _context;
		private List<Action<IHubProxy>> callbacks;

		public CommonClient (TextWriter traceWriter, IGeoLocation geolocatorInstance, ISession sessionInstance, SynchronizationContext context)
		{
			_traceWriter = traceWriter;
			_session = sessionInstance;
			_geolocation = geolocatorInstance;
			_context = context;
			callbacks = new List<Action<IHubProxy>> ();
		}

		public async Task RunAsync (Action<Post> handler)
		{
			try {
				await RunHubConnectionAPI (handler);
			} catch (HttpClientException httpClientException) {
				_traceWriter.WriteLine ("HttpClientException: {0}", httpClientException.Response);
				throw;
			} catch (Exception exception) {
				_traceWriter.WriteLine ("Exception: {0}", exception);
				throw;
			}
		}

		public void sendPost(Action<IHubProxy> callback){
			if (!connectionReady) {
				callbacks.Add (callback);
			} else {
				callback (hubProxy);
			}
		}

		private async Task RunHubConnectionAPI (Action<Post> handler)
		{

			var queryString = await getQueryString ();

			var hubConnection = new HubConnection (url, queryString);
			hubConnection.TraceWriter = _traceWriter;

			hubProxy = hubConnection.CreateHubProxy ("streamHub");
			hubProxy.On<Post> ("appendPost", (post) => _context.Post (async delegate {
				//Write to Log
				hubConnection.TraceWriter.WriteLine (post);
				handler (post);
                    
			}, null));

			await hubConnection.Start ();
			hubConnection.TraceWriter.WriteLine ("transport.Name={0}", hubConnection.Transport.Name);

			connectionReady = true;

			_geolocation.OnGeoPositionChanged (async(position) => {
				await hubProxy.Invoke ("updateLocation", position);
			});

//			_geolocation.OnGeoPositionChanged += async (object sender, GeoPositionChangedEventArgs e) => {
//				await hubProxy.Invoke ("updateLocation", e.position);
//			};

			if (callbacks.Any ()) {
				foreach (var callback in callbacks) {
					callback (hubProxy);
				}

				callbacks.Clear ();
			}

		}

		private string url {
			get {
				var url = string.Format ("{0}/", "http://geonowapp.azurewebsites.net");
				return url;
			}
		}

		private async Task<Dictionary<string, string>> getQueryString ()
		{

			var geolocationString = await _geolocation.GetCurrentPosition ();
			var querystringData = new Dictionary<string, string> ();
			querystringData.Add ("userID", _session.GetCurrentUser ().ID.ToString ());
			querystringData.Add ("geoLocation", geolocationString);

			return querystringData;

		}
	}
}

