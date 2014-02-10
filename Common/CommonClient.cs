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

	/// <summary>
	/// Note that there may be a potential issue with the geolocation being stale on reconnection if the client has moved away from his original location
	/// </summary>
	public class CommonClient
	{
		private IHubProxy hubProxy;
		private bool connectionReady = false;
		private ISession _session;
		private IGeoLocation _geolocation;
		private TextWriter _traceWriter;
		private SynchronizationContext _context;
		private List<Action<IHubProxy>> sendPostCallbacks = new List<Action<IHubProxy>> ();

		private HubConnection hubConnection;

		private List<Action<CommonClient>> onConnectionAbortedCallbacks = new List<Action<CommonClient>> ();


		private static CommonClient _instance = null;
		public static CommonClient GetInstance(TextWriter traceWriter, IGeoLocation geolocatorInstance, ISession sessionInstance, SynchronizationContext context)
		{
			return _instance ?? (_instance = new CommonClient(traceWriter, geolocatorInstance, sessionInstance, context));
		}

		private CommonClient (TextWriter traceWriter, IGeoLocation geolocatorInstance, ISession sessionInstance, SynchronizationContext context)
		{
			_traceWriter = traceWriter;
			_session = sessionInstance;
			_geolocation = geolocatorInstance;
			_context = context;
		}

		public void OnConnectionAborted(Action<CommonClient> handler){
			//Todo: need to lock this collection for thread safety
			onConnectionAbortedCallbacks.Add (handler);
		}

		public async Task Start(Action<Post> handler)
		{
			try {
				await RunHubConnectionAPI (handler);
			} catch (HttpClientException httpClientException) {
				_traceWriter.WriteLine ("------------------------------------>HttpClientException: {0}", httpClientException.Response);
				//throw;
				abortConnection ();
			} catch(HubException ex){
				_traceWriter.WriteLine ("------------------------------------>HubException: {0}", ex);
				//throw;
				abortConnection ();
			}catch (Exception exception) {
				_traceWriter.WriteLine ("------------------------------------>Exception: {0}", exception);
				//throw;
				abortConnection ();
			}
		}

		private void abortConnection(){
			_traceWriter.WriteLine ("------------------------------------>Connection aborted event");
			hubConnection.Stop();
			connectionReady = false;
			foreach (var handler in onConnectionAbortedCallbacks) {
				handler (this);
			}
		}

		public void sendPost(Action<IHubProxy> callback)
		{
			lock(sendPostCallbacks)
			{
				if (!connectionReady) {
					sendPostCallbacks.Add (callback);
				} else {
					callback (hubProxy);
				}
			}
		}

		private async Task RunHubConnectionAPI (Action<Post> handler)
		{
			//Ensure this is set to false at the beginning of this method
			connectionReady = false;
			var queryString = await getQueryString ();

			hubConnection = new HubConnection (url, queryString);
			hubConnection.TraceWriter = Console.Out;
			//hubConnection.TraceWriter = _traceWriter;

			hubConnection.StateChanged += (change) => {
				_traceWriter.WriteLine ("hubConnection.StateChanged {0} => {1}", change.OldState, change.NewState);
			};

			//On Reconnecting event handler
			hubConnection.Reconnecting += async ()=> {
				_traceWriter.WriteLine ("------------------------------------>Hub Connection Reconnecting Event");
				connectionReady = false;
			};

			//On slow connection
			hubConnection.ConnectionSlow += async () => {
				_traceWriter.WriteLine ("------------------------------------>Hub Connection Slow Event");
			};

			hubConnection.Reconnected += async () => {
				_traceWriter.WriteLine ("------------------------------------>Hub Connection Reconnected Event");
				connectionReady = true;
			};

			hubConnection.Closed += async () => {
				_traceWriter.WriteLine ("------------------------------------>Hub Connection Closed Event");

				//hubConnection.Stop();
				connectionReady = false;

				await start ();

			};

			hubConnection.Error += async (obj) => {
				_traceWriter.WriteLine ("------------------------------------>Hub Connection Error Event");

			};

			hubConnection.Received += async (obj) => {
				_traceWriter.WriteLine ("------------------------------------>Hub Connection Received Event");
			};




			hubProxy = hubConnection.CreateHubProxy ("streamHub");

			hubProxy.On<Post> ("appendPost", (post) => _context.Post (async delegate {
				//Write to Log
				_traceWriter.WriteLine ("------------------------------------Append Post--> UserId: {0}, UserName : {1}, PostText : {2}>", post.UserID, post.UserDisplayName, post.Text);


				handler(post);

                    
			}, null));

			await start ();


			_geolocation.OnGeoPositionChanged (async(position) => {
				if(connectionReady){
					await hubProxy.Invoke ("updateLocation", position);
					_traceWriter.WriteLine ("------------------------------------>Position Changed={0}", position);
				}
			});

//			_geolocation.OnGeoPositionChanged += async (object sender, GeoPositionChangedEventArgs e) => {
//				await hubProxy.Invoke ("updateLocation", e.position);
//			};
		}

		private async Task start(){
			await hubConnection.Start ();

			_traceWriter.WriteLine ("------------------------------------>HubConnection started with transport.Name={0}", hubConnection.Transport.Name);



			lock (sendPostCallbacks) {

				if (sendPostCallbacks.Any ()) {
					foreach (var callback in sendPostCallbacks) {
						callback (hubProxy);
					}

					sendPostCallbacks.Clear ();
				}
				connectionReady = true;

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

