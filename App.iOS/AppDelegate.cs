using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
//delete from here
using System.Threading;
using App.Core.Portable.Device;
using App.Common.Shared;
using System.Linq;
using System.Collections.Generic;

namespace App.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UINavigationController navigationController;
		UIWindow window;
		IGeoLocation _geoLocationInstance;
		ISession _sessionInstance;

		//
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
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


		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			var controller = new MasterViewController ();
			navigationController = new UINavigationController (controller);
			window.RootViewController = navigationController;

			// make the window visible
			window.MakeKeyAndVisible ();
			
			return true;
		}
	}
}

