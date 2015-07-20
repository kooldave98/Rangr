using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using App.Common;
using common_lib;

namespace rangr.ios
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            Analytics.Current.Initialize(Resources.AnalyticsApiKey);

            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}