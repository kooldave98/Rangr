using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
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

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//
			// Build the UI
			//
			mainViewController = new MainViewController ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = new UINavigationController (mainViewController);
			window.MakeKeyAndVisible ();
			//Roy test

			return true;
		}

		public override void WillEnterForeground (UIApplication application)
		{
		}

		public override void DidEnterBackground (UIApplication application)
		{
		}
			
	}
}
