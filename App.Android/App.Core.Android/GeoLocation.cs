using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Portable.Device;
using Android.App;
using Android.Content;

namespace App.Common.Shared
{
	public partial class GeoLocation
	{
		public static GeoLocation GetInstance(Context appContext)
		{
			return _instance ?? (_instance = new GeoLocation(appContext));
		}

		private GeoLocation(Context appContext)
		{
			if (this.geolocator != null)
				return;
				
			geolocator = new Geolocator (appContext);

			Init ();
		}
	}
}

