#if __IOS__
using System;
using UIKit;

//// <summary>
//// //See here for how to animate switching window root-viewcontroller 
////http://stackoverflow.com/a/26403108/502130 and a better more robust one here:
////https://forums.xamarin.com/discussion/8733/changing-the-rootviewcontroller
//// </summary>
using System.Threading.Tasks;


namespace common_lib
{
    public static class UIWindowExtensions
    {
        /// <summary>
        /// Transitions a controller to the rootViewController, for a fullscreen transition
        /// </summary>
        public static void SwitchRootViewController(this UIWindow window, UIViewController controller, bool animated = true)
        {

            // Return if it's already the root controller
            if (window.RootViewController == controller)
                return;

            // Set the root controller
            window.RootViewController = controller;

            // Peform an animation, note that null is not allowed as a callback, so I use delegate { }
            if (animated)
                UIView.Transition(
                    window, 
                    .3, 
                    UIViewAnimationOptions.TransitionCrossDissolve, delegate { }, delegate { });
        }

        public async static Task PresentViewController(this UIWindow window, UIViewController controller, bool animated = true)
        {

            // Return if it's already the root controller
            if (window.RootViewController == controller)
                return;

            // Set the root controller
            await window.RootViewController.PresentViewControllerAsync(controller, animated);
        }
    }
}

#endif