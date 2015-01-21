using System;
using System.Linq;
using System.Collections.Generic;

using Foundation;
using UIKit;
using Google.Maps;

namespace experiments.ios
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            MapServices.ProvideAPIKey("AIzaSyACSPtVSdTYtRYQTjNh1Y6sUmNtVpshP4o");

            window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                RootViewController = new MapViewController()
            };
            window.MakeKeyAndVisible();
            
            return true;
        }
    }
}

