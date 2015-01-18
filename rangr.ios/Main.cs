using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin;

namespace rangr.ios
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Insights.Initialize("4d76332a65b66f1deaab446e299dbbe898ba5695");

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
