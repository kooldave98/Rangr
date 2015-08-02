#if __IOS__
using System;
using Foundation;
using System.Threading.Tasks;

namespace common_lib
{
    public static class iOS_GeneralExtensions
    {
        public static void SafeInvokeOnMainThread (this NSObject nsobject, Action action)
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

