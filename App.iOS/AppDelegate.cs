using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using App.Common;
using Google.Maps;

namespace App.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;

        const string MapsApiKey = "AIzaSyACSPtVSdTYtRYQTjNh1Y6sUmNtVpshP4o";

        public UINavigationController NavigationController { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            MapServices.ProvideAPIKey(MapsApiKey);

            window = new UIWindow(UIScreen.MainScreen.Bounds);
            NavigationController = new UINavigationController(new MainViewController());

            window.RootViewController = NavigationController;

//            if (AppGlobal.Current.CurrentUserAndConnectionExists)
//            {
//                NavigationController.PushViewController(new StreamViewController(), false);
//            }
//            else
//            {
//                NavigationController.PushViewController(new SignInViewController(), false);
//            }

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
