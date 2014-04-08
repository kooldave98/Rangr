using System;
using System.Threading;

namespace App.Common.Shared
{
	public static class JavaScriptTimer
	{
		//http://www.dailycoding.com/Posts/easytimer__javascript_style_settimeout_and_setinterval_in_c.aspx
		public static IDisposable SetInterval (Action method, int delayInMilliseconds)
		{
			System.Timers.Timer timer = new System.Timers.Timer (delayInMilliseconds);
			timer.Elapsed += (source, e) => {
				method ();
			};

			timer.Enabled = true;
			timer.Start ();

			// Returns a stop handle which can be used for stopping
			// the timer, if required
			return timer as IDisposable;
		}

		public static IDisposable SetTimeout (Action method, int delayInMilliseconds)
		{
			System.Timers.Timer timer = new System.Timers.Timer (delayInMilliseconds);
			timer.Elapsed += (source, e) => {
				method ();
			};

			timer.AutoReset = false;
			timer.Enabled = true;
			timer.Start ();

			// Returns a stop handle which can be used for stopping
			// the timer, if required
			return timer as IDisposable;
		}
	}
}

