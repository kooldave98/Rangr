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

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Consider using a USING statement to wrap all the code in between.
            var window = new UIWindow(UIScreen.MainScreen.Bounds);
            var start_screen = new SimpleStartScreenController();


            /////wrap body start



            start_screen.OnStartApp += async () =>
            {
                var result = await new MobileEntrySequence(new SequenceViewModel()).StartAsync(window);

                if (!result.Canceled)
                {   
                        var login = 
                            new LoginViewController()
                                //.Init(c => c.set_user_name(result.EnteredMobileNumber))
                                .Init(c=> c.LoginSucceeded += () => window.SwitchRootViewController(new_tab_bar(), true));
                            
                        
                    window.SwitchRootViewController(login, true);

                    login.set_user_name(result.EnteredMobileNumber);
                    
                }
            };



            ////Wrap body end
            window.RootViewController = start_screen;

            window.MakeKeyAndVisible();

            return true;
        }



        public MapViewController new_map_view_controller()
        {
            MapServices.ProvideAPIKey("AIzaSyACSPtVSdTYtRYQTjNh1Y6sUmNtVpshP4o");
            return new MapViewController();
        }

        public UITabBarController new_tab_bar()
        {
            var red_square = new ConstraintsVC();
            var lorem_table = new ItemsViewController().ToNavigationController();
            var map = new_map_view_controller();

            red_square.TabBarItem = new UITabBarItem("First", UIImage.FromBundle("first.png"), 1);
            lorem_table.TabBarItem = new UITabBarItem("Second", UIImage.FromBundle("second.png"), 2);
            map.TabBarItem = new UITabBarItem("Third", UIImage.FromBundle("first.png"), 3);

            var controllers = new UIViewController[]
            { 
                red_square, lorem_table, map
            };

            return controllers.ToTabBarController();
        }
    }
}

