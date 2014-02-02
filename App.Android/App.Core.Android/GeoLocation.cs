using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Portable.Device;
using Android.App;

namespace App.Common.Shared
{
	public partial class GeoLocation
	{
		public static GeoLocation GetInstance(SynchronizationContext context, Activity activityContext)
		{
			return _instance ?? (_instance = new GeoLocation(context, activityContext));
		}

		private GeoLocation(SynchronizationContext context, Activity activityContext)
		{
			if (this.geolocator != null)
				return;

			_context = context;
			geolocator = new Geolocator (activityContext);

			Init ();
		}
	}
}

