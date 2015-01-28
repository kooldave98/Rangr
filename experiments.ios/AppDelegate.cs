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
        //UITabBarController container;
        UINavigationController navigation = new UINavigationController();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            MapServices.ProvideAPIKey("AIzaSyACSPtVSdTYtRYQTjNh1Y6sUmNtVpshP4o");

            window = new UIWindow(UIScreen.MainScreen.Bounds);

            window.RootViewController = navigation;
            window.MakeKeyAndVisible();

            var login = new LoginViewController();

            login.LoginSucceeded += () =>
            {
                navigation.DismissViewController(true, () =>
                    {
                        navigation.PushViewController(new MapViewController(), true);
                    });
            };

            navigation.PresentViewController(login, true, null);
            
            return true;
        }
    }
}

