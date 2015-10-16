#if __ANDROID__
using System;
using Android.Gms.Maps;

namespace rangr.common
{
    public static class GoogleMapExtensions
    {
        public static void OnMapReady(this MapFragment map_fragment, Action<GoogleMap> map_ready_handler)
        {
            var on_map_ready = new OnGoogleMapReady();

            on_map_ready.MapReady += map_ready_handler;
            
            map_fragment.GetMapAsync(on_map_ready);
        }
    }

    public class OnGoogleMapReady : Java.Lang.Object, IOnMapReadyCallback
    {
        public event Action<GoogleMap> MapReady = delegate {};

        public void OnMapReady(GoogleMap googleMap)
        {
            MapReady(googleMap);
        }
    }
}
#endif

