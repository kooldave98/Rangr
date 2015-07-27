#if __IOS__
using System;
using Foundation;
using System.Threading.Tasks;

namespace common_lib
{
    public static class iOS_GeneralExtensions
    {
        public static NSObject nsobject = new NSObject();

        public static void RunOnMainThread (Action action)
        {
            if (NSThread.Current.IsMainThread) {
                action ();
                return;
            }

            nsobject.BeginInvokeOnMainThread (() => action());
        }
    }
}
#endif

