using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Portable.Device;

namespace App.Common.Shared
{
	public partial class GeoLocation
	{
		public static GeoLocation GetInstance()
		{
			return _instance ?? (_instance = new GeoLocation());
		}


		private GeoLocation()
		{
			if (this.geolocator != null)
				return;

			geolocator = new Geolocator();

			Init ();

		}
	}
}