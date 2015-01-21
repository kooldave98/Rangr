using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using App.Common;

using Google.Maps;

namespace rangr.ios
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        const string MapsApiKey = "AIzaSyACSPtVSdTYtRYQTjNh1Y6sUmNtVpshP4o";
        public static AppDelegate Shared;

        private UIWindow window;
        private UINavigationController navigation = new UINavigationController();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Shared = this;
            MapServices.ProvideAPIKey(MapsApiKey);

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
            navigation.NavigationBar.BarTintColor = Color.Blue;

            window.RootViewController = navigation;
            window.MakeKeyAndVisible();
            return true;
        }

        public void show_login()
        {
            var login = new LoginViewController();
            login.LoginSucceeded += () =>
            {
                show_feed();
            };


            navigation.PushViewController(login, true);
        }

        public void show_feed()
        {
            var vc = new PostListViewController();
            vc.PostItemSelected += show_detail;
            navigation.PushViewController(vc, true);
        }

        public void show_detail(Post post_item)
        {
            var dc = new DetailViewController();
            dc.SetDetailItem(post_item);
            navigation.PushViewController(dc, true);
        }
    }
}

