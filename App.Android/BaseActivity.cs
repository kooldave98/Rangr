using System;

using Android.App;
using Android.Content;

namespace App.Android {
	public class BaseActivity : ListActivity {

		protected override void OnPause ()
		{
			base.OnPause ();

			Android.Application.LastUseTime = DateTime.UtcNow;
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			if (ShouldShowLogin (Android.Application.LastUseTime)) {
				var intent = new Intent (this, typeof (LoginActivity));
				intent.SetFlags (ActivityFlags.ClearTop);
				StartActivity(intent);
			}
		}

		public static bool ShouldShowLogin (DateTime? lastUseTime)
		{
			if (!lastUseTime.HasValue) {
				return true;
			}

			return (DateTime.UtcNow - lastUseTime) > Android.Application.ForceLoginTimespan;
		}
	}
}