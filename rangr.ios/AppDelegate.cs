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
using common_lib;

namespace rangr.ios
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public static AppDelegate Shared;
        public override UIWindow Window { get; set; }

        private UINavigationController main_navigation { get; set;}

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Shared = this;
            MapServices.ProvideAPIKey(Resources.MapsApiKey);
            RangrTheme.Apply();

            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            if (AppGlobal.Current.CurrentUserExists)
                Window.RootViewController = get_main_tab_bar();
            else
            Window.RootViewController =
                new SimpleStartScreenController()
                        .Chain(sc =>sc.OnStartApp += async () => {
                            var result = await new MobileEntrySequence(new SequenceViewModel()).StartAsync(Window);

                            if (!result.Canceled)
                            {   
                                new VerifyNumberViewController(result.EnteredMobileNumber)
                                    .Chain(c => Window.PresentViewController(c, true))
                                    .Chain(c => c.set_user_id(result.EnteredMobileNumber))
                                    .Chain(c => c.VerificationSucceeded += () => Window.SwitchRootViewController(get_main_tab_bar(), true))
                                    .Chain(c => c.VerificationFailed += async () => c.DismissViewControllerAsync(true));
                            }
                    });



            Window.MakeKeyAndVisible();
            return true;
        }

        public UITabBarController get_main_tab_bar()
        {
            return 
                new UIViewController[] { 
                    get_feed()
                        .Chain(n => n.TabBarItem = new UITabBarItem("Feed", UIImage.FromBundle("running.png"),1))
                        .Chain(vc=>vc.TabBarItem.BadgeValue = "3"), 
                    new MapViewController()
                        .Chain(m=>m.TabBarItem = new UITabBarItem("Map", UIImage.FromBundle("world_times.png"),2)),
                    UIViewControllerFactory.Generic()
                        .Chain(v=>v.TabBarItem = new UITabBarItem(UITabBarSystemItem.Featured, 3))
                }.ToTabBarController();
        }

        public UINavigationController get_feed()
        {
            return 
                main_navigation ??
                (main_navigation =
                    new PostListViewController()
                        .Chain(c => c.PostItemSelected += show_detail)
                        .Chain(c => c.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Add), false))
                        .Chain(c => c.NavigationItem.RightBarButtonItem.Clicked += show_new_post)
                    .ToNavigationController());
        }

        public void show_detail(Post post_item)
        {
            var dc = new PostDetailViewController(post_item);
            main_navigation.PushViewController(dc, true);
        }

        public async void show_new_post(object sender, EventArgs e)
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

                main_navigation.PresentViewController(dc_wrapped, true, ()=>{ dispose_media_file(media_file); });
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

    public class SequenceViewModel : MobileEntrySequenceViewModel
    {
        public override string format_input(string input)
        {            
            return PhoneNumberFormatter.Current.format_number(input);
        }

        public override bool is_valid_international_number(string input)
        {
            return PhoneNumberValidator.Current.is_valid_number(input);
        }

        public override ISOCountry[] iso_countries { get; protected set;}


        public SequenceViewModel()
        {
            iso_countries = new GetCountryCodes().execute().ToArray();
        }
    }
}