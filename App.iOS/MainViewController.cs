using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using App.Common.Shared;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using App.Core.Portable.Network;

namespace App.iOS
{
	public partial class MainViewController : UITableViewController
	{
		IHttpRequest _httpRequest;
		IGeoLocation _geoLocationInstance;
		ISession _sessionInstance;
		UITextView textView;
		ConsoleView consoleView;
		Global _global;
		PostsTableViewSource dataSource;
		Connections ConnectionServices;

		public MainViewController () : base ("MainViewController", null)
		{
			_httpRequest = HttpRequest.Current;
			_geoLocationInstance = GeoLocation.GetInstance ();
			_sessionInstance = Session.GetInstance (PersistentStorage.Current);
			ConnectionServices = new Connections (_httpRequest);
			_global = Global.Current;

			// Custom initialization
			consoleView = new ConsoleView () {
				//Frame = View.Frame
			};

			textView = consoleView.Console;
			Title = NSBundle.MainBundle.LocalizedString ("Main", "Main");
		}

		public DetailViewController DetailViewController { get; set; }

		public CreatePostViewController CreatePostViewController { get; set; }

		public UIViewController ConsoleViewController { get; set; }

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.

			showModal ();
		}

		public async void Initialize ()
		{
			// Perform any additional setup after loading the view, typically from a nib.

			NavigationItem.SetRightBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Add), false);
			NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
				//if (this.NewPostViewController == null)
				this.CreatePostViewController = new CreatePostViewController ();
				//this.NewPostViewController.ModalInPopover = true;

				// Pass the selected object to the new view controller.
				this.NavigationController.PushViewController (this.CreatePostViewController, true);

			};


			NavigationItem.SetLeftBarButtonItem (new UIBarButtonItem (UIBarButtonSystemItem.Action), false);
			NavigationItem.LeftBarButtonItem.Clicked += (object sender, EventArgs e) => {
				if (ConsoleViewController == null) {
					this.ConsoleViewController = new UIViewController ();
					consoleView.Frame = ConsoleViewController.View.Frame;
					this.ConsoleViewController.Add (consoleView);
				}
				// Pass the selected object to the new view controller.
				this.NavigationController.PushViewController (this.ConsoleViewController, true);

			};

			TableView.Source = dataSource = new PostsTableViewSource (this);

			var location = await _geoLocationInstance.GetCurrentPosition ();
			
			var user = _sessionInstance.GetCurrentUser ();

			//CreateConnection here
			_global.current_connection = await ConnectionServices.Create(user.user_id.ToString(), location.geolocation_value, location.geolocation_accuracy.ToString());


			//init heartbeat here

			_geoLocationInstance.OnGeoPositionChanged (async (geo_value)=>{
				_global.current_connection = await ConnectionServices
					.Update(_global.current_connection.connection_id.ToString(), geo_value.geolocation_value, geo_value.geolocation_accuracy.ToString());

			});	


			JavaScriptTimer.SetInterval(async () =>
			{
					var position = await _geoLocationInstance.GetCurrentPosition();		

					_global.current_connection = await ConnectionServices
						.Update(_global.current_connection.connection_id.ToString(), position.geolocation_value, position.geolocation_accuracy.ToString());

			}, 270000);//4.5 minuets (4min 30sec) [since 1000 is 1 second]



//			var traceWriter = new TextViewWriter (SynchronizationContext.Current, textView);
//
//			Action<SeenPost> routine = (post) => {
//				dataSource.Objects.Insert (0, post);
//
//				using (var indexPath = NSIndexPath.FromRowSection (0, 0)) {
//					TableView.InsertRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
//				}
//			};

			//showModal;

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

		}


		private void showModal()
		{
			UIAlertView alert = new UIAlertView ();
			alert.Title = "Add Something";
			alert.AddButton ("One");
			alert.AddButton ("Two");
			alert.AddButton ("Three");
			alert.Message = "Low Memory o";
			alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;//UIAlertViewStyle.LoginAndPasswordInput;
			alert.Show ();

		}
	}

}
