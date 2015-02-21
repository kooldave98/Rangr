using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using App.Common;

using Google.Maps;

namespace rangr.ios
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        const string MapsApiKey = "AIzaSyACSPtVSdTYtRYQTjNh1Y6sUmNtVpshP4o";
        public static AppDelegate Shared;

        private UIWindow window;
        private UITabBarController tab_bar = new UITabBarController();
        private UINavigationController navigation = new UINavigationController();


        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Shared = this;
            MapServices.ProvideAPIKey(MapsApiKey);

            window = new UIWindow(UIScreen.MainScreen.Bounds);

            Theme.Apply();

            navigation.NavigationBar.Translucent = false;
            tab_bar.TabBar.Translucent = false;

            show_feed();

            tab_bar.AddChildViewController(navigation);

            window.RootViewController = tab_bar;
            window.MakeKeyAndVisible();

            if (!AppGlobal.Current.CurrentUserAndConnectionExists)
            {
                var login = new LoginViewController();
                login.LoginSucceeded += () =>
                {
                    login.DismissViewController(true, null);
                };

                tab_bar.PresentViewController(login, true, null);
            }

            return true;
        }

        public void show_feed()
        {
            var vc = new PostListViewController();
            vc.PostItemSelected += show_detail;

            //vc.NewPostSelected += show_new_post;
            navigation.PushViewController(vc, true);
            vc.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add), false);
            vc.NavigationItem.RightBarButtonItem.Clicked += (sender, e) =>
            {
                show_new_post();

            };
        }

        public void show_new_post()
        {
            var dc = new NewPostViewController();
            dc.CreatePostSucceeded += () =>
            {
                navigation.PopToRootViewController(true);
            };
            navigation.PushViewController(dc, true);
        }

        public void show_detail(Post post_item)
        {
            var dc = new PostDetailViewController();
            dc.SetDetailItem(post_item);
            navigation.PushViewController(dc, true);
        }
    }
}

