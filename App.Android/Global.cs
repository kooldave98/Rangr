//using System;
//using Android.App;
//using Android.Content;
//
//namespace App.Android
//{
//
//		[Application (Label = "@string/app_name", Theme = "@style/AppTheme")]
//		public class Global : global::Android.App.Application
//		{
//			public static Global Current {
//				get {
//					return _instance;
//				}
//			}
//	
//			private static Global _instance = null;
//	
//			public Global (IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
//				: base (handle, transfer)
//			{
//				_instance = this;
//			}
//	
//			public override void OnCreate ()
//			{
//				base.OnCreate ();
//			}
//	
//			//		public override void OnCreate ()
//			//		{
//			//			base.OnCreate ();
//			//
//			//			TestFlight.TestFlight.TakeOff (this, "dfc77da6-f29c-4fff-acd6-59dc5ad774ab");
//			//			AndroidEnvironment.UnhandledExceptionRaiser += HandleUnhandledException;
//			//		}
//			//
//			//		void HandleUnhandledException (object sender, RaiseThrowableEventArgs e)
//			//		{
//			//			TestFlight.TestFlight.SendCrash (e.Exception);
//			//		}
//			//
//			//		protected override void Dispose (bool disposing)
//			//		{
//			//			AndroidEnvironment.UnhandledExceptionRaiser -= HandleUnhandledException;
//			//			base.Dispose (disposing);
//			//		}
//		}
//
//}