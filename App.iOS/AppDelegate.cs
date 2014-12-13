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
		UINavigationController navigationController;

		public UINavigationController NavigationController 
		{
			get
			{
				return navigationController;
			}
			set
			{
				navigationController = value;
			}
		}


		//MainViewController mainViewController;
		//SignInViewController mainViewController;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			//
			// Build the UI
			//

			//mainViewController = new StreamViewController ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			navigationController = new UINavigationController ();

			window.RootViewController = navigationController;

			if (AppGlobal.Current.CurrentUserAndConnectionExists) {
				navigationController.PushViewController (new StreamViewController (), false);
			} else {
				navigationController.PushViewController (new SignInViewController (), false);
			}

			window.MakeKeyAndVisible ();

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
