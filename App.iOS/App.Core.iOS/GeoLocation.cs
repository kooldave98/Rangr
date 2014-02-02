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
		public static GeoLocation GetInstance(SynchronizationContext context)
		{
			return _instance ?? (_instance = new GeoLocation(context));
		}


		private GeoLocation(SynchronizationContext context)
		{
			if (this.geolocator != null)
				return;

			_context = context;
			geolocator = new Geolocator();

		}
	}
}