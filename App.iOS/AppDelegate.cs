using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using App.Core.Portable.Device;
using System.Linq;
using System.Collections.Generic;
using App.Common;

namespace App.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		MainViewController mainViewController;
		ISession _sessionInstance = Session.GetInstance();

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			if(!UIDevice.CurrentDevice.CheckSystemVersion(7,0))
				Theme.Apply ();

			//
			// Build the UI
			//
			mainViewController = new MainViewController ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (mainViewController);
			window.MakeKeyAndVisible ();

			//
			// Show the login screen at startup
			//

			//Check if the user exists first before loading the login view
			var user = _sessionInstance.GetCurrentUser (true);
			if (user != null) {
				mainViewController.Initialize ();

			} else {
				var login = new LoginViewController (mainViewController.Initialize);
				mainViewController.PresentViewController (login, false, null);
			}


			return true;
		}

		public override void WillEnterForeground (UIApplication application)
		{
//			if (ShouldShowLogin (Global.LastUseTime)) {
//
//				var login = new LoginViewController (mainViewController.Initialize);
//				mainViewController.PresentViewController (login, false, null);
//			}
		}

		public override void DidEnterBackground (UIApplication application)
		{
			Global.LastUseTime = DateTime.UtcNow;
		}

		public static bool ShouldShowLogin (DateTime? lastUseTime)
		{
			if (!lastUseTime.HasValue) {
				return true;
			}

			return (DateTime.UtcNow - lastUseTime) > Global.ForceLoginTimespan;
		}
	}
}



//
//
// This method is invoked when the application has loaded and is ready to run. In this
// method you should instantiate the window, load the UI into it and then make the window
// visible.
//
// You have 17 seconds to return from this method, or iOS will terminate your application.
//
//
//		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
//		{
//			window = new UIWindow(UIScreen.MainScreen.Bounds);
//
//			_geoLocationInstance = GeoLocation.GetInstance (SynchronizationContext.Current);
//			_sessionInstance = Session.GetInstance ();
//
//			var controller = new UIViewController();
//
//			var label = new UILabel(new RectangleF(0, 0, 320, 30));
//			label.Text = "SignalR Client";
//
//			var textView = new UITextView(new RectangleF(0, 35, 320, 500));
//
//			controller.Add(label);
//			controller.Add(textView);
//
//			window.RootViewController = controller;
//			window.MakeKeyAndVisible();
//
//
//			var traceWriter = new TextViewWriter(SynchronizationContext.Current, textView);
//
//			var client = new CommonClient(traceWriter, _geoLocationInstance, _sessionInstance, SynchronizationContext.Current);
//
//			var task = client.RunAsync((results) => {
//				if (results.Any())
//				{
//					traceWriter.WriteLine("received data");
////					results.ForEach(p=>{
////						dataSource.Objects.Insert (0, p.Text);
////
////						using (var indexPath = NSIndexPath.FromRowSection (0, 0))
////							TableView.InsertRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
////					});
//				}
//			});
//
//			return true;
//		}
//
