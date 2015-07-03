using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using App.Common;

using Google.Maps;
using CoreFoundation;
using System.Runtime.InteropServices;
using Media.Plugin.Abstractions;
using System.Threading.Tasks;
using ios_ui_lib;
using CoreGraphics;
using Media.Plugin;

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

            //setting these to translucent makes the content to be pushed and not overlaid
            navigation.NavigationBar.Translucent = false;
            tab_bar.TabBar.Translucent = false;

            navigation.TabBarItem = new UITabBarItem("Feed", UIImage.FromBundle("running.png"),1);
            map_view.TabBarItem = new UITabBarItem("Map", UIImage.FromBundle("world_times.png"),2);

            tab_bar.AddChildViewController(navigation);
            tab_bar.AddChildViewController(map_view);

            show_feed();

            window.RootViewController = tab_bar;
            window.MakeKeyAndVisible();

            if (!AppGlobal.Current.CurrentUserExists)
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

            navigation.PushViewController(vc, true);
            vc.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add), false);
            vc.NavigationItem.RightBarButtonItem.Clicked += async (sender, e) => {
                await show_new_post();
            };
        }

        public void show_detail(Post post_item)
        {
            var dc = new PostDetailViewController(post_item);
            navigation.PushViewController(dc, true);
        }

        public async Task show_new_post()
        {
            await select_picture();
        }

        private async Task select_picture()
        {
            var media_file = await CrossMedia.Current.PickPhotoAsync();

            set_caption(media_file);
        }

        private void set_caption(MediaFile media_file)
        {
            if (media_file != null)
            {
                var dc = new NewPostViewController();
                var dc_wrapped = dc.ToNavigationController();

                dc.CreatePostSucceeded += () => {
                    dc_wrapped.DismissViewController(true, null);
                };
                dc.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Cancel), false);
                dc.NavigationItem.LeftBarButtonItem.Clicked += (sender, e) => {
                    dc_wrapped.DismissViewController(true, null);
                };
                dc.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Done), false);
                dc.NavigationItem.RightBarButtonItem.Clicked += (sender, e) => {
                    dc.Save(sender, e);
                };

                dc.selected_image = CropCenterSquare(UIImage.FromFile(media_file.Path));

                navigation.PresentViewController(dc_wrapped, true, ()=>{ dispose_media_file(media_file); });
            }
        }



        private void dispose_media_file(MediaFile Media)
        {
            if (Media != null) {
                Media.Dispose();
                Media = null;
            }
        }

        // crop a square from the center of the image, without resizing
        //Adapted from here: 
        //http://forums.xamarin.com/discussion/4170/resize-images-and-save-thumbnails
        private UIImage CropCenterSquare(UIImage sourceImage)
        {
            nfloat shortest_length;

            nfloat crop_x; nfloat crop_y; 

            if (sourceImage.Size.Height == sourceImage.Size.Width)
            {
                shortest_length = sourceImage.Size.Height;//Any(Width or Height)

                crop_y = 0;
                crop_x = 0;
            }


            if (sourceImage.Size.Height < sourceImage.Size.Width)
            {
                shortest_length = sourceImage.Size.Height;

                crop_y = 0;
                crop_x = (sourceImage.Size.Width - shortest_length) / 2;
            }

            if(sourceImage.Size.Width < sourceImage.Size.Height)
            {
                shortest_length = sourceImage.Size.Width;

                crop_y = (sourceImage.Size.Height - shortest_length) / 2;
                crop_x = 0;
            }



            nfloat width = shortest_length;
            nfloat height = shortest_length;



            var imgSize = sourceImage.Size;
            UIGraphics.BeginImageContext(new CGSize(width, height));
            var context = UIGraphics.GetCurrentContext();
            var clippedRect = new CGRect(0, 0, width, height);
            context.ClipToRect(clippedRect);
            var drawRect = new CGRect(-crop_x, -crop_y, imgSize.Width, imgSize.Height);
            sourceImage.Draw(drawRect);
            var modifiedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return modifiedImage;
        }
    }
}

//public static string RemoveWhitespace(this string input)
//{
//    return new string(input.ToCharArray()
//        .Where(c => !Char.IsWhiteSpace(c))
//        .ToArray());
//}