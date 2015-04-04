using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using App.Common;

using Google.Maps;
using CoreFoundation;
using System.Runtime.InteropServices;

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
        private MapViewController map_view = new MapViewController();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Shared = this;
            MapServices.ProvideAPIKey(MapsApiKey);

            window = new UIWindow(UIScreen.MainScreen.Bounds);

            Theme.Apply();

            navigation.NavigationBar.Translucent = false;
            tab_bar.TabBar.Translucent = false;

            navigation.TabBarItem = new UITabBarItem("Feed", UIImage.FromBundle("running.png"),1);
            map_view.TabBarItem = new UITabBarItem("Map", UIImage.FromBundle("world_times.png"),2);

            tab_bar.AddChildViewController(navigation);
            tab_bar.AddChildViewController(map_view);

            show_feed();

            window.RootViewController = tab_bar;
            window.MakeKeyAndVisible();

            if (!AppGlobal.Current.CurrentUserAndConnectionExists)
            {
                var login = new LoginViewController();
                login.LoginSucceeded += () => {

                    login.DismissViewController(true, null);
                };

                window.RootViewController.PresentViewController(login, true, null);
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
            vc.NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
                show_new_post();
            };
        }

        public void show_new_post()
        {
            var dc = new NewPostViewController();
            dc.CreatePostSucceeded += () => {
                navigation.PopToRootViewController(true);
            };

            dc.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Done), false);
            dc.NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
                dc.Save(sender, e);
            };

            navigation.PushViewController(dc, true);

            select_picture(dc);
        }

        public void show_detail(Post post_item)
        {
            var dc = new PostDetailViewController(post_item);
            navigation.PushViewController(dc, true);
        }

        private void select_picture(NewPostViewController controller)
        {
            var picker = new UIImagePickerController();

            picker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

            picker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;

            picker.Canceled += (sender, evt) => { picker.DismissViewController(true, null); };

            picker.FinishedPickingMedia += (s, e) => {


                picker.DismissViewController(true, null);

                //image = [info valueForKey:UIImagePickerControllerOriginalImage];
                //[imagepickerview setImage:image];

                var rum = e.Info[UIImagePickerController.OriginalImage];//.ObjectForKey(new NSString("UIImagePickerController.OriginalImage"));

                var image = (UIImage)rum;



                controller.BindDataToView(image);
            };

            controller.PresentViewController(picker, true, null);
        }
    }
}