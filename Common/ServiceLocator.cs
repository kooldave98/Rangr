using System;
using App.Core.Portable.Device;

namespace App.Common.Shared
{
	public class ServiceLocator
	{
		private IGeoLocation _geoLocation { get; set;}
		public IGeoLocation IGeoLocationService{
			get{ 
				return _geoLocation ?? (_geoLocation = GeoLocation.GetInstance ());
			}
		}
	}
}
