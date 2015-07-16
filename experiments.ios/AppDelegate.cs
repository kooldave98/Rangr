using System;
using Foundation;
using UIKit;
using Google.Maps;
using ios_ui_lib;
using System.Threading.Tasks;
using CoreGraphics;
using common_lib;

namespace experiments.ios
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            Window.RootViewController =
                
                new SimpleStartScreenController()
                    .Init(sc =>sc.OnStartApp += async () => {
                        var result = await new MobileEntrySequence(new SequenceViewModel()).StartAsync(Window);

                        if (!result.Canceled)
                        {   
                            new UILoginViewController()
                                .Init(c => Window.SwitchRootViewController(c, true))
                                .Init(c => c.set_user_id(result.EnteredMobileNumber))
                                .Init(c => c.LoginRequested += (o, e) => Window.SwitchRootViewController(new_tab_bar(), true));
                        }
                    });

            Window.MakeKeyAndVisible();

            return true;
        }

        public UITabBarController new_tab_bar()
        {
            return
                new UIViewController[] {

                    new ConstraintsVC().Init(c=>c.TabBarItem = new UITabBarItem("First", UIImage.FromBundle("first.png"), 1)),

                    new ItemsViewController().ToNavigationController().Init(c=>c.TabBarItem = new UITabBarItem("Second", UIImage.FromBundle("second.png"), 2)),

                    new_map_view_controller().Init(c=>c.TabBarItem = new UITabBarItem("Third", UIImage.FromBundle("first.png"), 3))

                }.ToTabBarController();
        }

        public MapViewController new_map_view_controller()
        {
            MapServices.ProvideAPIKey("AIzaSyACSPtVSdTYtRYQTjNh1Y6sUmNtVpshP4o");
            return new MapViewController();
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}

