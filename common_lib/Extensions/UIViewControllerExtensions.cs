#if __IOS__
using System;
using System.Linq;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace common_lib
{
    public static class UIViewControllerExtensions
    {
        public static UINavigationController ToNavigationController(this UIViewController controller, bool translucent_bar = false)
        {
            var navigation_controller = new UINavigationController(controller);
            navigation_controller.NavigationBar.Translucent = translucent_bar;

            return navigation_controller;
        }

        public static UITabBarController ToTabBarController(this IEnumerable<UIViewController> controllers, bool translucent_bar = false)
        {
            var tab_bar_controller = new UITabBarController();
            tab_bar_controller.TabBar.Translucent = translucent_bar;

            foreach (var controller in controllers)
            {
                tab_bar_controller.AddChildViewController(controller);
            }

            return tab_bar_controller;
        }
            
    }
}
#endif

