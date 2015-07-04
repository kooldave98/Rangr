using System;

using UIKit;
using CoreLocation;
using CoreGraphics;
using common_lib;

using Google.Maps;


namespace experiments.ios
{
    public class MapViewController : SimpleViewController
    {
        public override string TitleLabel { 
            get{ return "Map"; } 
        }

        MapView mapView;

        public override void LoadView()
        {
            base.LoadView();

            CameraPosition camera = CameraPosition.FromCamera(37.797865, -122.402526, 6);

            mapView = MapView.FromCamera(new CGRect(8, 200, 304, 348), camera);
            mapView.MyLocationEnabled = true;

            var xamMarker = new Marker()
            {
                Title = "Xamarin HQ",
                Snippet = "Where the magic happens.",
                Position = new CLLocationCoordinate2D(37.797865, -122.402526),
                Map = mapView
            };

            mapView.SelectedMarker = xamMarker;

            View = mapView;
        }
    }
}

