using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Threading;
using App.Core.Portable.Device;
using App.Core.Portable.Models;
using App.Common.Shared;

namespace App.iOS
{
	public partial class MainViewController : UITableViewController
	{
		IGeoLocation _geoLocationInstance;
		ISession _sessionInstance;
		UITextView textView;
		ConsoleView consoleView;
		Global _global;
		PostsTableViewSource dataSource;

		public MainViewController () : base ("MainViewController", null)
		{
			Title = NSBundle.MainBundle.LocalizedString ("Main", "Main");

			// Custom initialization
			consoleView = new ConsoleView () {
				//Frame = View.Frame
			};
				
			textView = consoleView.Console;

			_geoLocationInstance = GeoLocation.GetInstance ();
			_sessionInstance = Session.GetInstance ();
			//_postRepository = new PostRepository (new HttpRequest ());
			_global = Global.Current;
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

		public void Initialize ()
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


			var traceWriter = new TextViewWriter (SynchronizationContext.Current, textView);


			_global.client = CommonClient.GetInstance (traceWriter, _geoLocationInstance, _sessionInstance, SynchronizationContext.Current);

			Action<Post> routine = (post) => {
				dataSource.Objects.Insert (0, post);

				using (var indexPath = NSIndexPath.FromRowSection (0, 0)) {
					TableView.InsertRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
				}
			};

			_global.client.OnConnectionAborted ((client) => {
				client.Start (routine);
			});

			_global.client.Start (routine);

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
