using System;

using Foundation;
using UIKit;
using Google.Maps;

namespace experiments.ios
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;
        UITabBarController tab_bar = new UITabBarController();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            MapServices.ProvideAPIKey("AIzaSyACSPtVSdTYtRYQTjNh1Y6sUmNtVpshP4o");

            window = new UIWindow(UIScreen.MainScreen.Bounds);

            //Theme.Apply();


            var red_square = new ConstraintsVC();
            var lorem_table = new UINavigationController(new ItemsViewController());
            var map = new MapViewController();

            lorem_table.NavigationBar.Translucent = false;
            tab_bar.TabBar.Translucent = false;


            red_square.TabBarItem = new UITabBarItem("First", UIImage.FromBundle("first.png"),1);
            lorem_table.TabBarItem = new UITabBarItem("Second", UIImage.FromBundle("second.png"),2);
            map.TabBarItem = new UITabBarItem("Third", UIImage.FromBundle("first.png"),3);

            tab_bar.AddChildViewController(red_square);
            tab_bar.AddChildViewController(lorem_table);
            tab_bar.AddChildViewController(map);

            window.RootViewController = tab_bar;
            window.MakeKeyAndVisible();
            
            return true;
        }

        private void show_login()
        {
//            var login = new LoginViewController();
//
//            login.LoginSucceeded += () => {
//                navigation.DismissViewController(true, () => {
//                        //navigation.PushViewController(new MapViewController(), true);
//                    });
//            };
//
//            navigation.PresentViewController(login, true, null);
        }
    }
}

