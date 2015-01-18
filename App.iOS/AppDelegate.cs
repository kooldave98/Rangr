using System;
using CoreGraphics;
using Foundation;
using UIKit;
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
        const string MapsApiKey = "AIzaSyACSPtVSdTYtRYQTjNh1Y6sUmNtVpshP4o";

        public static AppDelegate Shared;

        UIWindow window;
        UINavigationController navigation = new UINavigationController();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            MapServices.ProvideAPIKey(MapsApiKey);

            Shared = this;

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

            window = new UIWindow(UIScreen.MainScreen.Bounds);
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes
                {
                    TextColor = UIColor.White
                });

            if (AppGlobal.Current.CurrentUserAndConnectionExists)
            {
                show_feed();
            }
            else
            {
                show_login();
            }


            navigation.NavigationBar.TintColor = UIColor.White;
            navigation.NavigationBar.BarTintColor = App.Common.Color.Blue;

            window.RootViewController = navigation;
            window.MakeKeyAndVisible();
            return true;
        }

        private void show_login()
        {
            var login = new LoginViewController();
            login.LoginSucceeded += show_feed;
            navigation.PushViewController(login, false);
        }

        public void show_feed()
        {
            var main = new MainViewController();
            //main.ProductTapped += ShowProductDetail;
            navigation.PushViewController(main, false);
        }

        public override void WillEnterForeground(UIApplication application)
        {
        }

        public override void DidEnterBackground(UIApplication application)
        {
        }
    }
}
