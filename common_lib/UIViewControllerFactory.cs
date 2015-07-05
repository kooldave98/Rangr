#if __IOS__
using System;
using UIKit;
using CoreGraphics;

namespace common_lib
{
    public static class UIViewControllerFactory
    {        
        public static UIViewController Generic(UIColor color = null)
        {
            var controller = new UIViewController();

            controller.View.BackgroundColor = color ?? UIColor.White;

            return controller;
        }
    }
}
#endif

