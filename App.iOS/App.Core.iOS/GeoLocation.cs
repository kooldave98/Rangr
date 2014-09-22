using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Geolocation;
using System.Threading;
using System.Threading.Tasks;

namespace App.Common
{
	public partial class GeoLocation
	{
		public static GeoLocation GetInstance ()
		{
			return _instance ?? (_instance = new GeoLocation ());
		}


		private GeoLocation ()
		{
			if (this.geolocator != null)
				return;

			geolocator = new Geolocator ();

			Init ();

		}
	}
}