using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;

namespace App.Common
{
	public partial class GeoLocation
	{
		public static GeoLocation GetInstance ()
		{
			return _instance ?? (_instance = new GeoLocation (Application.Context));
		}

		private GeoLocation (Context appContext)
		{
			if (this.geolocator != null)
				return;
				
			geolocator = new Geolocator (appContext);

			Init ();
		}
	}
}

