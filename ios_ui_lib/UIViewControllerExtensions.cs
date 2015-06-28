using System;
using UIKit;

namespace ios_ui_lib
{
    public static class UIViewControllerExtensions
    {
        public static UINavigationController ToNavigationController(this UIViewController controller)
        {
            var navigation_controller = new UINavigationController(controller);
            navigation_controller.NavigationBar.Translucent = false;

            return navigation_controller;
        }
    }
}

