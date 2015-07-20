#if __IOS__
using System;
using Foundation;

namespace common_lib
{
    public static class iOS_GeneralExtensions
    {
        public static void RunOnMainThread (Action action)
        {
            if (NSThread.Current.IsMainThread) {
                action ();
                return;
            }

            new NSObject().BeginInvokeOnMainThread (() => action());
        }
    }
}
#endif

