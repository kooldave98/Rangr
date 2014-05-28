//http://stackoverflow.com/questions/3667022/android-is-application-running-in-background/13809991#13809991

//using AndroidApplication = Android.App.Application;
//using Android.App;
//using Android.OS;
//
//namespace App.Android
//{
//
//	public class MyLifecycleHandler : AndroidApplication.IActivityLifecycleCallbacks 
//	{
//		public void Dispose ()
//		{
//			throw new System.NotImplementedException ();
//		}
//
//		public System.IntPtr Handle {
//			get {
//				throw new System.NotImplementedException ();
//			}
//		}
//
//
//
//		// I use four separate variables here. You can, of course, just use two and
//		// increment/decrement them instead of using four and incrementing them all.
//		private int resumed;
//		private int paused;
//		private int started;
//		private int stopped;
//
//
//		public override void OnActivityCreated(Activity activity, Bundle savedInstanceState) {
//		}
//			
//		public override void OnActivityDestroyed(Activity activity) {
//		}
//			
//		public override void OnActivityResumed(Activity activity) {
//			++resumed;
//		}
//
//
//		public override void OnActivityPaused(Activity activity) {
//			++paused;
//			//android.util.Log.w("test", "application is in foreground: " + (resumed > paused));
//		}
//
//
//		public override void OnActivitySaveInstanceState(Activity activity, Bundle outState) {
//		}
//			
//		public override void OnActivityStarted(Activity activity) {
//			++started;
//		}
//			
//		public override void OnActivityStopped(Activity activity) {
//			++stopped;
//			//android.util.Log.w("test", "application is visible: " + (started > stopped));
//		}
//
//		// If you want a static function you can use to check if your application is
//		// foreground/background, you can use the following:
//		/*
//    // Replace the four variables above with these four
//		private static int resumed;
//		private static int paused;
//		private static int started;
//		private static int stopped;
//
//		// And these two public static functions
//		public static boolean isApplicationVisible() {
//			return started > stopped;
//		}
//
//		public static boolean isApplicationInForeground() {
//			return resumed > stopped;
//		}
//		*/
//	}
//
//}
//
//
//
