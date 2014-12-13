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
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;

        public UINavigationController NavigationController { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            window = new UIWindow(UIScreen.MainScreen.Bounds);
            NavigationController = new UINavigationController();

            window.RootViewController = NavigationController;

            if (AppGlobal.Current.CurrentUserAndConnectionExists)
            {
                NavigationController.PushViewController(new StreamViewController(), false);
            }
            else
            {
                NavigationController.PushViewController(new SignInViewController(), false);
            }

            window.MakeKeyAndVisible();

            return true;
        }

        public override void WillEnterForeground(UIApplication application)
        {
        }

        public override void DidEnterBackground(UIApplication application)
        {
        }
    }
}
